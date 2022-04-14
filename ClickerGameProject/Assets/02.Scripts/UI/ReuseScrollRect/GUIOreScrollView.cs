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

    //셀의 갯수
    private int cellCount;

    //셀 상태에 대한 배열
    //선택한 셀이 max인지 구별하기 위한 변수이다. (max여야 다음 셀이 잠금해제 되는 형식)
    private CellState[] states;



    #region Property

    //마지막으로 소유한 Cell의 인덱스 번호
    public int curIndex { 
        get { return DBManager.Inst.PlayerData.idx_lastOwned; }
        set { DBManager.Inst.PlayerData.idx_lastOwned = value; }
    }

    //마지막으로 소유한 Cell의 레벨
    public int curLevel {
        get { return DBManager.Inst.PlayerData.level_lastOwned; }
        set { DBManager.Inst.PlayerData.level_lastOwned = value; }
    }

    #endregion



    private void Awake()
    {
        grid = GetComponentInChildren<UIReuseGrid>();
        grid.onClickEvent += Upgrade;
    }

    private void Start()
    {
        LoadData();

        InitializeData();
    }
    private void OnEnable()
    {
        if(grid.m_ScrollRect != null)
            grid.SrollToCellWithinTime(curIndex);
    }

    private void LoadData()
    {
        //임시 데이터 (나중에 데이터 로드할 것)
        database = DBManager.Inst.GetOreOriginDatas();
        cellCount = database.Count;
        grid.m_ClickIndexID = curIndex;
    }
    

    /// <summary>
    /// 데이터를 초기화한다. (처음데이터)
    /// </summary>
    private void InitializeData()
    {
        //curIndex = 0;
        //curLevel = 0;
        states = new CellState[cellCount];

        //그리드에 데이터를 추가
        for (int i = 0; i < cellCount; ++i)
        {
            ProductCellData _cell;
            CellState state;
            if (i < curIndex)                       //max state
            {
                _cell = GetCellDataOfMaxLevel(i);
                state = CellState.MaxCompletion;
            }
            else if (i == curIndex)                 //unlock state
            {
                _cell = temp(i, curLevel);
                state = CellState.Unlock;
            }
            else                                   //lock state
            {
                _cell = temp(i, 0);
                state = CellState.Lock;
            }
            _cell.cellState = states[i] = state;
            grid.AddItem(_cell);
        }
        grid.RefreshAllCell();
    }


    /// <summary>
    /// 업그레이드한 셀만 갱신한다.
    /// </summary>
    private void Upgrade(int index)
    {
        //예외처리
        if (cellCount == 0) 
            return;
        
        //구매광석을 DB에 저장
        curIndex = index;

        int maxLevel = database[index].levelTable.Count - 1;

        //레벨이 0 ~ maxLevel 범위가 아닐 경우 리턴
        if (0 > curLevel || curLevel >= maxLevel) 
            return;

        //0레벨에서 업그레이드할 경우 광석을 교체한다.
        if (curLevel == 0)
        {
            if (Ore.onOreChanged != null)
                Ore.onOreChanged(false);
        }

        //재화 소모
        MoneyManager.Instance.SubJewel(database[index].levelTable[curLevel].cost);      //재화 소모
        BigInteger prevAmount = database[index].levelTable[curLevel].amountPerTouch;
        //레벨업
        ++curLevel;
        BigInteger addAmount = database[index].levelTable[curLevel].amountPerTouch;
        //변화량만큼 재화 추가
        MoneyManager.Instance.AddJewelPerClick(addAmount - prevAmount);

        //최대레벨에 도달할 경우
        if (curLevel == maxLevel && index < cellCount - 1)
        {
            //이번 셀 UI를 Max상태로 업데이트
            states[index] = CellState.MaxCompletion;
            grid.SetItem(index, GetCellDataOfMaxLevel(index));

            //다음 셀로 넘어가기
            curLevel = 0;
            ++index;
        }

        //잠금해제 상태로 업데이트 (max일 경우, 다음 셀이 해당)
        states[index] = CellState.Unlock;

        //그리드에 데이터 변경
        ProductCellData data = temp(index, curLevel);
        grid.SetItem(index, data);
    }


    #region Change to CellData

    /// <summary>
    /// 원하는 인덱스의 셀 데이터를 가져온다.
    /// </summary>
    private ProductCellData temp(int index, int level)
    {
        if (index < 0 || index > database.Count) return null;

        int maxLevel = database[index].levelTable.Count - 1;

        ProductCellData _cell = new ProductCellData();
        _cell.index = index;
        _cell.name = database[index].name;
        _cell.imageName = database[index].spriteName;
        _cell.level = level;
        _cell.nextLevel = (level == maxLevel) ? maxLevel : level+1;
        _cell.currentAmount = database[index].levelTable[level].amountPerTouch;
        _cell.nextAmount = (level == maxLevel) ? database[index].levelTable[level].amountPerTouch : database[index].levelTable[level + 1].amountPerTouch;
        _cell.cost = database[index].levelTable[level].cost;
        _cell.cellState = states[index];
        return _cell;
    }

    private ProductCellData GetCellDataOfMaxLevel(int index)
    {
        if (index < 0 || index > database.Count) return null;

        int maxLevel = database[index].levelTable.Count-1;

        ProductCellData _cell = new ProductCellData();
        _cell.index = index;
        _cell.name = database[index].name;
        _cell.imageName = database[index].spriteName;
        _cell.level = maxLevel;
        _cell.nextLevel = _cell.level;
        _cell.currentAmount = database[index].levelTable[maxLevel].amountPerTouch;
        _cell.nextAmount = _cell.currentAmount;
        _cell.cost = database[index].levelTable[maxLevel].cost;
        _cell.cellState = states[index];
        return _cell;
    }


    #endregion

}

