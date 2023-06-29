using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerater : MonoBehaviour
{
    public static float[,] GenerateChunk(Vector2 ChunkPosition,int ChunkSize,int Seed)
    {
        NoiseSettings noiseSettings = new NoiseSettings();
        noiseSettings.normalizeMode = Noise.NormalizeMode.Global;
        noiseSettings.lacunarity = 2;
        noiseSettings.offset = ChunkPosition * ChunkSize;
        noiseSettings.scale = 1;
        noiseSettings.persistance = 0.5f;
        noiseSettings.octaves = 2;
        noiseSettings.seed = Seed;

        float[,] Chunk = Noise.GenerateNoiseMap(32, 32, noiseSettings);
        return Chunk;
    }
}
