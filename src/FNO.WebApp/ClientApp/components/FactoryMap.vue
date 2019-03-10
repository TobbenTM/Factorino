<template>
  <div class="map" v-on:resize="handleResize">
    <canvas
      :width="width || calculatedWidth"
      :height="height || calculatedHeight"
      ref="canvas"
    />
  </div>
</template>

<script>
import { select } from 'd3-selection';
import { transition } from 'd3-transition';
import { interpolate } from 'd3-interpolate';
import { geoOrthographic, geoCentroid, geoPath } from 'd3-geo';
import { feature, mesh } from 'topojson-client';
import WorldMap from '@/assets/world.json';

export default {
  computed: {
  },
  props: {
    /**
     * List of locations to place on the globe
     */
    locations: {
      type: Array,
      required: true,
    },
    /**
     * Currently selected location of the available locations
     */
    selectedLocation: {
      type: Object,
      required: true,
    },
    /**
     * Set the color everything most elements will be based on <'r, g, b'>
     */
    color: {
      type: String,
      required: false,
      default: '255, 255, 255',
    },
    /**
     * If you need to set a static width, you can use this
     */
    width: {
      type: Number,
      required: false,
      default: 0,
    },
    /**
     * If you need to set a static height, you can use this
     */
    height: {
      type: Number,
      required: false,
      default: 0,
    },
  },
  data() {
    return {
      calculatedWidth: 0,
      calculatedHeight: 0,
      projection: null,
      path: null,
      ctx: null,
      mapData: null,
    };
  },
  ready: function () {
    window.addEventListener('resize', this.handleResize)
  },
  beforeDestroy: function () {
    window.removeEventListener('resize', this.handleResize)
  },
  mounted() {
    this.initialize();
    this.handleResize();
  },
  watch: {
    selectedLocation() {
      this.tween();
    },
  },
  methods: {
    handleResize() {
      this.calculatedWidth = this.width || Math.max(this.$el.clientWidth, this.$el.clientHeight);
      this.calculatedHeight = this.height || Math.max(this.$el.clientWidth, this.$el.clientHeight);

      const canvas = this.$refs.canvas;

      this.ctx = select(canvas).node().getContext('2d');

      this.projection = geoOrthographic()
            .translate([this.calculatedWidth / 2, this.calculatedHeight / 2])
            .scale(Math.min(this.calculatedHeight, this.calculatedWidth) / 2 - 20)
            .clipAngle(90)
            .precision(0.6);
      this.path = geoPath()
            .projection(this.projection)
            .context(this.ctx);

      this.$nextTick(() => {
        this.tween();
      });
    },
    initialize() {
      const factories = this.locations.map(f => ({
        id: f.seed,
        type: 'Feature',
        geometry: {
          type: 'Point',
          coordinates: [f.longitude, f.latitude],
        },
        properties: {
          name: f.name,
        },
      }));

      const land = feature(WorldMap, WorldMap.objects.land);
      const borders = mesh(WorldMap, WorldMap.objects.countries, function(a, b) { return a !== b; });

      this.mapData = { land, factories, borders };
    },
    tween() {
      const vm = this;
      const { land, factories, borders } = this.mapData;
      transition()
        .duration(1500)
        .tween("rotate", () => {
          const selected = factories.find(f => f.id === vm.selectedLocation.seed);
          const p = geoCentroid(selected);
          const r = interpolate(vm.projection.rotate(), [-p[0], -p[1]]);

          return function(t) {
            vm.projection.rotate(r(t));

            vm.ctx.clearRect(0, 0, vm.calculatedWidth, vm.calculatedHeight);

            // Fill normal land
            vm.ctx.fillStyle = `rgba(${vm.color}, .2)`, vm.ctx.beginPath(), vm.path(land), vm.ctx.fill();

            // Fill factories
            vm.ctx.fillStyle = `rgba(${vm.color}, .5)`;
            factories.forEach((f) => {
              vm.ctx.beginPath();
              vm.path(f);
              vm.ctx.fill();
            });

            // Fill selected factory
            vm.ctx.fillStyle = 'rgba(255, 5, 5, 1)', vm.ctx.beginPath(), vm.path(selected), vm.ctx.fill();

            // Stroke borders
            vm.ctx.strokeStyle = `rgba(${vm.color}, .6)`, vm.ctx.lineWidth = .5, vm.ctx.beginPath(), vm.path(borders), vm.ctx.stroke();

            // Stroke globe
            vm.ctx.strokeStyle = `rgba(${vm.color}, .05)`, vm.ctx.lineWidth = 2, vm.ctx.beginPath(), vm.path({ type: 'Sphere' }), vm.ctx.stroke();
          };
        })
        .transition();
    }
  },
};
</script>

<style lang="scss" scoped>
</style>
