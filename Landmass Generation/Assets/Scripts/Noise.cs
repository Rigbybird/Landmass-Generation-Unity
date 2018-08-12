using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{

    public static float[,] GenerateNoiseMap(int MapWidth, int MapHeight, int Seed, float Scale, int Octaves, float Persistance, float Lacunarity, Vector2 Offset)
    {
        float[,] NoiseMap = new float[MapWidth, MapHeight];

        System.Random prng = new System.Random(Seed);
        Vector2[] OctaveOffsets = new Vector2[Octaves];
        for (int i = 0; i < Octaves; i++)
        {
            float OffsetX = prng.Next(-100000, 100000) + Offset.x;
            float OffsetY = prng.Next(-100000, 100000) + Offset.y;
            OctaveOffsets[i] = new Vector2(OffsetX, OffsetY);
        }

        if (Scale <= 0)
        {
            Scale = 0.0001f;
        }

        float MaxNoiseHeight = float.MinValue;
        float MinNoiseHeight = float.MaxValue;

        float HalfWidth = MapWidth / 2f;
        float HalfHeight = MapHeight / 2f;

        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                float Amplitude = 1;
                float Frequency = 1;
                float NoiseHeight = 0;

                for (int i = 0; i < Octaves; i++)
                {
                    float SampleX = (x-HalfWidth) / Scale * Frequency + OctaveOffsets[i].x;
                    float SampleY = (y-HalfHeight) / Scale * Frequency + OctaveOffsets[i].y;

                    float PerlinValue = Mathf.PerlinNoise(SampleX, SampleY) * 2 - 1;
                    NoiseHeight += PerlinValue * Amplitude;

                    Amplitude *= Persistance;
                    Frequency *= Lacunarity;
                }

                if (NoiseHeight > MaxNoiseHeight)
                {
                    MaxNoiseHeight = NoiseHeight;
                }
                else if (NoiseHeight < MinNoiseHeight)
                {
                    MinNoiseHeight = NoiseHeight;
                }
                NoiseMap[x, y] = NoiseHeight;
            }
        }

        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                NoiseMap[x, y] = Mathf.InverseLerp(MinNoiseHeight, MaxNoiseHeight, NoiseMap[x, y]);
            }
        }


        return NoiseMap;
    }
}