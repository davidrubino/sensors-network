#ifndef COMMON_H
#define COMMON_H

/** Sensor (or actuator) type */
enum sensor_type {
  SWITCH_SENSOR = 0,
  LIGHT_SENSOR = 1,
  LED_ACTUATOR = 2
};
typedef uint8 sensor_type_t;

enum sensor_datatype {
  BOOL = 0,
  INT = 1,
  FLOAT = 2
};
typedef uint8 sensor_datatype_t;

/** Union for storing the current value or threshold of a sensor/actuator */
typedef union {
  int int_val;
  float float_val;
  bool bool_val;
} sensor_val_t; //4 bytes

/** Data packet sent from a sensor/actuator to the coordinator to report its current value */
__packed typedef struct {
  sensor_val_t val;
  sensor_type_t type;
  sensor_datatype_t datatype;
} sensor_state_t;

#endif