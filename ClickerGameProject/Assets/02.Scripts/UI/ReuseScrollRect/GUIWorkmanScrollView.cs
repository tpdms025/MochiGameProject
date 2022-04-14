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

    //셀의 갯수
    private int cellCount;

    //셀 상태에 대한 배열
    private CellState[] states;



    #region Property

    ////소유한 셀 레벨에 대한 배열
    public int[] curLevelList
    {
        get { return DBManager.Inst.PlayerData.workmanLevelList; }
        set { DBManager.Inst.PlayerData.workmanLevelList = value; }
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
        if (grid.m_ScrollRect != null)
            grid.SrollToCellWithinTime(curLevelList[0]);
    }



    private void LoadData()
    {
        //임시 데이터 (나중에 데이터 로드할 것)
        database = DBManager.Inst.GetWorkmanOriginDatas();
        cellCount = database.Count;
    }

    /// <summary>
    /// 데이터를 초기화한다. (처음데이터)
    /// </summary>
    private void InitializeData()
    {
        states = new CellState[cellCount];
        //states = Enumerable.Repeat(CellState.Unlock, cellCount).ToArray();  //배열의 값을 'unlock'으로 초기화
         
        //그리드에 데이터를 추가
        for (int i = 0; i < cellCount; ++i)     
        {
            ProductCellData _cell = temp(i, curLevelList[i]);
            CellState state;

            if(_cell.level == _cell.nextLevel)          //max state
            {
                state = CellState.MaxCompletion;
            }
            else                                        //unlock state
            {
                state = CellState.Unlock;
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

        int curLevel = curLevelList[index];
        int maxLevel = database[index].levelTable.Count - 1;

        //레벨이 0 ~ maxLevel 범위가 아닐 경우 리턴
        if (0 > curLevel || curLevel >= maxLevel) 
            return;

        //0레벨에서 업그레이드할 경우 일꾼을 스폰한다.
        if (curLevel == 0)
        {
            if (WorkmanSpawner.onSpawned != null)
                WorkmanSpawner.onSpawned(index);
        }

        //레벨업을 한다.
        MoneyManager.Instance.SubJewel(database[index].levelTable[curLevel].cost);      //재화 소모
        BigInteger prevAmount = database[index].levelTable[curLevel].amountPerTouch;
        ++curLevel;
        BigInteger addAmount = database[index].levelTable[curLevel].amountPerTouch;
        MoneyManager.Instance.AddJewelPerSec(addAmount - prevAmount);

        //최대레벨에 도달할 경우
        if (curLevel == maxLevel && index < cellCount - 1)
        {
            //이번 셀 UI를 Max상태로 업데이트
            states[index] = CellState.MaxCompletion;
            grid.SetItem(index, GetCellDataOfMaxLevel(index));
        }

        //레벨 데이터를 DB에 저장
        curLevelList[index] = curLevel;

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
        _cell.nextLevel = (level == maxLevel) ? maxLevel : level + 1;
        _cell.currentAmount = database[index].levelTable[level].amountPerTouch;
        _cell.nextAmount = (level == maxLevel) ? database[index].levelTable[level].amountPerTouch : database[index].levelTable[level + 1].amountPerTouch;
        _cell.cost = database[index].levelTable[level].cost;
        _cell.cellState = states[index];
        return _cell;
    }

    private ProductCellData GetCellDataOfMaxLevel(int index)
    {
        if (index < 0 || index > database.Count) return null;

        int maxLevel = database[index].levelTable.Count - 1;

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
