using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;

[Serializable]
public class Database 
{
   
    public class BuffTable
    {
        public int id;
        public string name;
        public int time;
        public int curTime;
        public BuffTable(int id, string name, int time, int curTime)
        {
            this.id = id;
            this.name = name;
            this.time = time;
            this.curTime = curTime;
        }
    }

}
