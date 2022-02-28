using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBat : MonoBehaviour
{
    //도착 위치
    [SerializeField] private Transform targetPos;

    //시작 위치
    private Vector3 startPos;

    //이동 속도
    [SerializeField] private float speed = 2.0f;

    //날고 있는지 여부
    [SerializeField] private bool isFlying = false;

    private IEnumerator coroutine = null;

    //객체 터치시 이벤트
    public static Action onObjectTouched;




    #region Unity methods
    private void Awake()
    {
        startPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NotFlyingZone"))
        {
            EndTheFlight();
        }
    }
    #endregion




    /// <summary>
    /// 객체를 이동한다.
    /// </summary>
    public void MoveObject()
    {
        if (coroutine != null)
            return;

        coroutine = MoveCosinePath();
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// 코사인 이동경로로 이동한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveCosinePath()
    {
        //float distance = Vector3.Distance(transform.position, targetPos.position);

        isFlying = true;
        float runningTime = 0;
        while (isFlying/*Mathf.Abs(distance) >= 1*/ && IsTouchedObject().Equals(false))
        {
            //distance = Vector3.Distance(transform.position, targetPos.position);
            //Debug.Log("distance" + distance);
            //transform.position = Vector3.Lerp(transform.position, targetPos.position, speed * Time.deltaTime);

            runningTime += Time.deltaTime * speed;
            float x = runningTime + startPos.x;
            float y = Mathf.Cos(runningTime);
            transform.position = new Vector3(x, y, transform.position.z);
            IsTouchedObject();
            yield return null;
        }

        EndTheFlight();
        yield return null;
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
                if (onObjectTouched != null)
                {
                    onObjectTouched.Invoke();
                }
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 비행을 끝내다.
    /// </summary>
    private void EndTheFlight()
    {
        isFlying = false;
        transform.position = startPos;
        coroutine = null;
    }

   
}
