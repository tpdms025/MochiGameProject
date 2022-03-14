using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class JewelTouchControll : MonoBehaviour/*, IPointerDownHandler, IPointerUpHandler*/
{
    private GameObject vfx_touch = null;
    private GameObject floatingGold = null;


    private void Start()
    {
        vfx_touch = Resources.Load("Prefabs/VFX_TouchEvent") as GameObject;
        floatingGold = Resources.Load("Prefabs/FloatingGold") as GameObject;

        Input.multiTouchEnabled = false;


        StartCoroutine(Loop_TouchEvent());
    }

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

    /// <summary>
    /// ��ġ�� �� ������ �����Ѵ�.
    /// </summary>
    public void OnTouch()
    {
        MoneyManager.Instance.AddJewel( MoneyManager.Instance.JewelPerTouch);
    }


    /// <summary>
    /// ��ġ �̺�Ʈ�� �������Ӵ� Ȯ���Ѵ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Loop_TouchEvent()
    {
        while(true)
        {
            if(Input.touchCount > 0 )
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
                        OnTouch();
                        //CreateParticle(tempTouchs.position, transform);
                        //CreateGoldPopup(MoneyManager.Instance.JewelPerClick.ToString(), tempTouchs.position, transform);
                    }
                }
            }
            yield return null;
        }
    }


    /// <summary>
    /// ��ġ�� �� ��ƼŬ�� �����Ѵ�.
    /// </summary>
    /// <param name="position"></param>
    private void CreateParticle(Vector2 position, Transform parents)
    {
        GameObject vfx = Instantiate(vfx_touch);
        vfx.transform.SetParent(parents);
        vfx.SetActive(true);
        vfx.GetComponent<RectTransform>().position = position;
    }

    /// <summary>
    /// ������ �˾��� �����Ѵ�.
    /// </summary>
    /// <param name="goldValue"></param>
    /// <param name="position"></param>
    /// <param name="parents"></param>
    private void CreateGoldPopup(string goldValue, Vector2 position, Transform parents)
    {
        GameObject popup = Instantiate(floatingGold);
        popup.transform.SetParent(parents);
        popup.SetActive(true);
        popup.GetComponent<RectTransform>().position = position;
    }
}
