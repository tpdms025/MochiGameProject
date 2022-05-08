using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAnimator : MonoBehaviour
{
    //�ִϸ����� �迭
    public RuntimeAnimatorController[] runtimeAnimators;
    //����ü ��������Ʈ �迭
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
