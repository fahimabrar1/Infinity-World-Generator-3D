using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTextureSplatting : MonoBehaviour
{
    public Terrain terrain;
    public Texture2D[] terrainTextures;
    public float[] textureScales;
    public float minHeight = 0.3f;
    public float maxHeight = 0.7f;

    void Start()
    {
        TerrainData terrainData = terrain.terrainData;

        TerrainLayer[] terrainLayers = GenerateTerrainLayers();

        terrainData.terrainLayers = terrainLayers;

        int heightmapWidth = terrainData.heightmapResolution;
        int heightmapHeight = terrainData.heightmapResolution;
        float[,] heights = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);
        float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainLayers.Length];

        for (int x = 0; x < terrainData.alphamapWidth; x++)
        {
            for (int y = 0; y < terrainData.alphamapHeight; y++)
            {
                float normalizedHeight = heights[y * (heightmapHeight - 1) / (terrainData.alphamapHeight - 1), x * (heightmapWidth - 1) / (terrainData.alphamapWidth - 1)];
                for (int i = 0; i < terrainLayers.Length; i++)
                {
                    if (normalizedHeight >= minHeight && normalizedHeight <= maxHeight)
                    {
                        splatmapData[x, y, i] = 1.0f;
                    }
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, splatmapData);
    }

    TerrainLayer[] GenerateTerrainLayers()
    {
        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTextures.Length];

        for (int i = 0; i < terrainTextures.Length; i++)
        {
            terrainLayers[i] = new TerrainLayer
            {
                diffuseTexture = terrainTextures[i],
                tileSize = new Vector2(textureScales[i], textureScales[i])
            };
        }

        return terrainLayers;
    }
}
