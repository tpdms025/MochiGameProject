using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class GUIEnhancementScrollView : MonoBehaviour
{
    private AbilityEnhance _system;

    public UIReuseGrid grid { get; private set; }

    // ����,���, ������.. �� �����͵�
    private List<Database.ProductOriginData_> database;

    //�ִ� ����
    private int maxLevel;


    //���� ����
    private int cellCount;

    ////�� ���¿� ���� �迭
    //private CellState[] states;

    //���� �߰��ɶ� ȣ��Ǵ� �̺�Ʈ (int : index)
    public static System.Action<int> onCellAdded;

    //�߰��� ���� �ִ��� üũ�ϴ� bool �� ����
    private bool added = false;



    #region Property

    //������ �� ������ ���� �迭
    public int[] curLevelList
    {
        get { return DBManager.Inst.inventory.enhanceLevels_owned; }
        set { DBManager.Inst.inventory.enhanceLevels_owned = value; }
    }

    #endregion


    #region Unity methods

    private void Awake()
    {
        _system = GetComponent<AbilityEnhance>();
        grid = GetComponentInChildren<UIReuseGrid>();
        grid.InitData();

        //�̺�Ʈ ���
        grid.onClickEvent += Upgrade;
        onCellAdded += AddCell;

        LoadData();
        InitializeData();
    }


    private void OnEnable()
    {
        //if (grid.m_ScrollRect != null)
        //    grid.SrollToCellWithinTime(curLevelList[0]);

        if (added)
        {
            //Refresh Grid
            grid.RefreshAllCell();
            grid.SortCellData_IdOrder();
            added = false;
        }
    }

    private void OnDestroy()
    {
        grid.onClickEvent -= Upgrade;
        onCellAdded -= AddCell;
    }

    #endregion



    /// <summary>
    /// �ϲ� ��ȭ ������ �߰��Ѵ�.
    /// </summary>
    public void AddCell(int workmanIdx)
    {
        //����ó�� - �̹� �ϲ��� �����ϰ� ���� ��� ����
        if (DBManager.Inst.inventory.workmanLevels_owned[workmanIdx] != 0)
            return;

        //�ϲ۰�ȭ�� �ε����� 1�����Ͷ� 1+i
        grid.AddItem(GetToCellData(1 + workmanIdx, 0));
        added = true;
    }


    /// <summary>
    /// �ε����� ������ �ش��ϴ� ���� �����͸� �����´�.
    /// </summary>
    private Database.ProductOriginData_ GetDB(int type, int level)
    {
        return database[(type * maxLevel) + level];
    }




    private void LoadData()
    {
        database = DBManager.Inst.GetEnhancementOriginDatas();
        maxLevel = DBManager.Inst.enhancementMaxLevel;
        cellCount = database.Count/maxLevel;
        //states = new CellState[cellCount];
    }


    /// <summary>
    /// �����͸� �ʱ�ȭ�Ѵ�. (ó��������)
    /// </summary>
    private void InitializeData()
    {
        if (database == null)
            return;

        //�������� ���� �ϲ��� ��ȭ�� �� �� ������ ��ũ�Ѻ信 �����͸� �߰����� �ʴ´�.
        Queue<int> ignoreIdx= new Queue<int>();
        for(int i=0; i< DBManager.Inst.inventory.workmanLevels_owned.Length; i++)
        {
            if(DBManager.Inst.inventory.workmanLevels_owned[i] == 0)
            {
                //�ϲ۰�ȭ�� 1�����Ͷ� 1+i
                ignoreIdx.Enqueue(1+i);
            }
        }

        //�׸��忡 �����͸� �߰�
        for (int i = 0; i < cellCount; ++i)     
        {
            //�����ؾ��� �ε������ �����͸� �߰����� �ʴ´�.
            if(ignoreIdx.Count > 0)
            {
                if(ignoreIdx.Peek() == i)
                {
                    ignoreIdx.Dequeue();
                    continue;
                }
            }

            ProductCellData _cell = GetToCellData(i, curLevelList[i]);
            grid.AddItem(_cell);
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

        int curLevel = curLevelList[type];
        Debug.Log("index" + index + "type" + type);

        //������ 0 ~ maxLevel ������ �ƴ� ��� ����
        if (0 > curLevel || curLevel >= maxLevel) 
            return;


        //��ȭ�� �Ҹ��ϰ� �������� �Ѵ�.
        MoneyManager.Inst.SubJewel(GetToCellData(type, curLevel).cost);
        //MoneyManager.Inst.SubJewel(database[index].levelTable[curLevel].cost);

        //��ȭ �ɷ� �߰�
        var addAmount = GetToCellData(type, curLevel).nextAmount;
        //float addAmount = (float)database[index].levelTable[curLevel].amount;
        _system.Enhance(type, (float)addAmount);

        //���� �����͸� DB�� ����
        curLevelList[type] = ++curLevel;

        //�ִ뷹���� ������ ���
        if (curLevel == maxLevel)
        {
            //�̹� �� UI�� Max���·� ������Ʈ
            //states[index] = CellState.MaxCompletion;
            grid.SetItem(index, GetToCellData(type, maxLevel));

            if (type >= cellCount - 1)
            {
                return;
            }
        }


        //�׸��忡 ������ ����
        grid.SetItem(index, GetToCellData(type, curLevel));
    }


    #region Change to CellData

    /// <summary>
    /// ���ϴ� �ε����� �� �����͸� �����´�.
    /// </summary>
    private ProductCellData GetToCellData(int type, int level)
    {
        if (type < 0 || type > cellCount) return null;

        int index;
        ProductCellData _cell = new ProductCellData();
        if (level == 0)
            index = (type * maxLevel) + 0;
        else
            index = (type * maxLevel) + (level - 1);
        _cell.index = index;
        _cell.id = type;
        _cell.name = database[index].name;
        _cell.imageName = database[index].spriteName;
        _cell.description = database[index].description;
        _cell.level = level;
        _cell.nextLevel = (level == maxLevel) ? maxLevel : level + 1;

        if(level == 0)
        {
            _cell.currentAmount = 0;
            _cell.nextAmount = GetDB(type, level).amount;
        }
        else
        {
            _cell.currentAmount = GetDB(type, level - 1).amount;
            _cell.nextAmount = (level == maxLevel) ? 0 : GetDB(type, level).amount;
        }

        if(level == maxLevel)
            _cell.cost = 0;
        else
            _cell.cost = GetDB(type, level).cost;
        _cell.cellState = (level == maxLevel) ? CellState.MaxCompletion : CellState.Unlock;
        return _cell;
    }

    private ProductCellData GetCellDataOfMaxLevel(int type)
    {
        if (type < 0 || type > cellCount) return null;

        int index;
        ProductCellData _cell = new ProductCellData();
        _cell.index = index = (type*maxLevel)+ (maxLevel-1);
        _cell.id = database[index].ID;
        _cell.name = database[index].name;
        _cell.imageName = database[index].spriteName;
        _cell.description = database[index].description;
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
