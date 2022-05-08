using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TableManager : MonoBehaviour
{
#if UNITY_EDITOR
    readonly public string actualPath = Path.Combine(Application.streamingAssetsPath, "CSV");
#elif UNITY_ANDROID
    readonly public string actualPath = Path.Combine ("jar:file://" + Application.dataPath + "!/assets", "CSV");
#elif UNITY_IOS
    readonly public string actualPath = Path.Combine (Application.dataPath + "Raw", "CSV");
#endif

    public readonly string oreFileName = "Ore.csv";
    public readonly string workmanFileName = "Workman.csv";
    public readonly string enhancementFileName = "Enhancement.csv";

    private TableTemplate<Database.ProductOriginData> originOreTable;
    private TableTemplate<Database.ProductOriginData> originWorkmanTable;
    private TableTemplate<Database.ProductOriginData_> originEnhancementTable;

    private const int oreMaxLevel = 5;

    //public TMPro.TextMeshProUGUI testText;

    private void Awake()
    {
        if (Inst != null)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;
        DontDestroyOnLoad(gameObject);
        //testText.text = System.IO.Path.Combine(actualPath, oreFileName);
    }

    public void LoadAllTable()
    {
        LoadTable(oreFileName, actualPath, out originOreTable);
        LoadTable(workmanFileName, actualPath, out originWorkmanTable);
        LoadTable(enhancementFileName, actualPath, out originEnhancementTable);
    }

    public void LoadTable(string fileName, string targetPath, out TableTemplate<Database.ProductOriginData> table)
    {
        string str =string.Empty;
        string fullPath = System.IO.Path.Combine(targetPath, fileName);
        table = new TableTemplate<Database.ProductOriginData>(fullPath);
        table.Load(ref str);
        //testText.text += str;
    }
    public void LoadTable(string fileName, string targetPath, out TableTemplate<Database.ProductOriginData_> table)
    {
        string str = string.Empty;
        string fullPath = System.IO.Path.Combine(targetPath, fileName);
        table = new TableTemplate<Database.ProductOriginData_>(fullPath);
        table.Load(ref str);
        //testText.text += str;
    }

    public Database.ProductOriginData GetOriginOreData(int nID)
    {
        if (originOreTable == null) return null;
        return originOreTable.GetData(nID);
    }

    public List<Database.ProductOriginData> GetOriginOreDataList()
    {
        if (originOreTable == null) return null;
        return originOreTable.GetDataList();
    }

    public Database.ProductOriginData GetOriginWorkmanData(int nID)
    {
        if (originWorkmanTable == null) return null;
        return originWorkmanTable.GetData(nID);
    }
    public List<Database.ProductOriginData> GetOriginWorkmanList()
    {
        if (originWorkmanTable == null) return null;
        return originWorkmanTable.GetDataList();
    }

    public Database.ProductOriginData_ GetOriginEnhancementData(int nID)
    {
        if (originEnhancementTable == null) return null;
        return originEnhancementTable.GetData(nID);
    }
    public List<Database.ProductOriginData_> GetOriginEnhancementList()
    {
        if (originEnhancementTable == null) return null;
        return originEnhancementTable.GetDataList();
    }


    #region Instance
    private static TableManager instance;
    public static TableManager Inst { get { return instance; } set { instance = value; } }

    #endregion

}
