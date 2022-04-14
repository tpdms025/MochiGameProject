using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //스프라이트 렌더러
    private SpriteRenderer renderer;

    //폭발 파티클
    private ParticleSystem explosionParticle;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        explosionParticle = transform.Find("Particle System-Explosion").GetComponent<ParticleSystem>();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //광석에 부딪히면 폭발 이펙트 생성
    //    if (collision.CompareTag("Ore"))
    //    {
    //        //StartCoroutine(Cor_Destroy());
    //    }
    //}




    public void Init(float flightTime, Vector3 startPos, Vector3 p1, Vector3 targetPos)
    {
        renderer.enabled = true;
        explosionParticle.gameObject.SetActive(false);
        StartCoroutine(Cor_Flight(flightTime,startPos, p1, targetPos));
    }




    private IEnumerator Cor_Flight(float flightTime,Vector3 startPos,Vector3 p1,  Vector3 targetPos)
    {
        float time = 0.0f;
        Vector3 pA = startPos;
        Vector3 pB = p1;
        Vector3 pC = targetPos;

        float t;
        while (time < flightTime)
        {
            time +=Time.deltaTime;
            t = time / flightTime;

            float u = (1 - t);
            float t2 = t * t;
            float u2 = u * u;

            transform.position = pA * u2 +
                  pB * (t * u * 2) +
                  pC * t2;
            yield return null;
        }

        StartCoroutine(Cor_Destroy());

    }

    private IEnumerator Cor_Destroy()
    {
        renderer.enabled = false;
        explosionParticle.gameObject.SetActive(true);

        PlayParticle();

        //1초후 리셋
        yield return new WaitForSeconds(1.0f);
        OnReset();
    }


    /// <summary>
    /// 파티클을 재생한다.
    /// </summary>
    private void PlayParticle()
    {
        explosionParticle.Stop(true);
        if (explosionParticle.isStopped)
        {
            explosionParticle.Play(true);
        }
    }

    /// <summary>
    /// 원래상태로 돌려둔다.
    /// </summary>
    private void OnReset()
    {
        //오브젝트 풀에 넣기
        ObjectPool.Instance.PushToPool("Projectile", this.gameObject);
    }

    
}
