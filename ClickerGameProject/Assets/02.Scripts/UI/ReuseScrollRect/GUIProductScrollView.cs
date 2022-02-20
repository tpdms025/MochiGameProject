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

    //
    private CellState[] states;

    //현재 선택 중인 Cell의 인덱스 번호
    [SerializeField] private int curIndex;

    //현재 선택 중인 Cell의 레벨
    [SerializeField] private int curLevel;


    private void Awake()
    {
        grid = GetComponentInChildren<UIReuseGrid>();
        grid.onClickEvent += Upgrade;
    }

    private void Start()
    {
        //임시 데이터 (나중에 데이터 로드할 것)
        database = TempGemInfo();
        InitializeData();

        //그리드에 데이터를 추가
        for (int i = 0; i < database.Count; ++i)
        {
            ProductCellData _cell = temp(i, 0);
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
        curIndex = -1;
        curLevel = 0;
        states = new CellState[database.Count];
        states[0] = CellState.Unlock;
    }

    /// <summary>
    /// 업그레이드
    /// </summary>
    /// <param name="index"></param>
    private void Upgrade(int index)
    {
        if (database.Count == 0) return;

        curIndex = index;
        int maxLevel = database[index].Count - 1;

        //레벨업
        if (0 <= curLevel && curLevel < maxLevel)
        {
            //비용
            MoneyManager.Instance.SubJewel(database[curIndex][curLevel].cost);

            BigInteger prevAmount = database[curIndex][curLevel].amountPerTouch;
            BigInteger addAmount = database[curIndex][++curLevel].amountPerTouch;
            MoneyManager.Instance.AddJewelPerClick(addAmount - prevAmount);
        }

        //최대레벨에 도달할 경우
        if (curLevel.Equals(maxLevel))
        {
            //이번 셀 MaxUI
            states[curIndex] = CellState.MaxCompletion;
            grid.SetItem(curIndex, GetCellDataOfMaxLevel(curIndex));

            //다음 셀로 넘어가기
            curLevel = 0;
            curIndex++;
        }

        //셀 잠금해제
        states[curIndex] = CellState.Unlock;

        ProductCellData data = temp(curIndex, curLevel);
        grid.SetItem(curIndex, data);
    }

    #region TEST
    //private void Upgrade(int index)
    //{
    //    if (database.Count == 0) return;

    //    curIndex = index;
    //    int maxLevel = database[index].Count - 1;

    //    if (0 < curLevel && curLevel < maxLevel - 1)    //레벨업
    //    {
    //        MoneyManager.Instance.SubJewel(database[index][curLevel].cost);
    //        MoneyManager.Instance.AddJewelPerClick(database[index][curLevel].amountPerTouch);

    //        curLevel++;


    //        Debug.Log(curLevel + "으로 레벨업 max레벨은 " + maxLevel);
    //    }
    //    else // 다음 보석으로 교체 (Max)
    //    {
    //        if (curIndex >= 0)
    //        {
    //            //최대레벨인 cell 
    //            states[curIndex] = ProductCellData.PurchaseState.MaxCompletion;
    //            grid.SetItem(curIndex, GetCellDataOfMaxLevel(curIndex));
    //        }
    //        curLevel = 1;
    //        curIndex++;

    //        Debug.Log(curIndex + "으로 교체");
    //    }
    //    states[curIndex] = ProductCellData.PurchaseState.Unlock;
    //    //레벨업한 cell의 데이터 갱신
    //    ProductCellData data = temp(curIndex, curLevel);
    //    grid.SetItem(curIndex, data);

    //    grid.RefreshAllCell();

    //    Debug.Log("현재 레벨은 " + curLevel + "이며,현재 인덱스는" + curIndex + " max레벨은 " + maxLevel);
    //}
    #endregion

    #region Data Function

    /// <summary>
    /// 원하는 인덱스의 셀 데이터를 가져온다.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    private ProductCellData temp(int index, int level)
    {
        if(index < 0 || index > database.Count) return null;

        int maxLevel = database[index].Count - 1;

        ProductCellData _cell = new ProductCellData();
        _cell.index = index;
        _cell.name = database[index][level].name;
        _cell.level = database[index][level].level;
        _cell.nextLevel = level.Equals(maxLevel) ? maxLevel : database[index][level+1].level;
        _cell.jewelPerClick = database[index][level].amountPerTouch;
        _cell.nextJewelPerClick = level.Equals(maxLevel) ? database[index][level].amountPerTouch : database[index][level+1].amountPerTouch;
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
        _cell.jewelPerClick = database[index][maxLevel].amountPerTouch;
        _cell.nextJewelPerClick = _cell.jewelPerClick;
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
public class JewelOriginData
{
    public string name;
    public int level;
    public BigInteger amountPerTouch;
    public BigInteger cost;

    public JewelOriginData() { }

    public JewelOriginData(string _name, int _level, BigInteger _amountPerTouch, BigInteger _cost)
    {
        name = _name;
        level = _level;
        amountPerTouch = _amountPerTouch;
        cost = _cost;
    }
}