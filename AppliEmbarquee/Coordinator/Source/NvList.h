#ifndef NV_LIST_H
#define NV_LIST_H

#include "ZComDef.h"

/* Linked list implementation using NVRAM items for storage
 * Internal structure :
 *  - Elements and lists are represented with their NVRAM ID
 *  - A list consists of a list_t struct which contains the ID of the first element of the list
 *    or 0 in case the list is empty
 *  - A list element is a list_item_t structure (with variable size) which contains data and
 *    a pointer to the ID of the next element (or 0 if it's the last item in the list)
*/

/** Base list item structure */

/* List layout in NVRAM */
typedef struct {
  uint16 first_id;
} list_head_t;

/* Item layout in NVRAM */
typedef struct {
  uint16 next_id;
  uint8 data[0];
} list_item_t;


/** Create a new item in NVRAM by scanning the specified ID range for an unused ID
  * @param search_start_id first ID to scan for
  * @param search_count number of IDs to try before giving up
  * @param data pointer to initial data
  * @param len length of the list_item_t structure
  * @return the new item ID, or 0 if no free item ID was found
  */
uint16 nvlist_create(uint16 search_start_id, uint16 search_count, const list_item_t* data, int len);


/** Write data to a list item
  * @param item_id item ID to write data into
  * @param data pointer to the data to write into the NVRAM
  * @param len length of the list_item_t structure
  * @return true if the item was found, false otherwise
  */
bool nvlist_set(uint16 item_id, const list_item_t* data, int len);


/** Read data from a list item
  * @param item_id item ID to write data into
  * @param data pointer to write the data from the NVRAM into
  * @param len length of the list_item_t structure
  * @return true if the item was found, false otherwise
  */
bool nvlist_get(uint16 item_id, list_item_t* data, int len);


/** Read the successor of an item
  * @param item_id item ID for which we're looking for a successor
  * @param next_data pointer to store the next item data into
  *                  (can be NULL if only interested in the ID)
  * @param len length of the list_item_t structure
  * @return ID of the next element
  */
uint16 nvlist_get_next(uint16 item_id, list_item_t *next_data, int len);


/** Read the predecessor of an item in the list
  * @param list_id ID of the the list
  * @param item_id ID of the item for which we're looking for a predecessor
  * @param pred_data pointer to store the predecessor item data into
  *                  (can be NULL if only interested in the ID)
  * @param len length of the list_item_t structure
  * @return ID of the predecessor, 0 if not found
  */
uint16 nvlist_get_prev(uint16 list_id, uint16 item_id, list_item_t *pred_data, int len);


/** Append an element to the beginning of the list
  * @param list_id ID of the the list
  * @param item_id ID of the item to append
  * @return true if the element was successfully added, false otherwise
  * @note this changes the content of the item, so make sure not to override it with the previous
  *       value after calling this function.
  */
bool nvlist_append(uint16 list_id, uint16 item_id);


/** Delete an element from a list
  * @param list_id ID of the the list
  * @param deleted_id ID of the item to delete
  * @return true if the element was successfully deleted, false otherwise
  */
bool nvlist_delete(uint16 list_id, uint16 deleted_id);


/** Destroy a list, deleting all its items (and optionally the list itself)
  * @param list_id ID of the the list
  * @param only_items true to delete only the items and not the list itself
  * @return true if all items were successfully destroyed, false otherwise
  */
bool nvlist_destroy(uint16 list_id, bool only_items);


/** Count the number of items in a list
  * @param list_id ID of the list
  * @return the number of items in the list
  */
int nvlist_count(uint16 list_id);

/** Dump the list to serial port for debugging purposes
  * @param list_id ID of the list
  */
void nvlist_dump(uint16 list_id);


/** Check if the list contains the specified item
  * @param list_id ID of the list
  * @param item_id item ID to search for
  */
bool nvlist_contains(uint16 list_id, uint16 item_id);


/** Get the ID of the first element of a list
  * @param list_id ID of the list
  */
uint16 nvlist_first(uint16 list_id, list_item_t *data, int len);

#endif