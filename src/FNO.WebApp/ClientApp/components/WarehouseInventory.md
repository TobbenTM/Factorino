Example inventory:

```js
const inventory = [
  {
    "warehouseInventoryId": "42a78969-f14b-4332-92fa-08bb5cd5a4fc","quantity": 420,
    "itemId": "iron-plate",
    "item": {
      "name": "iron-plate",
      "type": "item",
      "icon": "graphics/icons/iron-plate.png",
      "stackSize": 100,
      "subgroup": "raw-material",
      "fluid": false
    },
    "ownerId":"e54ebba2-6391-4539-9085-53fed6ae7394"
  }
];
<warehouse-inventory
  :inventory="inventory"
/>
```
