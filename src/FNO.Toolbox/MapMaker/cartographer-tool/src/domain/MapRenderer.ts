import {
    Scene,
    OrthographicCamera,
    WebGLRenderer,
    Mesh,
    MeshLambertMaterial,
    BoxBufferGeometry,
    DirectionalLight,
    AxesHelper,
    BufferGeometry,
    Matrix4,
} from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls';
import { BufferGeometryUtils } from 'three/examples/jsm/utils/BufferGeometryUtils';
import IChunkGeneratorSettings from "./models/IChunkGeneratorSettings";
import MapGenerator from "./MapGenerator";
import Chunk from './models/Chunk';
import TileType from './models/TileType';
import Stats from 'stats.js';

const materials = {
    deepWater: new MeshLambertMaterial({ color: 0x3791b5 }),
    water: new MeshLambertMaterial({ color: 0x68d3fd }),
    sand: new MeshLambertMaterial({ color: 0xfffcd4 }),
    grass: new MeshLambertMaterial({ color: 0x6bc345 }),
    dirt: new MeshLambertMaterial({ color: 0x9b7a55 }),
    concrete: new MeshLambertMaterial({ color: 0x606060 }),
    default: new MeshLambertMaterial({ color: 0x060606 }),
};

const baseTileGeometry = new BoxBufferGeometry(1, 1, 1);

class MapRenderer {
    // The Map Generator will generate chunks for us
    mapGenerator: MapGenerator;

    // A typical three.js loop needs a:
    renderer: WebGLRenderer;
    camera: OrthographicCamera;
    scene: Scene;

    // And a way to keep render stats
    stats: Stats;

    // Flag for when we want to stop animating
    isDestroyed: boolean = false;

    // The request id is useful for if we need to destroy the instance
    animationRequestId: number = 0;

    constructor(chunkGeneratorSettings: IChunkGeneratorSettings,
                context: WebGLRenderingContext,
                canvas: HTMLCanvasElement) {
        this.mapGenerator = new MapGenerator(chunkGeneratorSettings);

        // Stats init
        this.stats = new Stats();
        this.stats.showPanel(1);
        document.body.appendChild(this.stats.dom);

        this.renderer = new WebGLRenderer({
            context: context,
            alpha: false,
        });
        this.renderer.setSize(canvas.width, canvas.height);
        this.renderer.setClearColor(0x000000, 1);

        this.scene = new Scene();
        
        const aspectRatio = canvas.width / canvas.height;
        const viewSize = 50;
        // this.camera = new PerspectiveCamera(90, width / height, 1, 10_000);
        this.camera = new OrthographicCamera(aspectRatio * (-viewSize), aspectRatio * viewSize, viewSize, - viewSize / 2, -1000, 2000); // width / - 10, width / 10, height / 10, height / - 10
        this.camera.position.set(50, 25, 50);
        this.camera.lookAt(this.scene.position);

        const light = new DirectionalLight(0xffffff, .5);
        light.position.set(0, 1, 0);
        this.scene.add(light);

        this.scene.add(new AxesHelper(3000));

        // const gridGeometry = new PlaneBufferGeometry( 1000, 1000, 100, 100 );
        // const gridMaterial = new MeshBasicMaterial( { wireframe: true, opacity: 0.5, transparent: true } );
        // const grid = new Mesh( gridGeometry, gridMaterial );
        // this.scene.add( grid );

        const controls = new OrbitControls(this.camera, canvas);
        controls.addEventListener('change', () => this.renderer.render(this.scene, this.camera));
        controls.enableZoom = true;
        controls.enablePan = true;

        // TODO: Determine which chunks to generate
        const chunks = [
            this.mapGenerator.getChunkAt(0, 0),
            this.mapGenerator.getChunkAt(-1, 0),
            this.mapGenerator.getChunkAt(0, -1),
            this.mapGenerator.getChunkAt(-1, -1),
        ];

        chunks.forEach(chunk => {
            const chunkMesh = this.renderBufferedChunk(chunk);
            chunkMesh.forEach(mesh => this.scene.add(mesh));
        });
    }

    onResize = (width: number, height: number) => {
        if (!this.camera || !this.renderer) throw new Error('MapRenderer not initialized yet!');

        this.renderer.setSize(width, height);
    }

    animate = () => {
        if (this.isDestroyed) return;

        this.stats.begin();

        // TODO: Update scene content

        // Render the context
        this.renderer.render(this.scene, this.camera);

        this.stats.end();

        // Schedule a new animation frame
        this.animationRequestId = window.requestAnimationFrame(this.animate);
    }

    destroy = () => {
        this.isDestroyed = true;
        if (this.animationRequestId) {
            window.cancelAnimationFrame(this.animationRequestId);
        }
    }

    renderChunk = (chunk: Chunk): Array<Mesh> => {
        const resultMesh: Array<Mesh> = [];

        chunk.tiles.forEach((column, x) => {
            column.forEach((tile, y) => {
                let height = 1;
                if (true || (chunk.x+chunk.y) % 2 === 0) {
                    height = Math.max(15/3, tile.height * 15);
                }
                const geometry = baseTileGeometry.clone();
                const mesh = new Mesh(geometry, this.tileToMaterial(tile.type));
                mesh.position.x = ((chunk.x * column.length) + x);
                mesh.position.z = -((chunk.y * column.length) + y);
                mesh.position.y = height/2;
                resultMesh.push(mesh);
            })
        });

        return resultMesh;
    }

    renderBufferedChunk = (chunk: Chunk): Array<Mesh> => {
        const tileGeometry: Array<BufferGeometry> = [];

        chunk.tiles.forEach((column, x) => {
            column.forEach((tile, y) => {
                let height = 1;
                if (true || (chunk.x+chunk.y) % 2 === 0) {
                    height = Math.max(15/3, tile.height * 15);
                }
                const geometry = baseTileGeometry.clone();
                const xTranslate = ((chunk.x * column.length) + x);
                const zTranslate = -((chunk.y * column.length) + y);
                const yTranslate = height/2;
                geometry.applyMatrix4(new Matrix4().makeTranslation(xTranslate, yTranslate, zTranslate))
                tileGeometry.push(geometry);
            })
        });

        const resultGeometry = BufferGeometryUtils.mergeBufferGeometries(tileGeometry);
        return [new Mesh(resultGeometry, materials.grass)];
    }
    
    tileToMaterial = (tile: TileType): MeshLambertMaterial => {
        switch (tile) {
            case TileType.deepwater:
                return materials.deepWater;
            case TileType.water:
                return materials.water;
            case TileType.sand_1:
            case TileType.sand_2:
            case TileType.sand_3:
                return materials.sand;
            case TileType.grass_1:
            case TileType.grass_2:
            case TileType.grass_3:
            case TileType.grass_4:
                return materials.grass;
            case TileType.dirt_1:
            case TileType.dirt_2:
            case TileType.dirt_3:
            case TileType.dirt_4:
            case TileType.dirt_5:
            case TileType.dirt_6:
            case TileType.dirt_7:
                return materials.dirt;
            case TileType.concrete:
                return materials.concrete;
            default:
                return materials.default;
        }
    }
}

export default MapRenderer;
