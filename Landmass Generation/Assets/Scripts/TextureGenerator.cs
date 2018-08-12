using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator {

    public static Texture2D TextureFromColourMap(Color[] ColourMap, int Width, int Height)
    {
        Texture2D Texture = new Texture2D(Width, Height);
        Texture.filterMode = FilterMode.Point;
        Texture.wrapMode = TextureWrapMode.Clamp;
        Texture.SetPixels(ColourMap);
        Texture.Apply();
        return Texture;
    }

    public static Texture2D TextureFromHeightMap(float[,] HeightMap)
    {
        int Width = HeightMap.GetLength(0);
        int Height = HeightMap.GetLength(1);

        Color[] ColorMap = new Color[Width * Height];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                ColorMap[y * Width + x] = Color.Lerp(Color.black, Color.white, HeightMap[x, y]);
            }
        }

        return TextureFromColourMap(ColorMap, Width, Height);
    }
}