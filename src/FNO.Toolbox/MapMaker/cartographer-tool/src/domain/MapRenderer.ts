import {
    Scene,
    OrthographicCamera,
    WebGLRenderer,
    AxesHelper,
    Vector3,
} from 'three';
import TWEEN from '@tweenjs/tween.js';
import IChunkGeneratorSettings from "./models/IChunkGeneratorSettings";
import MapGenerator from "./MapGenerator";
import RenderedChunk from './models/RenderedChunk';
import Stats from 'stats.js';
import Chunk from './models/Chunk';

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
        const viewSize = 100;
        this.camera = new OrthographicCamera(aspectRatio * (-viewSize), aspectRatio * viewSize, viewSize, - viewSize, -300, 300);
        this.camera.position.set(1, 1, 1);
        this.camera.lookAt(this.scene.position);

        this.scene.add(new AxesHelper(3000));

        // const gridGeometry = new PlaneBufferGeometry( 1000, 1000, 100, 100 );
        // const gridMaterial = new MeshBasicMaterial( { wireframe: true, opacity: 0.5, transparent: true } );
        // const grid = new Mesh( gridGeometry, gridMaterial );
        // this.scene.add( grid );

        // const controls = new OrbitControls(this.camera, canvas);
        // // controls.addEventListener('change', () => this.renderer.render(this.scene, this.camera));
        // // controls.enableDamping = true;
        // controls.enableZoom = true;
        // controls.enablePan = true;

        // TODO: Determine which chunks to generate
        const chunks = [
            this.mapGenerator.getChunkAt(0, 0),
            this.mapGenerator.getChunkAt(1, 0),
            this.mapGenerator.getChunkAt(0, 1),
            this.mapGenerator.getChunkAt(1, 1),
            this.mapGenerator.getChunkAt(-1, 0),
            this.mapGenerator.getChunkAt(0, -1),
            this.mapGenerator.getChunkAt(-1, -1),
            this.mapGenerator.getChunkAt(1, -1),
            this.mapGenerator.getChunkAt(-1, 1),
            this.mapGenerator.getChunkAt(2, 0),
            this.mapGenerator.getChunkAt(0, 2),
            this.mapGenerator.getChunkAt(2, 2),
            this.mapGenerator.getChunkAt(-2, 0),
            this.mapGenerator.getChunkAt(0, -2),
            this.mapGenerator.getChunkAt(-2, -2),
            this.mapGenerator.getChunkAt(2, -2),
            this.mapGenerator.getChunkAt(-2, 2),
        ];

        chunks.forEach(chunk => {
            const renderedChunk = new RenderedChunk(chunk);
            this.scene.add(renderedChunk.object);
        });

        setInterval(() => {
            this.focusOn(chunks[Math.random() * chunks.length >> 0]);
        }, 5_000);
    }

    onResize = (width: number, height: number) => {
        if (!this.camera || !this.renderer) throw new Error('MapRenderer not initialized yet!');

        this.renderer.setSize(width, height);
    }

    animate = (time: number) => {
        if (this.isDestroyed) return;
        this.stats.begin();

        TWEEN.update(time);

        // TODO: Update scene content

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

    focusOn = (chunk: Chunk) => {
        const position = this.camera.position.clone();
        const toPos = new Vector3((chunk.x+0.5)*chunk.tiles.length, position.y, (chunk.y+0.5)*chunk.tiles[0].length);
        
        console.log(`Focusing on chunk ${chunk.getId()}`, toPos);

        const tween = new TWEEN.Tween(position as any)
            .to(toPos as any, 2000)
            .easing(TWEEN.Easing.Cubic.InOut)
            .onUpdate(() => {
                this.camera.position.copy(position);
            })
            .onComplete(() => {
                console.log('Completed camera position..', position);
                this.camera.position.copy(toPos);
            });
        tween.start(TWEEN.now());
    }
}

export default MapRenderer;
