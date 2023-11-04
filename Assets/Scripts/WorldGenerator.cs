using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public Terrain terrainPrefab;
    public int chunkSize = 256; // Size of each chunk
    public int worldSize = 5; // Number of chunks in each dimension
    public float scale = 20.0f;
    public float terrainHeightMultiplier = 10.0f;
    public Texture2D[] terrainTextures;
    public float[] textureScales;
    public GameObject[] treePrefabs; // Array of tree prefabs
    public float treeDensityMultiplier = 1.0f;

    private readonly List<Terrain> activeChunks = new List<Terrain>();

    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int z = 0; z < worldSize; z++)
            {
                Vector3 chunkPosition = new(x * chunkSize, 0, z * chunkSize);
                Terrain chunk = GenerateChunk(chunkPosition);
                activeChunks.Add(chunk);
                PlaceTrees(chunk, chunkPosition);
            }
        }
    }

    Terrain GenerateChunk(Vector3 position)
    {
        Terrain chunk = Instantiate(terrainPrefab, position, Quaternion.identity);
        chunk.terrainData = GenerateTerrainData(chunk.terrainData);
        ApplyTextures(chunk.terrainData);
        return chunk;
    }

    TerrainData GenerateTerrainData(TerrainData terrainData)
    {
        terrainData.heightmapResolution = chunkSize + 1;
        terrainData.size = new Vector3(chunkSize, terrainHeightMultiplier, chunkSize);
        terrainData.SetHeights(0, 0, GenerateHeights(chunkSize + 1));
        return terrainData;
    }

    float[,] GenerateHeights(int resolution)
    {
        float[,] heights = new float[resolution, resolution];

        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                float xCoord = (float)x / resolution * scale;
                float zCoord = (float)z / resolution * scale;
                float heightValue = Mathf.PerlinNoise(xCoord, zCoord);
                heights[x, z] = heightValue;
            }
        }

        return heights;
    }

    void ApplyTextures(TerrainData terrainData)
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

        terrainData.terrainLayers = terrainLayers;
    }

    void PlaceTrees(Terrain chunk, Vector3 chunkPosition)
    {
        TerrainData terrainData = chunk.terrainData;
        TreePrototype[] treePrototypes = new TreePrototype[treePrefabs.Length];

        for (int i = 0; i < treePrefabs.Length; i++)
        {
            treePrototypes[i] = new TreePrototype { prefab = treePrefabs[i] };
        }

        terrainData.treePrototypes = treePrototypes;

        List<TreeInstance> treeInstances = new List<TreeInstance>();

        int treeIndex = 0; // Use a counter to determine tree selection

        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                float normalizedX = (float)x / chunkSize;
                float normalizedZ = (float)z / chunkSize;
                float noiseValue = Mathf.PerlinNoise(
                    (chunkPosition.x + normalizedX) * scale,
                    (chunkPosition.z + normalizedZ) * scale
                );
                float adjustedTreeThreshold = treeDensityMultiplier;

                if (noiseValue > adjustedTreeThreshold)
                {
                    treeIndex = (treeIndex + 1) % treePrefabs.Length; // Determine the tree using the counter
                    TreeInstance treeInstance = new TreeInstance
                    {
                        position = new Vector3(normalizedX, noiseValue, normalizedZ),
                        widthScale = 1f,
                        heightScale = 1f,
                        color = Color.white,
                        lightmapColor = Color.white,
                        prototypeIndex = treeIndex
                    };
                    treeInstances.Add(treeInstance);
                }
            }
        }

        terrainData.treeInstances = treeInstances.ToArray();
    }
}
