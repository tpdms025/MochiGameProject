using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workman : MonoBehaviour
{
    private int hashIdle = Animator.StringToHash("isIdle");
    private int hashAttack = Animator.StringToHash("isAttack");
    private enum WorkmanState { Idle,Attack};
    private WorkmanState _state;

    //����(����->��ٸ�)�� �ð�
    private float patternTime = 4.0f;

    //����ü�� �߻� �ð�
    private float projectileTime = 0.5f;

    //����ü�� ���� ����
    private Vector3 targetPoint;

    //����ü�� �ְ� ����
    private Vector3 maxPoint;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Init(Vector3 _maxPoint, Vector3 _targetPoint)
    {
        maxPoint = _maxPoint;
        targetPoint = _targetPoint;
    }


    public IEnumerator FSM()
    {
        //�����ǰ� n�� �Ŀ� ���ݻ��·� ����
        anim.SetBool(hashIdle,true);
        yield return new WaitForSeconds(3.0f);
        anim.SetBool(hashIdle,false);

        anim.SetBool(hashAttack,true);
        float t = 0.0f;
        while(true)
        {
            if (t > patternTime)
            {
                Shoot();
                t= 0.0f;
            }
            t += Time.deltaTime;
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
        obj.transform.position = transform.position;
        obj.GetComponent<Projectile>().Init(projectileTime, transform.position, maxPoint, targetPoint);
    }
}
