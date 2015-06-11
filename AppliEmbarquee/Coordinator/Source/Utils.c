#include <string.h>
#include <stddef.h>

#include "Coordinator.h"
#include "Debug.h"
#include "Utils.h"
#include "onboard.h"
#include "hal_uart.h"
#include "hal_led.h"
#include "hal_lcd.h"
#include "osal_nv.h"
#include "OSAL_Clock.h"

// See Utils.h for the documentation of functions

uint16 get_sensor_item_from_ieee(const ZLongAddr_t ieee_addr, sensor_item_t *item) {
  sensor_item_t temp_item;
  uint16 current_id = nvlist_first(SENSOR_ITEM_LIST_ID, (list_item_t*)&temp_item, sizeof(sensor_item_t));

  if (current_id == 0) {
    return 0;
  }

  while (current_id != 0) {
    if (memcmp(temp_item.info.ieee_addr, ieee_addr, Z_EXTADDR_LEN) == 0) {
      //item found with the same ieee_addr
      memcpy(item, &temp_item, sizeof(sensor_item_t));
      return current_id;
    }
    current_id = nvlist_get_next(current_id, (list_item_t*) &temp_item, sizeof(sensor_item_t));
  }
  return 0;
}

void handle_message(afIncomingMSGPacket_t *pkt) {
  sensor_item_t item;
  ZLongAddr_t ieee_addr;
  sensor_state_t *state;
  uint16 item_id;
  bool changed = false;

  // Get the address in long extended format (64 bits)
  if (pkt->srcAddr.addrMode == afAddr16Bit) {
    APSME_LookupExtAddr(pkt->srcAddr.addr.shortAddr, ieee_addr);
  } else if (pkt->srcAddr.addrMode == afAddr64Bit) {
    memcpy(ieee_addr, pkt->srcAddr.addr.extAddr, Z_EXTADDR_LEN);
  } else {
    // Other addressing modes not handled
    return;
  }
  state = (sensor_state_t *) pkt->cmd.Data;

  // Search for a sensor/actuator with this address in the NVRAM
  if (!(item_id = get_sensor_item_from_ieee(ieee_addr, &item))) {
    // If the sensor/actuator doesn't exist, create it
    changed = true;
    memset(&item, 0, sizeof(sensor_item_t));
    memcpy(item.info.ieee_addr, ieee_addr, Z_EXTADDR_LEN);
    // Create another sensor_item in the nvram
    item_id = nvlist_create(FREE_START_ID, FREE_ID_COUNT, (list_item_t *) &item, sizeof(sensor_item_t));
    // Append this item at the beginning of the list
    nvlist_append(SENSOR_ITEM_LIST_ID, item_id);

    // Important: read back the item because nvlist_append changes its next_id
    // and we don't want to overwrite this value
    nvlist_get(item_id,(list_item_t *) &item, sizeof(sensor_item_t));
  } else if (memcmp(&item.last_val, &state->val, sizeof(sensor_val_t)) != 0) {
    changed = true;
  }

  // Update sensor info from received packet
  item.LQI = pkt->LinkQuality;
  item.info.datatype = state->datatype;
  item.info.type = state->type;
  item.last_val = state->val;
  item.last_seen_sec = osal_getClock();
  // Save it
  nvlist_set(item_id, (list_item_t *) &item, sizeof(sensor_item_t));

  // Signal with a short LED blink if we have an alarmed sensor
  if (is_sensor_alarmed(&item)) {
    HalLedSet(HAL_LED_1, HAL_LED_MODE_BLINK);
  }

  // If the sensor value was changed, evaluate trigger rules
  if (changed) {
    evaluate_triggers();
  }
}

