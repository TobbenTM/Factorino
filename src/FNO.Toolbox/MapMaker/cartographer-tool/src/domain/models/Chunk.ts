import Deed from "./Deed";
import TileType from "./TileType";
import Tile from "./Tile";

class Chunk {
    x: number;
    y: number;
    tiles: Array<Array<Tile>>;
    partOfDeed?: Deed;

    constructor(x: number, y: number, tiles: Array<Array<Tile>>) {
        this.x = x;
        this.y = y;
        this.tiles = tiles;
    }

    // Will reduce the resolution of the current chunk
    pixelate(resolution: number): Array<Array<TileType>> {
        throw new Error('Not implemented');
    }

    getId: () => string = () => {
        return `x${this.x}y${this.y}`;
    }
}

export default Chunk;
