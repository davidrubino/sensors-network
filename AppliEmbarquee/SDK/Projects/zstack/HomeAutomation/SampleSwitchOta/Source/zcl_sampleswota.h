/**************************************************************************************************
  Filename:       zcl_sampleswota.h
  Revised:        $Date: 2013-08-02 16:56:53 -0700 (Fri, 02 Aug 2013) $
  Revision:       $Revision: 34876 $


  Description:    This file contains the Zigbee Cluster Library Home
                  Automation Sample Application.


  Copyright 2006-2013 Texas Instruments Incorporated. All rights reserved.

  IMPORTANT: Your use of this Software is limited to those specific rights
  granted under the terms of a software license agreement between the user
  who downloaded the software, his/her employer (which must be your employer)
  and Texas Instruments Incorporated (the "License").  You may not use this
  Software unless you agree to abide by the terms of the License. The License
  limits your use, and you acknowledge, that the Software may not be modified,
  copied or distributed unless embedded on a Texas Instruments microcontroller
  or used solely and exclusively in conjunction with a Texas Instruments radio
  frequency transceiver, which is integrated into your product.  Other than for
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
**************************************************************************************************/

#ifndef ZCL_SAMPLESWOTA_H
#define ZCL_SAMPLESWOTA_H

#ifdef __cplusplus
extern "C"
{
#endif

/*********************************************************************
 * INCLUDES
 */
#include "zcl.h"

/*********************************************************************
 * CONSTANTS
 */
#define SAMPLESWOTA_ENDPOINT           8

#define SAMPLESWOTA_MAX_ATTRIBUTES     22

#define LIGHT_OFF                       0x00
#define LIGHT_ON                        0x01

// Events for the sample app
#define SAMPLESWOTA_IDENTIFY_TIMEOUT_EVT         0x0001
#define SAMPLESWOTA_QUERY_SERVER_EVT             0x0002

#define SAMPLESWOTA_OTA_MAX_SERVERS                 6

#define SAMPLESWOTA_OTA_IDLE_STATE                  0
#define SAMPLESWOTA_OTA_CBKE_STATE                  1
#define SAMPLESWOTA_OTA_DL_STATE                    2

/*********************************************************************
 * MACROS
 */
#define DEVICE_POLL_RATE                 8000   // Poll rate for end device
/*********************************************************************
 * TYPEDEFS
 */

/*********************************************************************
 * VARIABLES
 */
extern SimpleDescriptionFormat_t zclSampleSwOta_SimpleDesc;

extern CONST zclAttrRec_t zclSampleSwOta_Attrs[];

extern uint8  zclSampleSwOta_OnOff;

extern uint16 zclSampleSwOta_IdentifyTime;

// OTA Cluster Attributes
extern uint8 zclSampleSwOta_UpgServerID[];
extern uint32 zclSampleSwOta_FileOffset;
extern uint32 zclSampleSwOta_CurrentFileVersion;
extern uint16 zclSampleSwOta_CurrentZigBeeStackVersion;
extern uint32 zclSampleSwOta_DownloadedFileVersion;
extern uint16 zclSampleSwOta_DownloadedZigBeeStackVersion;
extern uint8 zclSampleSwOta_ImgUpgradeStatus;
extern uint16 zclSampleSwOta_MfrID;
extern uint16 zclSampleSwOta_ImgTypeID;
extern uint16 zclSampleSwOta_MinBlockReqDelay;
extern uint32 zclSampleSwOta_ImgStamp;

/*********************************************************************
 * FUNCTIONS
 */

 /*
  * Initialization for the task
  */
extern void zclSampleSwOta_Init( byte task_id );

/*
 *  Event Process for the task
 */
extern UINT16 zclSampleSwOta_event_loop( byte task_id, UINT16 events );


/*********************************************************************
*********************************************************************/

#ifdef __cplusplus
}
#endif

#endif /* ZCL_SAMPLEAPP_H */
