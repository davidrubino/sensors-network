/******************************************************************************
  Filename:       Coordinator.c
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
#include "OSAL.h"
#include "AF.h"
#include "ZDApp.h"
#include "ZDObject.h"
#include "ZDProfile.h"

#include "Coordinator.h"
#include "DebugTrace.h"
#include "Debug.h"

#if !defined( WIN32 ) || defined( ZBIT )
  #include "OnBoard.h"
#endif

/* HAL */
#include "hal_lcd.h"
#include "hal_led.h"
#include "hal_key.h"
#include "hal_uart.h"

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
// This list should be filled with Application specific Cluster IDs.
const cId_t Coordinator_ClusterList[COORDINATOR_MAX_CLUSTERS] =
{
  COORDINATOR_CLUSTERID
};

const SimpleDescriptionFormat_t Coordinator_SimpleDesc =
{
  COORDINATOR_ENDPOINT,              //  int Endpoint;
  COORDINATOR_PROFID,                //  uint16 AppProfId[2];
  COORDINATOR_DEVICEID,              //  uint16 AppDeviceId[2];
  COORDINATOR_DEVICE_VERSION,        //  int   AppDevVer:4;
  COORDINATOR_FLAGS,                 //  int   AppFlags:4;
  COORDINATOR_MAX_CLUSTERS,          //  byte  AppNumInClusters;
  (cId_t *)Coordinator_ClusterList,  //  byte *pAppInClusterList;
  COORDINATOR_MAX_CLUSTERS,          //  byte  AppNumInClusters;
  (cId_t *)Coordinator_ClusterList   //  byte *pAppInClusterList;
};

// This is the Endpoint/Interface description.  It is defined here, but
// filled-in in Coordinator_Init().  Another way to go would be to fill
// in the structure here and make it a "const" (in code space).  The
// way it's defined in this sample app it is define in RAM.
endPointDesc_t Coordinator_epDesc;

/*********************************************************************
 * EXTERNAL VARIABLES
 */

/*********************************************************************
 * EXTERNAL FUNCTIONS
 */

/*********************************************************************
 * LOCAL VARIABLES
 */
byte Coordinator_TaskID;   // Task ID for internal task/event processing
                          // This variable will be received when
                          // Coordinator_Init() is called.

devStates_t Coordinator_NwkState;

byte Coordinator_TransID;  // This is the unique message ID (counter)

afAddrType_t Coordinator_DstAddr;

// Time interval for evaluating trigger rules
static uint32 txMsgDelay = COORDINATOR_TIMER_DELAY;

// Number of recieved messages
static uint16 rxMsgCount;

/*********************************************************************
 * LOCAL FUNCTIONS
 */
static void Coordinator_ProcessZDOMsgs( zdoIncomingMsg_t *inMsg );
static void Coordinator_HandleKeys( byte shift, byte keys );
static void Coordinator_MessageMSGCB( afIncomingMSGPacket_t *pckt );

static void SerialCallback(uint8 port, uint8 event);

/*********************************************************************
 * NETWORK LAYER CALLBACKS
 */

/*********************************************************************
 * PUBLIC FUNCTIONS
 */

/*********************************************************************
 * @fn      Coordinator_Init
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
void Coordinator_Init( uint8 task_id )
{
  halUARTCfg_t uartConfig;
  Coordinator_TaskID = task_id;
  Coordinator_NwkState = DEV_INIT;
  Coordinator_TransID = 0;

  // Device hardware initialization can be added here or in main() (Zmain.c).
  // If the hardware is application specific - add it here.
  // If the hardware is other parts of the device add it in main().

  Coordinator_DstAddr.addrMode = afAddr64Bit;
  Coordinator_DstAddr.endPoint = COORDINATOR_ENDPOINT;

  // Fill out the endpoint description.
  Coordinator_epDesc.endPoint = COORDINATOR_ENDPOINT;
  Coordinator_epDesc.task_id = &Coordinator_TaskID;
  Coordinator_epDesc.simpleDesc
            = (SimpleDescriptionFormat_t *)&Coordinator_SimpleDesc;
  Coordinator_epDesc.latencyReq = noLatencyReqs;

  // Update the display
#if defined ( LCD_SUPPORTED )
  HalLcdWriteString( "Coordinator \\o/", HAL_LCD_LINE_1 );
#endif

  uartConfig.configured           = TRUE;              // 2x30 don't care - see uart driver.
  uartConfig.baudRate             = SERIAL_BAUD;
  uartConfig.flowControl          = TRUE;
  uartConfig.flowControlThreshold = SERIAL_THRESH; // 2x30 don't care - see uart driver.
  uartConfig.rx.maxBufSize        = SERIAL_RX_SZ;  // 2x30 don't care - see uart driver.
  uartConfig.tx.maxBufSize        = SERIAL_TX_SZ;  // 2x30 don't care - see uart driver.
  uartConfig.idleTimeout          = SERIAL_IDLE;   // 2x30 don't care - see uart driver.
  uartConfig.intEnable            = TRUE;              // 2x30 don't care - see uart driver.
  uartConfig.callBackFunc         = &SerialCallback;
  HalUARTOpen (SERIAL_PORT, &uartConfig);

  ZMacLqiAdjustMode(LQI_ADJ_MODE1);

  init_nvram();
  print_str("Coordinator ready!\r\n");

  // Register the endpoint description with the AF
  afRegister( &Coordinator_epDesc );

  // Register for all key events - This app will handle all key events
  RegisterForKeys( Coordinator_TaskID );
}

/*********************************************************************
 * @fn      Coordinator_ProcessEvent
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
uint16 Coordinator_ProcessEvent( uint8 task_id, uint16 events )
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
    MSGpkt = (afIncomingMSGPacket_t *)osal_msg_receive( Coordinator_TaskID );
    while ( MSGpkt )
    {
      switch ( MSGpkt->hdr.event )
      {
        case ZDO_CB_MSG:
          Coordinator_ProcessZDOMsgs( (zdoIncomingMsg_t *)MSGpkt );
          break;

        case KEY_CHANGE:
          Coordinator_HandleKeys( ((keyChange_t *)MSGpkt)->state, ((keyChange_t *)MSGpkt)->keys );
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
            // The data wasn't delivered -- Do something
          }
          break;

        case AF_INCOMING_MSG_CMD:
          Coordinator_MessageMSGCB( MSGpkt );
          break;

        case ZDO_STATE_CHANGE:
          Coordinator_NwkState = (devStates_t)(MSGpkt->hdr.status);
          if (Coordinator_NwkState == DEV_ZB_COORD) {
            //Network created
            // Evaluate trigger rules at regular interval
            osal_start_timerEx( Coordinator_TaskID,
                                COORDINATOR_TIMER_EVT,
                                txMsgDelay );
          }
          break;

        default:
          break;
      }

      // Release the memory
      osal_msg_deallocate( (uint8 *)MSGpkt );

      // Next
      MSGpkt = (afIncomingMSGPacket_t *)osal_msg_receive( Coordinator_TaskID );
    }

    // return unprocessed events
    return (events ^ SYS_EVENT_MSG);
  }

  if ( events & COORDINATOR_TIMER_EVT )
  {
    evaluate_triggers();

    // Setup the timer again
    osal_start_timerEx( Coordinator_TaskID,
                        COORDINATOR_TIMER_EVT,
                        txMsgDelay );
    return (events ^ COORDINATOR_TIMER_EVT);
  }

  // Discard unknown events
  return 0;
}

/*********************************************************************
 * Event Generation Functions
 */

