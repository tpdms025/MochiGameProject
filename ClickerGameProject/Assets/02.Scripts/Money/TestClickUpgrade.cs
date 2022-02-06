//클릭시 골드가 증가하는 아이템의 업그레이드 버튼

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestClickUpgrade : MonoBehaviour
{
    #region Data

    //버튼 이름
    public string upgradeName;

    //레벨
    private int level;

    //한번 업그레이드 할때마다 goldPerClick 변수가 얼만큼 증가할지에 대한 변수
    [SerializeField] private BigInteger goldByUpgrade;

    //처음으로 업그레이드 할때 증가하는 goldPerClick 값
    private BigInteger startGoldByUpgrade = 1;

    //현재 업그레이드의 비용
    private BigInteger currentCost;

    //처음 업그레이드의 비용
    private BigInteger startCost = 1;

    //구입할 수 있는 갯수
    private int countPurchase = 1;



    //goldPerClick 증가에 필요한 상수값
    public float upgradePow = 7.0f;

    //비용 증가에 필요한 상수값
    public float costPow = 16.0f;


    #endregion

    #region Fields

    [SerializeField] private TextMeshProUGUI titleText;


    #endregion

    #region Unity methods

    private void Start()
    {
        LoadDB();

        UpdateUI();
    }

    #endregion

    #region Methods

    /// <summary>
    /// 구입하기
    /// </summary>
    public void PurchaseUpgrade()
    {
        //구매 가능하면 
        if (MoneyManager.Instance.Jewel >= currentCost)
        {
            MoneyManager.Instance.SubJewel(currentCost);
            level++;

            UpgradeItem();
            MoneyManager.Instance.AddJewelPerClick(goldByUpgrade);
            UpdateUI();
        }
    }


    #endregion

    #region Private Methods

    /// <summary>
    /// 비용과 한번 업그레이드 되는 골드의 양을 증가한다.
    /// </summary>
    public void UpgradeItem()
    {
        //임시로 공식둠.
        goldByUpgrade = startGoldByUpgrade * (BigInteger)Mathf.Pow(upgradePow, level);
        currentCost = startCost * (BigInteger)Mathf.Pow(costPow, level);
    }

    /// <summary>
    /// UI들을 새로 갱신한다.
    /// </summary>
    private void UpdateUI()
    {
        Debug.Log("goldByUpgrade?" + goldByUpgrade);
        string strGoldPer = CurrencyParser.ToCurrencyString(goldByUpgrade);
        titleText.text = string.Format("{0}", strGoldPer);
    }

    /// <summary>
    /// DB를 불러온다.
    /// </summary>
    private void LoadDB()
    {
        level = 1;
        currentCost = startCost;
        goldByUpgrade = startGoldByUpgrade;

    }
    #endregion
}
