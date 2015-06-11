#include <string.h>

#include "ZComDef.h"
#include "hal_types.h"
#include "hal_uart.h"
#include "onboard.h"
#include "osal_nv.h"

#include "NvList.h"
#include "Debug.h"

void print_str(char *str) {
  uint16 len = strlen(str);
  uint16 i = 0;
  while (i < len) {
    i += HalUARTWrite(0, (uint8*)&str[i], len - i);
  }
}

void print_num(uint16 num, int radix) {
  char buffer[16];
  _itoa(num, (uint8*)buffer, radix);
  print_str(buffer);
}

void print_byte(uint8 num) {
  char buffer[2];
  _itoa(num, (uint8*)buffer, 16);
  if (num < 0x10) {
    print_str("0");
  }
  print_str(buffer);
}

void print_bytes(uint8 *data, uint16 len) {
  for (int i = 0; i < len; i++) {
    print_byte(data[i]);
    print_str(" ");
  }
}

void println() {
  print_str("\r\n");
}

void hexdump(uint8 *data, int len) {
  for (int i = 0; i < len; i += 16) {
    if (i > 0) println();
    print_byte(i);
    print_str(": ");
    print_bytes(&data[i], MIN(16, len - i));
  }
  println();
}