void evaluate_triggers() {
  trigger_event_t rule;
  uint16 rule_id = nvlist_first(TRIGGER_EVENT_LIST_ID, (list_item_t*)&rule, sizeof(rule));
  uint8 matched_count = 0;

  // TODO: properly handle multiple rules with the same actuator address
  // currently, if there are two rules for the same actuator
  // the last rule wins (which is actually the first added)

  // Scan through every rule
  while (rule_id != 0) {
    addr_list_t addr;
    uint16 addr_id = nvlist_first(rule.addr_list_id, (list_item_t*)&addr, sizeof(addr));

    // Check if all sensors concerned by this rule are alarmed
    bool all_triggered = true;

    while (addr_id != 0) {
      sensor_item_t sensor;
      uint16 sensor_id = get_sensor_item_from_ieee(addr.ieee_addr, &sensor);

      if (sensor_id == 0 || !is_sensor_alarmed(&sensor)) {
        all_triggered = false;
        break;
      }

      addr_id = nvlist_get_next(addr_id, (list_item_t*)&addr, sizeof(addr));
    }

    if (all_triggered) {
      matched_count++;
    }

    sensor_item_t actuator_item;
    uint16 actuator_id = get_sensor_item_from_ieee(rule.ieee_addr_target, &actuator_item);

    if (actuator_item.info.datatype != BOOL || actuator_item.last_val.bool_val != all_triggered) {
      // Send the new sensor state (a single boolean value) if the actuator value differs from the
      // desired trigger status
      Coordinator_SendMessage(rule.ieee_addr_target, (uint8*)&all_triggered, 1);
    }
    rule_id = nvlist_get_next(rule_id, (list_item_t*)&rule, sizeof(rule));
  }

  HalLcdWriteStringValue( "Active triggers: ", matched_count, 10, HAL_LCD_LINE_4 );
}

void init_nvram() {
  list_head_t list = { 0 };
  uint32 magic_number = 0;

  // We use a magic number stored at a fixed ID to make sure that data
  // stucture in the coordinator matches the one expected by this program
  // version. This magic number needs to be incremented if the internal data structures
  // change to avoid undefined behaviors of the program.

  osal_nv_read(MAGIC_NUMBER_ID, 0, sizeof(magic_number), &magic_number);

  // If the magic number doesn't match the expected one, we reset it to the correct value and
  // clear the NVRAM

  if (magic_number != MAGIC_NUMBER) {
    magic_number = MAGIC_NUMBER;
    osal_nv_delete(MAGIC_NUMBER_ID, osal_nv_item_len(MAGIC_NUMBER_ID));
    osal_nv_item_init(MAGIC_NUMBER_ID, sizeof(magic_number), &magic_number);
    clear_all();
    return;
  }

  // Create the lists if they don't exist yet
  nvlist_create(SENSOR_ITEM_LIST_ID, 1, (list_item_t *) &list, sizeof(list));
  nvlist_create(TRIGGER_EVENT_LIST_ID, 1, (list_item_t *) &list, sizeof(list));

  // Initialize all sensors times to unknown (0)
  sensor_item_t sensor = { .last_seen_sec = 0 };
  uint16 sensor_id = nvlist_first(SENSOR_ITEM_LIST_ID, NULL, 0);

  while (sensor_id != 0) {
    osal_nv_write(sensor_id, offsetof(sensor_item_t, last_seen_sec), sizeof(sensor.last_seen_sec), &sensor.last_seen_sec);
    sensor_id = nvlist_get_next(sensor_id, NULL, 0);
  }
}

void send_sensor_list(void) {
  sensor_item_t sensor;
  uint16 sensor_id = nvlist_first(SENSOR_ITEM_LIST_ID, (list_item_t*)&sensor, sizeof(sensor));
  uint8 count = nvlist_count(SENSOR_ITEM_LIST_ID);

  HalUARTWrite(SERIAL_PORT, &count, sizeof(count));
  while (count--) {
    bool alarm = is_sensor_alarmed(&sensor);

    HalUARTWrite(SERIAL_PORT, (uint8*)&sensor.info, sizeof(sensor.info));
    HalUARTWrite(SERIAL_PORT, (uint8*)&sensor.last_seen_sec, sizeof(sensor.last_seen_sec));
    HalUARTWrite(SERIAL_PORT, (uint8*)&sensor.last_val, sizeof(sensor.last_val));
    HalUARTWrite(SERIAL_PORT, (uint8*)&alarm, sizeof(alarm));
    HalUARTWrite(SERIAL_PORT, &sensor.LQI, sizeof(uint8));
    sensor_id = nvlist_get_next(sensor_id, (list_item_t*)&sensor, sizeof(sensor));
  }
}

