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
        //=================광석 정보=================
        //마지막으로 소유한 광석 인덱스 번호
        public int oreIdx_lastOwned;

        //마지막으로 소유한 광석 레벨
        public int oreLevel_lastOwned;

        //=================일꾼 정보=================
        //소유한 일꾼의 레벨들
        public int[] workmanLevels_owned;

        ////소유한 일꾼의 레벨들 (key:index, value:level)
        //public Dictionary<int, int> workmanLevels_owned;

        //소유한 일꾼 갯수
        public int workmanCount;

        //=================강화 정보=================
        //소유한 강화 레벨들
        public int[] enhanceLevels_owned;

        //소유한 강화 비율들
        public float[] enhanceAmounts_owned;


        //=================버프 정보=================
        //누적 터치 횟수
        public int touchCount;

        //자동채굴 여부
        public bool isAutoMining;

        //남은 버프시간 리스트
        public int[] remainingbuffTimes;

        public int[] remaining_skillTimes;
        public int[] remaining_skillCooldowns;
        //public Skill_Info[] skillInfo;
        //===========================================

        /// <summary>
        /// 분리 기호 기준으로 잘라 배열로 변환한다.
        /// </summary>
        public int[] Convert_IntToArray(string str)
        {
            return Array.ConvertAll(str.Split(','), int.Parse);
        }

        /// <summary>
        /// 분리 기호 기준으로 잘라 배열로 변환한다.
        /// </summary>
        public float[] Convert_FloatToArray(string str)
        {
            return Array.ConvertAll(str.Split(','), float.Parse);
        }

        /// <summary>
        /// 분리기호와 결합하여 형성된 문자열로 변환한다.
        /// </summary>
        public string Convert_ToString(int[] array)
        {
            return String.Join(",", array);
        }

        /// <summary>
        /// 분리기호와 결합하여 형성된 문자열로 변환한다.
        /// </summary>
        public string Convert_ToString(float[] array)
        {
            return String.Join(",", array);
        }
    }

    [Serializable]
    public class PlayData
    {
        //마지막에 종료한 날짜
        public DateTime lastDateTime;

        //=================환생 정보=================
        //환생 횟수
        public int prestigeCount;
        //누적 환생 보너스 퍼센트
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
