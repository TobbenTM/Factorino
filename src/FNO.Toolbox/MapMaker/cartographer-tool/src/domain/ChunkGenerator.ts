import * as openSimplexNoise from 'open-simplex-noise';
import Tile from './models/Tile';
import IChunkGeneratorSettings from './models/IChunkGeneratorSettings';
import Chunk from './models/Chunk';

// From deepest to highest
const layers: Array<Tile|Array<Tile>> = [
    Tile.deepwater,
    Tile.water,
    [
        Tile.sand_1,
        Tile.sand_2,
        Tile.sand_3,
    ],
    [
        Tile.grass_1,
        Tile.grass_2,
        Tile.grass_3,
        Tile.grass_4,
    ],
    [
        Tile.dirt_1,
        Tile.dirt_2,
        Tile.dirt_3,
        Tile.dirt_4,
        Tile.dirt_5,
        Tile.dirt_6,
        Tile.dirt_7,
    ],
    Tile.concrete,
];

function  heightToTile(h: number): Tile {
    // We can assume 0 < h < 1
    // TODO: Change sealevel?
    const layer = layers[(h * layers.length) >> 0];
    if (layer instanceof Array) {
        const permutations = layer.length;
        return layer[(Math.random() * permutations) >> 0];
    } else {
        return layer;
    }
}

class ChunkGenerator {
    private noise: openSimplexNoise.Noise2D;
    private settings: IChunkGeneratorSettings;

    constructor(settings: IChunkGeneratorSettings) {
        this.noise = openSimplexNoise.makeNoise2D(settings.seed);
        this.settings = settings;
    }

    generateChunk(x: number, y: number): Chunk {
        var tiles = this.generateChunkTiles(x, y);
        return new Chunk(x, y, tiles);
    }

    generateChunkTiles(x: number, y: number): [Tile, number][][] {
        const columns: [Tile, number][][] = [];

        for (let tileX = x * this.settings.chunkSize; tileX < (x + 1) * this.settings.chunkSize; tileX += 1) {
            const column: [Tile, number][] = [];

            for (let tileY = y * this.settings.chunkSize; tileY < (y + 1) * this.settings.chunkSize; tileY += 1) {
                const height = (this.noise(tileX / this.settings.scale, tileY / this.settings.scale) + 1) / 2;
                column.push([heightToTile(height), height]);
            }

            columns.push(column);
        }

        return columns;
    }
}

export default ChunkGenerator;
