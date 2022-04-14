using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //��������Ʈ ������
    private SpriteRenderer renderer;

    //���� ��ƼŬ
    private ParticleSystem explosionParticle;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        explosionParticle = transform.Find("Particle System-Explosion").GetComponent<ParticleSystem>();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //������ �ε����� ���� ����Ʈ ����
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

        //1���� ����
        yield return new WaitForSeconds(1.0f);
        OnReset();
    }


    /// <summary>
    /// ��ƼŬ�� ����Ѵ�.
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
    /// �������·� �����д�.
    /// </summary>
    private void OnReset()
    {
        //������Ʈ Ǯ�� �ֱ�
        ObjectPool.Instance.PushToPool("Projectile", this.gameObject);
    }

    
}
