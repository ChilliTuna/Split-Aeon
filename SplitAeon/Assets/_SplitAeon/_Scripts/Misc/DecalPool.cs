using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalPool : MonoBehaviour
{

    public static DecalPool POOL;

    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        POOL = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject temp;

        for (int i = 0; i < amountToPool; i++)
        {
            temp = Instantiate(objectToPool);
            temp.SetActive(false);
            pooledObjects.Add(temp);
        }

    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

}
