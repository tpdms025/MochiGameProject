using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class TouchController : MonoBehaviour/*, IPointerDownHandler, IPointerUpHandler*/
{
    //최대 터치 횟수
    private const int maxTouchCount = 30;

    //유저가 터치할 때 발생하는 이벤트 델리게이트 (누적 터치횟수, 최대 터치횟수)
    public static event Action<int, int> onUserTouched;

    //누적 터치 횟수를 초기화 할 때 발생하는 이벤트 델리게이트
    public static Action onTouchReset;

    //피버효과를 발동할 수 있는지
    public bool feverTrigger { get; private set; }

    //자동채굴의 여부
    public bool isAutomatic
    {
        get { return DBManager.Inst.PlayerData.isAutoMining; }
    }



    //누적 터치 횟수
    public int touchCount
    {
        get { return DBManager.Inst.PlayerData.touchCount; }
        private set
        {
            //유저가 터치할 시 이벤트 발생
            if (onUserTouched != null)
            {
                onUserTouched.Invoke(value, maxTouchCount);
            }
            DBManager.Inst.PlayerData.touchCount = value;
        }
    }



    private void Start()
    {
        onTouchReset += ResetFever;
        ResetFever();
        //Input.multiTouchEnabled = false;

        StartCoroutine(Loop_TouchEvent());
    }

    #region 터치 인식 이전버전
    //public void OnPointerDown(PointerEventData data)
    //{
    //    OnTouch();
    //    CreateParticle(data.pressPosition, transform);
    //    CreateGoldPopup(MoneyManager.Instance.JewelPerClick.ToString(), data.pressPosition, transform);

    //    //Debug.Log("OnPointerDown");
    //}
    //public void OnPointerUp(PointerEventData data)
    //{
    //    //Debug.Log("OnPointerUp");
    //    //OnTouch();

    //    //CreateParticle(data.pressPosition, transform);
    //    //CreateGoldPopup(MoneyManager.Instance.JewelPerClick.ToString(), data.pressPosition, transform);

    //}
    #endregion

    /// <summary>
    /// 영역에 터치를 한다.
    /// </summary>
    public void OnTouch(Vector2 pos)
    {
        //파티클 생성
        CreateParticle(MoneyManager.Instance.strJewelPerTouch, pos, transform);
        //보석 증가
        MoneyManager.Instance.AddJewel(MoneyManager.Instance.JewelPerTouch);
    }



    private void Initialize()
    {
    }


    /// <summary>
    /// 터치 이벤트를 검사한다.
    /// </summary>
    private IEnumerator Loop_TouchEvent()
    {
        while (true)
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch tempTouchs = Input.GetTouch(i);

                    if (EventSystem.current.IsPointerOverGameObject(tempTouchs.fingerId))
                    {
                        Debug.Log("Touch UI!");
                        continue;
                    }

                    if (tempTouchs.phase.Equals(TouchPhase.Began))
                    {
                        OnTouch(tempTouchs.position);

                        //피버 판정
                        if (feverTrigger == false)
                        {
                            touchCount = Mathf.Clamp(touchCount + 1, 0, maxTouchCount);
                            feverTrigger = CheckFever();
                        }
                    }
                }
            }

            yield return null;
        }
    }

    /// <summary>
    /// 터치 이펙트를 생성한다.
    /// </summary>
    private void CreateParticle(string jewelStr, Vector2 position, Transform parents)
    {
        //오브젝트 풀에서 꺼내온다.
        GameObject obj = ObjectPool.Instance.PopFromPool("TouchEffect", true, transform);
        obj.transform.position = position;
        obj.GetComponent<TouchEffect>().Init(jewelStr);
    }

    #region Fever

    /// <summary>
    /// 피버의 발동 조건을 확인한다.
    /// </summary>
    private bool CheckFever()
    {
        if (touchCount >= maxTouchCount)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 피버모드를 리셋한다.
    /// </summary>
    private void ResetFever()
    {
        feverTrigger = false;
        touchCount = 0;
    }

    #endregion

    #region Automatic Mining

    /// <summary>
    ///  자동채굴을 한다.
    /// </summary>
    private IEnumerator Loop_AutoMining()
    {
        float t = 0.0f;
        while (true)
        {
            if (isAutomatic)
            {
                t += Time.deltaTime;
                if (t > 1.0f)
                {
                    t = 0.0f;
                    //보석 증가
                    MoneyManager.Instance.AddJewel(MoneyManager.Instance.JewelPerTouch * 3);
                }
            }
            yield return null;
        }
    }


    #endregion

}
