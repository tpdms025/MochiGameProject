using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBat : MonoBehaviour
{
    [Header("Attribute")]
    //���� ��ġ
    private Vector3 targetPoint;

    //���� ��ġ
    private Vector3 startPoint;

    //�̵� �ӵ�
    [SerializeField] private float speed = 2.0f;

    //���� �ִ��� ����
    [SerializeField] private bool isFlying = false;

    //��ü ��ġ�� �̺�Ʈ
    public static Action onObjectTouched;

    [Space(10)]
    [Header("Spawn")]
    //�ð� ���� ����
    public float reductionRate;

    //�⺻ ���� �ð�
    private float baseSpawnTime;

    //���� ���� �ð�
    public float finalSpawnTime;

    //���� ���� �ð�
    [SerializeField]private float spawnDuration;


    [Space(10)]
    [Header("Flight")]
    //�� �̵� �ð�
    private const float flightTime = 8.0f;

    //�� �̵��� �Ÿ�
    public float totaldistance = 0.0f;

    //�������Ӹ��� �̵��� ����
    private float distUnit = 0.0f;



    private Animator animator;




    #region Unity methods
    private void Awake()
    {
        animator = GetComponent<Animator>();

        startPoint = transform.position + new Vector3(0.0f, Camera.main.orthographicSize * 0.2f);
        targetPoint = new Vector3(5.0f, 3.72f, 0);
        distUnit = (targetPoint - startPoint).x / flightTime;

        baseSpawnTime = 150.0f;
        //��ȭ�Ҷ� ����� ����
        //�ӽð�
        reductionRate = 0.0f;
        reductionRate = 90.0f;
        ChangeSpawnTime();
    }

    private void Start()
    {
        transform.position = startPoint;
        OnReset();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Ư�� �ݶ��̴��� �浹�ϸ� �����Ѵ�.
        if (isFlying && collision.gameObject.CompareTag("NotFlyingZone"))
        {
            OnReset();
        }
    }
    #endregion


    #region Spawn

    public void ChangeSpawnTime()
    {
        finalSpawnTime = baseSpawnTime - (baseSpawnTime * reductionRate * 0.01f);
    }


    /// <summary>
    /// ��ġ�� �����ð��� �����Ѵ�.
    /// </summary>
    private void OnReset()
    {
        EndTheFlight();
        StartCoroutine(Cor_SpawnTimer());
    }

    private IEnumerator Cor_SpawnTimer()
    {
        spawnDuration = 0.0f;
        while (spawnDuration < finalSpawnTime)
        {
            spawnDuration += Time.deltaTime;
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
    private IEnumerator MoveCosinePath()
    {
        isFlying = true;
        animator.SetBool("isFlying", isFlying);

        //float distance = Vector3.Distance(transform.position, targetPos.position);
        float time = 0;
        totaldistance = 0;
        distUnit = (targetPoint - startPoint).x / flightTime;
        while (isFlying/*Mathf.Abs(distance) >= 1*/)
        {
            //��ü�� ��ġ�ߴٸ� �˾�â ����
            if (IsTouchedObject())
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
            time += Time.deltaTime;
            totaldistance = time * distUnit;
            float x = startPoint.x + totaldistance;
            float y = Mathf.Cos(totaldistance);
            transform.position = new Vector3(x, y, transform.position.z);

            yield return null;
        }
    }
    private Vector3[] cosinePoints;


    /// <summary>
    /// �ڻ��� ����� �������� �̸� ����Ѵ�.
    /// </summary>
    private void CalculateCosinePoints(int count)
    {
        cosinePoints = new Vector3[count+1];
        float unit = Mathf.PI *2 / count;

        float t = 0f;
        float x, y;
        for (int i = 0; i < count + 1; i++, t += unit)
        {
            x = t;
            y = Mathf.Cos(t);
            cosinePoints[i] = new Vector3(x, y, transform.position.z);
        }

    }

    /// <summary>
    /// ������ ������.
    /// </summary>
    private void EndTheFlight()
    {
        isFlying = false;
        animator.SetBool("isFlying", isFlying);

        transform.position = startPoint;
    }


    /// <summary>
    /// ��ü�� ��ġ�ߴ��� Ȯ���Ѵ�.
    /// </summary>
    /// <returns></returns>
    private bool IsTouchedObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //�浹�� ������ ����
            RaycastHit2D hit = Physics2D.Raycast(touchPos,Camera.main.transform.forward);
            //���콺 ����Ʈ ��ǥ�� �����.
            if (hit.collider !=null)
            {
                Debug.Log("hit?" + hit.collider.gameObject.name);
                if (hit.collider.tag == "BonusBat")
                {
                    return true;
                }
            }

            ////�浹�� ������ ����
            //RaycastHit hit;
            ////���콺 ����Ʈ ��ǥ�� �����.
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray.origin, ray.direction, out hit))
            //{
            //    Debug.Log("hit?" + hit.collider.gameObject.name);
            //    if (hit.collider.tag == "BonusBat")
            //    {
            //        return true;
            //    }
            //}
        }
        return false;
    }
   
}
