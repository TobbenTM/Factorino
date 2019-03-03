Example Factorio dialog:

```js
let open = false;

<div>
  <factorio-button v-on:click="open = true" text="Open dialog" />
  <factorio-dialog title="Hello world!" v-if="open" v-on:close="open = false">
    This is some thoughtful content
  </factorio-dialog>
</div>
```

You can disallow closing:

```js
let open = false;

<div>
  <factorio-button v-on:click="open = true" text="Open dialog" />
  <factorio-dialog
    title="Hello world!"
    v-if="open"
    v-on:close="open = false"
    :dismissable="false"
  >
    This is some thoughtful content<br/><br/>
    <factorio-button v-on:click="open = false" text="Close me" :small="true" />
  </factorio-dialog>
</div>
```
