using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace BallLine
{
    [System.Serializable]
    public class PoolObject
    {
        public List<ObjectPoolItem> itemsToPool;
    }
    [System.Serializable]
    public class ObjectPoolItem
    {
        public GameObject objectToPool;
        public bool shouldExpand;
        public int amountToPool;
    }

    public class ObjectPooling : MonoBehaviour
    {
        public static ObjectPooling SharedInstance;
        public List<PoolObject> poolObject;
        public List<GameObject> pooledObjects;

        int currentID;

      [SerializeField]  private int currentCharacterID;

        void Awake()
        {
            SharedInstance = this;
        }

        private void Start()
        {
            currentCharacterID = CharacterManager.Instance.CurrentCharacterIndex;
            if(!SceneManager.GetActiveScene().name.Equals(SceneNames.SGameplay.ToString()))
              return;
            PoolingObject(currentCharacterID);
        }

        
        //Note : Pooling and creating the pool , this is pre game , this value can be altered
        public void PoolingObject(int packageID)
        {
            pooledObjects = new List<GameObject>();
            for (int j=0;j< poolObject[packageID].itemsToPool.Count; j++)
            {
                for (int i = 0; i < poolObject[packageID].itemsToPool[j].amountToPool; i++)
                {
                    GameObject obj = (GameObject)Instantiate(poolObject[packageID].itemsToPool[j].objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                }
            }
        }

        public void DestroyPoolObject()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                Destroy(pooledObjects[i]);
                //pooledObjects.Remove(pooledObjects[i]);
            }
        }

        public GameObject GetPooledObjectByTag(string tag)
        {
  
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
                {
                    return pooledObjects[i];
                }
            }
            foreach (ObjectPoolItem item in poolObject[Random.Range(1,9)].itemsToPool)
            {
                if (item.objectToPool.tag == tag)
                {
                    if (item.shouldExpand)
                    {
                        GameObject obj = (GameObject)Instantiate(item.objectToPool);
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