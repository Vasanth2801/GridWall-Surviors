using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPool
    {
        public string objectname;
        public int poolSize;
        public GameObject poolPrefab;
    }

    public List<ObjectPool> pools;

    public Dictionary<string, Queue<GameObject>> poolofDictionary;

    private void Start()
    {
        poolofDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (ObjectPool item in pools)
        {
            Queue<GameObject> obj = new Queue<GameObject>();

            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject objPool = Instantiate(item.poolPrefab);
                objPool.SetActive(false);
                obj.Enqueue(objPool);
            }

            poolofDictionary.Add(item.objectname, obj);
        }
    }

    public GameObject SpawnObjects(string tag, Vector3 position, Quaternion rotation)
    {
        GameObject objectToSpawn = poolofDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolofDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
