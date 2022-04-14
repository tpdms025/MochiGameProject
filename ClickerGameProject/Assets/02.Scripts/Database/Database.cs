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
        //���� ����
        //���������� ������ Cell�� �ε��� ��ȣ
        public int idx_lastOwned;

        //���������� ������ Cell�� ����
        public int level_lastOwned;

        //�ϲ� ����
        //������ �� ������ ���� �迭
        public int[] workmanLevelList;


        //���� ��ġ Ƚ��
        public int touchCount;

        //�ڵ�ä�� ����
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
