using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBat : MonoBehaviour
{
    //���� ��ġ
    [SerializeField] private Transform targetPos;

    //���� ��ġ
    private Vector3 startPos;

    //�̵� �ӵ�
    [SerializeField] private float speed = 2.0f;

    //���� �ִ��� ����
    [SerializeField] private bool isFlying = false;

    private IEnumerator coroutine = null;

    //��ü ��ġ�� �̺�Ʈ
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
    /// ��ü�� �̵��Ѵ�.
    /// </summary>
    public void MoveObject()
    {
        if (coroutine != null)
            return;

        coroutine = MoveCosinePath();
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// �ڻ��� �̵���η� �̵��Ѵ�.
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
    /// ������ ������.
    /// </summary>
    private void EndTheFlight()
    {
        isFlying = false;
        transform.position = startPos;
        coroutine = null;
    }

   
}
