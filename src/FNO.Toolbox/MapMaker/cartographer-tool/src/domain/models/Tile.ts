import TileType from "./TileType";

class Tile {
    type: TileType;
    height: number;

    constructor(type: TileType, height: number) {
        this.type = type;
        this.height = height;
    }
}

export default Tile;
