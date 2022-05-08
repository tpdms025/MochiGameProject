using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleEffectAnim : MonoBehaviour
{
    private int hashStarted = Animator.StringToHash("isStarted");

    private Transform logoTrans;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        logoTrans = transform.Find("LogoImage").GetComponent<Transform>();
        logoTrans.localScale = Vector3.zero;
    }
    private void Start()
    {
        Initialize();
    }

 
    private void Initialize()
    {
    }

    /// <summary>
    /// �ִϸ��̼��� �����Ѵ�.
    /// </summary>
    public void PlayAnimation()
    {
        animator.SetBool(hashStarted, true);
    }
}
