import Chunk from "./models/Chunk";
import ChunkGenerator from "./ChunkGenerator";
import IChunkGeneratorSettings from "./models/IChunkGeneratorSettings";

class MapGenerator {
    chunks: Record<string, Chunk> = {};
    chunkGenerator: ChunkGenerator;

    constructor(chunkGeneratorSettings: IChunkGeneratorSettings) {
        this.chunkGenerator = new ChunkGenerator(chunkGeneratorSettings);
    }

    coordinateToId(x: number, y: number): string {
        return `x${x}y${y}`;
    }

    getChunkAt(x: number, y: number): Chunk {
        let chunk = this.chunks[this.coordinateToId(x, y)];
        if (chunk) return chunk;
        chunk = this.chunkGenerator.generateChunk(x, y);
        this.chunks[this.coordinateToId(x, y)] = chunk;
        return chunk;
    }
}

export default MapGenerator;
