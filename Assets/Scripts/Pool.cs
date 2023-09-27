using System.Collections.Generic;
using UnityEngine;


// class for pooling game objects that would otherwise
// have many instances created and destroyed throughout a run
public class Pool : MonoBehaviour
{
    
    [SerializeField] GameObject prefab;
    [SerializeField] int count;
    ObjectManager objects;

    public List<GameObject> pool;
    Dictionary<GameObject, int> objToIndex;

    Transform myTransform;
    int firstAvailable;

    private void Awake() {
        objects = GameObject.Find("RunManager").GetComponent<ObjectManager>();

        myTransform = transform;
        
        pool = new List<GameObject>();
        objToIndex = new Dictionary<GameObject, int>();

        for(int i = 0; i < count; i++) {
            pool.Add(Instantiate(prefab));
            objToIndex.Add(pool[i], i);
            pool[i].transform.SetParent(myTransform, false);
            pool[i].SetActive(false);
        }

        firstAvailable = 0;
    }


    // grabs the last available
    public GameObject GetPooledObject() {
        if (firstAvailable == count)
            return null;
        else {
            pool[firstAvailable].SetActive(true);
            return pool[firstAvailable++];
        }
    }

    // return a pooled object back to the subset of available objects
    // by swapping it with the last object still in use (if any exists)
    public void ReturnPooledObject(GameObject pooledObject) {
        pooledObject.SetActive(false);

        if (firstAvailable == 0) return;
        else if (firstAvailable == 1) {
            firstAvailable--;
            return;
        }

        // swap the passed object with the first object still in use
        GameObject temp = pool[--firstAvailable];
        int newIndex = objToIndex[pooledObject];

        pool[firstAvailable] = pooledObject;
        objToIndex[pooledObject] = firstAvailable;

        pool[newIndex] = temp;
        objToIndex[temp] = newIndex;
    }


}
