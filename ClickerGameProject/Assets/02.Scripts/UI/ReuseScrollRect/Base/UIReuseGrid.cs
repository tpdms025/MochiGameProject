using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReuseGrid : MonoBehaviour, LoopScrollPrefabSource, LoopScrollMultiDataSource
{
    //사용하고자 하는 재사용 스크롤 변수
    [SerializeField] private LoopScrollRectMulti m_LoopScrollRect;

    //재사용 데이터를 보관하는 변수
    [HideInInspector] public ReuseBankBase m_ReuseBank;
    //private UIReuseItemCell[] m_cellList;



    //사용중인 재사용 스크롤 변수
    public LoopScrollRectMulti m_ScrollRect { get; private set; }
    // Is Use MulitiPrefab
    public bool m_IsUseMultiPrefabs = false;
    // Cell Prefab
    public GameObject m_Item;
    // Cell MulitiPrefab
    public List<GameObject> m_ItemList = new List<GameObject>();

    // Implement your own Cache Pool here. The following is just for example.
    private Stack<Transform> pool = new Stack<Transform>();
    private Dictionary<string, Stack<Transform>> m_Pool_Type = new Dictionary<string, Stack<Transform>>();


    //현재 셀의 인덱스 번호
    public int m_ClickIndexID=-1;

    private void Awake()
    {
        if (GetComponentInChildren<LoopScrollRectBase>() == null)
        {
            Debug.LogError("m_ReuseScrollView == null");
            return;
        }
        InitData();
    }

    private void InitData()
    {
        if (m_ReuseBank == null)
        {
            m_ReuseBank = new ReuseBankBase();
            m_ReuseBank.m_CellSizes.Add(new Vector2(120, 200));
        }

        if (m_ScrollRect == null)
        {
            if (m_LoopScrollRect != null)
                m_ScrollRect = m_LoopScrollRect;
            else
                m_ScrollRect = GetComponent<LoopScrollRectMulti>();
        }
        m_ScrollRect.prefabSource = this;
        m_ScrollRect.dataSource = this;
    }

    #region Data Function

    public void AddItem(IReuseCellData cellData, bool Update = false)
    {
        if (!m_ReuseBank.IsInit())
            Debug.LogWarning("m_listData null");

        m_ReuseBank.AddCellData(cellData);
        if (Update)
            RefreshAllCell();
    }
    public void InsertItem(int idx, IReuseCellData cellData, bool Update = false)
    {
        m_ReuseBank.InsertCellData(idx, cellData);
        if (Update)
            RefreshAllCell();
    }
    public void SetItem(int idx, IReuseCellData cellData, bool Update = false)
    {
        m_ReuseBank.SetCellData(idx,cellData);
        if (Update)
            m_ScrollRect.RefillCells(idx - 1, false);

        //RefreshAllCell();
    }
    public void SetItem(List<IReuseCellData> ListData, bool Update = false)
    {
        m_ReuseBank.SetListData(ListData);
        if (Update)
            RefillAllCell();
    }
    public void RemoveItem(IReuseCellData ListData, bool Update = false)
    {
        //if (ListData == null)
        //    return;

        //m_ReuseBank.DeleteCellData(ListData);
        //if (Update)
        //{
        //    m_ScrollRect.ClearCells();
        //    RefillAllCell();
        //}
    }
    public void RemoveFirstItem()
    {
        m_ReuseBank.DelCellDataByIndex(0);

        float offset;
        int LeftIndex = m_ScrollRect.GetFirstItem(out offset);

        m_ScrollRect.ClearCells();
        m_ScrollRect.totalCount = m_ReuseBank.GetListDataLength();
        if (LeftIndex > 0)
            // try keep the same position
            m_ScrollRect.RefillCells(LeftIndex - 1, false, offset);
        else
            m_ScrollRect.RefillCells();
    }

    public void ClearItem(bool Update = false)
    {
        if (!m_ReuseBank.IsInit())
            return;

        m_ReuseBank.ClearListData();
        if (Update)
        {
            m_ScrollRect.ClearCells();
            RefreshAllCell();
        }
    }
    #endregion

    /// <summary>
    /// 모든 셀 데이터를 갱신한다.
    /// </summary>
    public void RefreshAllCell()
    {
        m_ScrollRect.totalCount = m_ReuseBank.GetListDataLength();
        m_ScrollRect.RefreshCells();
    }

    /// <summary>
    /// 모든 셀 데이터를 교체한다.
    /// </summary>
    public void RefillAllCell()
    {
        m_ScrollRect.totalCount = m_ReuseBank.GetListDataLength();
        m_ScrollRect.RefillCells();
    }

    /// <summary>
    /// 셀 데이터를 인덱스 순서로 정렬한다.
    /// </summary>
    /// <param name="isReverse"></param>
    public void SortCellData_IndexOrder(bool isReverse = false)
    {
        // 람다식으로 정렬 구현
        var TempContent = m_ReuseBank.GetListData();
        if (!isReverse)
            TempContent.Sort((x, y) => x.name.CompareTo(y.name));
        else
            TempContent.Sort((x, y) => -x.name.CompareTo(y.name));


        m_ScrollRect.ClearCells();
        RefillAllCell();
    }

    /// <summary>
    /// 타겟 인덱스인 셀로 스크롤한다.
    /// </summary>
    /// <param name="targetIndx"></param>
    /// <param name="moveSpeed"></param>
    public void SrollToCell(int targetIndx, int moveSpeed = -1)
    {
        m_ScrollRect.SrollToCell(targetIndx, moveSpeed);
    }

    /// <summary>
    /// 타겟 인덱스인 셀로 정해진 시간동안 스크롤한다.
    /// </summary>
    /// <param name="targetIndx"></param>
    /// <param name="time"></param>
    public void SrollToCellWithinTime(int targetIndx, float time = 0.5f)
    {
        m_ScrollRect.SrollToCellWithinTime(targetIndx, time);
    }


    #region interface Method
    public virtual GameObject GetObject(int index)
    {
        Transform candidate = null;
        UIReuseItemCell TempScrollIndexCallbackBase = null;
        // Is Use MulitiPrefab
        if (!m_IsUseMultiPrefabs)
        {
            if (pool.Count == 0)
            {
                candidate = Instantiate(m_Item.transform);
            }
            else
            {
                candidate = pool.Pop();
            }

            //코드로 Cell Size 조정하는 법. (현재 프리팹에서 조정함.)
            // One Cell Prefab, Set PreferredSize as runtime.
            TempScrollIndexCallbackBase = candidate.GetComponent<UIReuseItemCell>();
            if (null != TempScrollIndexCallbackBase)
            {
                if (m_ScrollRect.horizontal)
                {
                    float RandomWidth = m_ReuseBank.GetCellPreferredSize(index).x;
                    TempScrollIndexCallbackBase.SetLayoutElementPreferredWidth(RandomWidth);
                }

                if (m_ScrollRect.vertical)
                {
                    float RandomHeight = m_ReuseBank.GetCellPreferredSize(index).y;
                    TempScrollIndexCallbackBase.SetLayoutElementPreferredHeight(RandomHeight);
                }
            }
        }
        else
        {
            // Cell MulitiPrefab, Get Cell Preferred Type by custom data
            int CellTypeIndex = m_ReuseBank.GetCellPreferredTypeIndex(index);
            if (m_ItemList.Count <= CellTypeIndex)
            {
                Debug.LogWarningFormat("TempPrefab is null! CellTypeIndex: {0}", CellTypeIndex);
                return null;
            }
            var TempPrefab = m_ItemList[CellTypeIndex];

            Stack<Transform> TempStack = null;
            if (!m_Pool_Type.TryGetValue(TempPrefab.name, out TempStack))
            {
                TempStack = new Stack<Transform>();
                m_Pool_Type.Add(TempPrefab.name, TempStack);
            }

            if (TempStack.Count == 0)
            {
                candidate = Instantiate(TempPrefab).GetComponent<Transform>();
                TempScrollIndexCallbackBase = candidate.GetComponent<UIReuseItemCell>();
                if (null != TempScrollIndexCallbackBase)
                {
                    TempScrollIndexCallbackBase.prefabName = TempPrefab.name;
                }
            }
            else
            {
                candidate = TempStack.Pop();
                candidate.gameObject.SetActive(true);
            }
        }

        TempScrollIndexCallbackBase = candidate.gameObject.GetComponent<UIReuseItemCell>();
        if (null != TempScrollIndexCallbackBase)
        {
            //TempScrollIndexCallbackBase.SetUniqueID(m_ReuseBank.GetCellData(index).UniqueID);

            TempScrollIndexCallbackBase.onClick_InitOnStart.RemoveAllListeners();
            TempScrollIndexCallbackBase.onClick_InitOnStart.AddListener(() => OnButtonScrollIndexCallbackClick(TempScrollIndexCallbackBase, index));

            TempScrollIndexCallbackBase.onClick_Index = null;
            TempScrollIndexCallbackBase.onClick_Index += onClickEvent;
        }

        return candidate.gameObject;
    }


    public virtual void ReturnObject(Transform trans)
    {
        trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
        trans.gameObject.SetActive(false);
        trans.SetParent(transform, false);
        // Is Use MulitiPrefab
        if (!m_IsUseMultiPrefabs)
        {
            pool.Push(trans);
        }
        else
        {
            // Use PrefabName as Key for Pool Manager
            UIReuseItemCell TempScrollIndexCallbackBase = trans.GetComponent<UIReuseItemCell>();
            if (null == TempScrollIndexCallbackBase)
            {
                // Use `DestroyImmediate` here if you don't need Pool
                DestroyImmediate(trans.gameObject);
                return;
            }

            Stack<Transform> TempStack = null;
            if (m_Pool_Type.TryGetValue(TempScrollIndexCallbackBase.prefabName, out TempStack))
            {
                TempStack.Push(trans);
            }
            else
            {
                TempStack = new Stack<Transform>();
                TempStack.Push(trans);

                m_Pool_Type.Add(TempScrollIndexCallbackBase.prefabName, TempStack);
            }
        }
    }

    public virtual void ProvideData(Transform transform, int idx)
    {
        //??
        //transform.SendMessage("UpdateData", idx);

        // Use direct call for better performance
        transform.GetComponent<UIReuseItemCell>()?.UpdateData(idx, m_ReuseBank.GetCellData(idx), m_ClickIndexID);
    }

    #endregion
    public event Action<int> onClickEvent;
    private void OnButtonScrollIndexCallbackClick(UIReuseItemCell ScrollIndexCallback, int ClickIndexID)
    {
        //Debug.LogWarningFormat("InitOnStartMulti => Click index: {0}", ClickIndexID);

        m_ClickIndexID = ClickIndexID;
        //m_ClickObject = content;

        foreach (var TempScrollIndexCallback in m_LoopScrollRect.content.GetComponentsInChildren<UIReuseItemCell>())
        {
            IReuseCellData content = m_ReuseBank.GetCellData(TempScrollIndexCallback.m_Index);
            TempScrollIndexCallback.RefreshUI(ClickIndexID, content);
        }
    }
}

