import React from 'react';
import './App.css';
import { MapCanvas } from './components/MapCanvas';
import IChunkGeneratorSettings from './domain/models/IChunkGeneratorSettings';

const chunkGeneratorSettings: IChunkGeneratorSettings = {
  seed: 2,
  scale: 25,
  chunkSize: 32,
}

function App() {
  return (
    <div className="App" style={{ height: '100%' }}>
      <MapCanvas chunkGeneratorSettings={chunkGeneratorSettings}/>
    </div>
  );
}

export default App;