/*********************************************************************
 * @fn      Coordinator_ProcessZDOMsgs()
 *
 * @brief   Process response messages
 *
 * @param   none
 *
 * @return  none
 */
static void Coordinator_ProcessZDOMsgs( zdoIncomingMsg_t *inMsg )
{
  switch ( inMsg->clusterID )
  {
    //Nothing
  }
}

/*********************************************************************
 * @fn      Coordinator_HandleKeys
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
static void Coordinator_HandleKeys( uint8 shift, uint8 keys )
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
}

/*********************************************************************
 * LOCAL FUNCTIONS
 */

/*********************************************************************
 * @fn      Coordinator_MessageMSGCB
 *
 * @brief   Data message processor callback.  This function processes
 *          any incoming data - probably from other devices.  So, based
 *          on cluster ID, perform the intended action.
 *
 * @param   none
 *
 * @return  none
 */
static void Coordinator_MessageMSGCB( afIncomingMSGPacket_t *pkt )
{
  switch ( pkt->clusterId )
  {
    case COORDINATOR_CLUSTERID:
      handle_message(pkt);
      rxMsgCount += 1;  // Count this message
      HalLedSet ( HAL_LED_2, HAL_LED_MODE_BLINK );  // Blink an LED
      HalLcdWriteStringValue( "Rcvd:", rxMsgCount, 10, HAL_LCD_LINE_5 );
      break;
  }
}

static void SerialCallback(uint8 port, uint8 event)
{
  uint8 cmd;
 
  if (port == SERIAL_PORT/* && (event & HAL_UART_RX_TIMEOUT)*/) {
    if (HalUARTRead(SERIAL_PORT, &cmd, 1) != 1)
      return;

    switch (cmd) {
    case CMD_GET_SENSOR_LIST:
      send_sensor_list();
      break;

    case CMD_DELETE_SENSOR:
      delete_sensor();
      break;

    case CMD_MODIFY_SENSOR:
      modify_sensor();
      break;
      
    case CMD_DUMP_SENSORS:
      dump_sensors();
      break;

    case CMD_ADD_RULE:
      add_rule();
      break;

    case CMD_GET_RULE_LIST:
      send_rule_list();
      break;

    case CMD_DELETE_RULE:
      delete_rule();
      break;

    case CMD_DUMP_RULES:
      nvlist_dump(TRIGGER_EVENT_LIST_ID);
      break;

    case CMD_GET_TIME:
      send_time();
      break;

    case CMD_SEND_DATA:
      send_data();
      break;

    case CMD_NVRAM_INFO:
      nvram_info();
      break;

    case CMD_PING:
      ping();
      break;

    case CMD_CLEAR_ALL:
      clear_all();
      break;
    }
  }
}

void Coordinator_SendMessage(ZLongAddr_t address, uint8 *data, uint16 len) {
    memcpy(Coordinator_DstAddr.addr.extAddr, address, sizeof(ZLongAddr_t));

    AF_DataRequest(&Coordinator_DstAddr, &Coordinator_epDesc,
                   COORDINATOR_CLUSTERID,
                   len,
                   data,
                   &Coordinator_TransID,
                   AF_DISCV_ROUTE, AF_DEFAULT_RADIUS );
}

/*********************************************************************
 */
