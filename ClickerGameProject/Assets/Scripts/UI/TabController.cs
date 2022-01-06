using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    #region Data
    #endregion

    #region Fields
    private RectTransform toggleList;
    private Toggle[] toggles;
    private RectTransform listPanelRect;
    public ScrollRect[] scrollViews;
    #endregion

    #region Unity methods
    private void Awake()
    {
        toggleList = transform.Find("ToggleList").GetComponent<RectTransform>();
        toggles = toggleList.GetComponentsInChildren<Toggle>();
        //listPanelRect = transform.Find("ListPanel").GetComponent<RectTransform>();
        //scrollViews = listPanelRect.GetComponentsInChildren<ScrollRect>();
    }

    private void Start()
    {
        for(int i=0;i<toggles.Length;i++)
        {
            int a = i;
            toggles[i].onValueChanged.AddListener(delegate { OnChanged(toggles[a],scrollViews[a].transform); });
        }
    }
    #endregion

    #region Methods
    #endregion

    #region Private Methods
    public void OnChanged(Toggle _toggle, Transform _scroll)
    {
        CloseAllView();

        if (_toggle.isOn)
        {
            _scroll.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 모든 스크롤뷰를 닫습니다.
    /// </summary>
    private void CloseAllView()
    {
        foreach(ScrollRect scroll in scrollViews)
        {
            scroll.gameObject.SetActive(false);
        }
    }

    #endregion

}


//#region Data
//#endregion

//#region Fields
//#endregion

//#region Unity methods
//#endregion

//#region Methods
//#endregion

//#region Private Methods
//#endregion