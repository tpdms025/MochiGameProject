using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{

    #region Fields

    private RectTransform toggleParent;
    private Toggle[] m_toggles;
    public GameObject[] m_pageList;

    #endregion

    #region Unity methods
    private void Awake()
    {
        toggleParent = transform.Find("ToggleList").GetComponent<RectTransform>();
        m_toggles = toggleParent.GetComponentsInChildren<Toggle>();
        //listPanelRect = transform.Find("ListPanel").GetComponent<RectTransform>();
        //scrollViews = listPanelRect.GetComponentsInChildren<ScrollRect>();
    }

    private void Start()
    {
        if (m_toggles.Length.Equals(m_pageList.Length))
        {
            for (int i = 0; i < m_toggles.Length; i++)
            {
                int a = i;
                m_toggles[i].onValueChanged.AddListener(delegate { OnToggleChanged(m_toggles[a], m_pageList[a].transform); });
                
                if(m_toggles[i].isOn)
                {
                    OnToggleChanged(m_toggles[a], m_pageList[a].transform);
                }
            }
        }
    }
    #endregion

    #region Methods
    #endregion

    #region Private Methods
    /// <summary>
    /// 토글 값이 변경될 때 호출된다.
    /// </summary>
    /// <param name="_toggle"></param>
    /// <param name="_scroll"></param>
    public void OnToggleChanged(Toggle _toggle, Transform _scroll)
    {
        CloseAllView();

        if (_toggle.isOn)
        {
            _scroll.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 모든 스크롤뷰를 닫는다.
    /// </summary>
    private void CloseAllView()
    {
        foreach(GameObject scroll in m_pageList)
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