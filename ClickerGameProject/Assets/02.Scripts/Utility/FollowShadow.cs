using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShadow : MonoBehaviour
{
    public Transform targetObj;

    private void Start()
    {
        StartCoroutine(Cor_LoopFollow());
    }
    private IEnumerator Cor_LoopFollow()
    {
        while(!DBManager.Inst.isGameStop)
        {
            transform.localPosition = new Vector3(targetObj.localPosition.x, 0, 0f);
            yield return null;
        }
    }
}
