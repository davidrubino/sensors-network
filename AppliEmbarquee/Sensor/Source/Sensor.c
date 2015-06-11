/******************************************************************************
  Filename:       Sensor.c
  Revised:        $Date: 2014-09-07 13:36:30 -0700 (Sun, 07 Sep 2014) $
  Revision:       $Revision: 40046 $

  Description:    Generic Application (no Profile).


  Copyright 2004-2014 Texas Instruments Incorporated. All rights reserved.

  IMPORTANT: Your use of this Software is limited to those specific rights
  granted under the terms of a software license agreement between the user
  who downloaded the software, his/her employer (which must be your employer)
  and Texas Instruments Incorporated (the "License"). You may not use this
  Software unless you agree to abide by the terms of the License. The License
  limits your use, and you acknowledge, that the Software may not be modified,
  copied or distributed unless embedded on a Texas Instruments microcontroller
  or used solely and exclusively in conjunction with a Texas Instruments radio
  frequency transceiver, which is integrated into your product. Other than for
  the foregoing purpose, you may not use, reproduce, copy, prepare derivative
  works of, modify, distribute, perform, display or sell this Software and/or
  its documentation for any purpose.

  YOU FURTHER ACKNOWLEDGE AND AGREE THAT THE SOFTWARE AND DOCUMENTATION ARE
  PROVIDED “AS IS” WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED,
  INCLUDING WITHOUT LIMITATION, ANY WARRANTY OF MERCHANTABILITY, TITLE,
  NON-INFRINGEMENT AND FITNESS FOR A PARTICULAR PURPOSE. IN NO EVENT SHALL
  TEXAS INSTRUMENTS OR ITS LICENSORS BE LIABLE OR OBLIGATED UNDER CONTRACT,
  NEGLIGENCE, STRICT LIABILITY, CONTRIBUTION, BREACH OF WARRANTY, OR OTHER
  LEGAL EQUITABLE THEORY ANY DIRECT OR INDIRECT DAMAGES OR EXPENSES
  INCLUDING BUT NOT LIMITED TO ANY INCIDENTAL, SPECIAL, INDIRECT, PUNITIVE
  OR CONSEQUENTIAL DAMAGES, LOST PROFITS OR LOST DATA, COST OF PROCUREMENT
  OF SUBSTITUTE GOODS, TECHNOLOGY, SERVICES, OR ANY CLAIMS BY THIRD PARTIES
  (INCLUDING BUT NOT LIMITED TO ANY DEFENSE THEREOF), OR OTHER SIMILAR COSTS.

  Should you have any questions regarding your right to use this Software,
  contact Texas Instruments Incorporated at www.TI.com.
******************************************************************************/

/*********************************************************************
  This application isn't intended to do anything useful - it is
  intended to be a simple example of an application's structure.

  This application periodically sends a "Hello World" message to
  another "Generic" application (see 'txMsgDelay'). The application
  will also receive "Hello World" packets.

  This application doesn't have a profile, so it handles everything
  directly - by itself.

  Key control:
    SW1:  changes the delay between TX packets
    SW2:  initiates end device binding
    SW3:
    SW4:  initiates a match description request
*********************************************************************/

/*********************************************************************
 * INCLUDES
 */
#include <stdio.h>

#include "OSAL.h"
#include "AF.h"
#include "ZDApp.h"
#include "ZDObject.h"
#include "ZDProfile.h"

#include "Sensor.h"
#include "Common.h"
#include "DebugTrace.h"

#if !defined( WIN32 ) || defined( ZBIT )
  #include "OnBoard.h"
#endif

/* HAL */
#include "hal_lcd.h"
#include "hal_led.h"
#include "hal_key.h"
#include "hal_uart.h"

#include "bsp.h"
#include "bsp_led.h"
#include "als_sfh5711.h"

/*********************************************************************
 * MACROS
 */

/*********************************************************************
 * CONSTANTS
 */

/*********************************************************************
 * TYPEDEFS
 */

/*********************************************************************
 * GLOBAL VARIABLES
 */

#warning Do not forget to set sensor info here before compiling
sensor_state_t Sensor_State = {
  .type = LED_ACTUATOR,
  .datatype = BOOL,
};