void modify_sensor() {
  sensor_item_t item;
  sensor_info_t info;
  uint16 id;
  uint8 status = ERROR_SUCCESS;

  if (HalUARTRead(SERIAL_PORT, (uint8*)&info, sizeof(info)) < sizeof(info)) {
    status = ERROR_INVALID_FORMAT;
    goto end;
  }

  memset(&item, 0, sizeof(item));

  id = get_sensor_item_from_ieee(info.ieee_addr, &item);

  if (id == 0) {
    // This sensor is not already present, we have to create it and add it to the list
    id = nvlist_create(FREE_START_ID, FREE_ID_COUNT, (list_item_t*)&item, sizeof(item));
    nvlist_append(SENSOR_ITEM_LIST_ID, id);

    // Important: read back the item because nvlist_append changes its next_id
    // and we don't want to overwrite this value
    nvlist_get(id, (list_item_t*)&item, sizeof(item));
  }

  memcpy(&item.info, &info, sizeof(info));
  nvlist_set(id, (list_item_t*)&item, sizeof(item));

end:
  HalUARTWrite(SERIAL_PORT, &status, sizeof(status));
}

void dump_sensors() {
  sensor_item_t sensor;
  uint16 sensor_id = nvlist_first(SENSOR_ITEM_LIST_ID, (list_item_t*)&sensor, sizeof(sensor));

  while (sensor_id != 0) {
    print_str("Sensor ID=0x"); print_num(sensor_id, 16); println();

    print_str("Address (reversed): ");
    print_bytes(sensor.info.ieee_addr, sizeof(sensor.info.ieee_addr)); println();

    print_str("Type=");
    switch (sensor.info.type) {
    case SWITCH_SENSOR:
      print_str("Switch sensor\r\n");
      break;
    case LIGHT_SENSOR:
      print_str("Light sensor\r\n");
      break;
    case LED_ACTUATOR:
      print_str("LED actuator\r\n");
      break;
    default:
      print_str("Unknown\r\n");
      break;
    }

    switch (sensor.info.datatype) {
    case BOOL:
      print_str("Data Type: Bool\r\n");
      print_str("Value: ");
      print_str(sensor.last_val.bool_val ? "True" : "False"); println();
      print_str("Threshold: ");
      print_str(sensor.info.threshold.bool_val ? "True" : "False");
      break;
    case INT:
      print_str("Data Type: Int\r\n");
      print_str("Value: ");
      print_num(sensor.last_val.int_val, 10); println();
      print_str("Threshold: ");
      print_num(sensor.info.threshold.int_val, 10);
      break;
    case FLOAT:
      print_str("Data Type: Float\r\n");
      print_str("Value: ");
      // Sorry, I don't know how to print floats. Have the binary representation instead!
      print_bytes((uint8*)&sensor.last_val.float_val, sizeof(float)); println();
      print_str("Threshold: ");
      print_bytes((uint8*)&sensor.info.threshold.float_val, sizeof(float));
      break;
    default:
      print_str("Unknown");
      break;
    }
    println();

    print_str("Threshold type: ");
    switch (sensor.info.threshold_type) {
      case NONE: print_str("None"); break;
      case EQ: print_str("=="); break;
      case GT: print_str(">"); break;
      case GTE: print_str(">="); break;
      case LT: print_str("<"); break;
      case LTE: print_str("<="); break;
      default: print_str("???"); break;
    }
    println();

    print_str("Last activity: ");
    if (sensor.last_seen_sec == 0) {
      print_str("Never\r\n");
    } else {
      print_num(osal_getClock() - sensor.last_seen_sec, 10);
      print_str(" secs ago.\r\n");
    }
    print_str("Link Quality: "); print_num(sensor.LQI, 10); println();

    print_str("Alarm: ");
    print_str(is_sensor_alarmed(&sensor) ? "Yes" : "No");
    println();

    print_str("-----------------\r\n");

    sensor_id = nvlist_get_next(sensor_id, (list_item_t*)&sensor, sizeof(sensor));
  }
}

