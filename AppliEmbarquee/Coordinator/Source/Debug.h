/** Debug functions */

#ifndef DEBUG_H
#define DEBUG_H

void print_str(char *str);

void print_num(uint16 num, int radix);

void print_byte(uint8 num);

void print_bytes(uint8 *data, uint16 len);

void println();

void hexdump(uint8 *data, int len);

void nvlist_dump(uint16 list_id);

#endif // DEBUG_H