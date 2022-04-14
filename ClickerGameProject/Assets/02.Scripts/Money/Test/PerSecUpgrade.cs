//1초당 자동으로 골드가 증가하는 아이템의 업그레이드 버튼

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerSecUpgrade : MonoBehaviour
{
    #region Data

    [Header("Attribute")]
    //아이디
    public string _id;

    //레벨
    private int level;

    //한번 업그레이드 할때마다 초당 증가하는 보석량
    [SerializeField] private BigInteger goldPerSec;

    //처음으로 업그레이드 할때의 초당 보석량
    private BigInteger startGoldPerSec = 1;

    //현재 업그레이드의 비용
    private BigInteger currentCost;

    //처음 업그레이드의 비용
    private BigInteger startCost = 1;

    //구입할 수 있는 갯수
    private int countPurchase = 1;

    //구매했는지에 대한 bool 값
    [HideInInspector] public bool isPurchased;



    //goldPerClick 증가에 필요한 상수값
    private float upgradePow = 5f;

    //비용 증가에 필요한 상수값
    private float costPow = 50.0f;


    #endregion

    #region Fields

    [Header("Fields")]
    [SerializeField] private Image preview;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI goldPerSecText;
    [SerializeField] private TextMeshProUGUI buyInfoText;
    [SerializeField] private Button upgradeBtn;

    [Space]
    [Header("LockPanel Fields")]
    [SerializeField] private Button firstPurchaseBtn;
    [SerializeField] private GameObject lockPreview;
    [SerializeField] private GameObject lockPanel;
    [SerializeField] private TextMeshProUGUI firstPurchaseText;

    #endregion

    #region Unity methods

    private void Awake()
    {
        firstPurchaseBtn.onClick.AddListener(FirstPurchase);
        upgradeBtn.onClick.AddListener(PurchaseUpgrade);
    }


    private void Start()
    {
        LoadDB();

        firstPurchaseText.text = string.Format("Buy Item<br>{0}", currentCost);
    }

    #endregion

    #region Methods

    /// <summary>
    /// 최초로 아이템을 구입한다.
    /// </summary>
    public void FirstPurchase()
    {
        if (!isPurchased && MoneyManager.Instance.Jewel >= currentCost)
        {
            lockPanel.SetActive(false);
            lockPreview.SetActive(false);
            isPurchased = true;
            //DB저장 isPurchased
            //

            PurchaseUpgrade();
        }
    }


    /// <summary>
    /// 아이템 구입하여 업그레이드를 한다.
    /// </summary>
    public void PurchaseUpgrade()
    {
        //구매 가능하면 
        if (MoneyManager.Instance.Jewel >= currentCost)
        {
            MoneyManager.Instance.SubJewel(currentCost);

            UpgradeItem();
            //MoneyManager.Instance.AddJewelPerSec(goldPerSec);
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
        level++;
        //임시로 공식둠. (PerSec는 초기 비용이 있기때문에 level-1)
        currentCost = startCost * (BigInteger)Mathf.Pow(costPow, level - 1);
        goldPerSec = startGoldPerSec * (BigInteger)Mathf.Pow(upgradePow, level - 1);
    }

    /// <summary>
    /// UI들을 새로 갱신한다.
    /// </summary>
    private void UpdateUI()
    {
        levelText.text = string.Format("Lv.{0}", level);

        string strGoldPer = CurrencyParser.ToCurrencyString(goldPerSec);
        goldPerSecText.text = string.Format("{0}/s", strGoldPer);

        string strCurCost = CurrencyParser.ToCurrencyString(currentCost);
        buyInfoText.text = string.Format("buy x {0} <br>{1}", countPurchase, strCurCost);

    }

    /// <summary>
    /// 데이터들을 초기화한다.
    /// </summary>
    private void LoadDB()
    {

        level = 0;
        currentCost = startCost;
        goldPerSec = startGoldPerSec;

        isPurchased = false;
    }
    #endregion
}
