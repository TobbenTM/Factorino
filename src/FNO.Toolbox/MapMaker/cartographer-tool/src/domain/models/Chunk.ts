import Deed from "./Deed";
import Tile from "./Tile";

class Chunk {
    x: number;
    y: number;
    tiles: Array<Array<[Tile, number]>>;
    partOfDeed?: Deed;

    constructor(x: number, y: number, tiles: Array<Array<[Tile, number]>>) {
        this.x = x;
        this.y = y;
        this.tiles = tiles;
    }

    // Will reduce the resolution of the current chunk
    pixelate(resolution: number): Array<Array<Tile>> {
        throw new Error('Not implemented');
    }

    getId: () => string = () => {
        return `x${this.x}y${this.y}`;
    }
}

export default Chunk;
