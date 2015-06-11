/**************************************************************************************************
  Filename:       zcl_sampleswota_data.c
  Revised:        $Date: 2013-08-14 15:04:49 -0700 (Wed, 14 Aug 2013) $
  Revision:       $Revision: 34972 $


  Description:    Zigbee Cluster Library - sample device application.


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

/*********************************************************************
 * INCLUDES
 */
#include "ZComDef.h"
#include "OSAL.h"
#include "AF.h"
#include "ZDConfig.h"

#include "zcl.h"
#include "zcl_general.h"
#include "zcl_ha.h"
#include "zcl_ota.h"

#include "zcl_sampleswota.h"

/*********************************************************************
 * CONSTANTS
 */

#define SAMPLESWOTA_DEVICE_VERSION     0
#define SAMPLESWOTA_FLAGS              0

#define SAMPLESWOTA_HWVERSION          0
#define SAMPLESWOTA_ZCLVERSION         0

/*********************************************************************
 * TYPEDEFS
 */

/*********************************************************************
 * MACROS
 */

/*********************************************************************
 * GLOBAL VARIABLES
 */

// Basic Cluster
const uint8 zclSampleSwOta_HWRevision = SAMPLESWOTA_HWVERSION;
const uint8 zclSampleSwOta_ZCLVersion = SAMPLESWOTA_ZCLVERSION;
const uint8 zclSampleSwOta_ManufacturerName[] = { 16, 'T','e','x','a','s','I','n','s','t','r','u','m','e','n','t','s' };
const uint8 zclSampleSwOta_ModelId[] = { 16, 'T','I','0','0','0','1',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ' };
const uint8 zclSampleSwOta_DateCode[] = { 16, '2','0','0','6','0','8','3','1',' ',' ',' ',' ',' ',' ',' ',' ' };
const uint8 zclSampleSwOta_PowerSource = POWER_SOURCE_MAINS_1_PHASE;

uint8 zclSampleSwOta_LocationDescription[17] = { 16, ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ' };
uint8 zclSampleSwOta_PhysicalEnvironment = 0;
uint8 zclSampleSwOta_DeviceEnable = DEVICE_ENABLED;

// Identify Cluster
uint16 zclSampleSwOta_IdentifyTime = 0;

// On/Off Cluster
uint8  zclSampleSwOta_OnOff = LIGHT_OFF;

// OTA Cluster
uint8 zclSampleSwOta_UpgServerID[8] = { 0, 0, 0, 0, 0, 0, 0, 0 };
uint32 zclSampleSwOta_FileOffset = 0;
uint32 zclSampleSwOta_CurrentFileVersion = 0;
uint16 zclSampleSwOta_CurrentZigBeeStackVersion = 0;
uint32 zclSampleSwOta_DownloadedFileVersion = 0;
uint16 zclSampleSwOta_DownloadedZigBeeStackVersion = 0;
uint8 zclSampleSwOta_ImgUpgradeStatus = 0;
uint16 zclSampleSwOta_MfrID = 0;
uint16 zclSampleSwOta_ImgTypeID = 0;
uint16 zclSampleSwOta_MinBlockReqDelay = 0;
uint32 zclSampleSwOta_ImgStamp = 0;

/*********************************************************************
 * ATTRIBUTE DEFINITIONS - Uses REAL cluster IDs
 */
CONST zclAttrRec_t zclSampleSwOta_Attrs[SAMPLESWOTA_MAX_ATTRIBUTES] =
{
  // *** General Basic Cluster Attributes ***
  {
    ZCL_CLUSTER_ID_GEN_BASIC,             // Cluster IDs - defined in the foundation (ie. zcl.h)
    {  // Attribute record
      ATTRID_BASIC_HW_VERSION,            // Attribute ID - Found in Cluster Library header (ie. zcl_general.h)
      ZCL_DATATYPE_UINT8,                 // Data Type - found in zcl.h
      ACCESS_CONTROL_READ,                // Variable access control - found in zcl.h
      (void *)&zclSampleSwOta_HWRevision  // Pointer to attribute variable
    }
  },
  {
    ZCL_CLUSTER_ID_GEN_BASIC,
    { // Attribute record
      ATTRID_BASIC_ZCL_VERSION,
      ZCL_DATATYPE_UINT8,
      ACCESS_CONTROL_READ,
      (void *)&zclSampleSwOta_ZCLVersion
    }
  },
  {
    ZCL_CLUSTER_ID_GEN_BASIC,
    { // Attribute record
      ATTRID_BASIC_MANUFACTURER_NAME,
      ZCL_DATATYPE_CHAR_STR,
      ACCESS_CONTROL_READ,
      (void *)zclSampleSwOta_ManufacturerName
    }
  },
  {
    ZCL_CLUSTER_ID_GEN_BASIC,
    { // Attribute record
      ATTRID_BASIC_MODEL_ID,
      ZCL_DATATYPE_CHAR_STR,
      ACCESS_CONTROL_READ,
      (void *)zclSampleSwOta_ModelId
    }
  },
  {
    ZCL_CLUSTER_ID_GEN_BASIC,
    { // Attribute record
      ATTRID_BASIC_DATE_CODE,
      ZCL_DATATYPE_CHAR_STR,
      ACCESS_CONTROL_READ,
      (void *)zclSampleSwOta_DateCode
    }
  },
  {
    ZCL_CLUSTER_ID_GEN_BASIC,
    { // Attribute record
      ATTRID_BASIC_POWER_SOURCE,
      ZCL_DATATYPE_UINT8,
      ACCESS_CONTROL_READ,
      (void *)&zclSampleSwOta_PowerSource
    }
  },
  {
    ZCL_CLUSTER_ID_GEN_BASIC,
    { // Attribute record
      ATTRID_BASIC_LOCATION_DESC,
      ZCL_DATATYPE_CHAR_STR,
      (ACCESS_CONTROL_READ | ACCESS_CONTROL_WRITE),
      (void *)zclSampleSwOta_LocationDescription
    }
  },
  {
    ZCL_CLUSTER_ID_GEN_BASIC,
    { // Attribute record
      ATTRID_BASIC_PHYSICAL_ENV,
      ZCL_DATATYPE_UINT8,
      (ACCESS_CONTROL_READ | ACCESS_CONTROL_WRITE),
      (void *)&zclSampleSwOta_PhysicalEnvironment
    }
  },
  {
    ZCL_CLUSTER_ID_GEN_BASIC,
    { // Attribute record
      ATTRID_BASIC_DEVICE_ENABLED,
      ZCL_DATATYPE_UINT8,
      (ACCESS_CONTROL_READ | ACCESS_CONTROL_WRITE),
      (void *)&zclSampleSwOta_DeviceEnable
    }
  },

  // *** Identify Cluster Attribute ***
  {
    ZCL_CLUSTER_ID_GEN_IDENTIFY,
    { // Attribute record
      ATTRID_IDENTIFY_TIME,
      ZCL_DATATYPE_UINT16,
      (ACCESS_CONTROL_READ | ACCESS_CONTROL_WRITE),
      (void *)&zclSampleSwOta_IdentifyTime
    }
  },

  // *** On / Off Cluster Attributes ***
  {
    ZCL_CLUSTER_ID_GEN_ON_OFF,
    { // Attribute record
      ATTRID_ON_OFF,
      ZCL_DATATYPE_UINT8,
      ACCESS_CONTROL_READ,
      (void *)&zclSampleSwOta_OnOff
    }
  },

  // *** OTA Cluster Attributes ***
  {
    ZCL_CLUSTER_ID_OTA,
    { // Attribute record
      ATTRID_UPGRADE_SERVER_ID,
      ZCL_DATATYPE_IEEE_ADDR,
      ACCESS_CONTROL_READ,
      (void *)zclSampleSwOta_UpgServerID
    }
  },
  {
    ZCL_CLUSTER_ID_OTA,
    { // Attribute record
      ATTRID_FILE_OFFSET,
      ZCL_DATATYPE_UINT32,
      ACCESS_CONTROL_READ,
      (void *)&zclSampleSwOta_FileOffset
    }
  },
  {
    ZCL_CLUSTER_ID_OTA,
    { // Attribute record
      ATTRID_CURRENT_FILE_VERSION,
      ZCL_DATATYPE_UINT32,
      ACCESS_CONTROL_READ,
      (void *)&zclSampleSwOta_CurrentFileVersion
    }
  },
  {
    ZCL_CLUSTER_ID_OTA,
    { // Attribute record
      ATTRID_CURRENT_ZIGBEE_STACK_VERSION,
      ZCL_DATATYPE_UINT16,
      ACCESS_CONTROL_READ,
      (void *)&zclSampleSwOta_CurrentZigBeeStackVersion
    }
  },
  {
    ZCL_CLUSTER_ID_OTA,
    { // Attribute record
      ATTRID_DOWNLOADED_FILE_VERSION,
      ZCL_DATATYPE_UINT32,
      ACCESS_CONTROL_READ,
      (void *)&zclSampleSwOta_DownloadedFileVersion
    }
  },
  {
    ZCL_CLUSTER_ID_OTA,
    { // Attribute record
      ATTRID_DOWNLOADED_ZIGBEE_STACK_VERSION,
      ZCL_DATATYPE_UINT16,
      ACCESS_CONTROL_READ,
      (void *)&zclSampleSwOta_DownloadedZigBeeStackVersion
    }
  },
  {
    ZCL_CLUSTER_ID_OTA,
    { // Attribute record
      ATTRID_IMAGE_UPGRADE_STATUS,
      ZCL_DATATYPE_ENUM8,
      ACCESS_CONTROL_READ,
      (void *)&zclSampleSwOta_ImgUpgradeStatus
    }
  },
  {
    ZCL_CLUSTER_ID_OTA,
    { // Attribute record
      ATTRID_MANUFACTURER_ID,
      ZCL_DATATYPE_UINT16,
      ACCESS_CONTROL_READ,
      (void *)&zclSampleSwOta_MfrID
    }
  },
  {
    ZCL_CLUSTER_ID_OTA,
    { // Attribute record
      ATTRID_IMAGE_TYPE_ID,
      ZCL_DATATYPE_UINT16,
      ACCESS_CONTROL_READ,
      (void *)&zclSampleSwOta_ImgTypeID
    }
  },
  {
    ZCL_CLUSTER_ID_OTA,
    { // Attribute record
      ATTRID_MINIMUM_BLOCK_REQ_DELAY,
      ZCL_DATATYPE_UINT16,
      ACCESS_CONTROL_READ,
      (void *)&zclSampleSwOta_MinBlockReqDelay
    }
  },
  {
    ZCL_CLUSTER_ID_OTA,
    { // Attribute record
      ATTRID_IMAGE_STAMP,
      ZCL_DATATYPE_UINT32,
      ACCESS_CONTROL_READ,
      (void *)&zclSampleSwOta_ImgStamp
    }
  }
};

/*********************************************************************
 * SIMPLE DESCRIPTOR
 */
// This is the Cluster ID List and should be filled with Application
// specific cluster IDs.
#define ZCLSAMPLESWOTA_MAX_INCLUSTERS       1
const cId_t zclSampleSwOta_InClusterList[ZCLSAMPLESWOTA_MAX_INCLUSTERS] =
{
  ZCL_CLUSTER_ID_GEN_BASIC
};

#define ZCLSAMPLESWOTA_MAX_OUTCLUSTERS       2
const cId_t zclSampleSwOta_OutClusterList[ZCLSAMPLESWOTA_MAX_OUTCLUSTERS] =
{
  ZCL_CLUSTER_ID_GEN_ON_OFF,
  ZCL_CLUSTER_ID_OTA
};

SimpleDescriptionFormat_t zclSampleSwOta_SimpleDesc =
{
  SAMPLESWOTA_ENDPOINT,                  //  int Endpoint;
  ZCL_HA_PROFILE_ID,                  //  uint16 AppProfId[2];
  ZCL_HA_DEVICEID_ON_OFF_SWITCH,      //  uint16 AppDeviceId[2];
  SAMPLESWOTA_DEVICE_VERSION,            //  int   AppDevVer:4;
  SAMPLESWOTA_FLAGS,                     //  int   AppFlags:4;
  ZCLSAMPLESWOTA_MAX_INCLUSTERS,         //  byte  AppNumInClusters;
  (cId_t *)zclSampleSwOta_InClusterList, //  byte *pAppInClusterList;
  ZCLSAMPLESWOTA_MAX_OUTCLUSTERS,        //  byte  AppNumInClusters;
  (cId_t *)zclSampleSwOta_OutClusterList //  byte *pAppInClusterList;
};

/*********************************************************************
 * GLOBAL FUNCTIONS
 */

/*********************************************************************
 * LOCAL FUNCTIONS
 */

/****************************************************************************
****************************************************************************/


