using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class GoldClickControll : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    private GameObject vfx_touch = null;
    private GameObject floatingGold = null;

    private void Start()
    {
        vfx_touch = Resources.Load("Prefabs/VFX_TouchEvent") as GameObject;
        floatingGold = Resources.Load("Prefabs/FloatingGold") as GameObject;
    }

    public void OnPointerDown(PointerEventData data)
    {
    }
    public void OnPointerUp(PointerEventData data)
    {
        OnTouch();

        CreateParticle(data.pressPosition,transform);
        CreateGoldPopup(GoldManager.Instance.GoldPerClick.ToString(), data.pressPosition, transform);
    }

    /// <summary>
    /// ��ġ�� �� ������ �����Ѵ�.
    /// </summary>
    private void OnTouch()
    {
        GoldManager.Instance.AddGold(GoldManager.Instance.GoldPerClick);
        Debug.Log(GoldManager.Instance.GoldPerClick);

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

    private void CreateGoldPopup(string goldValue,Vector2 position, Transform parents)
    {
        GameObject popup = Instantiate(floatingGold);
        popup.transform.SetParent(parents);
        popup.SetActive(true);
        popup.GetComponent<RectTransform>().position = position;
    }
}
