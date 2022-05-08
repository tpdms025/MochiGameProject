using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workman : MonoBehaviour
{
    private readonly int hashIdle = Animator.StringToHash("isIdle");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private enum WorkmanState { Idle,Attack};
    private WorkmanState _state;

    //����(����->��ٸ�)�� �ð�
    private float attackTime;

    //����ü�� �߻� �ð�
    private readonly float projectileTime = 0.5f;

    //����ü�� ���� ����
    private Vector3 targetPoint;

    //����ü�� �ְ� ����
    private Vector3 maxPoint;

    //����ü ��������Ʈ
    private Sprite projectileSprite;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Init(Vector3 _maxPoint, Vector3 _targetPoint,Sprite _projectileSprite)
    {
        maxPoint = _maxPoint;
        targetPoint = _targetPoint;
        projectileSprite = _projectileSprite;
    }

    public void SetAnimator(RuntimeAnimatorController controller)
    {
        anim.runtimeAnimatorController = controller;
    }


    public IEnumerator FSM(float _waitTime)
    {
        //�����ǰ� n�� �Ŀ� ���ݻ��·� ����
        anim.SetBool(hashIdle, true);
        yield return new WaitForSeconds(_waitTime);

        anim.SetBool(hashIdle, false);
        anim.SetBool(hashAttack, true);

        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            yield return null;
        }

        AnimationClip clip = anim.GetCurrentAnimatorClipInfo(0)[0].clip;
        AnimatorStateInfo animationState = anim.GetCurrentAnimatorStateInfo(0);
        attackTime = clip.length;
        float t = attackTime;
        while (true)
        {
            t += Time.deltaTime;
            if (t >= attackTime)
            {
                Shoot();
                t = 0.0f;
            }
            yield return null;
        }
    }


    private void Shoot()
    {
        CreateProjectile();
    }


    /// <summary>
    /// ����ü�� �����Ѵ�.
    /// </summary>
    /// <param name="position"></param>
    private void CreateProjectile()
    {
        //������Ʈ Ǯ���� �����´�.
        GameObject obj = ObjectPool.Instance.PopFromPool("Projectile");
        obj.transform.position = transform.position + new Vector3(0, 0.5f, 0f);
        obj.GetComponent<Projectile>().Init(projectileTime, obj.transform.position, maxPoint, targetPoint);
        obj.GetComponent<Projectile>().SetSprite(projectileSprite);
    }
}
