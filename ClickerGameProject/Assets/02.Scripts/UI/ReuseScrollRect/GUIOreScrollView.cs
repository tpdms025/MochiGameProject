using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

// ���� Ŭ����. ���⼭ grid�� �����͸� �߰������ش�.
public class GUIOreScrollView : MonoBehaviour
{
    public UIReuseGrid grid { get; private set; }

    // ����,���, ������.. �� �����͵�
    private List<Database.ProductOriginData> database;

    //���� ����
    private int cellCount;

    //�� ���¿� ���� �迭
    //������ ���� max���� �����ϱ� ���� �����̴�. (max���� ���� ���� ������� �Ǵ� ����)
    private CellState[] states;



    #region Property

    //���������� ������ Cell�� �ε��� ��ȣ
    public int curIndex { 
        get { return DBManager.Inst.PlayerData.idx_lastOwned; }
        set { DBManager.Inst.PlayerData.idx_lastOwned = value; }
    }

    //���������� ������ Cell�� ����
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
        //�ӽ� ������ (���߿� ������ �ε��� ��)
        database = DBManager.Inst.GetOreOriginDatas();
        cellCount = database.Count;
        grid.m_ClickIndexID = curIndex;
    }
    

    /// <summary>
    /// �����͸� �ʱ�ȭ�Ѵ�. (ó��������)
    /// </summary>
    private void InitializeData()
    {
        //curIndex = 0;
        //curLevel = 0;
        states = new CellState[cellCount];

        //�׸��忡 �����͸� �߰�
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
    /// ���׷��̵��� ���� �����Ѵ�.
    /// </summary>
    private void Upgrade(int index)
    {
        //����ó��
        if (cellCount == 0) 
            return;
        
        //���ű����� DB�� ����
        curIndex = index;

        int maxLevel = database[index].levelTable.Count - 1;

        //������ 0 ~ maxLevel ������ �ƴ� ��� ����
        if (0 > curLevel || curLevel >= maxLevel) 
            return;

        //0�������� ���׷��̵��� ��� ������ ��ü�Ѵ�.
        if (curLevel == 0)
        {
            if (Ore.onOreChanged != null)
                Ore.onOreChanged(false);
        }

        //��ȭ �Ҹ�
        MoneyManager.Instance.SubJewel(database[index].levelTable[curLevel].cost);      //��ȭ �Ҹ�
        BigInteger prevAmount = database[index].levelTable[curLevel].amountPerTouch;
        //������
        ++curLevel;
        BigInteger addAmount = database[index].levelTable[curLevel].amountPerTouch;
        //��ȭ����ŭ ��ȭ �߰�
        MoneyManager.Instance.AddJewelPerClick(addAmount - prevAmount);

        //�ִ뷹���� ������ ���
        if (curLevel == maxLevel && index < cellCount - 1)
        {
            //�̹� �� UI�� Max���·� ������Ʈ
            states[index] = CellState.MaxCompletion;
            grid.SetItem(index, GetCellDataOfMaxLevel(index));

            //���� ���� �Ѿ��
            curLevel = 0;
            ++index;
        }

        //������� ���·� ������Ʈ (max�� ���, ���� ���� �ش�)
        states[index] = CellState.Unlock;

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

