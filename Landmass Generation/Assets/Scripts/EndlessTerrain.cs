using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour {

    public const float MaxViewDistance = 300;
    public Transform Viewer;

    public static Vector2 ViewerPosition;
    int ChunkSize;
    int ChunksVisableInViewDistance;

	void Start()
	{
        ChunkSize = MapGenerator.MapChunkSize - 1;
        ChunksVisableInViewDistance = Mathf.RoundToInt(MaxViewDistance / ChunkSize);
	}
}