// This list should be filled with Application specific Cluster IDs.
const cId_t Sensor_ClusterList[SENSOR_MAX_CLUSTERS] =
{
  SENSOR_CLUSTERID
};

const SimpleDescriptionFormat_t Sensor_SimpleDesc =
{
  SENSOR_ENDPOINT,              //  int Endpoint;
  SENSOR_PROFID,                //  uint16 AppProfId[2];
  SENSOR_DEVICEID,              //  uint16 AppDeviceId[2];
  SENSOR_DEVICE_VERSION,        //  int   AppDevVer:4;
  SENSOR_FLAGS,                 //  int   AppFlags:4;
  SENSOR_MAX_CLUSTERS,          //  byte  AppNumInClusters;
  (cId_t *)Sensor_ClusterList,  //  byte *pAppInClusterList;
  SENSOR_MAX_CLUSTERS,          //  byte  AppNumInClusters;
  (cId_t *)Sensor_ClusterList   //  byte *pAppInClusterList;
};

// This is the Endpoint/Interface description.  It is defined here, but
// filled-in in Sensor_Init().  Another way to go would be to fill
// in the structure here and make it a "const" (in code space).  The
// way it's defined in this sample app it is define in RAM.
endPointDesc_t Sensor_epDesc;

/*********************************************************************
 * EXTERNAL VARIABLES
 */

/*********************************************************************
 * EXTERNAL FUNCTIONS
 */

/*********************************************************************
 * LOCAL VARIABLES
 */
byte Sensor_TaskID;   // Task ID for internal task/event processing
                          // This variable will be received when
                          // Sensor_Init() is called.

devStates_t Sensor_NwkState;

byte Sensor_TransID;  // This is the unique message ID (counter)

afAddrType_t Sensor_DstAddr;

// Time interval between sending messages
static uint32 txMsgDelay = SENSOR_SEND_MSG_TIMEOUT;

/*********************************************************************
 * LOCAL FUNCTIONS
 */
static void Sensor_ProcessZDOMsgs( zdoIncomingMsg_t *inMsg );
static void Sensor_HandleKeys( byte shift, byte keys );
static void Sensor_MessageMSGCB( afIncomingMSGPacket_t *pckt );
static void Sensor_SendTheMessage( void );

/*********************************************************************
 * NETWORK LAYER CALLBACKS
 */

/*********************************************************************
 * PUBLIC FUNCTIONS
 */

/*********************************************************************
 * @fn      Sensor_Init
 *
 * @brief   Initialization function for the Generic App Task.
 *          This is called during initialization and should contain
 *          any application specific initialization (ie. hardware
 *          initialization/setup, table initialization, power up
 *          notificaiton ... ).
 *
 * @param   task_id - the ID assigned by OSAL.  This ID should be
 *                    used to send messages and set timers.
 *
 * @return  none
 */
void Sensor_Init( uint8 task_id )
{
  Sensor_TaskID = task_id;
  Sensor_NwkState = DEV_INIT;
  Sensor_TransID = 0;

  // Device hardware initialization can be added here or in main() (Zmain.c).
  // If the hardware is application specific - add it here.
  // If the hardware is other parts of the device add it in main().

  Sensor_DstAddr.addrMode = (afAddrMode_t)Addr16Bit;
  Sensor_DstAddr.endPoint = SENSOR_ENDPOINT;
  Sensor_DstAddr.addr.shortAddr = 0;

  // Fill out the endpoint description.
  Sensor_epDesc.endPoint = SENSOR_ENDPOINT;
  Sensor_epDesc.task_id = &Sensor_TaskID;
  Sensor_epDesc.simpleDesc
            = (SimpleDescriptionFormat_t *)&Sensor_SimpleDesc;
  Sensor_epDesc.latencyReq = noLatencyReqs;

  // Register the endpoint description with the AF
  afRegister( &Sensor_epDesc );

  // Register for all key events - This app will handle all key events
  RegisterForKeys( Sensor_TaskID );

  ZMacLqiAdjustMode(LQI_ADJ_MODE1);

  // Update the display
#if defined ( LCD_SUPPORTED )
#  ifdef RTR_NWK
  HalLcdWriteString( "Router ^_^", HAL_LCD_LINE_1 );
#  else
  HalLcdWriteString( "Sensor ^_^", HAL_LCD_LINE_1 );
#  endif
#endif

  char *typestr;

  switch (Sensor_State.type) {
  case LIGHT_SENSOR:
    alsInit();
    typestr = "Sensor: Light";
    break;
  case SWITCH_SENSOR:
    typestr = "Sensor: Switch";
    break;
  case LED_ACTUATOR:
    typestr = "Actuator: LED";
    break;
  }
  
  HalLcdWriteString(typestr, HAL_LCD_LINE_5);
}

