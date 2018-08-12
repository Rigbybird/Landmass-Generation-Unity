using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public enum DrawnMode { NoiseMap, ColourMap, Mesh};
    public DrawnMode drawMode;

    public const int MapChunkSize = 241; //Is actually 240 Chunks
    [Range(0, 6)]
    public int LevelOfDetail;
    public float NoiseScale;

    public int Octaves;
    [Range(0, 1)]
    public float Persistance;
    public float Lacunarity;

    public int Seed;
    public Vector2 Offset;

    public float MeshHeightMultiplier;
    public AnimationCurve MeshHeightCurve;

    public bool AutoUpdate = false;

    public TerrainType[] Regions;

    public void GenerateMap()
    {
        float[,] NoiseMap = Noise.GenerateNoiseMap(MapChunkSize, MapChunkSize, Seed, NoiseScale, Octaves, Persistance, Lacunarity, Offset);

        Color[] ColourMap = new Color[MapChunkSize * MapChunkSize];
        for (int y = 0; y < MapChunkSize; y++)
        {
            for (int x = 0; x < MapChunkSize; x++)
            {
                float CurrentHeight = NoiseMap[x, y];
                for (int i = 0; i < Regions.Length; i++)
                {
                    if (CurrentHeight <= Regions[i].Height )
                    {
                        ColourMap[y * MapChunkSize + x] = Regions[i].Colour;
                        break;
                    }
                }
            }
        }

        MapDisplay Display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawnMode.NoiseMap)
        {
            Display.DrawTexture(TextureGenerator.TextureFromHeightMap(NoiseMap));
        } else if (drawMode == DrawnMode.ColourMap)
        {
            Display.DrawTexture(TextureGenerator.TextureFromColourMap(ColourMap, MapChunkSize, MapChunkSize));
        } else if (drawMode == DrawnMode.Mesh)
        {
            Display.DrawMesh(MeshGenerator.GenerateTerrainMesh(NoiseMap, MeshHeightMultiplier, MeshHeightCurve, LevelOfDetail), TextureGenerator.TextureFromColourMap(ColourMap, MapChunkSize, MapChunkSize));
        }
    }

    void OnValidate()
    {
        if (Lacunarity < 1)
        {
            Lacunarity = 1;
        }

        if (Octaves < 0)
        {
            Octaves = 0;
        }
	}
}

[System.Serializable]
public struct TerrainType {
    public string Name;
    public float Height;
    public Color Colour;
}