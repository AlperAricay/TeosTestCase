using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Utils
{
    [Serializable]
    public class ObjectPoolItem
    {
        public GameObject objectToPool;
        public int amountToPool;
        public bool shouldExpand;
    }

    public class ObjectPooler : MonoBehaviour
    {
        public static ObjectPooler SharedInstance;
        public List<ObjectPoolItem> itemsToPool;
        [NonSerialized] public List<GameObject> pooledObjects;

        private void Awake()
        {
            SharedInstance = this;
        }

        private void Start()
        {
            pooledObjects = new List<GameObject>();
            foreach (var item in itemsToPool)
            {
                for (int i = 0; i < item.amountToPool; i++)
                {
                    var obj = Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                }
            }
        }

        public GameObject GetPooledObject(string targetTag)
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].CompareTag(targetTag))
                {
                    return pooledObjects[i];
                }
            }

            foreach (var item in itemsToPool)
            {
                if (item.objectToPool.CompareTag(targetTag))
                {
                    if (item.shouldExpand)
                    {
                        var obj = Instantiate(item.objectToPool);
                        obj.SetActive(false);
                        pooledObjects.Add(obj);
                        return obj;
                    }
                }
            }

            return null;
        }
    }
}