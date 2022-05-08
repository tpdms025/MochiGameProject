using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

// 메인 클래스. 여기서 grid에 데이터를 추가시켜준다.
public class GUIOreScrollView : MonoBehaviour
{
    public UIReuseGrid grid { get; private set; }

    // 레벨,비용, 증가량.. 등 데이터들
    private List<Database.ProductOriginData> database;

    //최대 레벨
    private int maxLevel;

    //셀의 갯수
    private int cellCount;

    //셀 상태에 대한 배열
    //선택한 셀이 max인지 구별하기 위한 변수이다. (max여야 다음 셀이 잠금해제 되는 형식)
    //private CellState[] states;


    #region Property

    //마지막으로 소유한 Cell의 인덱스 번호
    public int curIndex { 
        get { return DBManager.Inst.inventory.oreIdx_lastOwned; }
        set { DBManager.Inst.inventory.oreIdx_lastOwned = value; }
    }

    //마지막으로 소유한 Cell의 레벨
    public int curLevel {
        get { return DBManager.Inst.inventory.oreLevel_lastOwned; }
        set { DBManager.Inst.inventory.oreLevel_lastOwned = value; }
    }

    #endregion


    #region Unity methods

    private void Awake()
    {
        grid = GetComponentInChildren<UIReuseGrid>();
        grid.onClickEvent += Upgrade;
        grid.InitData();

        LoadData();
        InitializeData();
    }


    private void OnEnable()
    {
        //if(grid.m_ScrollRect != null)
        //    grid.SrollToCellWithinTime(curIndex);
    }

    private void OnDestroy()
    {
        grid.onClickEvent -= Upgrade;
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
        database = DBManager.Inst.GetOreOriginDatas();
        maxLevel = DBManager.Inst.oreMaxLevel;
        cellCount = database.Count/maxLevel;
        //states = new CellState[cellCount];
        grid.m_ClickIndexID = curIndex;
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
            ProductCellData _cell;
            //CellState state;
            if (i < curIndex)                       //max state
            {
                //state = CellState.MaxCompletion;
                _cell = GetCellDataOfMaxLevel(i);
                    grid.AddItem(_cell);
            }
            else if (i == curIndex)                 //unlock state
            {
                //state = CellState.Unlock;
                if (curLevel == maxLevel)
                {
                    _cell = GetCellDataOfMaxLevel(i);
                    grid.AddItem(_cell);
                    grid.AddItem(GetToCellData(++i, 0));
                }
                else
                {
                    _cell = GetToCellData(i, curLevel);
                    grid.AddItem(_cell);
                }
            }
            else                                   //lock state
            {
                //state = CellState.Lock;
                _cell = GetToCellData(i, 0, CellState.Lock);
                grid.AddItem(_cell);
            }
            //_cell.cellState = states[i] = state;
            //grid.AddItem(_cell);
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
        
        int level = curLevel;
        if (level == maxLevel)
            level = 0;

        //레벨이 0 ~ maxLevel 범위가 아닐 경우 리턴
        if (0 > level || level >= maxLevel) 
            return;


        //재화를 소모하고 레벨업을 한다.
        MoneyManager.Inst.SubJewel(GetToCellData(index, level).cost); 
        
        //변화량만큼 보석재화 획득
        var prevAmount = GetToCellData(index, level).currentAmount;
        var addAmount = GetToCellData(index, level).nextAmount;
        MoneyManager.Inst.SumJewelPerTouch(addAmount - prevAmount);


        //구매광석을 DB에 저장
        curIndex = index;
        curLevel = ++level;

        //0레벨에서 업그레이드할 경우 광석을 교체한다.
        if (level == 1)
        {
            if (OreWorld.onOreChanged != null)
                OreWorld.onOreChanged(false);
        }

        //최대레벨에 도달할 경우
        if (level == maxLevel)
        {
            //이번 셀 UI를 Max상태로 업데이트
            //states[index] = CellState.MaxCompletion;
            grid.SetItem(index, GetCellDataOfMaxLevel(index));

            if(index >= cellCount - 1)
            {
                return;
            }

            //다음 셀로 넘어가기
            level = 0;
            ++index;
        }


        ////잠금해제 상태로 업데이트 (max일 경우, 다음 셀이 해당)
        //states[index] = CellState.Unlock;

        //그리드에 데이터 변경
        ProductCellData data = GetToCellData(index, level, CellState.Unlock);
        //ProductCellData data = GetToCellData(index, level);
        grid.SetItem(index, data);
    }


    #region Change to CellData

    /// <summary>
    /// 원하는 인덱스의 셀 데이터를 가져온다.
    /// </summary>
    private ProductCellData GetToCellData(int type, int level,CellState state = CellState.Unlock)
    {
        if (type < 0 || type > cellCount) return null;

        int index;
        ProductCellData _cell = new ProductCellData();
        if (level == 0)
            index = (type * maxLevel) + 0;
        else
            index = (type * maxLevel) + (level - 1);
        _cell.index = index;
        _cell.id = database[index].ID;
        _cell.name = database[index].name;
        _cell.imageName = database[index].spriteName;
        _cell.level = level;
        _cell.nextLevel = (level == maxLevel) ? maxLevel : level+1;

        if (level == 0)
        {
            _cell.currentAmount = 0;
            _cell.nextAmount = GetDB(type, level).amount;
        }
        else
        {
            _cell.currentAmount = GetDB(type, level-1).amount;
            _cell.nextAmount = (level == maxLevel) ? 0 : GetDB(type, level).amount;
        }
        
        if(level == maxLevel)
            _cell.cost = 0;
        else
            _cell.cost = GetDB(type, level).cost;
        _cell.cellState = state;
        //_cell.cellState = states[type];
        return _cell;
    }

    private ProductCellData GetCellDataOfMaxLevel(int type)
    {
        if (type < 0 || type > cellCount) return null;

        int index;
        ProductCellData _cell = new ProductCellData();
        _cell.index = index = (type*maxLevel)+ (maxLevel-1);
        _cell.id = index;
        _cell.name = database[index].name;
        _cell.imageName = database[index].spriteName;
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

