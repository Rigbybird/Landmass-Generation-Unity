using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {

    public Renderer TextureRenderer;
    public MeshFilter MeshFilter;
    public MeshRenderer MeshRenderer;

    public void DrawTexture(Texture2D Texture)
    {
        TextureRenderer.sharedMaterial.mainTexture = Texture;
        TextureRenderer.transform.localScale = new Vector3(Texture.width, 1, Texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        MeshFilter.sharedMesh = meshData.CreateMesh();
        MeshRenderer.sharedMaterial.mainTexture = texture;
    }
}
