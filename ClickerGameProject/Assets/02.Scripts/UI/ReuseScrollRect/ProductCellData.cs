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


    //Ãß°¡
    public int level { get; set; }
    public int nextLevel { get; set; }


    public BigInteger jewelPerClick { get; set; }
    public BigInteger nextJewelPerClick { get; set; }

    public BigInteger cost { get; set; }
    
    public CellState cellState { get; set; }
}
