using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator {

    public static MeshData GenerateTerrainMesh(float[,] HeightMap, float HeightMultiplier, AnimationCurve HeightCurve, int LevelOfDetail)
    {
        int Width = HeightMap.GetLength(0);
        int Height = HeightMap.GetLength(1);
        float TopLeftX = (Width - 1) / -2f;
        float TopLeftZ = (Height - 1) / -2f;

        int MeshSimplificationIncrement = (LevelOfDetail == 2)?1:LevelOfDetail * 2;
        int VerticesPerLine = (Width - 1) / MeshSimplificationIncrement + 1;

        MeshData meshData = new MeshData(VerticesPerLine, VerticesPerLine);
        int VertexIndex = 0;

        for (int y = 0; y < Height; y += MeshSimplificationIncrement)
        {
            for (int x = 0; x < Width; x += MeshSimplificationIncrement)
            {
                meshData.Vertices[VertexIndex] = new Vector3(TopLeftX + x, HeightCurve.Evaluate(HeightMap[x, y]) * HeightMultiplier, TopLeftZ - y);
                meshData.uvs[VertexIndex] = new Vector2((x / (float)Width), (y / (float)Height));

                if (x < Width-1 && y < Height-1)
                {
                    meshData.AddTriangle(VertexIndex, VertexIndex + VerticesPerLine + 1, VertexIndex + VerticesPerLine);
                    meshData.AddTriangle(VertexIndex + VerticesPerLine + 1, VertexIndex, VertexIndex + 1);
                }

                VertexIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData {
    public Vector3[] Vertices;
    public int[] Triangles;
    public Vector2[] uvs;

    int TriangleIndex;

    public MeshData(int MeshWidth, int MeshHeight)
    {
        Vertices = new Vector3[MeshWidth * MeshHeight];
        uvs = new Vector2[MeshWidth * MeshHeight];
        Triangles = new int[(MeshWidth-1) * (MeshHeight-1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        Triangles[TriangleIndex] = a;
        Triangles[TriangleIndex+1] = b;
        Triangles[TriangleIndex+2] = c;
        TriangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}