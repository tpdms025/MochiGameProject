using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAnimator : MonoBehaviour
{
    //애니메이터 배열
    public RuntimeAnimatorController[] runtimeAnimators;
    //투사체 스프라이트 배열
    public Sprite[] sprites;

    private void Start()
    {
        runtimeAnimators = Resources.LoadAll<RuntimeAnimatorController>("Animator");
        sprites = Resources.LoadAll<Sprite>("Sprites/Projectile");
    }

    public RuntimeAnimatorController GetAnimatorController(int id)
    {
        return runtimeAnimators[id] as RuntimeAnimatorController;
    }
    public Sprite GetSprite(int id)
    {
        return sprites[id] as Sprite;
    }
}
