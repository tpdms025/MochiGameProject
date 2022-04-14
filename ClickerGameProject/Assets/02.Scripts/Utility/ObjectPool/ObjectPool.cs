

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool instance = null;
    public static ObjectPool Instance
    {
        get { return instance; }
        set { }
    }

    public Transform canvas;

    public List<PooledObject> objectPool = new List<PooledObject>();
    public List<PooledObject> ui_objectPool = new List<PooledObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for (int i= 0; i < objectPool.Count; ++i)
        {
            objectPool[i].Initialize(transform);
        }

        for (int i = 0; i < ui_objectPool.Count; ++i)
        {
            ui_objectPool[i].Initialize(canvas);
        }
    }

    public bool PushToPool(string itemName, GameObject item,bool isUI = false, Transform parent = null)
    {
        PooledObject pool = GetPoolItem(itemName, isUI);
        if (pool == null)
        {
            return false;
        }

        Transform _transform = (isUI) ? canvas : transform;
        pool.PushToPool(item, parent == null ? _transform : parent);
        return true;
    }

    PooledObject GetPoolItem(string itemName, bool isUI)
    {
        List<PooledObject> _objectPool = isUI ? ui_objectPool: objectPool;

        for (int i= 0;i< _objectPool.Count;++i)
        {
            if(_objectPool[i].poolItemName.Equals(itemName))
            {
                return _objectPool[i];
            }
        }
        return null;
    }

    public GameObject PopFromPool(string itemName, bool isUI = false, Transform parent = null)
    {
        PooledObject pool = GetPoolItem(itemName, isUI);
        if(pool==null)
        {
            return null;
        }

        return pool.PopFromPool(parent);
    }



}
