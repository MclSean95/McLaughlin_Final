using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicObjectPool : MonoBehaviour {
    public GameObject prefab;
    private Stack<GameObject> inactiveInstances = new Stack<GameObject>();

    public GameObject GetObj()
    {
        GameObject spawnObject;
        if (inactiveInstances.Count > 0)
        {
            spawnObject = inactiveInstances.Pop();
        }
        else
        {
            spawnObject = Instantiate(prefab);

            PooledObject pooledObject = spawnObject.AddComponent<PooledObject>();
            pooledObject.pool = this;
        }

        spawnObject.SetActive(true);
        return spawnObject;
    }

    public void ReturnObj(GameObject toReturn)
    {
        PooledObject pooledObject = toReturn.GetComponent<PooledObject>();

        if (pooledObject != null && pooledObject.pool == this)
        {
            toReturn.SetActive(false);
            inactiveInstances.Push(toReturn);
        }
        else
        {
            Destroy(toReturn);
        }
    }
}

public class PooledObject : MonoBehaviour
{
    public BasicObjectPool pool;
}
