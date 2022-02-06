using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

// 메인 클래스. 여기서 grid에 데이터를 추가시켜준다.
public class GUIProductScrollView : MonoBehaviour
{
    public UIReuseGrid grid { get; private set; }

    // 레벨,비용, 증가량.. 등 데이터들
    private List<DemoDB> database;

    //처음으로 업그레이드 할때 증가하는 goldPerClick 값
    private BigInteger startGoldByUpgrade = 1;

    //현재 잠금 해제된 Cell의 ID
    private int currentId;

    private void Awake()
    {
        grid = GetComponentInChildren<UIReuseGrid>();
    }

    private void Start()
    {
        currentId = 0;
        database = TempCreateDemoDB();

        //임시
        ProductCellData cell = new ProductCellData();
        cell.index = 0;
        cell.name = database[0].name;
        cell.jewelPerClick = BigInteger.Pow(3, 0);
        cell.cost = BigInteger.Pow(2, 0);
        cell.purchaseState = ProductCellData.PurchaseState.Unlock;
        grid.AddItem(cell);

        for (int i = 1; i < database.Count; ++i)
        {
            ProductCellData _cell = new ProductCellData();
            _cell.index = i;
            _cell.name = database[i].name;
            _cell.jewelPerClick = BigInteger.Pow(3, i);
            _cell.cost = BigInteger.Pow(2, i);
            _cell.purchaseState = ProductCellData.PurchaseState.Lock;
            grid.AddItem(_cell);
        }

        grid.RefreshAllCell();
    }

    /// <summary>
    /// 임시로 DB를 만든다.
    /// </summary>
    /// <returns></returns>
    private List<DemoDB> TempCreateDemoDB()
    {
        List<DemoDB> _database = new List<DemoDB>();
        for (int i = 0; i < 1000; i++)
        {
            string name = "ItemCell_" + i.ToString();
            _database.Add(new DemoDB(name));
        }
        return _database;
    }

    private int CurrentIdUp()
    {
        return ++currentId;
    }

}
