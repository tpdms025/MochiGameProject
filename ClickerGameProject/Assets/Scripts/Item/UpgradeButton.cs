//클릭시 골드가 증가하는 아이템의 업그레이드 버튼

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    #region Data

    //버튼 이름
    public string upgradeName;

    //레벨
    private int level = 1;

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

    //구매했는지에 대한 bool 값
    [HideInInspector] public bool isPurchased = false;



    //goldPerClick 증가에 필요한 상수값
    public float upgradePow = 25.0f;

    //비용 증가에 필요한 상수값
    public float costPow = 50.0f;


    #endregion

    #region Fields

    [SerializeField] private Image preview;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI goldPerText;
    [SerializeField] private TextMeshProUGUI buyInfoText;

    #endregion

    #region Unity methods

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
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
        if(GoldManager.Instance.Gold >= currentCost)
        {
            GoldManager.Instance.SubGold(currentCost);
            level++;
            GoldManager.Instance.AddGoldPerClick(goldByUpgrade);

            UpdateItem();
            UpdateUI();
        }
    }


    #endregion

    #region Private Methods

    /// <summary>
    /// 비용과 한번 업그레이드 되는 골드의 양을 증가한다.
    /// </summary>
    public void UpdateItem()
    {
        //임시로 공식둠.
        goldByUpgrade = startGoldByUpgrade * (BigInteger) Mathf.Pow(upgradePow, level);
        currentCost = startCost * (BigInteger)Mathf.Pow(costPow, level);
    }

    /// <summary>
    /// UI들을 새로 갱신한다.
    /// </summary>
    private void UpdateUI()
    {
        levelText.text = string.Format("Lv.{0}", level);
        string strGoldPer = CurrencyParser.ToCurrencyString(goldByUpgrade);
        goldPerText.text = string.Format("{0}/s", strGoldPer);

        string strCurCost = CurrencyParser.ToCurrencyString(currentCost);
        buyInfoText.text = string.Format("buy x {0} <br>{1}", countPurchase, strCurCost);

    }

    /// <summary>
    /// 데이터들을 초기화한다.
    /// </summary>
    private void Init()
    {
        level = 1;
        currentCost = startCost;
        goldByUpgrade = startGoldByUpgrade;
    }
    #endregion
}
