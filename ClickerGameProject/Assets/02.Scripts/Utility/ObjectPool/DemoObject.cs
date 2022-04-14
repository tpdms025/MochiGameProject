using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoObject : MonoBehaviour
{
    public string poolItemName = "DamagePopupObj";

    private DemoObject damagePopup;
    
    Transform _parent;

    private void Awake()
    {
    }

    private void Initialize()
    {
    }

    //public DemoObject Create(Vector3 position, int damageAmount, bool isCriticalHit,bool isMiss, Transform parent = null)
    //{
    //    _parent = parent;
    //    GameObject damageObject = ObjectPool.Instance.PopFromPool(poolItemName, parent);
    //    damagePopup = damageObject.transform.GetComponent<DemoObject>();
    //    damagePopup.Initialize();
    //    damagePopup.transform.position = position;
    //    damageObject.SetActive(true);

    //    return damagePopup;
    //}

  


}
