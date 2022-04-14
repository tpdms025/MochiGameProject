using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class DBManager : MonoBehaviour
{


    #region Fields

    private static DBManager instance = null;
    public static DBManager Inst
    {
        get { return instance; }
        set { instance = value; }
    }

    #endregion

    #region Unity methods

    private void Awake()
    {
        //�̱���
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
    }

    #endregion

    public void LoadData()
    {
        LoadPlayerData();
        LoadOreData();
        LoadWorkmanData();
        LoadBuffTable();
        LoadSkillTable();
    }

    #region Buff

    //���� ���� ������
    private List<Database.BuffTable> m_buffTables;

    public List<Database.BuffTable> LoadBuffTable()
    {
        List<Database.BuffTable> list = new List<Database.BuffTable>();
        list.Add(new Database.BuffTable(0, "��ġ����", 10, 7));
        list.Add(new Database.BuffTable(1, "�ϲ� ���귮 ����", 1800, 1800));
        list.Add(new Database.BuffTable(2, "�ǹ� ��� ", 5, 5));

        m_buffTables = list;
        return m_buffTables;
    }

    public Database.BuffTable GetBuffTable(int idx)
    {
        return m_buffTables[idx];
    }

    #endregion

    #region Skill

    //��ų ���� ������
    private List<Database.SkillTable> m_skillTables;

    public List<Database.SkillTable> LoadSkillTable()
    {
        List<Database.SkillTable> list = new List<Database.SkillTable>();
        list.Add(new Database.SkillTable(0, "�ڵ� ä�� ", 60, 60,1800,1800));

        m_skillTables = list;
        return m_skillTables;
    }

    public Database.SkillTable GetSkillTable(int idx)
    {
        return m_skillTables[idx];
    }

    #endregion


    #region Ore Excel Data

    // ������ ���� ������
    private List<Database.ProductOriginData> m_oreOriginDatas;

    private void LoadOreData()
    {
        m_oreOriginDatas = GetTempOreInfo();
    }

    public List<Database.ProductOriginData> GetOreOriginDatas()
    {
        if (m_oreOriginDatas == null) return null;
        return m_oreOriginDatas;
    }

    private string []oreSpriteNames = { "Ore_Ruby" , "Ore_PinkSapphire", "Ore_LightColorado",
    "Ore_Citrine","Ore_Peridot","Ore_Emerald","Ore_BlueZircon","Ore_Cobalt","Ore_Ametrine",
    "Ore_Iolite","Ore_Diamond"};
    private List<Database.ProductOriginData> GetTempOreInfo()
    {
        List<Database.ProductOriginData> table = new List<Database.ProductOriginData>();
        int ascii = (int)'A';
        for (int i = 0; i < 11; i++)
        {
            Database.ProductOriginData data = new Database.ProductOriginData();
            data.name = "����" + System.Convert.ToString(System.Convert.ToChar(ascii + i));
            data.spriteName = oreSpriteNames[i];
            data.levelTable = GetTempOreLevelValue(i);

            table.Add(data);
        }
        return table;
    }

    /// <summary>
    /// ���ϴ� �ε����� ���� ������������ �����´�.
    /// </summary>
    /// <returns></returns>
    private List<Database.ValueOfLevel> GetTempOreLevelValue(int num)
    {
        List<Database.ValueOfLevel> table = new List<Database.ValueOfLevel>();
        for (int i = 0; i < 5; i++)
        {
            Database.ValueOfLevel data = new Database.ValueOfLevel();
            //data.level = i;
            if (i == 0)
                data.amountPerTouch = 0;
            else
                data.amountPerTouch = BigInteger.Pow(2 + num, i);
            data.cost = BigInteger.Pow(3 + num, i);
            table.Add(data);
        }
        return table;
    }



    #endregion

    #region Workman Excel Data

    // ������ ���� ������
    private List<Database.ProductOriginData> m_workmanOriginDatas;

    private void LoadWorkmanData()
    {
        m_workmanOriginDatas = GetTempWorkmanInfo();
    }

    public List<Database.ProductOriginData> GetWorkmanOriginDatas()
    {
        if (m_workmanOriginDatas == null) return null;
        return m_workmanOriginDatas;
    }

    private string[] workmanSpriteNames = { "Work_Cat" , "Work_Rabbit", "Work_Hedgehog",
    "Work_Mole","Work_Dog","Work_Fox","Work_Elk","Work_Dragon"};
    private List<Database.ProductOriginData> GetTempWorkmanInfo()
    {
        List<Database.ProductOriginData> table = new List<Database.ProductOriginData>();
        int ascii = (int)'A';
        for (int i = 0; i < 8; i++)
        {
            Database.ProductOriginData data = new Database.ProductOriginData();
            data.name = "����" + System.Convert.ToString(System.Convert.ToChar(ascii + i));
            data.spriteName = workmanSpriteNames[i];
            data.levelTable = GetTempWorkmanLevelValue(i);

            table.Add(data);
        }
        return table;
    }

    /// <summary>
    /// ���ϴ� �ε����� ���� ������������ �����´�.
    /// </summary>
    /// <returns></returns>
    private List<Database.ValueOfLevel> GetTempWorkmanLevelValue(int num)
    {
        List<Database.ValueOfLevel> table = new List<Database.ValueOfLevel>();
        for (int i = 0; i < 100; i++)
        {
            Database.ValueOfLevel data = new Database.ValueOfLevel();
            //data.level = i;
            if (i == 0)
                data.amountPerTouch = 0;
            else
                data.amountPerTouch = BigInteger.Pow(2 + num, i);
            data.cost = BigInteger.Pow(3 + num, i);
            table.Add(data);
        }
        return table;
    }



    #endregion

    #region PlayerData

    // �÷��̾� ������
    private Database.PlayerData m_playerData;

    public Database.PlayerData PlayerData
    {
        get { return m_playerData; }
        private set { m_playerData = value; }
    }

    public void LoadPlayerData()
    {
        //�ӽ�
        string workmanLevelString = "0,0,0,0,0,0,0,0";
        m_playerData = new Database.PlayerData(0, 1, workmanLevelString);
        m_playerData.touchCount = 0;
        m_playerData.isAutoMining = false;
    }

    /// <summary>
    /// ���������� ������ ���������͸� �����´�.
    /// </summary>
    public Database.ProductOriginData GetLastOreDataOwned()
    {
        return m_oreOriginDatas[m_playerData.idx_lastOwned];
    }

    #endregion
}
