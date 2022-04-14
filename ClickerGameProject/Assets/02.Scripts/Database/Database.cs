using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;

[Serializable]
public class Database
{

    public class ValueOfLevel
    {
        //public int level;
        public BigInteger amountPerTouch;
        public BigInteger cost;
    }

    public class ProductOriginData
    {
        public string id;
        public string type;
        public string name;
        public string spriteName;
        public List<ValueOfLevel> levelTable;

        public ProductOriginData(){ }
        public ProductOriginData(string _name, string _spriteName, List<ValueOfLevel> _levelTable)
        {
            this.name = _name;
            this.spriteName = _spriteName;
            this.levelTable = _levelTable;
        }
    }

    [Serializable]
    public class Inventory
    {
    }

    [Serializable]
    public class PlayerData
    {
        //광석 정보
        //마지막으로 소유한 Cell의 인덱스 번호
        public int idx_lastOwned;

        //마지막으로 소유한 Cell의 레벨
        public int level_lastOwned;

        //일꾼 정보
        //소유한 셀 레벨에 대한 배열
        public int[] workmanLevelList;


        //누적 터치 횟수
        public int touchCount;

        //자동채굴 여부
        public bool isAutoMining;

        public PlayerData(int idx_lastUnlocked, int level_lastUnlocked, string workmanLevelList)
        {
            this.idx_lastOwned = idx_lastUnlocked;
            this.level_lastOwned = level_lastUnlocked;
            this.workmanLevelList = Array.ConvertAll(workmanLevelList.Split(','), int.Parse);
        }
    }


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

}
