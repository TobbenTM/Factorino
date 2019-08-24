<template>
  <factorio-panel class="shipments">
    <div class="shipments__grid">
      <factorio-panel-header title="Shipments">
        <factorio-panel-action
          v-on:click="createShipment"
          :icon="['fas', 'plus']"
          class="success"
        />
        <factorio-panel-action
          v-on:click="loadShipments"
          :icon="['fas', 'sync']"
        />
      </factorio-panel-header>
      <div
        class="shipments__loading"
        v-if="loadingShipments"
        v-inlay:light
      >
        <icon :icon="['fas', 'spinner']" spin/> Loading shipments..
      </div>
      <div
        class="shipments__empty"
        v-else-if="!shipments || shipments.length == 0"
        v-inlay:light
      >
        No shipments found!
      </div>
      <div
        v-else
        v-inlay:light
        style="max-height: 100%;"
      >
        <div class="shipments__list"><table>
            <thead>
              <tr>
                <th>Destination</th>
                <th>Station</th>
                <th>Stock</th>
                <th>State</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="shipment in shipments"
                :key="shipment.shipmentId"
                v-on:click="creatingShipment = shipment"
              >
                <td>
                  {{ shipment.factory.location.name }}
                </td>
                <td>
                  {{ shipment.destinationStation }}
                </td>
                <td>
                  {{ shipment.carts.reduce((acc, cart) => acc + cart.inventory.reduce((total, stock) => total + stock.count, 0), 0) }} items
                </td>
                <td>
                  <template v-if="shipment.state === ShipmentState.Requested"><icon :icon="['fas', 'spinner']" spin/> Requested</template>
                  <template v-else-if="shipment.state === ShipmentState.Fulfilled"><icon :icon="['fas', 'check']"/> Fulfilled</template>
                  <template v-else-if="shipment.state === ShipmentState.Received"><icon :icon="['fas', 'check']"/> Received</template>
                  <template v-else-if="shipment.state === ShipmentState.Completed"><icon :icon="['fas', 'check']"/> Completed</template>
                  <template v-else><icon :icon="['fas', 'question-circle']"/> Unknown</template>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <new-shipment-dialog
      v-if="creatingShipment"
      v-on:close="creatingShipment = false"
      :copy-shipment="creatingShipment === true ? null : creatingShipment"
    />
  </factorio-panel>
</template>

<script>
import { mapState, mapActions } from 'vuex';
import { ShipmentState } from '@/enums';
import NewShipmentDialog from './NewShipmentDialog';

export default {
  components: {
    NewShipmentDialog,
  },
  computed: {
    ...mapState('shipping', [ 'shipments', 'loadingShipments' ]),
  },
  data() {
    return {
      ShipmentState,
      creatingShipment: false,
    };
  },
  created() {
    this.loadShipments();
  },
  methods: {
    ...mapActions('shipping', ['loadShipments']),
    createShipment() {
      this.creatingShipment = true;
    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/css/mixins.scss';

.shipments {

  &__grid {
    height: 100%;
    padding: 0 8px;
    display: grid;
    grid-template-columns: 1fr;
    grid-template-rows: auto 1fr;
    padding-bottom: .3em;
    box-sizing: border-box;
  }

  &__empty, &__loading {
    display: flex;
    justify-content: center;
    align-items: center;
    color: #666666;
    font-size: 2em;
    text-shadow: 2px 2px 3px rgba(0, 0, 0, 0.5);

    svg {
      margin-right: .2em;
    }
  }

  &__list {
    width: 100%;
    max-height: 100%;
    overflow-y: auto;
    position: absolute;
    box-sizing: border-box;

    table {
      width: 100%;
      text-align: center;
      border-collapse: separate;
      border-spacing: 0;

      tr {
        cursor: pointer;
      }

      thead {
        color: grey;
        background: rgba(0, 0, 0, 0.3);
      }

      td {
        padding: .2em 0;
        border-top: 1px solid $emboss_light;
        border-bottom: 2px solid $emboss_dark;
      }

      td:first-child, th:first-child {
        text-align: left;
        padding-left: .4em;
      }

      td:last-child, th:last-child {
        padding-right: .4em;
      }
    }
  }
}
</style>
