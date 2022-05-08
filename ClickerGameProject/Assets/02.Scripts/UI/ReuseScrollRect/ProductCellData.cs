using System;
using System.Numerics;
using UnityEngine;

public enum CellState { Lock, Unlock, MaxCompletion }

public class ProductCellData : IReuseCellData
{
    public int index { get; set; }
    public int id { get; set; }
    public string name { get; set; }
    public string imageName { get; set; }
    public string description { get; set; }


    //Ãß°¡
    public int level { get; set; }
    public int nextLevel { get; set; }


    public double currentAmount { get; set; }
    public double nextAmount { get; set; }

    public double cost { get; set; }
    
    public CellState cellState { get; set; }
}
