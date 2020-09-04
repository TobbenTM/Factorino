import { Mesh, Object3D, Geometry, Vector3, Face3, MeshBasicMaterial, Matrix4, Color } from 'three';
import Chunk from './Chunk';
import TileType from './TileType';

const colors = {
    deepWater: new Color(0x3791b5),
    water: new Color(0x68d3fd),
    sand: new Color(0xfffcd4),
    grass: new Color(0x6bc345),
    dirt: new Color(0x9b7a55),
    concrete: new Color(0x606060),
};

class RenderedChunk {
    chunk: Chunk;
    object: Object3D;

    constructor(chunk: Chunk) {
        this.chunk = chunk;
        this.object = new Object3D();

        const meshes = this.render(chunk);
        meshes.forEach(mesh => {
            this.object.add(mesh);
        });
    }

    render = (chunk: Chunk): Array<Mesh> => {
        const geometry = new Geometry();

        chunk.tiles.forEach((column, x) => {
            column.forEach((tile, y) => {
                const height = Math.max(15/3, tile.height * 15) / 2;
                
                // Add surface
                const offset = geometry.vertices.length;
                geometry.vertices.push(
                    new Vector3(x, height, y),      // 0 - Surface - Top Left
                    new Vector3(x+1, height, y),    // 1 - Surface - Top Right
                    new Vector3(x, height, y+1),    // 2 - Surface - Bottom Left
                    new Vector3(x+1, height, y+1),  // 3 - Surface - Bottom Right
                );
                const surfaceFaces = geometry.faces.push(
                    new Face3(offset+0, offset+2, offset+1),
                    new Face3(offset+1, offset+2, offset+3),
                );
                geometry.faces[surfaceFaces-1].color = this.tileToColor(tile.type);
                geometry.faces[surfaceFaces-2].color = this.tileToColor(tile.type);

                if (x === chunk.tiles.length - 1)
                {
                    // Add right wall
                    const rightWall = geometry.vertices.push(
                        new Vector3(x+1, 0, y),       // -2 - Floor - Top Right
                        new Vector3(x+1, 0, y+1),     // -1 - Floor - Bottom Right
                    );
                    const faces = geometry.faces.push(
                        new Face3(offset+1, offset+3, rightWall-2),
                        new Face3(offset+3, rightWall-1, rightWall-2),
                    );
                    geometry.faces[faces-1].color = colors.concrete;
                    geometry.faces[faces-2].color = colors.concrete;
                }
                else if (chunk.tiles[x + 1][y].height < tile.height)
                {
                    // Add right wall
                    const neighborHeight = Math.max(15/3, chunk.tiles[x + 1][y].height * 15) / 2;
                    const rightWall = geometry.vertices.push(
                        new Vector3(x+1, neighborHeight, y),       // -2 - Floor - Top Right
                        new Vector3(x+1, neighborHeight, y+1),     // -1 - Floor - Bottom Right
                    );
                    const faces = geometry.faces.push(
                        new Face3(offset+1, offset+3, rightWall-2),
                        new Face3(offset+3, rightWall-1, rightWall-2),
                    );
                    geometry.faces[faces-1].color = colors.concrete;
                    geometry.faces[faces-2].color = colors.concrete;
                }

                if (y === chunk.tiles[0].length - 1)
                {
                    // Add bottom wall
                    const bottomWall = geometry.vertices.push(
                        new Vector3(x, 0, y+1),       // -2 - Floor - Bottom Left
                        new Vector3(x+1, 0, y+1),     // -1 - Floor - Bottom Right
                    );
                    const faces = geometry.faces.push(
                        new Face3(offset+2, bottomWall-2, offset+3),
                        new Face3(offset+3, bottomWall-2, bottomWall-1),
                    );
                    geometry.faces[faces-1].color = colors.concrete;
                    geometry.faces[faces-2].color = colors.concrete;
                }
                else if (chunk.tiles[x][y + 1].height < tile.height)
                {
                    // Add bottom wall
                    const neighborHeight = Math.max(15/3, chunk.tiles[x][y + 1].height * 15) / 2;
                    const bottomWall = geometry.vertices.push(
                        new Vector3(x, neighborHeight, y+1),       // -2 - Floor - Bottom Left
                        new Vector3(x+1, neighborHeight, y+1),     // -1 - Floor - Bottom Right
                    );
                    const faces = geometry.faces.push(
                        new Face3(offset+2, bottomWall-2, offset+3),
                        new Face3(offset+3, bottomWall-2, bottomWall-1),
                    );
                    geometry.faces[faces-1].color = colors.concrete;
                    geometry.faces[faces-2].color = colors.concrete;
                }
            })
        });

        geometry.applyMatrix4(new Matrix4().makeTranslation(chunk.x * chunk.tiles.length, 0, chunk.y * chunk.tiles[0].length));
        // return [new Mesh(geometry, new MeshBasicMaterial( { color: 0xffffff, wireframe: true } ))];
        return [new Mesh(geometry, new MeshBasicMaterial( { vertexColors: true } ))];
    }

    tileToColor = (tile: TileType): Color => {
        switch (tile) {
            case TileType.deepwater:
                return colors.deepWater;
            case TileType.water:
                return colors.water;
            case TileType.sand_1:
            case TileType.sand_2:
            case TileType.sand_3:
                return colors.sand;
            case TileType.grass_1:
            case TileType.grass_2:
            case TileType.grass_3:
            case TileType.grass_4:
                return colors.grass;
            case TileType.dirt_1:
            case TileType.dirt_2:
            case TileType.dirt_3:
            case TileType.dirt_4:
            case TileType.dirt_5:
            case TileType.dirt_6:
            case TileType.dirt_7:
                return colors.dirt;
            case TileType.concrete:
                return colors.concrete;
            default:
                return colors.concrete;
        }
    }
}

export default RenderedChunk;
