import {
    Scene,
    OrthographicCamera,
    WebGLRenderer,
    Mesh,
    MeshLambertMaterial,
    BoxBufferGeometry,
    DirectionalLight,
    AxesHelper,
} from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls';
import IChunkGeneratorSettings from "./models/IChunkGeneratorSettings";
import MapGenerator from "./MapGenerator";
import Chunk from './models/Chunk';
import Tile from './models/Tile';

const materials = {
    deepWater: new MeshLambertMaterial({ color: 0x3791b5 }),
    water: new MeshLambertMaterial({ color: 0x68d3fd }),
    sand: new MeshLambertMaterial({ color: 0xfffcd4 }),
    grass: new MeshLambertMaterial({ color: 0x6bc345 }),
    dirt: new MeshLambertMaterial({ color: 0x9b7a55 }),
    concrete: new MeshLambertMaterial({ color: 0x606060 }),
    default: new MeshLambertMaterial({ color: 0x060606 }),
};

class MapRenderer {
    // The Map Generator will generate chunks for us
    mapGenerator: MapGenerator;

    // A typical three.js loop needs a:
    renderer: WebGLRenderer;
    camera: OrthographicCamera;
    scene: Scene;

    // Flag for when we want to stop animating
    isDestroyed: boolean = false;

    // The request id is useful for if we need to destroy the instance
    animationRequestId: number = 0;

    constructor(chunkGeneratorSettings: IChunkGeneratorSettings,
                context: WebGLRenderingContext,
                canvas: HTMLCanvasElement) {
        this.mapGenerator = new MapGenerator(chunkGeneratorSettings);

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
            this.mapGenerator.getChunkAt(1, 0),
            this.mapGenerator.getChunkAt(0, -1),
            this.mapGenerator.getChunkAt(-1, 0),
            this.mapGenerator.getChunkAt(1, -1),
            this.mapGenerator.getChunkAt(-1, -1),
        ];
            
        chunks.forEach(chunk => {
            const chunkMesh = this.renderChunk(chunk);
            chunkMesh.forEach(mesh => this.scene.add(mesh));
        });
    }

    onResize = (width: number, height: number) => {
        if (!this.camera || !this.renderer) throw new Error('MapRenderer not initialized yet!');

        this.renderer.setSize(width, height);
    }

    animate = () => {
        if (this.isDestroyed) return;

        // TODO: Update scene content

        // Render the context
        this.renderer.render(this.scene, this.camera);

        // Schedule a new animation frame
        // this.animationRequestId = window.requestAnimationFrame(this.animate);
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
                if ((chunk.x+chunk.y) % 2 === 0) {
                    height = Math.max(15/3, tile[1] * 15);
                }
                const mesh = new Mesh(new BoxBufferGeometry(1, height, 1), this.tileToMaterial(tile[0]));
                mesh.position.x = ((chunk.x * column.length) + x);
                mesh.position.z = -((chunk.y * column.length) + y);
                mesh.position.y = 0;
                resultMesh.push(mesh);
            })
        });

        return resultMesh;
    }
    
    tileToMaterial = (tile: Tile): MeshLambertMaterial => {
        switch (tile) {
            case Tile.deepwater:
                return materials.deepWater;
            case Tile.water:
                return materials.water;
            case Tile.sand_1:
            case Tile.sand_2:
            case Tile.sand_3:
                return materials.sand;
            case Tile.grass_1:
            case Tile.grass_2:
            case Tile.grass_3:
            case Tile.grass_4:
                return materials.grass;
            case Tile.dirt_1:
            case Tile.dirt_2:
            case Tile.dirt_3:
            case Tile.dirt_4:
            case Tile.dirt_5:
            case Tile.dirt_6:
            case Tile.dirt_7:
                return materials.dirt;
            case Tile.concrete:
                return materials.concrete;
            default:
                return materials.default;
        }
    }
}

export default MapRenderer;
