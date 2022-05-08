using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class DBManager_Ver_PlayerPrefs : MonoBehaviour
{
    #region Fields

    private static DBManager_Ver_PlayerPrefs instance = null;
    public static DBManager_Ver_PlayerPrefs Inst
    {
        get { return instance; }
        set { instance = value; }
    }

    #endregion

    #region Unity methods

    private void Awake()
    {
        //싱글톤
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

    #endregion


    private void OnApplicationQuit()
    {
        //플레이어 데이터 저장
        SavePlayerData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            //플레이어 데이터 저장
            SavePlayerData();
        }
        else
        {
            //모든 데이터 로드
            LoadAllData();
        }
    }

    #region Load/Save

    /// <summary>
    /// 모든 테이블의 정보를 로드한다.
    /// </summary>
    public IEnumerator LoadAllData()
    {
        TableManager.Inst.LoadAllTable();

        //Load_OreTable();
        //Load_WorkmanTable();
        //Load_EnhancementTable();
        Load_BuffTable();
        Load_SkillTable();

        LoadPlayerData();
        yield return null;
    }

    /// <summary>
    /// 플레이어 데이터를 로드한다.
    /// </summary>
    public void LoadPlayerData()
    {
        //PlayerPrefs.DeleteAll();
        //플레이어가 소유한 인벤토리 데이터 로드
        Load_InventoryData();

        //플레이 기본 데이터 로드
        Load_PlayData();

        //(*중요*) 엑셀 테이블을 읽어서 사용하기에 맨마지막으로 로드할 것
        //재화 데이터 로드
        Load_MoneyData();
    }

    /// <summary>
    /// 플레이어 데이터를 저장한다.
    /// </summary>
    public void SavePlayerData()
    {
        PlayerPrefs.SetInt("save", 1);

        //플레이어가 소유한 인벤토리 데이터 저장
        Save_InventoryData();

        //플레이어 기본 데이터 저장
        Save_PlayData();

        //재화 데이터 저장
        Save_MoneyData();
        PlayerPrefs.Save();
    }

    #endregion

    /// <summary>
    /// 데이터를 처음상태의 값으로 초기화한다.
    /// </summary>
    public void ResetDataOnPrestige()
    {
        InitInventoryData();
        MoneyManager.Inst.ResetOnPrestige();
    }



    #region Buff Table

    //버프 엑셀 데이터
    private List<Database.BuffTable> m_buffTables;
    public Database.BuffTable GetBuffTable(int idx)
    {
        return m_buffTables[idx];
    }


    public List<Database.BuffTable> Load_BuffTable()
    {
        List<Database.BuffTable> list = new List<Database.BuffTable>();
        list.Add(new Database.BuffTable(0, "터치증가", 1800, 1800));
        list.Add(new Database.BuffTable(1, "일꾼 생산량 증가", 1800, 1800));
        list.Add(new Database.BuffTable(2, "피버 모드 ", 5, 5));

        m_buffTables = list;
        return m_buffTables;
    }

    #endregion

    #region Skill Table

    //스킬 엑셀 데이터
    private List<Database.SkillTable> m_skillTables;
    public Database.SkillTable GetSkillTable(int idx)
    {
        return m_skillTables[idx];
    }

    public List<Database.SkillTable> Load_SkillTable()
    {
        List<Database.SkillTable> list = new List<Database.SkillTable>();
        list.Add(new Database.SkillTable(0, "자동 채굴 ", 60, 0, 1800, 1800));

        m_skillTables = list;
        return m_skillTables;
    }


    #endregion


    #region Ore Excel Table
    public readonly int oreMaxLevel = 5;

    // 광석의 엑셀 데이터
    public Database.ProductOriginData GetOreOriginData(int idx, int level)
    {
        return TableManager.Inst.GetOriginOreData((idx * oreMaxLevel) + level);
    }
    public List<Database.ProductOriginData> GetOreOriginDatas()
    {
        return TableManager.Inst.GetOriginOreDataList();
    }


    private string []oreSpriteNames = { "Ore_Ruby" , "Ore_PinkSapphire", "Ore_LightColorado",
    "Ore_Citrine","Ore_Peridot","Ore_Emerald","Ore_BlueZircon","Ore_Cobalt","Ore_Ametrine",
    "Ore_Iolite","Ore_Diamond"};
    private List<Database._ProductOriginData> GetTempOreInfo()
    {
        List<Database._ProductOriginData> table = new List<Database._ProductOriginData>();
        int ascii = (int)'A';
        for (int i = 0; i < 11; i++)
        {
            Database._ProductOriginData data = new Database._ProductOriginData();
            data.name = "광석" + System.Convert.ToString(System.Convert.ToChar(ascii + i));
            data.spriteName = oreSpriteNames[i];
            data.levelTable = GetTempOreLevelValue(i);

            table.Add(data);
        }
        return table;
    }

    /// <summary>
    /// 원하는 인덱스의 광석 레벨정보들을 가져온다.
    /// </summary>
    private List<Database.ValueOfLevel> GetTempOreLevelValue(int num)
    {
        List<Database.ValueOfLevel> table = new List<Database.ValueOfLevel>();
        for (int i = 1; i <= 5; i++)
        {
            Database.ValueOfLevel data = new Database.ValueOfLevel();
            //data.level = i;
            data.amount = System.Math.Pow(2 + num, i);
            data.cost = System.Math.Pow(4 + num, i);
            table.Add(data);
        }
        return table;
    }



    #endregion

    #region Workman Excel Table

    // 일꾼의 엑셀 데이터
    public readonly int workmanMaxLevel = 5;

    public Database.ProductOriginData GetWorkmanOriginData(int idx, int level)
    {
        return TableManager.Inst.GetOriginWorkmanData((idx * workmanMaxLevel) + level);
    }
    public List<Database.ProductOriginData> GetWorkmanOriginDatas()
    {
        return TableManager.Inst.GetOriginWorkmanList();
    }



    //private List<Database._ProductOriginData> m_workmanOriginDatas;
    //public List<Database._ProductOriginData> GetWorkmanOriginDatas()
    //{
    //    if (m_workmanOriginDatas == null) return null;
    //    return m_workmanOriginDatas;
    //}


    //private void Load_WorkmanTable()
    //{
    //   m_workmanOriginDatas = GetTempWorkmanInfo();
    //}

    private string[] workmanSpriteNames = { "Work_Cat" , "Work_Rabbit", "Work_Hedgehog",
    "Work_Mole","Work_Dog","Work_Fox","Work_Elk","Work_Dragon"};
    private List<Database._ProductOriginData> GetTempWorkmanInfo()
    {
        List<Database._ProductOriginData> table = new List<Database._ProductOriginData>();
        int ascii = (int)'A';
        for (int i = 0; i < 8; i++)
        {
            Database._ProductOriginData data = new Database._ProductOriginData();
            data.name = "모찌" + System.Convert.ToString(System.Convert.ToChar(ascii + i));
            data.spriteName = workmanSpriteNames[i];
            data.levelTable = GetTempWorkmanLevelValue(i);

            table.Add(data);
        }
        return table;
    }

    /// <summary>
    /// 원하는 인덱스의 일꾼 레벨정보들을 가져온다.
    /// </summary>
    private List<Database.ValueOfLevel> GetTempWorkmanLevelValue(int num)
    {
        List<Database.ValueOfLevel> table = new List<Database.ValueOfLevel>();
        for (int i = 1; i <= 10; i++)
        {
            Database.ValueOfLevel data = new Database.ValueOfLevel();
            //data.level = i;
            data.amount = System.Math.Pow(2 + num, i);
            data.cost = System.Math.Pow(3 + num, i);
            table.Add(data);
        }
        return table;
    }



    #endregion

    #region Enhancement Excel Table

    // 강화 엑셀 데이터
    public readonly int enhancementMaxLevel = 5;

    public Database.ProductOriginData_ GetEnhancementOriginData(int idx, int level)
    {
        return TableManager.Inst.GetOriginEnhancementData((idx * enhancementMaxLevel) + level);
    }
    public List<Database.ProductOriginData_> GetEnhancementOriginDatas()
    {
        return TableManager.Inst.GetOriginEnhancementList();
    }



    //private List<Database._ProductOriginData> m_enhancementOriginDatas;
    //public List<Database._ProductOriginData> GetEnhancementOriginDatas()
    //{
    //    if (m_enhancementOriginDatas == null) return null;
    //    return m_enhancementOriginDatas;
    //}


    //private void Load_EnhancementTable()
    //{
    //    m_enhancementOriginDatas = GetTempEnhancementInfo();
    //}

    private string[] enhancementDescriptionNames = { "터치당 보석 획득량 증가" , "모찌냥 생산량 증가", "모찌묘 생산량 증가",
    "모찌도치 생산량 증가","두더모찌 생산량 증가","모찌멍 생산량 증가","구미모찌 생산량 증가","모찌라니 생산량 증가","모찌용 생산량 증가",
    "자동 채굴 스킬 지속시간 증가","자동 채굴 스킬 터치량 증가","피버 광석 지속시간 증가","피버 광석 보석량 증가"};
    private List<Database._ProductOriginData> GetTempEnhancementInfo()
    {
        List<Database._ProductOriginData> table = new List<Database._ProductOriginData>();
        int ascii = (int)'A';
        for (int i = 0; i < 13; i++)
        {
            Database._ProductOriginData data = new Database._ProductOriginData();
            data.name = "강화" + System.Convert.ToString(System.Convert.ToChar(ascii + i));
            data.ID = i;
            data.spriteName = "";
            data.description = enhancementDescriptionNames[i];
            data.levelTable = GetTempEnhancementLevelValue(i);

            table.Add(data);
        }
        return table;
    }

    /// <summary>
    /// 원하는 인덱스의 강화 레벨정보들을 가져온다.
    /// </summary>
    private List<Database.ValueOfLevel> GetTempEnhancementLevelValue(int num)
    {
        List<Database.ValueOfLevel> table = new List<Database.ValueOfLevel>();
        for (int i = 1; i <= 10; i++)
        {
            Database.ValueOfLevel data = new Database.ValueOfLevel();
            //data.level = i;
            data.amount = (10 + num) * i;
            data.cost = System.Math.Pow(4 + num, i);
            table.Add(data);
        }
        return table;
    }



    #endregion


    #region PlayData

    // 플레이 데이터
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
        if (PlayerPrefs.HasKey("save"))
        {
            //저장된 데이터 로드
            Database.PlayData data = playData = new Database.PlayData();
            //data.lastDateTime = System.DateTime.Now;
            data.lastDateTime = System.Convert.ToDateTime(PlayerPrefs.GetString("lastDateTime"));
            data.prestigeCount = PlayerPrefs.GetInt("prestigeCount");
            data.cumPrestigeRate = PlayerPrefs.GetFloat("cumPrestigeRate");
        }
        else
        {
            //저장하지 않을 경우 초기 데이터 로드 
            InitPlayData();
        }
    }
    private void Save_PlayData()
    {
        Database.PlayData data = playData;

        string strDateTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        Debug.Log("strDateTime" + strDateTime);
        PlayerPrefs.SetString("lastDateTime", strDateTime);
        PlayerPrefs.SetInt("prestigeCount", data.prestigeCount);
        PlayerPrefs.SetFloat("cumPrestigeRate", data.cumPrestigeRate);
    }

    #endregion

    #region Inventory

    // 소유하고 있는 데이터 (환생으로 초기화가 가능한)
    private Database.Inventory m_inventory;

    public Database.Inventory inventory
    {
        get { return m_inventory; }
        private set { m_inventory = value; }
    }

    /// <summary>
    /// 마지막으로 소유한 광석데이터를 가져온다.
    /// </summary>
    public Database.ProductOriginData GetLastOreDataOwned()
    {
        return GetOreOriginData(inventory.oreIdx_lastOwned , 0);
    }

    private void InitInventoryData()
    {
        Database.Inventory data = inventory = new Database.Inventory();
        data.oreIdx_lastOwned = 0;
        data.oreLevel_lastOwned = 1;            //광석은 1레벨부터 시작.
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
        if (PlayerPrefs.HasKey("save"))
        {
            string workmanLevelStr = PlayerPrefs.GetString("workmanLevelStr");
            string enhanceLevelStr = PlayerPrefs.GetString("enhanceLevelStr");
            string enhanceAmountStr = PlayerPrefs.GetString("enhanceAmountStr");
            string remainingBuffTimesStr = PlayerPrefs.GetString("remainingBuffTimesStr");
            string remainingSkill_TimesStr = PlayerPrefs.GetString("remainingSkill_TimesStr");
            string remainingSkill_CooldownStr = PlayerPrefs.GetString("remainingSkill_CooldownStr");

            Database.Inventory data = inventory = new Database.Inventory();
            data.oreIdx_lastOwned = PlayerPrefs.GetInt("oreIdx_lastOwned");
            data.oreLevel_lastOwned = PlayerPrefs.GetInt("oreLevel_lastOwned");
            data.workmanLevels_owned = data.Convert_IntToArray(workmanLevelStr);
            data.workmanCount = PlayerPrefs.GetInt("workmanCount");
            data.enhanceLevels_owned = data.Convert_IntToArray(enhanceLevelStr);
            data.enhanceAmounts_owned = data.Convert_FloatToArray(enhanceAmountStr);
            data.touchCount = PlayerPrefs.GetInt("touchCount");
            data.isAutoMining = PlayerPrefs.GetInt("isAutoMining") == 1 ? true : false;
            data.remainingbuffTimes = data.Convert_IntToArray(remainingBuffTimesStr);
            data.remaining_skillTimes = data.Convert_IntToArray(remainingSkill_TimesStr);
            data.remaining_skillCooldowns = data.Convert_IntToArray(remainingSkill_CooldownStr);
        }
        else
        {
            InitInventoryData();
        }
    }

    private void Save_InventoryData()
    {
        Database.Inventory data = inventory;

        PlayerPrefs.SetInt("oreIdx_lastOwned", data.oreIdx_lastOwned);
        PlayerPrefs.SetInt("oreLevel_lastOwned", data.oreLevel_lastOwned);
        PlayerPrefs.SetString("workmanLevelStr", data.Convert_ToString(data.workmanLevels_owned));
        PlayerPrefs.SetInt("workmanCount", data.oreLevel_lastOwned);
        PlayerPrefs.SetString("enhanceLevelStr", data.Convert_ToString(data.enhanceLevels_owned));
        PlayerPrefs.SetString("enhanceAmountStr", data.Convert_ToString(data.enhanceAmounts_owned));
        PlayerPrefs.SetInt("touchCount", data.touchCount);
        PlayerPrefs.SetInt("isAutoMining", data.isAutoMining ? 1 : 0);

        Save_BuffSkillTimeData();

        PlayerPrefs.SetString("remainingBuffTimesStr", data.Convert_ToString(data.remainingbuffTimes));
        PlayerPrefs.SetString("remainingSkill_TimesStr", data.Convert_ToString(data.remaining_skillTimes));
        PlayerPrefs.SetString("remainingSkill_CooldownStr", data.Convert_ToString(data.remaining_skillCooldowns));
    }


    private void Save_BuffSkillTimeData()
    {
        if (BuffControl.SaveAction != null)
        {
            BuffControl.SaveAction.Invoke();
            Debug.Log("SaveAction");
        }
    }



    #endregion

    #region Money
    public void Load_MoneyData()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            //double jewel = CurrencyParser.ToCurrencyDouble(PlayerPrefs.GetString("jewel"));
            //double coin = CurrencyParser.ToCurrencyDouble(PlayerPrefs.GetString("coin"));
            //double jewelPerTouch = CurrencyParser.ToCurrencyDouble(PlayerPrefs.GetString("jewelPerTouch"));
            //double jewelPerSec = CurrencyParser.ToCurrencyDouble(PlayerPrefs.GetString("jewelPerSec"));
            double jewel = System.Convert.ToDouble(PlayerPrefs.GetString("jewel"));
            double coin = System.Convert.ToDouble(PlayerPrefs.GetString("coin"));
            double jewelPerTouch = System.Convert.ToDouble(PlayerPrefs.GetString("jewelPerTouch"));
            double jewelPerSec = System.Convert.ToDouble(PlayerPrefs.GetString("jewelPerSec"));
            MoneyManager.Inst.SetMoneyData(jewel, coin, jewelPerTouch, jewelPerSec);
            MoneyManager.Inst.SetAbilityData(inventory.workmanLevels_owned);
        }
        else
        {
            MoneyManager.Inst.SetMoneyData(0, 0, 0, 0);
            MoneyManager.Inst.SetAbilityData(inventory.workmanLevels_owned);
            MoneyManager.Inst.InitializeAll();
        }
    }
    public void Save_MoneyData()
    {
        //PlayerPrefs.SetString("jewel", MoneyManager.Inst.strJewel);
        //PlayerPrefs.SetString("coin", MoneyManager.Inst.strCoin);
        PlayerPrefs.SetString("jewel", MoneyManager.Inst.Jewel.baseValue.ToString());
        PlayerPrefs.SetString("coin", MoneyManager.Inst.Coin.baseValue.ToString());

        //증가 및 감소 비율 퍼센트를 계산하지 않은 기본 값을 저장한다.
        //string strJewelPerTouch = CurrencyParser.ToCurrencyString(MoneyManager.Inst.JewelPerTouch.value);
        //string strJewelPerSec = CurrencyParser.ToCurrencyString(MoneyManager.Inst.JewelPerSec.value);
        PlayerPrefs.SetString("jewelPerTouch", MoneyManager.Inst.JewelPerTouch.baseValue.ToString());
        PlayerPrefs.SetString("jewelPerSec", MoneyManager.Inst.JewelPerSec.baseValue.ToString());
    }
    #endregion

    #region GameManager

    //반복 코루틴을 멈출 수 있는 bool 변수
    //환생 연출시 계산을 못하도록 예외처리
    private bool m_isGameStop;
    public bool isGameStop 
    {
        get { return m_isGameStop; }
        set { m_isGameStop = value; }
    }

    //어플리케이션을 처음 실행했는지
    private bool m_firstStart = false;
    public bool firstStart
    {
        get { return m_firstStart; }
        set { m_firstStart = value; }
    }



    #endregion
}