/*********************************************************************
 * @fn      Sensor_ProcessEvent
 *
 * @brief   Generic Application Task event processor.  This function
 *          is called to process all events for the task.  Events
 *          include timers, messages and any other user defined events.
 *
 * @param   task_id  - The OSAL assigned task ID.
 * @param   events - events to process.  This is a bit map and can
 *                   contain more than one event.
 *
 * @return  none
 */
uint16 Sensor_ProcessEvent( uint8 task_id, uint16 events )
{
  afIncomingMSGPacket_t *MSGpkt;
  afDataConfirm_t *afDataConfirm;

  // Data Confirmation message fields
  byte sentEP;
  ZStatus_t sentStatus;
  byte sentTransID;       // This should match the value sent
  (void)task_id;  // Intentionally unreferenced parameter

  if ( events & SYS_EVENT_MSG )
  {
    MSGpkt = (afIncomingMSGPacket_t *)osal_msg_receive( Sensor_TaskID );
    while ( MSGpkt )
    {
      switch ( MSGpkt->hdr.event )
      {
        case ZDO_CB_MSG:
          Sensor_ProcessZDOMsgs( (zdoIncomingMsg_t *)MSGpkt );
          break;

        case KEY_CHANGE:
          Sensor_HandleKeys( ((keyChange_t *)MSGpkt)->state, ((keyChange_t *)MSGpkt)->keys );
          break;

        case AF_DATA_CONFIRM_CMD:
          // This message is received as a confirmation of a data packet sent.
          // The status is of ZStatus_t type [defined in ZComDef.h]
          // The message fields are defined in AF.h
          afDataConfirm = (afDataConfirm_t *)MSGpkt;

          sentEP = afDataConfirm->endpoint;
          (void)sentEP;  // This info not used now
          sentTransID = afDataConfirm->transID;
          (void)sentTransID;  // This info not used now

          sentStatus = afDataConfirm->hdr.status;
          // Action taken when confirmation is received.
          if ( sentStatus != ZSuccess )
          {
            // The data wasn't delivered
            HalLedSet ( HAL_LED_4, HAL_LED_MODE_FLASH );
          }
          break;

        case AF_INCOMING_MSG_CMD:
          Sensor_MessageMSGCB( MSGpkt );
          break;

        case ZDO_STATE_CHANGE:
          Sensor_NwkState = (devStates_t)(MSGpkt->hdr.status);
          if (Sensor_NwkState == DEV_END_DEVICE || Sensor_NwkState == DEV_ROUTER)
          {
            // Start sending "the" message in a regular interval.
            osal_start_timerEx( Sensor_TaskID,
                                SENSOR_SEND_MSG_EVT,
                                txMsgDelay );
          }
          break;

        default:
          break;
      }

      // Release the memory
      osal_msg_deallocate( (uint8 *)MSGpkt );

      // Next
      MSGpkt = (afIncomingMSGPacket_t *)osal_msg_receive( Sensor_TaskID );
    }

    // return unprocessed events
    return (events ^ SYS_EVENT_MSG);
  }

  // Send a message out - This event is generated by a timer
  //  (setup in Sensor_Init()).
  if ( events & SENSOR_SEND_MSG_EVT )
  {
    // Send "the" message
    Sensor_SendTheMessage();

    // Setup to send message again
    osal_start_timerEx( Sensor_TaskID,
                        SENSOR_SEND_MSG_EVT,
                        txMsgDelay );

    // return unprocessed events
    return (events ^ SENSOR_SEND_MSG_EVT);
  }

  // Discard unknown events
  return 0;
}

/*********************************************************************
 * Event Generation Functions
 */

/*********************************************************************
 * @fn      Sensor_ProcessZDOMsgs()
 *
 * @brief   Process response messages
 *
 * @param   none
 *
 * @return  none
 */
