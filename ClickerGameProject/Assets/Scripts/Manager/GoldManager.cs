using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    #region Fields

    private static GoldManager instance = null;
    public static GoldManager Instance
    {
        get { return instance; }
        set { instance = value;  }
    }

    [SerializeField] private ShowGoodsUI goodsUI;
    #endregion

    #region Unity methods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            InitGold();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        //GlodController.Gold +=BigInteger.Pow(10, 55);
    }

    #endregion


    #region Methods
    public void TestAddGold()
    {
        AddGold((GoldController.Gold+1)*(GoldController.Gold+1));
    }

    public void AddGold(BigInteger _gold)
    {
        GoldController.Gold += _gold;
        string str = GoldController.ToCurrencyString(GoldController.Gold);
        goodsUI.OnJewelText(str);
    }

    public void RemoveGold(BigInteger _gold)
    {
        GoldController.Gold -= _gold;
        string str = GoldController.ToCurrencyString(GoldController.Gold);
        goodsUI.OnJewelText(str);
    }

    public void InitGold()
    {
        GoldController.Gold = 0;
        string str = GoldController.ToCurrencyString(GoldController.Gold);
        goodsUI.OnJewelText(str);
    }

    public BigInteger GetGold(BigInteger _add)
    {
        return GoldController.Gold;
    }

    #endregion

    #region Private Methods
    #endregion
}
