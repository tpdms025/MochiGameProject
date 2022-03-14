using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBat : MonoBehaviour
{
    [Header("Attribute")]
    //���� ��ġ
    [SerializeField] private Transform targetPos;

    //���� ��ġ
    private Vector3 startPos;

    //�̵� �ӵ�
    [SerializeField] private float speed = 2.0f;

    //���� �ִ��� ����
    [SerializeField] private bool isFlying = false;

    //��ü ��ġ�� �̺�Ʈ
    public static Action onObjectTouched;

    [Space(10)]
    [Header("Spawn")]
    //�⺻ ���� �ð�
    private float baseSpawnTime;

    //�ð� ���� �ۼ�Ʈ
    public float reductionPercentage;

    //���� ���� �ð�
    public float secondsBetweenSpawn;

    //��� �ð�
    [SerializeField]private float elapsedTime;




    #region Unity methods
    private void Awake()
    {
        startPos = transform.position;
        baseSpawnTime = 150.0f;

        //��ȭ�Ҷ� ����� ����
        //�ӽð�
        reductionPercentage = 0.0f;
        reductionPercentage = 90.0f;
        ChangeSpawnTime();
    }

    private void Start()
    {
        OnReset();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFlying && collision.gameObject.CompareTag("NotFlyingZone"))
        {
            //�����ϱ�
            OnReset();
        }
    }
    #endregion


    #region Spawn

    public void ChangeSpawnTime()
    {
        secondsBetweenSpawn = baseSpawnTime - (baseSpawnTime * reductionPercentage * 0.01f);
    }


    public void OnReset()
    {
        EndTheFlight();
        StartCoroutine(Cor_Timer());
    }

    private IEnumerator Cor_Timer()
    {
        elapsedTime = 0.0f;
        while (elapsedTime < secondsBetweenSpawn)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;

        //���� �̵�
        StartCoroutine(MoveCosinePath());
    }

    #endregion

   
    /// <summary>
    /// �ڻ��� �̵���η� �̵��Ѵ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveCosinePath()
    {
        //float distance = Vector3.Distance(transform.position, targetPos.position);

        isFlying = true;
        float runningTime = 0;
        while (isFlying/*Mathf.Abs(distance) >= 1*/)
        {
            //��ü�� ��ġ�ߴٸ� �˾�â ����
            if(IsTouchedObject())
            {
                if (onObjectTouched != null)
                {
                    onObjectTouched.Invoke();
                }
                OnReset();
                yield break;
            }

            //distance = Vector3.Distance(transform.position, targetPos.position);
            //Debug.Log("distance" + distance);
            //transform.position = Vector3.Lerp(transform.position, targetPos.position, speed * Time.deltaTime);

            runningTime += Time.deltaTime * speed;
            float x = runningTime + startPos.x;
            float y = Mathf.Cos(runningTime);
            transform.position = new Vector3(x, y, transform.position.z);
            yield return null;
        }

        OnReset();
    }

    /// <summary>
    /// ������ ������.
    /// </summary>
    private void EndTheFlight()
    {
        isFlying = false;
        transform.position = startPos;
    }


    /// <summary>
    /// ��ü�� ��ġ�ߴ��� Ȯ���Ѵ�.
    /// </summary>
    /// <returns></returns>
    private bool IsTouchedObject()
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(touchPos, Camera.main.transform.forward);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag.Equals("BonusBat"))
            {
                return true;
            }
        }
        return false;
    }

  
   
}
