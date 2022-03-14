using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBat : MonoBehaviour
{
    [Header("Attribute")]
    //도착 위치
    [SerializeField] private Transform targetPos;

    //시작 위치
    private Vector3 startPos;

    //이동 속도
    [SerializeField] private float speed = 2.0f;

    //날고 있는지 여부
    [SerializeField] private bool isFlying = false;

    //객체 터치시 이벤트
    public static Action onObjectTouched;

    [Space(10)]
    [Header("Spawn")]
    //기본 스폰 시간
    private float baseSpawnTime;

    //시간 감소 퍼센트
    public float reductionPercentage;

    //최종 스폰 시간
    public float secondsBetweenSpawn;

    //경과 시간
    [SerializeField]private float elapsedTime;




    #region Unity methods
    private void Awake()
    {
        startPos = transform.position;
        baseSpawnTime = 150.0f;

        //강화할때 변경될 예정
        //임시값
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
            //리셋하기
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

        //박쥐 이동
        StartCoroutine(MoveCosinePath());
    }

    #endregion

   
    /// <summary>
    /// 코사인 이동경로로 이동한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveCosinePath()
    {
        //float distance = Vector3.Distance(transform.position, targetPos.position);

        isFlying = true;
        float runningTime = 0;
        while (isFlying/*Mathf.Abs(distance) >= 1*/)
        {
            //객체를 터치했다면 팝업창 열기
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
    /// 비행을 끝내다.
    /// </summary>
    private void EndTheFlight()
    {
        isFlying = false;
        transform.position = startPos;
    }


    /// <summary>
    /// 객체를 터치했는지 확인한다.
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
