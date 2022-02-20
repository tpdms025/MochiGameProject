using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class UIReuseItemCell : MonoBehaviour
{
    [SerializeField] protected Button m_Button;
    public LayoutElement m_Element;

    public string prefabName { get; set; }
    public int m_Index { get; private set; }


    public virtual void UpdateData(int idx, IReuseCellData CellData, int ClickIndexID = -1)
    {
        m_Index = idx;
        gameObject.name = prefabName + idx.ToString();
    }

    public virtual void RefreshUI(int ClickIndexID, IReuseCellData ClickContent) { }
    
    #region Click Event
    [Serializable]
    public class ButtonClickedEvent : UnityEvent { }

    // Event delegates triggered on click for Base.
    [FormerlySerializedAs("onClick_InitOnStart")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick_InitOnStart = new ButtonClickedEvent();
    public ButtonClickedEvent onClick_InitOnStart
    {
        get { return m_OnClick_InitOnStart; }
        set { m_OnClick_InitOnStart = value; }
    }


    // Event delegates triggered on click for Custom.
    [FormerlySerializedAs("onClick_Custom")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick_Custom = new ButtonClickedEvent();
    public ButtonClickedEvent onClick_Custom
    {
        get { return m_OnClick_Custom; }
        set { m_OnClick_Custom = value; }
    }

    //현재 인덱스 값을 반환하는 델리게이트
    public Action<int> onClick_Index;


    protected virtual void Awake()
    {
        m_Button.onClick.AddListener(OnButtonClickCallBack);
    }

    protected virtual void OnDestroy()
    {
        m_Button.onClick.RemoveAllListeners();
    }

    private void OnButtonClickCallBack()
    {
        if(m_OnClick_Custom != null)
            m_OnClick_Custom.Invoke();
        if(m_OnClick_InitOnStart != null)
            m_OnClick_InitOnStart.Invoke();
    }
    #endregion
    
    #region Match  Layout Element

    // Set Element PreferredWidth
    public virtual void SetLayoutElementPreferredWidth(float value)
    {
        m_Element.preferredWidth = value;
    }

    // Set Element PreferredHeight
    public virtual void SetLayoutElementPreferredHeight(float value)
    {
        m_Element.preferredHeight = value;
    }
    #endregion
}
