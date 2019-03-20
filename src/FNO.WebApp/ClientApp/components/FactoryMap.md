Example map using production locations:

```js
const locations = require('@/styleguide/factory-locations.js').default;
const selected = locations[locations.length-1];
<factory-map
  :locations="locations"
  :selectedLocation="selected"
  :color="'0, 0, 0'"
  :height="256"
  :width="256"
/>
```

Or in a panel:

```js
const locations = require('@/styleguide/factory-locations.js').default;
const selected = locations[locations.length-1];
<factorio-panel>
  <factory-map
    :locations="locations"
    :selectedLocation="selected"
    :color="'0, 0, 0'"
    :height="256"
    :width="256"
  />
</factorio-panel>
```
