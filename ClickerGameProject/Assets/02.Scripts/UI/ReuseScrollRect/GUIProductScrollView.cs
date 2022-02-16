using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

// ���� Ŭ����. ���⼭ grid�� �����͸� �߰������ش�.
public class GUIProductScrollView : MonoBehaviour
{
    public UIReuseGrid grid { get; private set; }

    // ����,���, ������.. �� �����͵�
    private List<DemoDB> database;

    //ó������ ���׷��̵� �Ҷ� �����ϴ� goldPerClick ��
    private BigInteger startGoldByUpgrade = 1;

    //���� ��� ������ Cell�� ID
    private int currentId;

    private void Awake()
    {
        grid = GetComponentInChildren<UIReuseGrid>();
    }

    private void Start()
    {
        currentId = 0;
        database = TempCreateDemoDB();

        //�ӽ�
        ProductCellData cell = new ProductCellData();
        cell.index = 0;
        cell.name = database[0].name;
        cell.level = 0;
        cell.nextLevel = 1;
        cell.jewelPerClick = BigInteger.Pow(3, 0);
        cell.nextJewelPerClick = BigInteger.Pow(3, 1);
        cell.cost = BigInteger.Pow(2, 0);
        cell.purchaseState = ProductCellData.PurchaseState.Unlock;
        grid.AddItem(cell);

        for (int i = 1; i < database.Count; ++i)
        {
            ProductCellData _cell = new ProductCellData();
            _cell.index = i;
            _cell.name = database[i].name;
            _cell.level = 0;
            _cell.nextLevel = 1;
            _cell.jewelPerClick = BigInteger.Pow(3, i);
            _cell.nextJewelPerClick = BigInteger.Pow(3, i+1);
            _cell.cost = BigInteger.Pow(2, i);
            _cell.purchaseState = ProductCellData.PurchaseState.Lock;
            grid.AddItem(_cell);
        }

        grid.RefreshAllCell();
    }

    /// <summary>
    /// �ӽ÷� DB�� �����.
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

[System.Serializable]
public class JewelData
{
    public string name;
    public int level;
    public BigInteger amountPerTouch;
    public BigInteger cost;


    public JewelData(string _name, int _level, BigInteger _amountPerTouch, BigInteger _cost)
    {
        name = _name;
        level = _level;
        amountPerTouch = _amountPerTouch;
        cost = _cost;
    }
}