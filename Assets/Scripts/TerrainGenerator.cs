using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 20.0f;
    public float terrainHeightMultiplier = 10.0f;
    public Texture2D[] terrainTextures;
    public float[] textureScales;
    public GameObject[] treePrefabs; // Array of tree prefabs
    public GameObject[] propPrefabs; // Array of prop prefabs
    public float treeDensityMultiplier = 1.0f;
    public float propDensityMultiplier = 1.0f;
    public float terrainThreshold = 0.5f;
    public float treeThreshold = 0.6f;
    public float propThreshold = 0.8f;

    private Terrain terrain;
    private TerrainData terrainData;
    private NativeArray<float> heightmap;
    private JobHandle jobHandle;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
        heightmap = new NativeArray<float>(width * height, Allocator.Persistent);

        GenerateTerrain();
    }

    void OnDestroy()
    {
        heightmap.Dispose();
    }

    void GenerateTerrain()
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, terrainHeightMultiplier, height);

        var terrainJob = new TerrainGenerationJob
        {
            width = width,
            height = height,
            scale = scale,
            terrainHeightMultiplier = terrainHeightMultiplier,
            heightmap = heightmap
        };

        jobHandle = terrainJob.Schedule(width * height, 64);
        jobHandle.Complete();

        var heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = heightmap[x + y * width];
            }
        }

        terrainData.SetHeights(0, 0, heights);
        ApplyTextures();

        PlaceTreesAndProps();

    }

    void ApplyTextures()
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

    void PlaceTreesAndProps()
    {
        TreePrototype[] treePrototypes = new TreePrototype[treePrefabs.Length];
        for (int i = 0; i < treePrefabs.Length; i++)
        {
            treePrototypes[i] = new TreePrototype { prefab = treePrefabs[i] };
        }
        terrainData.treePrototypes = treePrototypes;

        List<TreeInstance> treeInstances = new List<TreeInstance>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float normalizedX = (float)x / width;
                float normalizedY = (float)y / height;
                float noiseValue = Mathf.PerlinNoise(normalizedX * scale, normalizedY * scale);

                if (noiseValue > terrainThreshold)
                {
                    // Texture blending here, if needed
                }

                float adjustedTreeThreshold = treeThreshold * treeDensityMultiplier;

                if (noiseValue > adjustedTreeThreshold)
                {
                    int randomTree = UnityEngine.Random.Range(0, treePrefabs.Length);
                    TreeInstance treeInstance = new TreeInstance
                    {
                        position = new Vector3(normalizedX, noiseValue, normalizedY),
                        widthScale = 1f,
                        heightScale = 1f,
                        color = Color.white,
                        lightmapColor = Color.white,
                        prototypeIndex = randomTree
                    };
                    treeInstances.Add(treeInstance);
                }
            }
        }

        terrainData.treeInstances = treeInstances.ToArray();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float normalizedX = (float)x / width;
                float normalizedY = (float)y / height;
                float noiseValue = Mathf.PerlinNoise(normalizedX * scale, normalizedY * scale);

                if (noiseValue > terrainThreshold)
                {
                    // Texture blending here, if needed
                }

                float adjustedPropThreshold = propThreshold * propDensityMultiplier;

                if (noiseValue > adjustedPropThreshold)
                {
                    int randomProp = UnityEngine.Random.Range(0, propPrefabs.Length);
                    Vector3 propPosition = new Vector3(normalizedX * width, 0, normalizedY * height);
                    Instantiate(propPrefabs[randomProp], propPosition, Quaternion.identity);
                }
            }
        }
    }

    [BurstCompile]
    private struct TerrainGenerationJob : IJobParallelFor
    {
        public int width;
        public int height;
        public float scale;
        public float terrainHeightMultiplier;
        public NativeArray<float> heightmap;

        public void Execute(int index)
        {
            int x = index % width;
            int y = index / width;

            float xCoord = (float)x / width * scale;
            float yCoord = (float)y / height * scale;
            float heightValue = Mathf.PerlinNoise(xCoord, yCoord) * terrainHeightMultiplier;

            heightmap[index] = heightValue;
        }
    }
}
