using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Numerics;



[Serializable] 
public class Database
{
    [Serializable]
    public class ValueOfLevel
    {
        //public int level;
        public double amount;
        public double cost;
    }

    [Serializable] 
    public class _ProductOriginData
    {
        public int ID;
        public string type;
        public string name;
        public string spriteName;
        public string description;
        public List<ValueOfLevel> levelTable;

        public _ProductOriginData(){ }
        public _ProductOriginData(string _name, string _spriteName, List<ValueOfLevel> _levelTable)
        {
            this.name = _name;
            this.spriteName = _spriteName;
            this.levelTable = _levelTable;
        }
    }

    [Serializable]
    public class ProductOriginData
    {
        public int ID;
        public int type;
        public string name;
        public string spriteName;
        public string description;
        public int level;
        public double cost;
        public double amount;

        public ProductOriginData() { }

        public ProductOriginData(string[] inputData)
        {
            int count = 0;
            this.ID = Convert.ToInt32(inputData[count++]);
            this.type = Convert.ToInt32(inputData[count++]);
            this.name = inputData[count++];
            this.spriteName = inputData[count++];
            //this.level = Convert.ToInt32(inputData[count++]);
            //this.cost = Convert.ToDouble(inputData[count++]);
            //this.amount = Convert.ToDouble(inputData[count++]);
            Int32.TryParse(inputData[count++], out this.level);
            Double.TryParse(inputData[count++], out this.cost);
            Double.TryParse(inputData[count++], out this.amount);
        }
    }

    [Serializable]
    public class ProductOriginData_
    {
        public int ID;
        public int type;
        public string name;
        public string spriteName;
        public string description;
        public int level;
        public double cost;
        public double amount;

        public ProductOriginData_() { }

        public ProductOriginData_(string[] inputData)
        {
            int count = 0;
            this.ID = Convert.ToInt32(inputData[count++]);
            this.type = Convert.ToInt32(inputData[count++]);
            this.name = inputData[count++];
            this.spriteName = inputData[count++];
            //this.level = Convert.ToInt32(inputData[count++]);
            //this.cost = Convert.ToDouble(inputData[count++]);
            //this.amount = Convert.ToDouble(inputData[count++]);
            Int32.TryParse(inputData[count++], out this.level);
            Double.TryParse(inputData[count++], out this.cost);
            Double.TryParse(inputData[count++], out this.amount);
            this.description = inputData[count++];
        }
    }

    [Serializable]
    public class Inventory
    {
        //=================���� ����=================
        //���������� ������ ���� �ε��� ��ȣ
        public int oreIdx_lastOwned;

        //���������� ������ ���� ����
        public int oreLevel_lastOwned;

        //=================�ϲ� ����=================
        //������ �ϲ��� ������
        public int[] workmanLevels_owned;

        ////������ �ϲ��� ������ (key:index, value:level)
        //public Dictionary<int, int> workmanLevels_owned;

        //������ �ϲ� ����
        public int workmanCount;

        //=================��ȭ ����=================
        //������ ��ȭ ������
        public int[] enhanceLevels_owned;

        //������ ��ȭ ������
        public float[] enhanceAmounts_owned;


        //=================���� ����=================
        //���� ��ġ Ƚ��
        public int touchCount;

        //�ڵ�ä�� ����
        public bool isAutoMining;

        //���� �����ð� ����Ʈ
        public int[] remainingbuffTimes;

        public int[] remaining_skillTimes;
        public int[] remaining_skillCooldowns;
        //public Skill_Info[] skillInfo;
        //===========================================

        /// <summary>
        /// �и� ��ȣ �������� �߶� �迭�� ��ȯ�Ѵ�.
        /// </summary>
        public int[] Convert_IntToArray(string str)
        {
            return Array.ConvertAll(str.Split(','), int.Parse);
        }

        /// <summary>
        /// �и� ��ȣ �������� �߶� �迭�� ��ȯ�Ѵ�.
        /// </summary>
        public float[] Convert_FloatToArray(string str)
        {
            return Array.ConvertAll(str.Split(','), float.Parse);
        }

        /// <summary>
        /// �и���ȣ�� �����Ͽ� ������ ���ڿ��� ��ȯ�Ѵ�.
        /// </summary>
        public string Convert_ToString(int[] array)
        {
            return String.Join(",", array);
        }

        /// <summary>
        /// �и���ȣ�� �����Ͽ� ������ ���ڿ��� ��ȯ�Ѵ�.
        /// </summary>
        public string Convert_ToString(float[] array)
        {
            return String.Join(",", array);
        }
    }

    [Serializable]
    public class PlayData
    {
        //�������� ������ ��¥
        public DateTime lastDateTime;

        //=================ȯ�� ����=================
        //ȯ�� Ƚ��
        public int prestigeCount;
        //���� ȯ�� ���ʽ� �ۼ�Ʈ
        public float cumPrestigeRate;


        public void AddPrestige(float Addrate)
        {
            prestigeCount++;
            cumPrestigeRate += Addrate;
        }
    }

    [Serializable]
    public class MoneyData
    {
        public string baseJewel;
        public string baseCoin;
        public string baseJewelPerTouch;
        public string baseJewelPerSec;
    }



    [Serializable]
    public class BuffTable
    {
        public int id;
        public string name;
        public int time;
        public int remainingTime;
        public BuffTable(int id, string name, int time, int remainingTime)
        {
            this.id = id;
            this.name = name;
            this.time = time;
            this.remainingTime = remainingTime;
        }
    }

    [Serializable]
    public class SkillTable
    {
        public int id;
        public string name;
        public int buffTime;
        public int buffRemainingTime;
        public int maxCooldown;
        public int cooldownRemaining;
        public SkillTable(int id, string name, int buffTime, int buffRemainingTime, int maxCooldown, int cooldownRemaining)
        {
            this.id = id;
            this.name = name;
            this.buffTime = buffTime;
            this.buffRemainingTime = buffRemainingTime;
            this.maxCooldown = maxCooldown;
            this.cooldownRemaining = cooldownRemaining;
        }
    }

    [Serializable]
    public class Skill_Info
    {
        public int buffRemainingTime;
        public int cooldownRemaining;
        public Skill_Info(int buffRemainingTime, int cooldownRemaining)
        {
            this.buffRemainingTime = buffRemainingTime;
            this.cooldownRemaining = cooldownRemaining;
        }
    }

}
