using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class UIReuseItemCell : MonoBehaviour
{
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
    #endregion

    public string prefabName { get; set; }
    public int m_Index { get; private set; }
    public virtual void UpdateData(int idx, IReuseCellData CellData)
    {
        m_Index = idx;
        gameObject.name = prefabName + idx.ToString();
    }

    public virtual void RefreshUI(string ClickUniqueID, IReuseCellData ClickContent) { }
    #region Match  LayoutElement
    public LayoutElement m_Element;
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
