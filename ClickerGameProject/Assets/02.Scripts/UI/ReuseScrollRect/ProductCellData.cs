using System;
using System.Numerics;
using UnityEngine;

public class ProductCellData : IReuseCellData
{
    private int m_index;
    public int index { get { return m_index; } set { m_index = value; } }

    private int m_id;
    public int id { get { return m_id; } set { m_id = value; } }

    [SerializeField]
    private string m_name;
    public string name { get { return m_name; } set { m_name = value; } }

    private string m_imageName;
    public string imageName { get { return m_imageName; } set { m_imageName = value; } }

    //Ãß°¡
    //private BigInteger m_jewelPerClick;
    //public BigInteger jewelPerClick { get { return m_jewelPerClick; } set { m_jewelPerClick = value; } }

    //private BigInteger m_cost;
    //public BigInteger cost { get { return m_cost; } set { m_cost = value; } }

    //private bool m_isPurchasedh;
    //public bool isPurchased { get { return m_isPurchasedh; } set { m_isPurchasedh = value; } }

    public BigInteger jewelPerClick { get; set; }
    public BigInteger cost { get; set; }
    public enum PurchaseState { Lock, Unlock, Select, Have }
    public PurchaseState purchaseState { get; set; }
}
