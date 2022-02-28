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


    //�߰�
    public int level { get; set; }
    public int nextLevel { get; set; }


    public BigInteger currentAmount { get; set; }
    public BigInteger nextAmount { get; set; }

    public BigInteger cost { get; set; }
    
    public CellState cellState { get; set; }
}
