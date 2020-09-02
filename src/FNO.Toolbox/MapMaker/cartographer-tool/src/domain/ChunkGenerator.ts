import * as openSimplexNoise from 'open-simplex-noise';
import TileType from './models/TileType';
import IChunkGeneratorSettings from './models/IChunkGeneratorSettings';
import Chunk from './models/Chunk';
import Tile from './models/Tile';

// From deepest to highest
const layers: Array<TileType|Array<TileType>> = [
    TileType.deepwater,
    TileType.deepwater,
    TileType.water,
    [
        TileType.sand_1,
        TileType.sand_2,
        TileType.sand_3,
    ],
    [
        TileType.grass_1,
        TileType.grass_2,
    ],
    [
        TileType.grass_3,
        TileType.grass_4,
    ],
    [
        TileType.dirt_1,
        TileType.dirt_2,
        TileType.dirt_3,
        TileType.dirt_4,
        TileType.dirt_5,
        TileType.dirt_6,
        TileType.dirt_7,
    ],
    TileType.concrete,
    TileType.concrete,
];

function  heightToTile(h: number): TileType {
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

    generateChunkTiles(x: number, y: number): Tile[][] {
        const columns: Tile[][] = [];

        for (let tileX = x * this.settings.chunkSize; tileX < (x + 1) * this.settings.chunkSize; tileX += 1) {
            const column: Tile[] = [];

            for (let tileY = y * this.settings.chunkSize; tileY < (y + 1) * this.settings.chunkSize; tileY += 1) {
                const height = (this.noise(tileX / this.settings.scale, tileY / this.settings.scale) + 1) / 2;
                column.push(new Tile(heightToTile(height), height));
            }

            columns.push(column);
        }

        return columns;
    }
}

export default ChunkGenerator;
