using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject playerPrefab;
    public ObjectPoolGameObject ObjectPoolGameObject = new();

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        StartCoroutine(SpawnPlayer());
    }

    private IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(2);
    }

    public GameObject DequeueChunk()
    {
        return ObjectPoolGameObject.DequeueObjectFromPool();
    }

    public void EnqueueChunk(GameObject Chunk)
    {
        ObjectPoolGameObject.EnqueueObjectToPool(Chunk);
    }
}
