using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class TestSecUpgrade : MonoBehaviour
{
    //레벨
    private int level;

    //*********************현재는 임시. 나중에 테이블 데이터 받아올 것.*****************
    //처음으로 업그레이드 할때 증가하는 goldPerClick 값
    private BigInteger startGoldByUpgrade = 1;

    //한번 업그레이드 할때마다 goldPerSec 변수가 얼만큼 증가할지에 대한 변수
    private BigInteger goldByUpgrade;

    //이전 업그레이드 매초증가량 값
    private BigInteger prevGoldByUpgrade = 0;

    //처음 업그레이드의 비용
    private BigInteger startCost = 1;

    //현재 업그레이드의 비용
    private BigInteger currentCost;

    //goldPerClick 증가에 필요한 상수값
    public float upgradePow = 7.0f;

    //비용 증가에 필요한 상수값
    public float costPow = 16.0f;



    [SerializeField] private TextMeshProUGUI titleText;



    private void Start()
    {
        level = 0;
        currentCost = startCost;
        goldByUpgrade = startGoldByUpgrade;

        UpgradeItem();
        prevGoldByUpgrade = 0;
        MoneyManager.Instance.AddJewelPerSec(goldByUpgrade - prevGoldByUpgrade);
        UpdateUI();
    }

    public void PurchaseUpgrade()
    {
        //구매 가능하면 
        if (MoneyManager.Instance.Jewel >= currentCost)
        {
            MoneyManager.Instance.SubJewel(currentCost);
            level++;

            UpgradeItem();
            MoneyManager.Instance.AddJewelPerSec(goldByUpgrade - prevGoldByUpgrade);
            UpdateUI();
        }
    }


    /// <summary>
    /// 비용과 한번 업그레이드 되는 골드의 양을 증가한다.
    /// </summary>
    private void UpgradeItem()
    {
        prevGoldByUpgrade = goldByUpgrade;
        //임시로 공식둠.
        goldByUpgrade = startGoldByUpgrade * (BigInteger)Mathf.Pow(upgradePow, level);
        currentCost = startCost * (BigInteger)Mathf.Pow(costPow, level);
    }

    /// <summary>
    /// UI들을 새로 갱신한다.
    /// </summary>
    private void UpdateUI()
    {
        string strGoldPer = CurrencyParser.ToCurrencyString(goldByUpgrade);
        titleText.text = string.Format("{0}/s", strGoldPer);
    }
}
