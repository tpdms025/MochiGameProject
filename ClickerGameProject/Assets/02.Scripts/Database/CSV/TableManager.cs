using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TableManager : MonoBehaviour
{
#if UNITY_EDITOR
    readonly public string actualPath = Path.Combine(Application.streamingAssetsPath, "CSV");
#elif UNITY_ANDROID
    readonly public string actualPath = Path.Combine ("jar:file://" + Application.dataPath + "!/assets/", "CSV");
#elif UNITY_IOS
    readonly public string actualPath = Path.Combine (Application.streamingAssetsPath + "Raw", "CSV");
#endif

    readonly public string oreFileName = "Energy_EachActivityMETS.csv";
    readonly public string workmanFileName = "Energy_EachActivityMETS.csv";

    private TableTemplate<Database.ProductOriginData> originOreTable;
    private TableTemplate<Database.ProductOriginData> originWorkmanTable;



    private void Awake()
    {
        if (Inst != null)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadAllTable()
    {
        LoadTable(oreFileName, actualPath);
        LoadTable(workmanFileName, actualPath);
    }

    public void LoadTable(string fileName, string targetPath)
    {
        string fullPath = System.IO.Path.Combine(targetPath, fileName);

        originOreTable = new TableTemplate<Database.ProductOriginData>(fullPath);
        originOreTable.Load();

        originWorkmanTable = new TableTemplate<Database.ProductOriginData>(fullPath);
        originWorkmanTable.Load();
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


    #region Instance
    private static TableManager instance;
    public static TableManager Inst { get { return instance; } set { instance = value; } }

    #endregion

}
