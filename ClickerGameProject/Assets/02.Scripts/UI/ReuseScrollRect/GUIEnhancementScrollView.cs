using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class GUIEnhancementScrollView : MonoBehaviour
{
    private AbilityEnhance _system;

    public UIReuseGrid grid { get; private set; }

    // 레벨,비용, 증가량.. 등 데이터들
    private List<Database.ProductOriginData_> database;

    //최대 레벨
    private int maxLevel;


    //셀의 갯수
    private int cellCount;

    ////셀 상태에 대한 배열
    //private CellState[] states;

    //셀이 추가될때 호출되는 이벤트 (int : index)
    public static System.Action<int> onCellAdded;

    //추가된 셀이 있는지 체크하는 bool 값 변수
    private bool added = false;



    #region Property

    //소유한 셀 레벨에 대한 배열
    public int[] curLevelList
    {
        get { return DBManager.Inst.inventory.enhanceLevels_owned; }
        set { DBManager.Inst.inventory.enhanceLevels_owned = value; }
    }

    #endregion


    #region Unity methods

    private void Awake()
    {
        _system = GetComponent<AbilityEnhance>();
        grid = GetComponentInChildren<UIReuseGrid>();
        grid.InitData();

        //이벤트 등록
        grid.onClickEvent += Upgrade;
        onCellAdded += AddCell;

        LoadData();
        InitializeData();
    }


    private void OnEnable()
    {
        //if (grid.m_ScrollRect != null)
        //    grid.SrollToCellWithinTime(curLevelList[0]);

        if (added)
        {
            //Refresh Grid
            grid.RefreshAllCell();
            grid.SortCellData_IdOrder();
            added = false;
        }
    }

    private void OnDestroy()
    {
        grid.onClickEvent -= Upgrade;
        onCellAdded -= AddCell;
    }

    #endregion



    /// <summary>
    /// 일꾼 강화 슬롯을 추가한다.
    /// </summary>
    public void AddCell(int workmanIdx)
    {
        //예외처리 - 이미 일꾼을 소유하고 있을 경우 제외
        if (DBManager.Inst.inventory.workmanLevels_owned[workmanIdx] != 0)
            return;

        //일꾼강화의 인덱스가 1번부터라서 1+i
        grid.AddItem(GetToCellData(1 + workmanIdx, 0));
        added = true;
    }


    /// <summary>
    /// 인덱스와 레벨에 해당하는 엑셀 데이터를 가져온다.
    /// </summary>
    private Database.ProductOriginData_ GetDB(int type, int level)
    {
        return database[(type * maxLevel) + level];
    }




    private void LoadData()
    {
        database = DBManager.Inst.GetEnhancementOriginDatas();
        maxLevel = DBManager.Inst.enhancementMaxLevel;
        cellCount = database.Count/maxLevel;
        //states = new CellState[cellCount];
    }


    /// <summary>
    /// 데이터를 초기화한다. (처음데이터)
    /// </summary>
    private void InitializeData()
    {
        if (database == null)
            return;

        //소유하지 않은 일꾼은 강화를 할 수 없도록 스크롤뷰에 데이터를 추가하지 않는다.
        Queue<int> ignoreIdx= new Queue<int>();
        for(int i=0; i< DBManager.Inst.inventory.workmanLevels_owned.Length; i++)
        {
            if(DBManager.Inst.inventory.workmanLevels_owned[i] == 0)
            {
                //일꾼강화가 1번부터라서 1+i
                ignoreIdx.Enqueue(1+i);
            }
        }

        //그리드에 데이터를 추가
        for (int i = 0; i < cellCount; ++i)     
        {
            //무시해야할 인덱스라면 데이터를 추가하지 않는다.
            if(ignoreIdx.Count > 0)
            {
                if(ignoreIdx.Peek() == i)
                {
                    ignoreIdx.Dequeue();
                    continue;
                }
            }

            ProductCellData _cell = GetToCellData(i, curLevelList[i]);
            grid.AddItem(_cell);
        }
        grid.RefreshAllCell();
    }

    /// <summary>
    /// 업그레이드한 셀만 갱신한다.
    /// </summary>
    private void Upgrade(int index,int type)
    {
        //예외처리
        if (cellCount == 0) 
            return;

        int curLevel = curLevelList[type];
        Debug.Log("index" + index + "type" + type);

        //레벨이 0 ~ maxLevel 범위가 아닐 경우 리턴
        if (0 > curLevel || curLevel >= maxLevel) 
            return;


        //재화를 소모하고 레벨업을 한다.
        MoneyManager.Inst.SubJewel(GetToCellData(type, curLevel).cost);
        //MoneyManager.Inst.SubJewel(database[index].levelTable[curLevel].cost);

        //강화 능력 추가
        var addAmount = GetToCellData(type, curLevel).nextAmount;
        //float addAmount = (float)database[index].levelTable[curLevel].amount;
        _system.Enhance(type, (float)addAmount);

        //레벨 데이터를 DB에 저장
        curLevelList[type] = ++curLevel;

        //최대레벨에 도달할 경우
        if (curLevel == maxLevel)
        {
            //이번 셀 UI를 Max상태로 업데이트
            //states[index] = CellState.MaxCompletion;
            grid.SetItem(index, GetToCellData(type, maxLevel));

            if (type >= cellCount - 1)
            {
                return;
            }
        }


        //그리드에 데이터 변경
        grid.SetItem(index, GetToCellData(type, curLevel));
    }


    #region Change to CellData

    /// <summary>
    /// 원하는 인덱스의 셀 데이터를 가져온다.
    /// </summary>
    private ProductCellData GetToCellData(int type, int level)
    {
        if (type < 0 || type > cellCount) return null;

        int index;
        ProductCellData _cell = new ProductCellData();
        if (level == 0)
            index = (type * maxLevel) + 0;
        else
            index = (type * maxLevel) + (level - 1);
        _cell.index = index;
        _cell.id = type;
        _cell.name = database[index].name;
        _cell.imageName = database[index].spriteName;
        _cell.description = database[index].description;
        _cell.level = level;
        _cell.nextLevel = (level == maxLevel) ? maxLevel : level + 1;

        if(level == 0)
        {
            _cell.currentAmount = 0;
            _cell.nextAmount = GetDB(type, level).amount;
        }
        else
        {
            _cell.currentAmount = GetDB(type, level - 1).amount;
            _cell.nextAmount = (level == maxLevel) ? 0 : GetDB(type, level).amount;
        }

        if(level == maxLevel)
            _cell.cost = 0;
        else
            _cell.cost = GetDB(type, level).cost;
        _cell.cellState = (level == maxLevel) ? CellState.MaxCompletion : CellState.Unlock;
        return _cell;
    }

    private ProductCellData GetCellDataOfMaxLevel(int type)
    {
        if (type < 0 || type > cellCount) return null;

        int index;
        ProductCellData _cell = new ProductCellData();
        _cell.index = index = (type*maxLevel)+ (maxLevel-1);
        _cell.id = database[index].ID;
        _cell.name = database[index].name;
        _cell.imageName = database[index].spriteName;
        _cell.description = database[index].description;
        _cell.level = maxLevel;
        _cell.nextLevel = _cell.level;
        _cell.currentAmount = GetDB(type, maxLevel).amount;
        _cell.nextAmount = 0;
        _cell.cost = 0;
        _cell.cellState = CellState.MaxCompletion;
        return _cell;
    }


    #endregion


}
