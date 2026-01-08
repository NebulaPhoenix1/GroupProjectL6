using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* 
   Base class for object spawners (obstacle, powerup and coins)
   It's set up so child classes can define their own logic if needed (I doubt it, but hey why not)
*/

public abstract class ObjectSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject[] prefabList;

    //Pool of objects to spawn from to reduce instantiating
    static private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    private static bool isInitialized = false;

    protected virtual void Awake()
    {
        //Ensure pool is cleared on scene load
        if (!isInitialized)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            isInitialized = true;
        }
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    //Spawn a random obstacle
    public virtual void SpawnObject()
    {
        if(prefabList.Length == 0)
        {
            return;
        }
        int selectedIndex = Random.Range(0, prefabList.Length);
        GameObject selectedPrefab = prefabList[selectedIndex];
        GameObject spawnedPrefab = GetPooledObject(selectedPrefab);
        spawnedPrefab.transform.position = transform.position;
        spawnedPrefab.transform.rotation = transform.rotation; 
        spawnedPrefab.SetActive(true);
    }

    //Get pooled object or create one
    protected GameObject GetPooledObject(GameObject searchObject)
    {
        //Create key if it doesn't exist
        if(!objectPool.ContainsKey(searchObject.name))
        {
            //Debug.Log("Making new key for object pool");
            objectPool.Add(searchObject.name, new Queue<GameObject>());
        }
        //Search pool
        if(objectPool[searchObject.name].Count > 0)
        {
            //Debug.Log("Reusing object from pool: " + searchObject.name);
            GameObject pooledObject = objectPool[searchObject.name].Dequeue();
            pooledObject.transform.parent = transform; //Reparent to spawner
            Debug.Log("Grabbed pooled object");
            return pooledObject;
        }
        else //No objects in pool, instantiate a new one
        {
            //Debug.Log("Instantiating new object for pool: " + searchObject.name);
            GameObject newObject = Instantiate(searchObject, transform);
            newObject.name = searchObject.name; //Ensure name matches for pooling
            return newObject;
        }
    }

    //Put object back in pool
    public static void ReturnObjectToPool(GameObject returnObject)
    {
        returnObject.SetActive(false);
        if(objectPool.ContainsKey(returnObject.name))
        {
            objectPool[returnObject.name].Enqueue(returnObject);
            Debug.Log("Returned object to pool: " + returnObject.name);
        }
        else
        {
            Debug.LogWarning("Attempted to return an object to a non-existent pool: " + returnObject.name);
            Destroy(returnObject);
        }
    }

    private static void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        objectPool.Clear();
    }
}