void delete_sensor() {
  sensor_item_t item;
  uint16 id;
  ZLongAddr_t ieee_addr;
  uint8 status = ERROR_SUCCESS;
  
  if (HalUARTRead(SERIAL_PORT, (uint8*)ieee_addr, sizeof(ieee_addr)) < sizeof(ieee_addr)) {
    status = ERROR_INVALID_FORMAT;
    goto end;
  }

  id = get_sensor_item_from_ieee(ieee_addr, &item);

  if (id == 0) {
    status = ERROR_NOT_FOUND;
    goto end;
  }

  if (!nvlist_delete(SENSOR_ITEM_LIST_ID, id)) {
    status = ERROR_INTERNAL;
  }

end:
  HalUARTWrite(SERIAL_PORT, &status, sizeof(status));
}

void ping() {
  HalUARTWrite(SERIAL_PORT, "PONG!", 5);
}

void clear_all() {
  print_str("Clearing all data...\r\n");
  for (uint16 id = MAGIC_NUMBER_ID + 1; id <= 0x0FFF; id++) {
    uint16 len = osal_nv_item_len(id);
    if (len) {
      print_str("Deleted ID "); print_num(id, 16); print_str(": ");
      print_num(len, 10); print_str(" bytes\r\n");
      osal_nv_delete(id, len);
    }
  }
  init_nvram();
  print_str("done.\r\n");
}

void add_rule() {
  uint8 trigger_count;
  uint16 rule_id;
  list_item_t list_item = { 0 };
  trigger_event_t event = { 0 };
  uint8 status = ERROR_SUCCESS;

  if (HalUARTRead(SERIAL_PORT, (uint8*) event.ieee_addr_target, sizeof(ZLongAddr_t)) < sizeof(ZLongAddr_t)) {
    status = ERROR_INVALID_FORMAT;
    goto end;
  }

  if (HalUARTRead(SERIAL_PORT, &trigger_count, sizeof(trigger_count)) < sizeof(trigger_count)) {
    status = ERROR_INVALID_FORMAT;
    goto end;
  }

  if (trigger_count < 1) {
    status = ERROR_INVALID_FORMAT;
    goto end;
  }

  event.addr_list_id = nvlist_create(FREE_START_ID, FREE_ID_COUNT, &list_item, sizeof(list_item_t));

  for (int i = 0; i < trigger_count; i++) {
    addr_list_t addr_item = { 0 };

    if (HalUARTRead(SERIAL_PORT, (uint8*) addr_item.ieee_addr, sizeof(ZLongAddr_t)) < sizeof(ZLongAddr_t)) {
      status = ERROR_INVALID_FORMAT;
      goto end;
    }

    uint16 id = nvlist_create(FREE_START_ID, FREE_ID_COUNT, (list_item_t *) &addr_item, sizeof(addr_list_t));
    if (id == 0) {
      status = ERROR_INTERNAL;
      goto end;
    }

    nvlist_append(event.addr_list_id, id);
  }

  rule_id = nvlist_create(FREE_START_ID, FREE_ID_COUNT, (list_item_t*) &event, sizeof(trigger_event_t));
  nvlist_append(TRIGGER_EVENT_LIST_ID, rule_id);

end:
  HalUARTWrite(SERIAL_PORT, &status, sizeof(status));
}

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
void send_rule_list() {
  trigger_event_t rule;
  uint16 rule_id = nvlist_first(TRIGGER_EVENT_LIST_ID, (list_item_t*)&rule, sizeof(rule));
  uint8 count = nvlist_count(TRIGGER_EVENT_LIST_ID);

  HalUARTWrite(SERIAL_PORT, &count, sizeof(count));
  while (count--) {
    addr_list_t sensor_addr;

    HalUARTWrite(SERIAL_PORT, (uint8*)&rule_id, sizeof(rule_id));
    HalUARTWrite(SERIAL_PORT, (uint8*)rule.ieee_addr_target, sizeof(ZLongAddr_t));

    uint16 addr_id = nvlist_first(rule.addr_list_id, (list_item_t*)&sensor_addr, sizeof(sensor_addr));
    uint8 addr_count = nvlist_count(rule.addr_list_id);

    HalUARTWrite(SERIAL_PORT, &addr_count, sizeof(addr_count));

    while (addr_count--) {
      HalUARTWrite(SERIAL_PORT, (uint8*)sensor_addr.ieee_addr, sizeof(ZLongAddr_t));
      addr_id = nvlist_get_next(addr_id, (list_item_t*)&sensor_addr, sizeof(sensor_addr));
    }

    rule_id = nvlist_get_next(rule_id, (list_item_t*)&rule, sizeof(rule));
  }
}

