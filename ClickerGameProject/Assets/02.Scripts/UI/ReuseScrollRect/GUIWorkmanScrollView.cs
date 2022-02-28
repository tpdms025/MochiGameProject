using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GUIWorkmanScrollView : MonoBehaviour
{
    public UIReuseGrid grid { get; private set; }

    // ����,���, ������.. �� �����͵�
    private List<List<JewelOriginData>> database;

    //���������� ��������� Cell�� �ε��� ��ȣ
    [SerializeField] private int curIndex;

    //������ �� ������ ���� �迭
    private int[] curLevelList;

    //�� ���¿� ���� �迭
    private CellState[] states;

    private int cellCount;


    private void Awake()
    {
        grid = GetComponentInChildren<UIReuseGrid>();
        grid.onClickEvent += Upgrade;
    }

    private void Start()
    {
        //�ӽ� ������ (���߿� ������ �ε��� ��)
        database = TempGemInfo();
        cellCount = database.Count;
        InitializeData();

        //�׸��忡 �����͸� �߰�
        for (int i = 0; i < cellCount; ++i)
        {
            ProductCellData _cell = temp(i, curLevelList[i]);
            ////������ ���� ������ �������
            //if (i <= curIndex + 1)
            //    _cell.cellState = CellState.Unlock;
            //else
            //    _cell.cellState = CellState.Lock;

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
        curIndex = 0;
        curLevelList = new int[cellCount];
        states =  new CellState[cellCount];
        states[0] = CellState.Unlock;
    }

    /// <summary>
    /// ���׷��̵�
    /// </summary>
    /// <param name="index"></param>
    private void Upgrade(int index)
    {
        if (cellCount == 0) return;

        int curLevel = curLevelList[index];
        int maxLevel = database[index].Count - 1;

        //������ 0 ~ maxLevel ������ �ƴ� ��� ����
        if (0 > curLevel || curLevel >= maxLevel) return;

        //0���� ���׷��̵� �� ���
        if (curLevel == 0 && index < cellCount-1)
        {
            //���� ���� �������
            curIndex = index + 1;
            states[curIndex] = CellState.Unlock;
            grid.SetItem(curIndex, temp(curIndex, 0));
        }

        //***������***
        //��ȭ �Ҹ�
        MoneyManager.Instance.SubJewel(database[index][curLevel].cost);
        BigInteger prevAmount = database[index][curLevel].amountPerTouch;
        curLevel++;
        BigInteger addAmount = database[index][curLevel].amountPerTouch;
        MoneyManager.Instance.AddJewelPerSec(addAmount - prevAmount);

        //���� ������ ����
        curLevelList[index] = curLevel;

        //�׸��忡 ������ ����
        ProductCellData data = temp(index, curLevel);
        grid.SetItem(index, data);
    }

    #region Data Function

    /// <summary>
    /// ���ϴ� �ε����� �� �����͸� �����´�.
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
        _cell.cellState =states[index];
        return _cell;
    }

    private ProductCellData GetCellDataOfMaxLevel(int index)
    {
        if (index < 0 || index > database.Count) return null;

        int maxLevel = database[index].Count - 1;

        ProductCellData _cell = new ProductCellData();
        _cell.index = index;
        _cell.name = database[index][maxLevel].name;
        _cell.level = database[index][maxLevel].level;
        _cell.nextLevel = _cell.level;
        _cell.currentAmount = database[index][maxLevel].amountPerTouch;
        _cell.nextAmount = _cell.currentAmount;
        _cell.cost = database[index][maxLevel].cost;
        return _cell;
    }

    #region ���� ������
    /// <summary>
    /// �ӽ÷� DB�� �����.
    /// </summary>
    /// <returns></returns>
    private List<JewelOriginData> TempGemTable(int num)
    {
        int ascii = (int)'A' + num;
        List<JewelOriginData> _database = new List<JewelOriginData>();
        for (int i = 0; i < 5; i++)
        {
            JewelOriginData data = new JewelOriginData();
            data.name = System.Convert.ToString(System.Convert.ToChar(ascii));
            data.level = i;
            data.amountPerTouch = BigInteger.Pow(2 + num, i);
            if (i == 0) data.amountPerTouch = 0;
            data.cost = BigInteger.Pow(3 + num, i);
            _database.Add(data);
        }
        return _database;
    }

    private List<List<JewelOriginData>> TempGemInfo()
    {
        List<List<JewelOriginData>> info = new List<List<JewelOriginData>>();
        for (int i = 0; i < 10; i++)
        {
            string _key = System.Convert.ToString(i + 65);
            info.Add(TempGemTable(i));
        }
        return info;
    }

    #endregion

    #endregion


}
