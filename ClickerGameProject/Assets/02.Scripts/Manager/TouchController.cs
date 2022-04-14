using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class TouchController : MonoBehaviour/*, IPointerDownHandler, IPointerUpHandler*/
{
    //�ִ� ��ġ Ƚ��
    private const int maxTouchCount = 30;

    //������ ��ġ�� �� �߻��ϴ� �̺�Ʈ ��������Ʈ (���� ��ġȽ��, �ִ� ��ġȽ��)
    public static event Action<int, int> onUserTouched;

    //���� ��ġ Ƚ���� �ʱ�ȭ �� �� �߻��ϴ� �̺�Ʈ ��������Ʈ
    public static Action onTouchReset;

    //�ǹ�ȿ���� �ߵ��� �� �ִ���
    public bool feverTrigger { get; private set; }

    //�ڵ�ä���� ����
    public bool isAutomatic
    {
        get { return DBManager.Inst.PlayerData.isAutoMining; }
    }



    //���� ��ġ Ƚ��
    public int touchCount
    {
        get { return DBManager.Inst.PlayerData.touchCount; }
        private set
        {
            //������ ��ġ�� �� �̺�Ʈ �߻�
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

    #region ��ġ �ν� ��������
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
    /// ������ ��ġ�� �Ѵ�.
    /// </summary>
    public void OnTouch(Vector2 pos)
    {
        //��ƼŬ ����
        CreateParticle(MoneyManager.Instance.strJewelPerTouch, pos, transform);
        //���� ����
        MoneyManager.Instance.AddJewel(MoneyManager.Instance.JewelPerTouch);
    }



    private void Initialize()
    {
    }


    /// <summary>
    /// ��ġ �̺�Ʈ�� �˻��Ѵ�.
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

                        //�ǹ� ����
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
    /// ��ġ ����Ʈ�� �����Ѵ�.
    /// </summary>
    private void CreateParticle(string jewelStr, Vector2 position, Transform parents)
    {
        //������Ʈ Ǯ���� �����´�.
        GameObject obj = ObjectPool.Instance.PopFromPool("TouchEffect", true, transform);
        obj.transform.position = position;
        obj.GetComponent<TouchEffect>().Init(jewelStr);
    }

    #region Fever

    /// <summary>
    /// �ǹ��� �ߵ� ������ Ȯ���Ѵ�.
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
    /// �ǹ���带 �����Ѵ�.
    /// </summary>
    private void ResetFever()
    {
        feverTrigger = false;
        touchCount = 0;
    }

    #endregion

    #region Automatic Mining

    /// <summary>
    ///  �ڵ�ä���� �Ѵ�.
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
                    //���� ����
                    MoneyManager.Instance.AddJewel(MoneyManager.Instance.JewelPerTouch * 3);
                }
            }
            yield return null;
        }
    }


    #endregion

}
