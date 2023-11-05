using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectPoolGameObject
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Transform parentTransform;
    [SerializeField]
    private Queue<GameObject> objectQueue = new();


    // /// <summary>
    // /// Initializes a new instance of the ObjectPool.
    // /// </summary>
    // /// <param name="prefab">The prefab of the object to be pooled.</param>
    // /// <param name="initialSize">The initial size of the object pool.</param>
    // /// <param name="parentTransform">Optional parent transform for pooled objects.</param>
    // public ObjectPoolGameObject(GameObject prefab, int initialSize = 100, Transform parentTransform = null)
    // {
    //     this.prefab = prefab;
    //     this.parentTransform = parentTransform;
    //     objectQueue = new Queue<GameObject>();

    //     for (int i = 0; i < initialSize; i++)
    //     {
    //         CreateAndEnqueueObject();
    //     }
    // }

    /// <summary>
    /// Retrieves an object from the pool.
    /// </summary>
    /// <returns>The retrieved object.</returns>
    public GameObject DequeueObjectFromPool()
    {
        if (objectQueue.Count == 0)
        {
            CreateAndEnqueueObject();
        }

        GameObject obj = objectQueue.Dequeue();
        obj.SetActive(true);

        return obj;
    }
    /// <summary>
    /// Returns an object to the pool.
    /// </summary>
    /// <param name="obj">The object to be returned to the pool.</param>
    public void EnqueueObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        objectQueue.Enqueue(obj);
    }

    /// <summary>
    /// Creates a new object and adds it to the pool.
    /// </summary>
    public void CreateAndEnqueueObject()
    {
        GameObject obj = GameObject.Instantiate(prefab, parentTransform.position, Quaternion.identity);
        EnqueueObjectToPool(obj);
    }
}
