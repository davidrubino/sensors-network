#include <stddef.h>

#include "NvList.h"
#include "onboard.h"
#include "OSAL_NV.h"

#include "Debug.h"

uint16 nvlist_create(uint16 search_start_id, uint16 search_count, const list_item_t* data, int len) {
  uint16 current_id;

  if (data == NULL || len < sizeof(list_item_t)) {
    return 0;
  }

  for (current_id = search_start_id; current_id < (search_start_id + search_count); current_id++) {
    if (osal_nv_item_len(current_id) == 0) {
      break;
    }
  }

  /* We didn't find any free ID */
  if (current_id == (search_start_id + search_count)) {
    return 0;
  }

  if (osal_nv_item_init(current_id, len, (void*) data) != NV_ITEM_UNINIT) {
    return 0;
  }

  return current_id;
}

bool nvlist_set(uint16 item_id, const list_item_t* data, int len) {
  if (item_id == 0 || data == NULL || len < sizeof(list_item_t)) {
    return false;
  }
  return (osal_nv_write(item_id, 0, len, (void*) data) == SUCCESS);
}

bool nvlist_get(uint16 item_id, list_item_t* data, int len) {
  if (item_id == 0 || data == NULL || len < sizeof(list_item_t)) {
    return false;
  }
  return (osal_nv_read(item_id, 0, len, (void*) data) == SUCCESS);
}

uint16 nvlist_get_next(uint16 item_id, list_item_t *next_data, int len) {
  list_item_t curr_item;

  if (!nvlist_get(item_id, &curr_item, sizeof(curr_item))) {
    return 0;
  }

  if (curr_item.next_id != 0 && next_data != NULL && len > 0) {
    nvlist_get(curr_item.next_id, next_data, len);
  }

  return curr_item.next_id;
}

uint16 nvlist_get_prev(uint16 list_id, uint16 item_id, list_item_t *pred_data, int len) {
  uint16 current_id = nvlist_first(list_id, NULL, 0);
  bool found = false;

  while (current_id != 0 && current_id != item_id) {
    uint16 next_id = nvlist_get_next(current_id, NULL, 0);

    if (next_id == item_id) {
      found = true;
      break;
    }

    current_id = next_id;
  }

  if (!found) {
    return 0;
  }

  if (pred_data != NULL && len > 0) {
    nvlist_get(current_id, pred_data, len);
  }

  return current_id;
}

bool nvlist_append(uint16 list_id, uint16 item_id) {
  list_item_t item;
  list_head_t list;

  if (item_id == 0 || list_id == 0) {
    return false;
  }

  if (osal_nv_item_len(item_id) == 0) {
    return false;
  }

  /* Make the new item point to the first element of the list before the insertion */
  item.next_id = nvlist_first(list_id, NULL, 0);
  if (!nvlist_set(item_id, &item, sizeof(list_item_t))) {
    return false;
  }

  /* Make the first element of the list the new element */
  list.first_id = item_id;
  return nvlist_set(list_id, (list_item_t*)&list, sizeof(list_head_t));
}

bool nvlist_delete(uint16 list_id, uint16 deleted_id) {
  uint16 first_id = nvlist_first(list_id, NULL, 0);
  uint16 pred_id = nvlist_get_prev(list_id, deleted_id, NULL, 0);
  uint16 deleted_len = osal_nv_item_len(deleted_id);
  uint16 next_id = nvlist_get_next(deleted_id, NULL, 0);

  if (list_id == 0 || deleted_len == 0 || deleted_id == 0) {
    return false;
  }

  if (osal_nv_delete(deleted_id, deleted_len) != SUCCESS) {
    return false;
  }

  if (pred_id == 0 || deleted_id == first_id) {
    /* If the deleted item was the first element of the list,
     make the list start with its successor */
    osal_nv_write(list_id, 0, sizeof(uint16), &next_id);
  } else {
    /* Make the predecessor of the deleted item point to its successor */
    osal_nv_write(pred_id, 0, sizeof(uint16), &next_id);
  }

  return true;
}

bool nvlist_destroy(uint16 list_id, bool only_items) {
  uint16 next_id = nvlist_first(list_id, NULL, 0);

  while (next_id != 0) {
    uint16 len = osal_nv_item_len(next_id);
    if (len == 0) {
      break;
    }
    uint16 future_next_id = nvlist_get_next(next_id, NULL, 0);
    osal_nv_delete(next_id, len);
    next_id = future_next_id;
  }
  
  if (!only_items) {
    osal_nv_delete(list_id, sizeof(list_head_t));
  }

  return true;
}

int nvlist_count(uint16 list_id) {
  uint8 count = 0;
  uint16 curr_id = nvlist_first(list_id, NULL, 0);

  while (curr_id != 0) {
    curr_id = nvlist_get_next(curr_id, NULL, 0);
    count++;
  }

  return count;
}

void nvlist_dump(uint16 list_id) {
  list_item_t *item;
  uint16 curr_id = nvlist_first(list_id, NULL, 0);
  print_str("Dumping list ID="); print_num(list_id, 10);
  print_str(" Count="); print_num(nvlist_count(list_id), 10);
  println();

  while (curr_id != 0) {
    int len = osal_nv_item_len(curr_id);
    item = osal_mem_alloc(len);
    nvlist_get(curr_id, item, len);
    print_str("-- Item #"); print_num(curr_id, 10);
    print_str(" - Next #"); print_num(item->next_id, 10);
    println();
    hexdump((uint8*)item, len);
    curr_id = item->next_id;
    osal_mem_free(item);
  }
  print_str("end\r\n");
}

bool nvlist_contains(uint16 list_id, uint16 item_id) {
  uint16 curr_id = nvlist_first(list_id, NULL, 0);

  while (curr_id != 0) {
    if (curr_id == item_id) {
      return true;
    }
    curr_id = nvlist_get_next(curr_id, NULL, 0);
  }

  return false;
}

uint16 nvlist_first(uint16 list_id, list_item_t *data, int len) {
  list_head_t list;
  if (!nvlist_get(list_id, (list_item_t*)&list, sizeof(list_head_t))) {
    return 0;
  }
  
  if (list.first_id == 0) {
    return 0;
  }

  if (data != NULL && len != 0) {
    nvlist_get(list.first_id, data, len);
  }

  return list.first_id;
}