using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class GUIWorkmanScrollView : MonoBehaviour
{
    public UIReuseGrid grid { get; private set; }

    // ����,���, ������.. �� �����͵�
    private List<Database.ProductOriginData> database;

    //���� ����
    private int cellCount;

    //�� ���¿� ���� �迭
    private CellState[] states;



    #region Property

    ////������ �� ������ ���� �迭
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
        //�ӽ� ������ (���߿� ������ �ε��� ��)
        database = DBManager.Inst.GetWorkmanOriginDatas();
        cellCount = database.Count;
    }

    /// <summary>
    /// �����͸� �ʱ�ȭ�Ѵ�. (ó��������)
    /// </summary>
    private void InitializeData()
    {
        states = new CellState[cellCount];
        //states = Enumerable.Repeat(CellState.Unlock, cellCount).ToArray();  //�迭�� ���� 'unlock'���� �ʱ�ȭ
         
        //�׸��忡 �����͸� �߰�
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
    /// ���׷��̵��� ���� �����Ѵ�.
    /// </summary>
    private void Upgrade(int index)
    {
        //����ó��
        if (cellCount == 0) 
            return;

        int curLevel = curLevelList[index];
        int maxLevel = database[index].levelTable.Count - 1;

        //������ 0 ~ maxLevel ������ �ƴ� ��� ����
        if (0 > curLevel || curLevel >= maxLevel) 
            return;

        //0�������� ���׷��̵��� ��� �ϲ��� �����Ѵ�.
        if (curLevel == 0)
        {
            if (WorkmanSpawner.onSpawned != null)
                WorkmanSpawner.onSpawned(index);
        }

        //�������� �Ѵ�.
        MoneyManager.Instance.SubJewel(database[index].levelTable[curLevel].cost);      //��ȭ �Ҹ�
        BigInteger prevAmount = database[index].levelTable[curLevel].amountPerTouch;
        ++curLevel;
        BigInteger addAmount = database[index].levelTable[curLevel].amountPerTouch;
        MoneyManager.Instance.AddJewelPerSec(addAmount - prevAmount);

        //�ִ뷹���� ������ ���
        if (curLevel == maxLevel && index < cellCount - 1)
        {
            //�̹� �� UI�� Max���·� ������Ʈ
            states[index] = CellState.MaxCompletion;
            grid.SetItem(index, GetCellDataOfMaxLevel(index));
        }

        //���� �����͸� DB�� ����
        curLevelList[index] = curLevel;

        //�׸��忡 ������ ����
        ProductCellData data = temp(index, curLevel);
        grid.SetItem(index, data);
    }


    #region Change to CellData

    /// <summary>
    /// ���ϴ� �ε����� �� �����͸� �����´�.
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
