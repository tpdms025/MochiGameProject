using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

// 메인 클래스. 여기서 grid에 데이터를 추가시켜준다.
public class GUIProductScrollView : MonoBehaviour
{
    public UIReuseGrid grid { get; private set; }

    // 레벨,비용, 증가량.. 등 데이터들
    private List<List<JewelOriginData>> database;


    //마지막으로 잠금해제된 Cell의 인덱스 번호
    [SerializeField] private int curIndex;

    //마지막으로 잠금해제된 Cell의 레벨
    [SerializeField] private int curLevel;

    //셀 상태에 대한 배열
    //선택한 셀이 max인지 구별하기 위한 변수이다. (max여야 다음 셀이 잠금해제 되는 형식)
    private CellState[] states;

    private int cellCount;


    private void Awake()
    {
        grid = GetComponentInChildren<UIReuseGrid>();
        grid.onClickEvent += Upgrade;
    }

    private void Start()
    {
        //임시 데이터 (나중에 데이터 로드할 것)
        database = TempGemInfo();
        cellCount = database.Count;
        InitializeData();

        //그리드에 데이터를 추가
        for (int i = 0; i < cellCount; ++i)
        {
            ProductCellData _cell;
            if( i < curIndex)                  //max state
                _cell = GetCellDataOfMaxLevel(i)    ;
            else if (i == curIndex)             //unlock state
                _cell = temp(i, curLevel);
            else                                //lock state
                _cell = temp(i,0);

            _cell.cellState = states[i];
            grid.AddItem(_cell);
        }
        grid.RefreshAllCell();
    }

    /// <summary>
    /// 데이터를 초기화한다. (처음데이터)
    /// </summary>
    private void InitializeData()
    {
        curIndex = 0;
        curLevel = 0;
        states = new CellState[cellCount];
        states[0] = CellState.Unlock;
    }

    /// <summary>
    /// 업그레이드
    /// </summary>
    /// <param name="index"></param>
    private void Upgrade(int index)
    {
        if (cellCount == 0) return;

        curIndex = index;
        int maxLevel = database[index].Count - 1;

        //레벨이 0 ~ maxLevel 범위가 아닐 경우 리턴
        if (0 > curLevel || curLevel >= maxLevel) return;

        //***레벨업***
        //재화 소모
        MoneyManager.Instance.SubJewel(database[index][curLevel].cost);
        BigInteger prevAmount = database[index][curLevel].amountPerTouch;
        curLevel++;
        BigInteger addAmount = database[index][curLevel].amountPerTouch;
        MoneyManager.Instance.AddJewelPerClick(addAmount - prevAmount);

        //최대레벨에 도달할 경우
        if (curLevel==maxLevel && index < cellCount - 1)
        {
            //이번 셀 UI를 Max상태로 업데이트
            states[index] = CellState.MaxCompletion;
            grid.SetItem(index, GetCellDataOfMaxLevel(index));

            //다음 셀로 넘어가기
            curLevel = 0;
            curIndex++;
        }

        //셀 잠금해제 상태 저장
        states[curIndex] = CellState.Unlock;

        //그리드에 데이터 변경
        ProductCellData data = temp(curIndex, curLevel);
        grid.SetItem(curIndex, data);
    }


    #region Data Function

    /// <summary>
    /// 원하는 인덱스의 셀 데이터를 가져온다.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    private ProductCellData temp(int index, int level)
    {
        if (index < 0 || index > database.Count) return null;

        int maxLevel = database[index].Count - 1;

        ProductCellData _cell = new ProductCellData();
        _cell.index = index;
        _cell.name = database[index][level].name;
        _cell.level = database[index][level].level;
        _cell.nextLevel = level == maxLevel ? maxLevel : database[index][level + 1].level;
        _cell.currentAmount = database[index][level].amountPerTouch;
        _cell.nextAmount = level == maxLevel ? database[index][level].amountPerTouch : database[index][level + 1].amountPerTouch;
        _cell.cost = database[index][level].cost;
        _cell.cellState = states[index];
        return _cell;
    }

    private ProductCellData GetCellDataOfMaxLevel(int index)
    {
        if (index < 0 || index > database.Count) return null;

        int maxLevel = database[index].Count-1;

        ProductCellData _cell = new ProductCellData();
        _cell.index = index;
        _cell.name = database[index][maxLevel].name;
        _cell.level = database[index][maxLevel].level;
        _cell.nextLevel = _cell.level;
        _cell.currentAmount = database[index][maxLevel].amountPerTouch;
        _cell.nextAmount = _cell.currentAmount;
        _cell.cost = database[index][maxLevel].cost;
        _cell.cellState = states[index];
        return _cell;
    }

    #region 엑셀 데이터
    /// <summary>
    /// 임시로 DB를 만든다.
    /// </summary>
    /// <returns></returns>
    private List<JewelOriginData> TempGemTable(int num)
    {
        int ascii = (int)'A' +num;
        List<JewelOriginData> _database = new List<JewelOriginData>();
        for (int i = 0; i < 5; i++)
        {
            JewelOriginData data = new JewelOriginData();
            data.name = System.Convert.ToString(System.Convert.ToChar(ascii));
            data.level = i;
            data.amountPerTouch =BigInteger.Pow(2+ num, i);
            if (i == 0) data.amountPerTouch = 0;
            data.cost = BigInteger.Pow(3 + num, i);
            _database.Add(data);
        }
        return _database;
    }

    private List< List<JewelOriginData>> TempGemInfo()
    {
        List < List < JewelOriginData >> info = new List<List<JewelOriginData>> ();
        for (int i=0;i< 10; i++)
        {
            string _key = System.Convert.ToString(i + 65);
            info.Add(TempGemTable(i));
        }
        return info;   
    }

    #endregion

    #endregion

}

[System.Serializable]
public struct JewelOriginData
{
    public string name;
    public int level;
    public BigInteger amountPerTouch;
    public BigInteger cost;


    public JewelOriginData(string _name, int _level, BigInteger _amountPerTouch, BigInteger _cost)
    {
        name = _name;
        level = _level;
        amountPerTouch = _amountPerTouch;
        cost = _cost;
    }
}