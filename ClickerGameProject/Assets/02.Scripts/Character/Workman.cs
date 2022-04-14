using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workman : MonoBehaviour
{
    private int hashIdle = Animator.StringToHash("isIdle");
    private int hashAttack = Animator.StringToHash("isAttack");
    private enum WorkmanState { Idle,Attack};
    private WorkmanState _state;

    //패턴(공격->기다림)의 시간
    private float patternTime = 4.0f;

    //투사체의 발사 시간
    private float projectileTime = 0.5f;

    //투사체의 도착 지점
    private Vector3 targetPoint;

    //투사체의 최고 지점
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
        //스폰되고 n초 후에 공격상태로 변경
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
    /// 투사체를 생성한다.
    /// </summary>
    /// <param name="position"></param>
    private void CreateProjectile()
    {
        //오브젝트 풀에서 꺼내온다.
        GameObject obj = ObjectPool.Instance.PopFromPool("Projectile");
        obj.transform.position = transform.position;
        obj.GetComponent<Projectile>().Init(projectileTime, transform.position, maxPoint, targetPoint);
    }
}
