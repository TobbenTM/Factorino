import React, { FunctionComponent } from 'react';
import IChunkGeneratorSettings from '../domain/models/IChunkGeneratorSettings';
import MapRenderer from '../domain/MapRenderer';

type MapCanvasProps = {
    chunkGeneratorSettings: IChunkGeneratorSettings,
}

export const MapCanvas: FunctionComponent<MapCanvasProps> = ({ chunkGeneratorSettings }) => {
    const canvasRef = React.useRef<HTMLCanvasElement>(null);
    const [mapRenderer, setMapRenderer] = React.useState<MapRenderer | null>(null);

    React.useEffect(() => {
        if (canvasRef.current && !mapRenderer) {
            // We haven't created the renderer yet, do it now
            const context = canvasRef.current.getContext('webgl');
            if (!context) throw new Error('Could not get webgl context from canvas!');
            const renderer = new MapRenderer(chunkGeneratorSettings, context, canvasRef.current);
            setMapRenderer(renderer);
        }

        if (canvasRef.current && mapRenderer) {
            canvasRef.current.style.width = '100%';
            canvasRef.current.style.height = '100%';
            
            canvasRef.current.width  = canvasRef.current.offsetWidth;
            canvasRef.current.height = canvasRef.current.offsetHeight;

            mapRenderer.onResize(canvasRef.current.width, canvasRef.current.height);

            mapRenderer.animate();
        }

        return () => mapRenderer?.destroy();
    }, [mapRenderer, chunkGeneratorSettings])

    return (
        <canvas ref={canvasRef} />
    );
};
