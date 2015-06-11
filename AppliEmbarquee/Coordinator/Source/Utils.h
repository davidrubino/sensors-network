#pragma once
#include <string.h>

#include "ZComDef.h"
#include "hal_types.h"
#include "af.h"

#include "NvList.h"
#include "Common.h"

#define MAGIC_NUMBER_ID 0x0401       // ID of the magic number (see init_nvram for details)
#define SENSOR_ITEM_LIST_ID 0x0402   // At this id, the nvram contain an uint16 which is the id of the beginning of the linked list of sensor_item
#define TRIGGER_EVENT_LIST_ID 0x0403 // Same as above for the list of trigger event
#define FREE_START_ID 0x0404         // From here, wen can start searching for free id
#define FREE_ID_COUNT (0xFFF - FREE_START_ID + 1)

enum threshold_type {
  NONE = 0,  /* No threshold set */
  EQ,        /* Equal */
  LTE,       /* Less than or equal (valid only for FLOAT and INT types) */
  LT,        /* Less than (strict, valid only for FLOAT and INT types) */
  GTE,       /* Greater than or equal (valid only for FLOAT and INT types) */
  GT         /* Greater than (strict, valid only for FLOAT and INT types) */
};
// Declared as uint8 rather than threshold_type because enum type cannot be specified with
// this compiler
typedef uint8 threshold_type_t;

/** Static information about a sensor, stored in the NVRAM inside sensor_item_t structure */
__packed typedef struct {
  ZLongAddr_t ieee_addr;           // Unique 64 bits address
  sensor_type_t type;
  sensor_datatype_t datatype;
  sensor_val_t threshold;
  threshold_type_t threshold_type;
} sensor_info_t; // 8 + 1 + 1 + 4 + 1 = 15 bytes
/* WARNING: Do not forget to increment MAGIC_NUMBER in Coordinator.h if you change this structure */

/*********************************************************************
 * NVRAM elements
 * Those structures represent elements of an NvList stored in the NVRAM to keep track 
 * of the network state. They all follow list_item_t convention (first next_id field points
 * to the next item ID or 0 at the end)
 */

/** Sensor state
  * contains static info (sensor_info_t) as well as last value and last communication time */
__packed typedef struct {
  uint16 next_id;
  sensor_info_t info;
  uint32 last_seen_sec;         // Time of last communication with the sensor (in seconds,
                                // measured relative to the coordinator start-up (0))
  sensor_val_t last_val;        // last data communicated by the sensor
  uint8 LQI;                    // Link quality indicator
} sensor_item_t; // 2 + 15 + 4 + 4 + 1 = 26 bytes
/* WARNING: Do not forget to increment MAGIC_NUMBER in Coordinator.h if you change this structure */

__packed typedef struct {
  uint16 next_id;
  ZLongAddr_t ieee_addr;
} addr_list_t;
/* WARNING: Do not forget to increment MAGIC_NUMBER in Coordinator.h if you change this structure */

__packed typedef struct {
  uint16 next_id;
  uint16 addr_list_id;
  ZLongAddr_t ieee_addr_target;
} trigger_event_t;
/* WARNING: Do not forget to increment MAGIC_NUMBER in Coordinator.h if you change this structure */

/** Commands that can be sent to the coordinator through the serial port */
typedef enum {
  CMD_GET_SENSOR_LIST = 0,
  CMD_DELETE_SENSOR = 1,
  CMD_MODIFY_SENSOR = 2,
  CMD_DUMP_SENSORS = 3,
  CMD_ADD_RULE = 4,
  CMD_GET_RULE_LIST = 5,
  CMD_DELETE_RULE = 6,
  CMD_DUMP_RULES = 7,
  CMD_GET_TIME = 8,
  CMD_SEND_DATA = 9,
  CMD_NVRAM_INFO = 10,
  CMD_PING = 0x42,
  CMD_CLEAR_ALL = 0xA0,
} operation_code_t;

/** Status codes used in reply to serial commands */
typedef enum {
  ERROR_SUCCESS = 0,         /* no error */
  ERROR_INVALID_FORMAT = 1,  /* some field was missing or had an invalid value */
  ERROR_NOT_FOUND = 2,       /* requested item was not found */
  ERROR_INTERNAL = 3,        /* internal error */
  ERROR_UNKNOWN = 0xFF,      /* unknown error */
} error_code_t;

/** Get the sensor_item matching the IEEE address
  * @param ieee_addr IEEE address
  * @param data pointer to store the sensor_item into (can NOT be NULL)
  * @return the id of the item if a match was found, 0 otherwise
  */
uint16 get_sensor_item_from_ieee(const ZLongAddr_t ieee_addr, sensor_item_t *data);

/** Handle an incoming message from a sensor
  * @param pkt the packet to handle
  */
void handle_message(afIncomingMSGPacket_t *pkt);

/** Check if a sensor is alarmed (value over threshold)
  */
bool is_sensor_alarmed(const sensor_item_t *sensor);

/** Send the list of all known sensors
  * Serial Output : - number of sensors (uint8)
  *                 - For each sensor :
  *                      - sensor info (sensor_info_t)
  *                      - time of last communication (uint32)
  *                      - last value (sensor_val_t)
  *                      - alarm status (bool)
  *                      - Link Quality (uint8)
  */
void send_sensor_list(void);

/** Create or modify a sensor
  * Serial input : - sensor_info_t
  */
void modify_sensor();

/** Dump sensors in ASCII form for debugging purposes */
void dump_sensors();

/** Delete a sensor
  * Serial input : - IEEE address
  */
void delete_sensor();


/** Ping ? Pong!
  * Serial output : PONG!
  */
void ping();

/** Init our structures in NVRAM (list of sensors and trigger events)
  */
void init_nvram();

/** Clear all data in the coordinator (factory reset)
  */
void clear_all();

/** Add a new trigger rule
  * Serial Input :
  *  - IEEE address of the actuator (8 bytes)
  *  - number of triggering sensors (uint8, can NOT be 0)
  *  For each triggering sensor :
        - IEEE address of the triggering sensor
  * Serial Output :
  *  - error code (error_code_t)
  */
void add_rule();

/** Send the list of all trigger rules
  * Serial Output :
  *   - number of rules (uint8)
  *   For each rule :
  *     - identifier of the rule
  *     - IEEE address of the actuator
  *     - number of triggering sensors
  *     For each triggering sensor:
  *        - IEEE address of the sensor
  */
void send_rule_list();

/** Delete a trigger rule
  * Serial Input :
  *   - identifier of the rule
  *   - IEEE address of the actuator
  * Serial Output :
  *   - error code (error_code_t)
  */
void delete_rule();

/** Evaluate trigger rules (this is called by the coordinator periodically) */
void evaluate_triggers();

/** Send local time (in secs since startup) for time calculation
  * Serial Output : - time (4 bytes)
  */
void send_time();

/** Send data to a device in the network
  * Serial Input :
  *   - IEEE address of the destination (8 bytes)
  *   - number of bytes of data to send (1 byte)
  *   - data to send
  * Serial Output :
  *   - error code (error_code_t)
  */
void send_data();

/** Dump NVRAM usage info over the serial port */
void nvram_info();
