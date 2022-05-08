//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.18444
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System;
using System.Reflection;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TableTemplate<T> : CSVParser
{
    string _baseFileName;
    Dictionary<int, T> m_Map = new Dictionary<int, T>();

    public TableTemplate(string fileName)
    {
        _baseFileName = fileName;
    }
    public override void Load(ref string str)
    {
        try  // exception by jade
        {
            LoadFile(_baseFileName, ref str);
        }
        catch (Exception e)
        {
           Debug.LogError("Table Load Error : "+e.ToString());
        }
    }
    public override bool VarifyKey(string keyValue)
    {
        return true;
    }
    public override void Parse(string[] inputData)
    {
        Type type = typeof(T);
        T temp = (T)Activator.CreateInstance(type, new object[] { inputData });
        int id = (int)type.InvokeMember("ID", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField, null, temp, null);
        m_Map.Add(id, temp);
    }

    public bool GetData(int nID, out T Data)
    {
        if (m_Map.TryGetValue(nID, out Data) == false)
        {
#if !SUPPORT_SERVICE
            Debug.LogWarning("Card Table 정보 없음" + nID.ToString());
#endif
            return false;
        }
        return true;
    }

    public T GetData(int nID)
    {
        if (m_Map.ContainsKey(nID) == false)
        {
#if !SUPPORT_SERVICES
            if (nID > 0)
                Debug.LogError("not table index" + nID.ToString());
#endif
            return default(T);
        }
        return m_Map[nID];
    }

    public Dictionary<int, T>.Enumerator GetData()
    {
        return m_Map.GetEnumerator();
    }

    public List<T> GetDataList()
    {
        return new List<T>(m_Map.Values); 
    }

    public int GetDataCount()
    {
        return m_Map.Count;
    }
    public bool CheckStrValue(T value)
    {
        if (m_Map.ContainsValue(value))
        {
            return true;
        }
        else
            return false;
    }

}