/** Delete a trigger rule
  * Serial Input :
  *   - identifier of the rule
  * Serial Output :
  *   - error code (error_code_t)
  */
void delete_rule() {
  uint8 status = ERROR_SUCCESS;
  trigger_event_t rule;
  uint16 item_id;

  if (HalUARTRead(SERIAL_PORT, (uint8*)&item_id, sizeof(item_id)) < sizeof(item_id)) {
    status = ERROR_INVALID_FORMAT;
    goto end;
  }

  /* Sanity check */
  if (!nvlist_contains(TRIGGER_EVENT_LIST_ID, item_id)) {
    status = ERROR_NOT_FOUND;
    goto end;
  }

  if (!nvlist_get(item_id, (list_item_t*)&rule, sizeof(rule))) {
    status = ERROR_INTERNAL;
    goto end;
  }

  nvlist_destroy(rule.addr_list_id, false);
  nvlist_delete(TRIGGER_EVENT_LIST_ID, item_id);

end:
  HalUARTWrite(SERIAL_PORT, &status, sizeof(status));
}

bool is_sensor_alarmed(const sensor_item_t *sensor) {
  if (sensor->info.threshold_type == NONE) {
    return false;
  }

#define CHECK_THRESHOLD(a, b)              \
    switch (sensor->info.threshold_type) { \
      case EQ: return (a) == (b);          \
      case LTE: return (a) <= (b);         \
      case LT: return (a) < (b);           \
      case GTE: return (a) >= (b);         \
      case GT: return (a) > (b);           \
      default: return false;               \
    }

  switch (sensor->info.datatype) {
  case BOOL:
    return (sensor->last_val.bool_val == sensor->info.threshold.bool_val);

  case INT:
    CHECK_THRESHOLD(sensor->last_val.int_val, sensor->info.threshold.int_val);

  case FLOAT:
    CHECK_THRESHOLD(sensor->last_val.float_val, sensor->info.threshold.float_val);
  }

#undef CHECK_THRESHOLD

  return false;
}

void send_time() {
  UTCTime time = osal_getClock();
  HalUARTWrite(SERIAL_PORT, (uint8*)&time, sizeof(time));
}

void send_data() {
  ZLongAddr_t addr;
  uint8 len;
  uint8 *data = NULL;
  uint8 status = ERROR_SUCCESS;

  if (HalUARTRead(SERIAL_PORT, addr, sizeof(ZLongAddr_t)) < sizeof(ZLongAddr_t)) {
    status = ERROR_INVALID_FORMAT;
    goto end;
  }

  if (HalUARTRead(SERIAL_PORT, &len, sizeof(len)) < sizeof(len)) {
    status = ERROR_INVALID_FORMAT;
    goto end;
  }

  data = osal_mem_alloc(len);

  if (HalUARTRead(SERIAL_PORT, data, len) < len) {
    status = ERROR_INVALID_FORMAT;
    goto end;
  }

  Coordinator_SendMessage(addr, data, len);

end:
  if (data != NULL) {
    osal_mem_free(data);
  }

  HalUARTWrite(SERIAL_PORT, &status, sizeof(status));
}

void nvram_info() {
  print_str("NVRAM usage info (application space)\r\n");
  print_str("Scan takes about 8secs, be patient!\r\n");
  for (uint16 id = 0x0401; id <= 0x0FFF; id++) {
    uint16 len = osal_nv_item_len(id);
    if (len) {
      print_str("ID "); print_num(id, 16); print_str(": ");
      print_num(len, 10); print_str(" bytes\r\n");
    }
  }
  print_str("End of scan\r\n");
}