static void Sensor_ProcessZDOMsgs( zdoIncomingMsg_t *inMsg )
{
  switch ( inMsg->clusterID )
  {
  }
}

/*********************************************************************
 * @fn      Sensor_HandleKeys
 *
 * @brief   Handles all key events for this device.
 *
 * @param   shift - true if in shift/alt.
 * @param   keys - bit field for key events. Valid entries:
 *                 HAL_KEY_SW_4
 *                 HAL_KEY_SW_3
 *                 HAL_KEY_SW_2
 *                 HAL_KEY_SW_1
 *
 * @return  none
 */
static void Sensor_HandleKeys( uint8 shift, uint8 keys )
{
  // Shift is used to make each button/switch dual purpose.
  if ( shift )
  {
    if ( keys & HAL_KEY_SW_1 )
    {
    }
    if ( keys & HAL_KEY_SW_2 )
    {
    }
    if ( keys & HAL_KEY_SW_3 )
    {
    }
    if ( keys & HAL_KEY_SW_4 )
    {
    }
  }
  else
  {
    if ( keys & HAL_KEY_SW_1 )
    {
      HalLedSet ( HAL_LED_1, HAL_LED_MODE_TOGGLE );
    }

    if ( keys & HAL_KEY_SW_2 )
    {
      //Force send of the message
      Sensor_SendTheMessage();
    }

    if ( keys & HAL_KEY_SW_3 )
    {
    }

    if ( keys & HAL_KEY_SW_4 )
    {
    }
  }
}

/*********************************************************************
 * LOCAL FUNCTIONS
 */

/*********************************************************************
 * @fn      Sensor_MessageMSGCB
 *
 * @brief   Data message processor callback.  This function processes
 *          any incoming data - probably from other devices.  So, based
 *          on cluster ID, perform the intended action.
 *
 * @param   none
 *
 * @return  none
 */
static void Sensor_MessageMSGCB( afIncomingMSGPacket_t *pkt )
{
  if (pkt->clusterId != SENSOR_CLUSTERID) {
    return;
  }

  /* Ignore messages not coming from the coordinator */
  if (pkt->srcAddr.addrMode != afAddr16Bit || pkt->srcAddr.addr.shortAddr != 0) {
    return;
  }

  switch (Sensor_State.type)
  {
  case LED_ACTUATOR:
    /* we're a LED actuator, just read a boolean value and set our LED accordingly */
    bool on = *((bool*)pkt->cmd.Data);
    bool previous = (HalLedGetState() & HAL_LED_1) != 0;
    if (on != previous) {
      HalLedSet(HAL_LED_1, on ? HAL_LED_MODE_ON : HAL_LED_MODE_OFF);
      Sensor_SendTheMessage(); /* immediately send our new state back to the coordinator */
    }
    break;
  }
}

/*********************************************************************
 * @fn      Sensor_SendTheMessage
 *
 * @brief   Send "the" message.
 *
 * @param   none
 *
 * @return  none
 */
static void Sensor_SendTheMessage( void )
{
  switch (Sensor_State.type) {
  case SWITCH_SENSOR:
  case LED_ACTUATOR:
    Sensor_State.val.bool_val = HalLedGetState() & HAL_LED_1;
    HalLcdWriteStringValue("Val: ", Sensor_State.val.bool_val, 10, HAL_LCD_LINE_6);
    break;
  case LIGHT_SENSOR:
    Sensor_State.val.int_val = alsRead();
    HalLcdWriteStringValue("Val: ", (uint16) (Sensor_State.val.int_val), 10, HAL_LCD_LINE_6);
    break;
  }

  if ( AF_DataRequest( &Sensor_DstAddr, &Sensor_epDesc,
                       SENSOR_CLUSTERID,
                       sizeof(sensor_state_t),
                       (byte *) &Sensor_State,
                       &Sensor_TransID,
                       AF_DISCV_ROUTE, AF_DEFAULT_RADIUS ) == afStatus_SUCCESS )
  {
    // Successfully requested to be sent.
    HalLedSet ( HAL_LED_2, HAL_LED_MODE_BLINK );
    HalLedSet ( HAL_LED_4, HAL_LED_MODE_OFF );
  }
  else
  {
    // Error occurred in request to send.
    HalLedSet ( HAL_LED_4, HAL_LED_MODE_FLASH );
  }
}

/*********************************************************************
 */
