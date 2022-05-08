using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class GUIWorkmanScrollView : MonoBehaviour
{
    public UIReuseGrid grid { get; private set; }

    // 레벨,비용, 증가량.. 등 데이터들
    private List<Database.ProductOriginData> database;

    //최대 레벨
    private int maxLevel;

    //셀의 갯수
    [SerializeField]private int cellCount;

    ////셀 상태에 대한 배열
    //private CellState[] states;

    //퍼센트 비율의 리스트
    private List<ValueModifiers> rates
    {
        get { return MoneyManager.Inst.workmansAmount; }
    }

    //강화했는지 체크하는 bool 값 변수
    private bool isEnhance = false;



    #region Property

    //소유한 셀 레벨에 대한 배열
    public int[] curLevelList
    {
        get { return DBManager.Inst.inventory.workmanLevels_owned; }
        set { DBManager.Inst.inventory.workmanLevels_owned = value; }
    }

    #endregion

    #region Unity methods


    private void Awake()
    {
        grid = GetComponentInChildren<UIReuseGrid>();
        grid.onClickEvent += Upgrade;
        grid.InitData();

        //이벤트 등록
        for (int i=0; i< MoneyManager.Inst.workmansAmount.Count;i++)
        {
            //그리드에 데이터 변경
            MoneyManager.Inst.workmansAmount[i].onValueChanged += RefreshCell;
            //MoneyManager.Inst.workmansAmount[i].onValueChanged += (double index)=> grid.SetItem((int)index - 1, GetToCellData((int)index - 1, curLevelList[(int)index - 1]));
            //MoneyManager.Inst.workmansAmount[i].onValueChanged += (double index)=> isEnhance = true;
        }

        LoadData();
        InitializeData();
    }

    public void RefreshCell(double index)
    {
        //그리드에 데이터 변경
        //일꾼 강화는 인덱스 1부터 시작하여 index -1로 설정.
        ProductCellData data = GetToCellData((int)index - 1, curLevelList[(int)index - 1]);
        Debug.Log("RefreshCell" + index);
        Debug.Log("RefreshCell data" + data.name);
        grid.SetItem((int)index - 1, data);
        isEnhance = true;

        //초당획득량 값을 다시 갱신.
        MoneyManager.Inst.SumAllWorkmanAmountToJewelPerSec();
    }


    private void OnEnable()
    {
        //if (grid.m_ScrollRect != null)
        //    grid.SrollToCellWithinTime(curLevelList[0]);

        //강화를 했다면 UI를 강화된 값으로 다시 갱신하기
        if(isEnhance)
        {
            //Refresh Grid
            grid.RefreshAllCell();
            //grid.RefillAllCell();
            isEnhance = false;
        }
    }

    private void OnDestroy()
    {
        grid.onClickEvent -= Upgrade;
        foreach (var workman in MoneyManager.Inst.workmansAmount)
        {
            workman.onValueChanged = null;
        }
    }

    #endregion



    /// <summary>
    /// 인덱스와 레벨에 해당하는 엑셀 데이터를 가져온다.
    /// </summary>
    private Database.ProductOriginData GetDB(int type, int level)
    {
        return database[(type * maxLevel) + level];
    }



    private void LoadData()
    {
        database = DBManager.Inst.GetWorkmanOriginDatas();
        maxLevel = DBManager.Inst.workmanMaxLevel;
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

        //그리드에 데이터를 추가
        for (int i = 0; i < cellCount; ++i)     
        {
            ProductCellData _cell = GetToCellData(i, curLevelList[i]);
            grid.AddItem(_cell);
        }
        grid.RefreshAllCell();
    }



    /// <summary>
    /// 선택한 셀만 업그레이드하고 UI를 갱신한다.
    /// </summary>
    private void Upgrade(int index, int type)
    {
        //예외처리
        if (cellCount == 0) 
            return;

        int curLevel = curLevelList[index];

        //레벨이 0 ~ maxLevel 범위가 아닐 경우 리턴
        if (0 > curLevel || curLevel >= maxLevel) 
            return;

        //0레벨에서 업그레이드할 경우 일꾼을 스폰한다.
        if (curLevel == 0)
        {
            DBManager.Inst.inventory.workmanCount++;

            //구매한 일꾼을 스폰한다.
            if (WorkmanSpawner.onSpawned != null)
                WorkmanSpawner.onSpawned.Invoke(index);

            //구매한 일꾼을 강화할 수 있도록 강화탭에 추가한다.
            if (GUIEnhancementScrollView.onCellAdded != null)
                GUIEnhancementScrollView.onCellAdded.Invoke(index);
        }


        //재화를 소모하고 레벨업을 한다.
        MoneyManager.Inst.SubJewel(GetToCellData(index,curLevel).cost);

        //변화량만큼 초당획득량을 추가
        var currentAmount = GetToCellData(index, curLevel).currentAmount;
        var nextAmount = GetToCellData(index, curLevel).nextAmount;
        MoneyManager.Inst.SumJewelPerSec(nextAmount - currentAmount);

        //레벨 데이터를 DB에 저장
        curLevelList[index] = ++curLevel;
        MoneyManager.Inst.workmansAmount[index].SetBaseValue(GetBaseCurrentAmount(index, curLevel));


        //최대레벨에 도달할 경우
        if (curLevel == maxLevel)
        {
            //이번 셀 UI를 Max상태로 업데이트
            grid.SetItem(index, GetToCellData(index, maxLevel));

            if (index >= cellCount - 1)
            {
                return;
            }
        }

       

        //그리드에 데이터 변경
        ProductCellData data = GetToCellData(index, curLevel);
        grid.SetItem(index, data);
    }


    #region Change to CellData

    private double GetBaseCurrentAmount(int type, int level)
    {
        return GetDB(type, level - 1).amount;
    }

    /// <summary>
    /// 원하는 인덱스의 셀 데이터를 가져온다.
    /// </summary>
    private ProductCellData GetToCellData(int type, int level)
    {
        if (type < 0 || type > cellCount) return null;


        int index;
        ProductCellData _cell = new ProductCellData();
        if(level == 0)
            index = (type*maxLevel)+0;
        else
            index = (type * maxLevel)+(level-1);
        _cell.index = index;
        _cell.id = type;
        _cell.name = database[index].name;
        _cell.imageName = database[index].spriteName;
        _cell.level = level;
        _cell.nextLevel = (level == maxLevel) ? maxLevel : level + 1;

        if (level == 0)
        {
            _cell.currentAmount = 0;
            _cell.nextAmount = GetDB(type, level).amount;
            //_cell.nextAmount = database[index].levelTable[level].amount;
        }
        else
        {
            _cell.currentAmount = GetDB(type, level-1).amount;
            _cell.nextAmount = (level == maxLevel) ? 0 : GetDB(type, level).amount;
            //_cell.currentAmount = database[index].levelTable[level - 1].amount;
            //_cell.nextAmount = (level == maxLevel) ? 0 : database[index].levelTable[level].amount;
            _cell.currentAmount *= rates[type].rateCalc.GetResultRate();
            _cell.nextAmount *= rates[type].rateCalc.GetResultRate();
        }

        if(level == maxLevel)
            _cell.cost = 0;
        else
            _cell.cost = GetDB(type, level).cost;
            //_cell.cost = database[index].levelTable[level].cost;
        _cell.cellState = (level == maxLevel) ? CellState.MaxCompletion : CellState.Unlock;
        return _cell;
    }

    private ProductCellData GetToCellDataOfMaxLevel(int type)
    {
        if (type < 0 || type > cellCount) return null;
        //if (index < 0 || index > database.Count) return null;

        //int maxLevel = database[index].levelTable.Count - 1;

        int index;
        ProductCellData _cell = new ProductCellData();
        _cell.index = index = (type*maxLevel)+ (maxLevel-1);
        _cell.id = database[index].ID;
        _cell.name = database[index].name;
        _cell.imageName = database[index].spriteName;
        _cell.level = maxLevel;
        _cell.nextLevel = _cell.level;
        _cell.currentAmount = GetDB(type, maxLevel).amount;
        //_cell.currentAmount = database[index].levelTable[maxLevel].amount;
        _cell.currentAmount *= rates[index].rateCalc.GetResultRate();
        _cell.nextAmount = 0;
        _cell.cost = 0;
        _cell.cellState = CellState.MaxCompletion;
        return _cell;
    }


    #endregion

}
