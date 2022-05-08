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

    //�ִ� ����
    private int maxLevel;

    //���� ����
    private int cellCount;

    //�� ���¿� ���� �迭
    //������ ���� max���� �����ϱ� ���� �����̴�. (max���� ���� ���� ������� �Ǵ� ����)
    //private CellState[] states;


    #region Property

    //���������� ������ Cell�� �ε��� ��ȣ
    public int curIndex { 
        get { return DBManager.Inst.inventory.oreIdx_lastOwned; }
        set { DBManager.Inst.inventory.oreIdx_lastOwned = value; }
    }

    //���������� ������ Cell�� ����
    public int curLevel {
        get { return DBManager.Inst.inventory.oreLevel_lastOwned; }
        set { DBManager.Inst.inventory.oreLevel_lastOwned = value; }
    }

    #endregion


    #region Unity methods

    private void Awake()
    {
        grid = GetComponentInChildren<UIReuseGrid>();
        grid.onClickEvent += Upgrade;
        grid.InitData();

        LoadData();
        InitializeData();
    }


    private void OnEnable()
    {
        //if(grid.m_ScrollRect != null)
        //    grid.SrollToCellWithinTime(curIndex);
    }

    private void OnDestroy()
    {
        grid.onClickEvent -= Upgrade;
    }

    #endregion


    /// <summary>
    /// �ε����� ������ �ش��ϴ� ���� �����͸� �����´�.
    /// </summary>
    private Database.ProductOriginData GetDB(int type, int level)
    {
        return database[(type * maxLevel) + level];
    }



    private void LoadData()
    {
        database = DBManager.Inst.GetOreOriginDatas();
        maxLevel = DBManager.Inst.oreMaxLevel;
        cellCount = database.Count/maxLevel;
        //states = new CellState[cellCount];
        grid.m_ClickIndexID = curIndex;
    }


    /// <summary>
    /// �����͸� �ʱ�ȭ�Ѵ�. (ó��������)
    /// </summary>
    private void InitializeData()
    {
        if (database == null)
            return;

        //�׸��忡 �����͸� �߰�
        for (int i = 0; i < cellCount; ++i)
        {
            ProductCellData _cell;
            //CellState state;
            if (i < curIndex)                       //max state
            {
                //state = CellState.MaxCompletion;
                _cell = GetCellDataOfMaxLevel(i);
                    grid.AddItem(_cell);
            }
            else if (i == curIndex)                 //unlock state
            {
                //state = CellState.Unlock;
                if (curLevel == maxLevel)
                {
                    _cell = GetCellDataOfMaxLevel(i);
                    grid.AddItem(_cell);
                    grid.AddItem(GetToCellData(++i, 0));
                }
                else
                {
                    _cell = GetToCellData(i, curLevel);
                    grid.AddItem(_cell);
                }
            }
            else                                   //lock state
            {
                //state = CellState.Lock;
                _cell = GetToCellData(i, 0, CellState.Lock);
                grid.AddItem(_cell);
            }
            //_cell.cellState = states[i] = state;
            //grid.AddItem(_cell);
        }
        grid.RefreshAllCell();
    }


    /// <summary>
    /// ���׷��̵��� ���� �����Ѵ�.
    /// </summary>
    private void Upgrade(int index,int type)
    {
        //����ó��
        if (cellCount == 0) 
            return;
        
        int level = curLevel;
        if (level == maxLevel)
            level = 0;

        //������ 0 ~ maxLevel ������ �ƴ� ��� ����
        if (0 > level || level >= maxLevel) 
            return;


        //��ȭ�� �Ҹ��ϰ� �������� �Ѵ�.
        MoneyManager.Inst.SubJewel(GetToCellData(index, level).cost); 
        
        //��ȭ����ŭ ������ȭ ȹ��
        var prevAmount = GetToCellData(index, level).currentAmount;
        var addAmount = GetToCellData(index, level).nextAmount;
        MoneyManager.Inst.SumJewelPerTouch(addAmount - prevAmount);


        //���ű����� DB�� ����
        curIndex = index;
        curLevel = ++level;

        //0�������� ���׷��̵��� ��� ������ ��ü�Ѵ�.
        if (level == 1)
        {
            if (OreWorld.onOreChanged != null)
                OreWorld.onOreChanged(false);
        }

        //�ִ뷹���� ������ ���
        if (level == maxLevel)
        {
            //�̹� �� UI�� Max���·� ������Ʈ
            //states[index] = CellState.MaxCompletion;
            grid.SetItem(index, GetCellDataOfMaxLevel(index));

            if(index >= cellCount - 1)
            {
                return;
            }

            //���� ���� �Ѿ��
            level = 0;
            ++index;
        }


        ////������� ���·� ������Ʈ (max�� ���, ���� ���� �ش�)
        //states[index] = CellState.Unlock;

        //�׸��忡 ������ ����
        ProductCellData data = GetToCellData(index, level, CellState.Unlock);
        //ProductCellData data = GetToCellData(index, level);
        grid.SetItem(index, data);
    }


    #region Change to CellData

    /// <summary>
    /// ���ϴ� �ε����� �� �����͸� �����´�.
    /// </summary>
    private ProductCellData GetToCellData(int type, int level,CellState state = CellState.Unlock)
    {
        if (type < 0 || type > cellCount) return null;

        int index;
        ProductCellData _cell = new ProductCellData();
        if (level == 0)
            index = (type * maxLevel) + 0;
        else
            index = (type * maxLevel) + (level - 1);
        _cell.index = index;
        _cell.id = database[index].ID;
        _cell.name = database[index].name;
        _cell.imageName = database[index].spriteName;
        _cell.level = level;
        _cell.nextLevel = (level == maxLevel) ? maxLevel : level+1;

        if (level == 0)
        {
            _cell.currentAmount = 0;
            _cell.nextAmount = GetDB(type, level).amount;
        }
        else
        {
            _cell.currentAmount = GetDB(type, level-1).amount;
            _cell.nextAmount = (level == maxLevel) ? 0 : GetDB(type, level).amount;
        }
        
        if(level == maxLevel)
            _cell.cost = 0;
        else
            _cell.cost = GetDB(type, level).cost;
        _cell.cellState = state;
        //_cell.cellState = states[type];
        return _cell;
    }

    private ProductCellData GetCellDataOfMaxLevel(int type)
    {
        if (type < 0 || type > cellCount) return null;

        int index;
        ProductCellData _cell = new ProductCellData();
        _cell.index = index = (type*maxLevel)+ (maxLevel-1);
        _cell.id = index;
        _cell.name = database[index].name;
        _cell.imageName = database[index].spriteName;
        _cell.level = maxLevel;
        _cell.nextLevel = _cell.level;
        _cell.currentAmount = GetDB(type, maxLevel).amount;
        _cell.nextAmount = 0;
        _cell.cost = 0;
        _cell.cellState = CellState.MaxCompletion;
        return _cell;
    }


    #endregion

}

