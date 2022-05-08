using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

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


    private void OnApplicationQuit()
    {
        if (loadAllCompleted)
        {
            //�÷��̾� ������ ����
            SavePlayerData();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (loadAllCompleted)
        {
            if (pause)
            {
                //�÷��̾� ������ ����
                SavePlayerData();
            }
            else
            {
                //��� ������ �ε�
                LoadAllData();
            }
        }
    }

    #endregion


    #region Json

#if UNITY_EDITOR
    readonly public string actualPath = Path.Combine(Application.streamingAssetsPath, "Data");
#elif UNITY_ANDROID
    readonly public string actualPath = Path.Combine(Application.persistentDataPath, "Data");
#elif UNITY_IOS
    readonly public string actualPath = Path.Combine(Application.persistentDataPath, "Data");
#endif

    /// <summary>
    /// ������ ����Ǿ����� Ȯ���Ѵ�.
    /// </summary>
    private bool IsSavedFile(string fileName)
    {
        string fullpath = string.Format("{0}/{1}.json", actualPath, fileName);

        if (File.Exists(fullpath))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    private T LoadJsonFile<T>(string fileName)
    {
        string targetPath = string.Format("{0}/{1}.json", actualPath, fileName);

        string jsonData = File.ReadAllText(targetPath);
        //FileStream fileStream = new FileStream(targetPath, FileMode.Open, FileAccess.Read);
        //byte[] data = new byte[fileStream.Length];
        //fileStream.Read(data, 0, data.Length);
        //fileStream.Close();
        //string jsonData = Encoding.UTF8.GetString(data);

        //return JsonUtility.FromJson<T>(jsonData);
        return JsonToObject<T>(jsonData);
    }


    private void SaveJsonFile<T>(T database, string fileName) where T : new()
    {
        string jsonData = ObjectToJson(database);
        WriteToFile(jsonData, fileName);
    }


    private void WriteToFile(string jsonData, string fileName)
    {
        //���丮�� ���ٸ� ����
        if (!File.Exists(actualPath))
        {
            Directory.CreateDirectory(actualPath);
        }

        string fullpath = string.Format("{0}/{1}.json", actualPath, fileName);
        File.WriteAllText(fullpath, jsonData);
        //FileStream fileStream = new FileStream(fullpath, FileMode.OpenOrCreate, FileAccess.Write);

        //byte[] data = Encoding.UTF8.GetBytes(jsonData);
        //fileStream.Write(data, 0, data.Length);
        //fileStream.Close();
    }



    private string ObjectToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    private T JsonToObject<T>(string jsonData)
    {
        return JsonConvert.DeserializeObject<T>(jsonData);
    }

#endregion


    #region Load/Save

    /// <summary>
    /// ��� ���̺��� ������ �ε��Ѵ�.
    /// </summary>
    public void LoadAllData()
    {
        TableManager.Inst.LoadAllTable();
        
        //Load_OreTable();
        //Load_WorkmanTable();
        //Load_EnhancementTable();
        Load_BuffTable();
        Load_SkillTable();

        LoadPlayerData();
        //yield return null;
    }

    /// <summary>
    /// �÷��̾� �����͸� �ε��Ѵ�.
    /// </summary>
    public void LoadPlayerData()
    {
        //PlayerPrefs.DeleteAll();
        //�÷��̾ ������ �κ��丮 ������ �ε�
        Load_InventoryData();

        //�÷��� �⺻ ������ �ε�
        Load_PlayData();

        //(*�߿�*) ���� ���̺��� �о ����ϱ⿡ �Ǹ��������� �ε��� ��
        //��ȭ ������ �ε�
        Load_MoneyData();
    }

    /// <summary>
    /// �÷��̾� �����͸� �����Ѵ�.
    /// </summary>
    public void SavePlayerData()
    {
        //�÷��̾ ������ �κ��丮 ������ ����
        Save_InventoryData();

        //�÷��̾� �⺻ ������ ����
        Save_PlayData();

        //��ȭ ������ ����
        Save_MoneyData();
    }

#endregion

    /// <summary>
    /// �����͸� ó�������� ������ �ʱ�ȭ�Ѵ�.
    /// </summary>
    public void ResetDataOnPrestige()
    {
        InitInventoryData();
        MoneyManager.Inst.ResetOnPrestige();
    }



    #region Buff Table

    //���� ���� ������
    private List<Database.BuffTable> m_buffTables;
    public Database.BuffTable GetBuffTable(int idx)
    {
        return m_buffTables[idx];
    }


    public List<Database.BuffTable> Load_BuffTable()
    {
        List<Database.BuffTable> list = new List<Database.BuffTable>();
        list.Add(new Database.BuffTable(0, "��ġ����", 1800, 1800));
        list.Add(new Database.BuffTable(1, "�ϲ� ���귮 ����", 1800, 1800));
        list.Add(new Database.BuffTable(2, "�ǹ� ��� ", 5, 5));

        m_buffTables = list;
        return m_buffTables;
    }

#endregion

    #region Skill Table

    //��ų ���� ������
    private List<Database.SkillTable> m_skillTables;
    public Database.SkillTable GetSkillTable(int idx)
    {
        return m_skillTables[idx];
    }

    public List<Database.SkillTable> Load_SkillTable()
    {
        List<Database.SkillTable> list = new List<Database.SkillTable>();
        list.Add(new Database.SkillTable(0, "�ڵ� ä�� ", 60, 0, 1800, 1800));

        m_skillTables = list;
        return m_skillTables;
    }


#endregion


    #region Ore Excel Table
    public readonly int oreMaxLevel = 5;

    // ������ ���� ������
    public Database.ProductOriginData GetOreOriginData(int idx, int level)
    {
        if (level == 0)
            level=1;
        return TableManager.Inst.GetOriginOreData((idx * oreMaxLevel) + level-1);
    }
    public List<Database.ProductOriginData> GetOreOriginDatas()
    {
        return TableManager.Inst.GetOriginOreDataList();
    }


#endregion

    #region Workman Excel Table

    // �ϲ��� ���� ������
    public readonly int workmanMaxLevel = 5;

    public Database.ProductOriginData GetWorkmanOriginData(int idx, int level)
    {
        if (level == 0)
            level=1;
        return TableManager.Inst.GetOriginWorkmanData((idx * workmanMaxLevel) + level-1);
    }
    public List<Database.ProductOriginData> GetWorkmanOriginDatas()
    {
        return TableManager.Inst.GetOriginWorkmanList();
    }


#endregion

    #region Enhancement Excel Table

    // ��ȭ ���� ������
    public readonly int enhancementMaxLevel = 5;

    public Database.ProductOriginData_ GetEnhancementOriginData(int idx, int level)
    {
        if (level == 0)
            level=1;
        return TableManager.Inst.GetOriginEnhancementData((idx * enhancementMaxLevel) + level-1);
    }
    public List<Database.ProductOriginData_> GetEnhancementOriginDatas()
    {
        return TableManager.Inst.GetOriginEnhancementList();
    }


#endregion


    #region PlayData

    // �÷��� ������
    private Database.PlayData m_playData;
    public Database.PlayData playData
    {
        get { return m_playData; }
        private set { m_playData = value; }
    }

    private void InitPlayData()
    {
        Database.PlayData data = playData = new Database.PlayData();
        data.lastDateTime = System.DateTime.Now;
        data.prestigeCount = 0;
        data.cumPrestigeRate = 0;
    }


    private void Load_PlayData()
    {
        if (IsSavedFile("PlayData"))
        {
            //����� ������ �ε�
            Database.PlayData data = LoadJsonFile<Database.PlayData>("PlayData");
            playData = data;
        }
        else
        {
            //�������� ���� ��� �ʱ� ������ �ε� 
            InitPlayData();
        }

        //if (PlayerPrefs.HasKey("save"))
        //{
        //    //����� ������ �ε�
        //    Database.PlayData data = playData = new Database.PlayData();
        //    //data.lastDateTime = System.DateTime.Now;
        //    data.lastDateTime = System.Convert.ToDateTime(PlayerPrefs.GetString("lastDateTime"));
        //    data.prestigeCount = PlayerPrefs.GetInt("prestigeCount");
        //    data.cumPrestigeRate = PlayerPrefs.GetFloat("cumPrestigeRate");
        //}
        //else
        //{
        //    //�������� ���� ��� �ʱ� ������ �ε� 
        //    InitPlayData();
        //}
    }
    private void Save_PlayData()
    {
        Database.PlayData data = playData;
        data.lastDateTime = System.DateTime.Now;
        SaveJsonFile<Database.PlayData>(data, "PlayData");

        //Database.PlayData data = playData;

        //string strDateTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        //Debug.Log("strDateTime" + strDateTime);
        //PlayerPrefs.SetString("lastDateTime", strDateTime);
        //PlayerPrefs.SetInt("prestigeCount", data.prestigeCount);
        //PlayerPrefs.SetFloat("cumPrestigeRate", data.cumPrestigeRate);
    }

#endregion

    #region Inventory

    // �����ϰ� �ִ� ������ (ȯ������ �ʱ�ȭ�� ������)
    private Database.Inventory m_inventory;

    public Database.Inventory inventory
    {
        get { return m_inventory; }
        private set { m_inventory = value; }
    }

    /// <summary>
    /// ���������� ������ ���������͸� �����´�.
    /// </summary>
    public Database.ProductOriginData GetLastOreDataOwned()
    {
        return GetOreOriginData(inventory.oreIdx_lastOwned, inventory.oreLevel_lastOwned);
    }

    private void InitInventoryData()
    {
        Database.Inventory data = inventory = new Database.Inventory();
        data.oreIdx_lastOwned = 0;
        data.oreLevel_lastOwned = 1;            //������ 1�������� ����.
        data.workmanLevels_owned = new int[8];
        data.workmanCount = 0;
        data.enhanceLevels_owned = new int[13];
        data.enhanceAmounts_owned = new float[13];
        data.touchCount = 0;
        data.isAutoMining = false;
        data.remainingbuffTimes = new int[3];
        data.remaining_skillTimes = new int[1];
        data.remaining_skillCooldowns = new int[1];
    }

    private void Load_InventoryData()
    {
        if (IsSavedFile("InventoryData"))
        {
            Database.Inventory data = LoadJsonFile<Database.Inventory>("InventoryData");
            inventory = data;
        }
        else
        {
            InitInventoryData();
        }



        //if (PlayerPrefs.HasKey("save"))
        //{
        //    string workmanLevelStr = PlayerPrefs.GetString("workmanLevelStr");
        //    string enhanceLevelStr = PlayerPrefs.GetString("enhanceLevelStr");
        //    string enhanceAmountStr = PlayerPrefs.GetString("enhanceAmountStr");
        //    string remainingBuffTimesStr = PlayerPrefs.GetString("remainingBuffTimesStr");
        //    string remainingSkill_TimesStr = PlayerPrefs.GetString("remainingSkill_TimesStr");
        //    string remainingSkill_CooldownStr = PlayerPrefs.GetString("remainingSkill_CooldownStr");

        //    Database.Inventory data = inventory = new Database.Inventory();
        //    data.oreIdx_lastOwned = PlayerPrefs.GetInt("oreIdx_lastOwned");
        //    data.oreLevel_lastOwned = PlayerPrefs.GetInt("oreLevel_lastOwned");
        //    data.workmanLevels_owned = data.Convert_IntToArray(workmanLevelStr);
        //    data.workmanCount = PlayerPrefs.GetInt("workmanCount");
        //    data.enhanceLevels_owned = data.Convert_IntToArray(enhanceLevelStr);
        //    data.enhanceAmounts_owned = data.Convert_FloatToArray(enhanceAmountStr);
        //    data.touchCount = PlayerPrefs.GetInt("touchCount");
        //    data.isAutoMining = PlayerPrefs.GetInt("isAutoMining") == 1 ? true : false;
        //    data.remainingbuffTimes = data.Convert_IntToArray(remainingBuffTimesStr);
        //    data.remaining_skillTimes = data.Convert_IntToArray(remainingSkill_TimesStr);
        //    data.remaining_skillCooldowns = data.Convert_IntToArray(remainingSkill_CooldownStr);
        //}
        //else
        //{
        //    InitInventoryData();
        //}
    }

    private void Save_InventoryData()
    {
        Save_BuffSkillTimeData();

        Database.Inventory data = inventory;
        SaveJsonFile<Database.Inventory>(data, "InventoryData");
    }



    //    Database.Inventory data = inventory;

    //    PlayerPrefs.SetInt("oreIdx_lastOwned", data.oreIdx_lastOwned);
    //    PlayerPrefs.SetInt("oreLevel_lastOwned", data.oreLevel_lastOwned);
    //    PlayerPrefs.SetString("workmanLevelStr", data.Convert_ToString(data.workmanLevels_owned));
    //    PlayerPrefs.SetInt("workmanCount", data.oreLevel_lastOwned);
    //    PlayerPrefs.SetString("enhanceLevelStr", data.Convert_ToString(data.enhanceLevels_owned));
    //    PlayerPrefs.SetString("enhanceAmountStr", data.Convert_ToString(data.enhanceAmounts_owned));
    //    PlayerPrefs.SetInt("touchCount", data.touchCount);
    //    PlayerPrefs.SetInt("isAutoMining", data.isAutoMining ? 1 : 0);

    //    Save_BuffSkillTimeData();

    //    PlayerPrefs.SetString("remainingBuffTimesStr", data.Convert_ToString(data.remainingbuffTimes));
    //    PlayerPrefs.SetString("remainingSkill_TimesStr", data.Convert_ToString(data.remaining_skillTimes));
    //    PlayerPrefs.SetString("remainingSkill_CooldownStr", data.Convert_ToString(data.remaining_skillCooldowns));
    //}


    private void Save_BuffSkillTimeData()
    {
        if (BuffControl.SaveAction != null)
        {
            BuffControl.SaveAction.Invoke();
        }
    }



#endregion

    #region Money
    public void Load_MoneyData()
    {
        if (IsSavedFile("MoneyData"))
        {
            Database.MoneyData data = LoadJsonFile<Database.MoneyData>("MoneyData");
            double jewel = System.Convert.ToDouble(data.baseJewel);
            double coin = System.Convert.ToDouble(data.baseCoin);
            double jewelPerTouch = System.Convert.ToDouble(data.baseJewelPerTouch);
            double jewelPerSec = System.Convert.ToDouble(data.baseJewelPerSec);
            MoneyManager.Inst.SetMoneyData(jewel, coin, jewelPerTouch, jewelPerSec);
            MoneyManager.Inst.SetAbilityData(inventory.workmanLevels_owned);
        }
        else
        {
            MoneyManager.Inst.SetMoneyData(0, 0, 0, 0);
            MoneyManager.Inst.SetAbilityData(inventory.workmanLevels_owned);
            MoneyManager.Inst.InitializeAll();
        }

        //if (PlayerPrefs.HasKey("save"))
        //{
        //    //double jewel = CurrencyParser.ToCurrencyDouble(PlayerPrefs.GetString("jewel"));
        //    //double coin = CurrencyParser.ToCurrencyDouble(PlayerPrefs.GetString("coin"));
        //    //double jewelPerTouch = CurrencyParser.ToCurrencyDouble(PlayerPrefs.GetString("jewelPerTouch"));
        //    //double jewelPerSec = CurrencyParser.ToCurrencyDouble(PlayerPrefs.GetString("jewelPerSec"));
        //    double jewel = System.Convert.ToDouble(PlayerPrefs.GetString("jewel"));
        //    double coin = System.Convert.ToDouble(PlayerPrefs.GetString("coin"));
        //    double jewelPerTouch = System.Convert.ToDouble(PlayerPrefs.GetString("jewelPerTouch"));
        //    double jewelPerSec = System.Convert.ToDouble(PlayerPrefs.GetString("jewelPerSec"));
        //    MoneyManager.Inst.SetMoneyData(jewel, coin, jewelPerTouch, jewelPerSec);
        //    MoneyManager.Inst.SetAbilityData(inventory.workmanLevels_owned);
        //}
        //else
        //{
        //    MoneyManager.Inst.SetMoneyData(0, 0, 0, 0);
        //    MoneyManager.Inst.SetAbilityData(inventory.workmanLevels_owned);
        //    MoneyManager.Inst.InitializeAll();
        //}
    }
    public void Save_MoneyData()
    {
        Database.MoneyData data = new Database.MoneyData();
        data.baseJewel = MoneyManager.Inst.Jewel.baseValue.ToString();
        data.baseCoin = MoneyManager.Inst.Coin.baseValue.ToString();
        data.baseJewelPerTouch = MoneyManager.Inst.JewelPerTouch.baseValue.ToString();
        data.baseJewelPerSec = MoneyManager.Inst.JewelPerSec.baseValue.ToString();
        
        SaveJsonFile<Database.MoneyData>(data,"MoneyData");

        ////PlayerPrefs.SetString("jewel", MoneyManager.Inst.strJewel);
        ////PlayerPrefs.SetString("coin", MoneyManager.Inst.strCoin);
        //PlayerPrefs.SetString("jewel", MoneyManager.Inst.Jewel.baseValue.ToString());
        //PlayerPrefs.SetString("coin", MoneyManager.Inst.Coin.baseValue.ToString());

        ////���� �� ���� ���� �ۼ�Ʈ�� ������� ���� �⺻ ���� �����Ѵ�.
        ////string strJewelPerTouch = CurrencyParser.ToCurrencyString(MoneyManager.Inst.JewelPerTouch.value);
        ////string strJewelPerSec = CurrencyParser.ToCurrencyString(MoneyManager.Inst.JewelPerSec.value);
        //PlayerPrefs.SetString("jewelPerTouch", MoneyManager.Inst.JewelPerTouch.baseValue.ToString());
        //PlayerPrefs.SetString("jewelPerSec", MoneyManager.Inst.JewelPerSec.baseValue.ToString());
    }
#endregion

    #region GameManager

    public bool loadAllCompleted = false;
    public bool loadDataCompleted = false;


    //�ݺ� �ڷ�ƾ�� ���� �� �ִ� bool ����
    //ȯ�� ����� ����� ���ϵ��� ����ó��
    private bool m_isGameStop;
    public bool isGameStop 
    {
        get { return m_isGameStop; }
        set { m_isGameStop = value; }
    }


    //���ø����̼��� ó�� �����ߴ���
    private bool m_firstStart = false;
    public bool firstStart
    {
        get { return m_firstStart; }
        set { m_firstStart = value; }
    }



#endregion
}
