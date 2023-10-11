using UnityEngine;
using System.Collections.Generic;

public abstract class ObjectPoolingSystem : MonoBehaviour
{
    protected Dictionary<string, Queue<GameObject>> poolDictionary;

    public abstract void InitializePools();

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
        if (poolDictionary.ContainsKey(tag))
        {
            poolDictionary[tag].Enqueue(objectToReturn);
        }
        else
        {
            Debug.LogWarning($"Trying to return an object to a pool with tag {tag} that doesn't exist.");
        }
    }
}
