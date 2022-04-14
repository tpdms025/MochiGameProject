using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager instance = null;
    public static ObjectPoolManager Instance
    {
        get { return instance; }
    }

    public ObjectPool objectPool;

    private void Awake()
    {
        if (instance !=null)
        {
            Destroy(instance);
            return;
        }
        instance = this;
      
    }
}
