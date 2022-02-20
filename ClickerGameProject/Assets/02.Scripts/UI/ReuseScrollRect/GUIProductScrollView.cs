using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

// ���� Ŭ����. ���⼭ grid�� �����͸� �߰������ش�.
public class GUIProductScrollView : MonoBehaviour
{
    public UIReuseGrid grid { get; private set; }

    // ����,���, ������.. �� �����͵�
    private List<List<JewelOriginData>> database;

    //
    private CellState[] states;

    //���� ���� ���� Cell�� �ε��� ��ȣ
    [SerializeField] private int curIndex;

    //���� ���� ���� Cell�� ����
    [SerializeField] private int curLevel;


    private void Awake()
    {
        grid = GetComponentInChildren<UIReuseGrid>();
        grid.onClickEvent += Upgrade;
    }

    private void Start()
    {
        //�ӽ� ������ (���߿� ������ �ε��� ��)
        database = TempGemInfo();
        InitializeData();

        //�׸��忡 �����͸� �߰�
        for (int i = 0; i < database.Count; ++i)
        {
            ProductCellData _cell = temp(i, 0);
            _cell.cellState = states[i];

            grid.AddItem(_cell);
        }
        grid.RefreshAllCell();
    }

    /// <summary>
    /// �����͸� �ʱ�ȭ�Ѵ�. (ó��������)
    /// </summary>
    private void InitializeData()
    {
        curIndex = -1;
        curLevel = 0;
        states = new CellState[database.Count];
        states[0] = CellState.Unlock;
    }

    /// <summary>
    /// ���׷��̵�
    /// </summary>
    /// <param name="index"></param>
    private void Upgrade(int index)
    {
        if (database.Count == 0) return;

        curIndex = index;
        int maxLevel = database[index].Count - 1;

        //������
        if (0 <= curLevel && curLevel < maxLevel)
        {
            //���
            MoneyManager.Instance.SubJewel(database[curIndex][curLevel].cost);

            BigInteger prevAmount = database[curIndex][curLevel].amountPerTouch;
            BigInteger addAmount = database[curIndex][++curLevel].amountPerTouch;
            MoneyManager.Instance.AddJewelPerClick(addAmount - prevAmount);
        }

        //�ִ뷹���� ������ ���
        if (curLevel.Equals(maxLevel))
        {
            //�̹� �� MaxUI
            states[curIndex] = CellState.MaxCompletion;
            grid.SetItem(curIndex, GetCellDataOfMaxLevel(curIndex));

            //���� ���� �Ѿ��
            curLevel = 0;
            curIndex++;
        }

        //�� �������
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

    //    if (0 < curLevel && curLevel < maxLevel - 1)    //������
    //    {
    //        MoneyManager.Instance.SubJewel(database[index][curLevel].cost);
    //        MoneyManager.Instance.AddJewelPerClick(database[index][curLevel].amountPerTouch);

    //        curLevel++;


    //        Debug.Log(curLevel + "���� ������ max������ " + maxLevel);
    //    }
    //    else // ���� �������� ��ü (Max)
    //    {
    //        if (curIndex >= 0)
    //        {
    //            //�ִ뷹���� cell 
    //            states[curIndex] = ProductCellData.PurchaseState.MaxCompletion;
    //            grid.SetItem(curIndex, GetCellDataOfMaxLevel(curIndex));
    //        }
    //        curLevel = 1;
    //        curIndex++;

    //        Debug.Log(curIndex + "���� ��ü");
    //    }
    //    states[curIndex] = ProductCellData.PurchaseState.Unlock;
    //    //�������� cell�� ������ ����
    //    ProductCellData data = temp(curIndex, curLevel);
    //    grid.SetItem(curIndex, data);

    //    grid.RefreshAllCell();

    //    Debug.Log("���� ������ " + curLevel + "�̸�,���� �ε�����" + curIndex + " max������ " + maxLevel);
    //}
    #endregion

    #region Data Function

    /// <summary>
    /// ���ϴ� �ε����� �� �����͸� �����´�.
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

    #region ���� ������
    /// <summary>
    /// �ӽ÷� DB�� �����.
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