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

    //�ִ� ����
    private int maxLevel;

    //���� ����
    [SerializeField]private int cellCount;

    ////�� ���¿� ���� �迭
    //private CellState[] states;

    //�ۼ�Ʈ ������ ����Ʈ
    private List<ValueModifiers> rates
    {
        get { return MoneyManager.Inst.workmansAmount; }
    }

    //��ȭ�ߴ��� üũ�ϴ� bool �� ����
    private bool isEnhance = false;



    #region Property

    //������ �� ������ ���� �迭
    public int[] curLevelList
    {
        get { return DBManager.Inst.inventory.workmanLevels_owned; }
        set { DBManager.Inst.inventory.workmanLevels_owned = value; }
    }

    #endregion

    #region Unity methods


    private void Awake()
    {
        grid = GetComponentInChildren<UIReuseGrid>();
        grid.onClickEvent += Upgrade;
        grid.InitData();

        //�̺�Ʈ ���
        for (int i=0; i< MoneyManager.Inst.workmansAmount.Count;i++)
        {
            //�׸��忡 ������ ����
            MoneyManager.Inst.workmansAmount[i].onValueChanged += RefreshCell;
            //MoneyManager.Inst.workmansAmount[i].onValueChanged += (double index)=> grid.SetItem((int)index - 1, GetToCellData((int)index - 1, curLevelList[(int)index - 1]));
            //MoneyManager.Inst.workmansAmount[i].onValueChanged += (double index)=> isEnhance = true;
        }

        LoadData();
        InitializeData();
    }

    public void RefreshCell(double index)
    {
        //�׸��忡 ������ ����
        //�ϲ� ��ȭ�� �ε��� 1���� �����Ͽ� index -1�� ����.
        ProductCellData data = GetToCellData((int)index - 1, curLevelList[(int)index - 1]);
        Debug.Log("RefreshCell" + index);
        Debug.Log("RefreshCell data" + data.name);
        grid.SetItem((int)index - 1, data);
        isEnhance = true;

        //�ʴ�ȹ�淮 ���� �ٽ� ����.
        MoneyManager.Inst.SumAllWorkmanAmountToJewelPerSec();
    }


    private void OnEnable()
    {
        //if (grid.m_ScrollRect != null)
        //    grid.SrollToCellWithinTime(curLevelList[0]);

        //��ȭ�� �ߴٸ� UI�� ��ȭ�� ������ �ٽ� �����ϱ�
        if(isEnhance)
        {
            //Refresh Grid
            grid.RefreshAllCell();
            //grid.RefillAllCell();
            isEnhance = false;
        }
    }

    private void OnDestroy()
    {
        grid.onClickEvent -= Upgrade;
        foreach (var workman in MoneyManager.Inst.workmansAmount)
        {
            workman.onValueChanged = null;
        }
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
        database = DBManager.Inst.GetWorkmanOriginDatas();
        maxLevel = DBManager.Inst.workmanMaxLevel;
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

        //�׸��忡 �����͸� �߰�
        for (int i = 0; i < cellCount; ++i)     
        {
            ProductCellData _cell = GetToCellData(i, curLevelList[i]);
            grid.AddItem(_cell);
        }
        grid.RefreshAllCell();
    }



    /// <summary>
    /// ������ ���� ���׷��̵��ϰ� UI�� �����Ѵ�.
    /// </summary>
    private void Upgrade(int index, int type)
    {
        //����ó��
        if (cellCount == 0) 
            return;

        int curLevel = curLevelList[index];

        //������ 0 ~ maxLevel ������ �ƴ� ��� ����
        if (0 > curLevel || curLevel >= maxLevel) 
            return;

        //0�������� ���׷��̵��� ��� �ϲ��� �����Ѵ�.
        if (curLevel == 0)
        {
            DBManager.Inst.inventory.workmanCount++;

            //������ �ϲ��� �����Ѵ�.
            if (WorkmanSpawner.onSpawned != null)
                WorkmanSpawner.onSpawned.Invoke(index);

            //������ �ϲ��� ��ȭ�� �� �ֵ��� ��ȭ�ǿ� �߰��Ѵ�.
            if (GUIEnhancementScrollView.onCellAdded != null)
                GUIEnhancementScrollView.onCellAdded.Invoke(index);
        }


        //��ȭ�� �Ҹ��ϰ� �������� �Ѵ�.
        MoneyManager.Inst.SubJewel(GetToCellData(index,curLevel).cost);

        //��ȭ����ŭ �ʴ�ȹ�淮�� �߰�
        var currentAmount = GetToCellData(index, curLevel).currentAmount;
        var nextAmount = GetToCellData(index, curLevel).nextAmount;
        MoneyManager.Inst.SumJewelPerSec(nextAmount - currentAmount);

        //���� �����͸� DB�� ����
        curLevelList[index] = ++curLevel;
        MoneyManager.Inst.workmansAmount[index].SetBaseValue(GetBaseCurrentAmount(index, curLevel));


        //�ִ뷹���� ������ ���
        if (curLevel == maxLevel)
        {
            //�̹� �� UI�� Max���·� ������Ʈ
            grid.SetItem(index, GetToCellData(index, maxLevel));

            if (index >= cellCount - 1)
            {
                return;
            }
        }

       

        //�׸��忡 ������ ����
        ProductCellData data = GetToCellData(index, curLevel);
        grid.SetItem(index, data);
    }


    #region Change to CellData

    private double GetBaseCurrentAmount(int type, int level)
    {
        return GetDB(type, level - 1).amount;
    }

    /// <summary>
    /// ���ϴ� �ε����� �� �����͸� �����´�.
    /// </summary>
    private ProductCellData GetToCellData(int type, int level)
    {
        if (type < 0 || type > cellCount) return null;


        int index;
        ProductCellData _cell = new ProductCellData();
        if(level == 0)
            index = (type*maxLevel)+0;
        else
            index = (type * maxLevel)+(level-1);
        _cell.index = index;
        _cell.id = type;
        _cell.name = database[index].name;
        _cell.imageName = database[index].spriteName;
        _cell.level = level;
        _cell.nextLevel = (level == maxLevel) ? maxLevel : level + 1;

        if (level == 0)
        {
            _cell.currentAmount = 0;
            _cell.nextAmount = GetDB(type, level).amount;
            //_cell.nextAmount = database[index].levelTable[level].amount;
        }
        else
        {
            _cell.currentAmount = GetDB(type, level-1).amount;
            _cell.nextAmount = (level == maxLevel) ? 0 : GetDB(type, level).amount;
            //_cell.currentAmount = database[index].levelTable[level - 1].amount;
            //_cell.nextAmount = (level == maxLevel) ? 0 : database[index].levelTable[level].amount;
            _cell.currentAmount *= rates[type].rateCalc.GetResultRate();
            _cell.nextAmount *= rates[type].rateCalc.GetResultRate();
        }

        if(level == maxLevel)
            _cell.cost = 0;
        else
            _cell.cost = GetDB(type, level).cost;
            //_cell.cost = database[index].levelTable[level].cost;
        _cell.cellState = (level == maxLevel) ? CellState.MaxCompletion : CellState.Unlock;
        return _cell;
    }

    private ProductCellData GetToCellDataOfMaxLevel(int type)
    {
        if (type < 0 || type > cellCount) return null;
        //if (index < 0 || index > database.Count) return null;

        //int maxLevel = database[index].levelTable.Count - 1;

        int index;
        ProductCellData _cell = new ProductCellData();
        _cell.index = index = (type*maxLevel)+ (maxLevel-1);
        _cell.id = database[index].ID;
        _cell.name = database[index].name;
        _cell.imageName = database[index].spriteName;
        _cell.level = maxLevel;
        _cell.nextLevel = _cell.level;
        _cell.currentAmount = GetDB(type, maxLevel).amount;
        //_cell.currentAmount = database[index].levelTable[maxLevel].amount;
        _cell.currentAmount *= rates[index].rateCalc.GetResultRate();
        _cell.nextAmount = 0;
        _cell.cost = 0;
        _cell.cellState = CellState.MaxCompletion;
        return _cell;
    }


    #endregion

}
