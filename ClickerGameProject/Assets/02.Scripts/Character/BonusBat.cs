using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBat : MonoBehaviour
{
    [Header("Attribute")]
    //도착 위치
    private Vector3 targetPoint;

    //시작 위치
    private Vector3 startPoint;

    //이동 속도
    [SerializeField] private float speed = 2.0f;

    //날고 있는지 여부
    [SerializeField] private bool isFlying = false;

    //객체 터치시 이벤트
    public static Action onObjectTouched;

    [Space(10)]
    [Header("Spawn")]
    //시간 감소 비율
    public float reductionRate;

    //기본 스폰 시간
    private float baseSpawnTime;

    //최종 스폰 시간
    public float finalSpawnTime;

    //스폰 지속 시간
    [SerializeField]private float spawnDuration;


    [Space(10)]
    [Header("Flight")]
    //총 이동 시간
    private const float flightTime = 8.0f;

    //총 이동한 거리
    public float totaldistance = 0.0f;

    //매프레임마다 이동할 단위
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
        //강화할때 변경될 예정
        //임시값
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
        //특정 콜라이더를 충돌하면 리셋한다.
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
    /// 위치와 스폰시간을 리셋한다.
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

        //박쥐 이동
        StartCoroutine(MoveCosinePath());
    }

    #endregion

    /// <summary>
    /// 코사인 이동경로로 이동한다.
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
            //객체를 터치했다면 팝업창 열기
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
    /// 코사인 경로의 지점들을 미리 계산한다.
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
    /// 비행을 끝내다.
    /// </summary>
    private void EndTheFlight()
    {
        isFlying = false;
        animator.SetBool("isFlying", isFlying);

        transform.position = startPoint;
    }


    /// <summary>
    /// 객체를 터치했는지 확인한다.
    /// </summary>
    /// <returns></returns>
    private bool IsTouchedObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //충돌이 감지된 영역
            RaycastHit2D hit = Physics2D.Raycast(touchPos,Camera.main.transform.forward);
            //마우스 포인트 좌표를 만든다.
            if (hit.collider !=null)
            {
                Debug.Log("hit?" + hit.collider.gameObject.name);
                if (hit.collider.tag == "BonusBat")
                {
                    return true;
                }
            }

            ////충돌이 감지된 영역
            //RaycastHit hit;
            ////마우스 포인트 좌표를 만든다.
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
