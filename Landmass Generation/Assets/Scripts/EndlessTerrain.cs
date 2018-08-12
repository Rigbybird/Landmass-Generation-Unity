using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour {

    public const float MaxViewDistance = 300;
    public Transform Viewer;

    public static Vector2 ViewerPosition;
    int ChunkSize;
    int ChunksVisableInViewDistance;

    Dictionary<Vector2, TerrainChunk> TerrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> TerrainChunksVisableLastUpdate = new List<TerrainChunk>();

	void Start()
	{
        ChunkSize = MapGenerator.MapChunkSize - 1;
        ChunksVisableInViewDistance = Mathf.RoundToInt(MaxViewDistance / ChunkSize);
	}

    void Update()
    {
        ViewerPosition = new Vector2(Viewer.position.x, Viewer.position.z);
        UpdateVisableChunks();
    }

    void UpdateVisableChunks()
    {
        for (int i = 0; i < TerrainChunksVisableLastUpdate.Count; i++)
        {
            TerrainChunksVisableLastUpdate[i].SetVisable(false);
        }
        TerrainChunksVisableLastUpdate.Clear();

        int CurrentChunkCoordX = Mathf.RoundToInt(ViewerPosition.x / ChunkSize);
        int CurrentChunkCoordY = Mathf.RoundToInt(ViewerPosition.y / ChunkSize);

        for (int yOffset = -ChunksVisableInViewDistance; yOffset <= ChunksVisableInViewDistance; yOffset++)
        {
            for (int xOffset = -ChunksVisableInViewDistance; xOffset <= ChunksVisableInViewDistance; xOffset++)
            {
                Vector2 ViewedChunkCoord = new Vector2(CurrentChunkCoordX + xOffset, CurrentChunkCoordY + yOffset);

                if (TerrainChunkDictionary.ContainsKey(ViewedChunkCoord))
                {
                    TerrainChunkDictionary[ViewedChunkCoord].UpdateTerrainChunk();
                } else
                {
                    TerrainChunkDictionary.Add(ViewedChunkCoord, new TerrainChunk(ViewedChunkCoord, ChunkSize));
                }
            }
        }
    }

    public class TerrainChunk {

        GameObject MeshObject;
        Vector2 Position;
        Bounds Bounds;

        public TerrainChunk(Vector2 Coord, int Size)
        {
            Position = Coord * Size;
            Bounds = new Bounds(Position, Vector2.one * Size);
            Vector3 PositionV3 = new Vector3(Position.x, 0, Position.y);

            MeshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            MeshObject.transform.position = PositionV3;
            MeshObject.transform.localScale = Vector3.one * Size / 10f;
            SetVisable(false);
        }

        public void UpdateTerrainChunk()
        {
            float ViewerDistanceFromNearestEdge = Mathf.Sqrt(Bounds.SqrDistance(ViewerPosition));
            bool Visable = ViewerDistanceFromNearestEdge <= MaxViewDistance;
            SetVisable(Visable);
        }

        public void SetVisable(bool Visable)
        {
            MeshObject.SetActive(Visable);
        }

        public bool IsVisable()
        {
            return MeshObject.activeSelf;
        }
    }
}