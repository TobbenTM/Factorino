<template>
  <div class="map">
    <canvas
      :width="width"
      :height="width"
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
import WorldMap from 'assets/world.json';
import FactorinoSpinner from 'components/FactorinoSpinner.vue';

export default {
  name: 'factory-map',
  components: {
    FactorinoSpinner,
  },
  props: {
    locations: {
      type: Array,
      required: true,
    },
    selectedLocation: {
      type: Object,
      required: true,
    },
    width: {
      type: Number,
      required: false,
      default: 720,
    },
  },
  data() {
    return {
    };
  },
  mounted() {
    this.initialize();
  },
  methods: {
    initialize() {
      const vm = this;

      // Based on the excellent example here: https://bl.ocks.org/mbostock/4183330
      const canvas = this.$refs.canvas;
      const ctx = select(canvas).node().getContext('2d');
      const projection = geoOrthographic()
            .translate([this.width / 2, this.width / 2])
            .scale(this.width / 2 - 20)
            .clipAngle(90)
            .precision(0.6);
      const path = geoPath()
            .projection(projection)
            .context(ctx);

      const globe = {type: "Sphere"};
      const land = feature(WorldMap, WorldMap.objects.land);
      const countries = feature(WorldMap, WorldMap.objects.countries).features;
      const borders = mesh(WorldMap, WorldMap.objects.countries, function(a, b) { return a !== b; });
      const i = 0;

      transition()
        .duration(1500)
        .tween("rotate", () => {
          var p = geoCentroid(countries[i]),
              r = interpolate(projection.rotate(), [-p[0], -p[1]]);
          return function(t) {
            projection.rotate(r(t));

            ctx.clearRect(0, 0, vm.width, vm.width);

            // Fill normal land
            ctx.fillStyle = "rgba(230, 230, 255, .2)", ctx.beginPath(), path(land), ctx.fill();
            // Fill selected land
            ctx.fillStyle = "rgba(255, 5, 5, .5)", ctx.beginPath(), path(countries[i]), ctx.fill();
            // Stroke borders
            ctx.strokeStyle = "rgba(255, 255, 255, .6)", ctx.lineWidth = .5, ctx.beginPath(), path(borders), ctx.stroke();
            // Stroke globe
            ctx.strokeStyle = "rgba(230, 230, 255, .05)", ctx.lineWidth = 2, ctx.beginPath(), path(globe), ctx.stroke();
          };
        })
        .transition();
    },
  },
};
</script>

<style lang="scss" scoped>
</style>
