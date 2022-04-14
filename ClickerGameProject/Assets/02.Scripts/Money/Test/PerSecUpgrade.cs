//1�ʴ� �ڵ����� ��尡 �����ϴ� �������� ���׷��̵� ��ư

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
    //���̵�
    public string _id;

    //����
    private int level;

    //�ѹ� ���׷��̵� �Ҷ����� �ʴ� �����ϴ� ������
    [SerializeField] private BigInteger goldPerSec;

    //ó������ ���׷��̵� �Ҷ��� �ʴ� ������
    private BigInteger startGoldPerSec = 1;

    //���� ���׷��̵��� ���
    private BigInteger currentCost;

    //ó�� ���׷��̵��� ���
    private BigInteger startCost = 1;

    //������ �� �ִ� ����
    private int countPurchase = 1;

    //�����ߴ����� ���� bool ��
    [HideInInspector] public bool isPurchased;



    //goldPerClick ������ �ʿ��� �����
    private float upgradePow = 5f;

    //��� ������ �ʿ��� �����
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
    /// ���ʷ� �������� �����Ѵ�.
    /// </summary>
    public void FirstPurchase()
    {
        if (!isPurchased && MoneyManager.Instance.Jewel >= currentCost)
        {
            lockPanel.SetActive(false);
            lockPreview.SetActive(false);
            isPurchased = true;
            //DB���� isPurchased
            //

            PurchaseUpgrade();
        }
    }


    /// <summary>
    /// ������ �����Ͽ� ���׷��̵带 �Ѵ�.
    /// </summary>
    public void PurchaseUpgrade()
    {
        //���� �����ϸ� 
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
    /// ���� �ѹ� ���׷��̵� �Ǵ� ����� ���� �����Ѵ�.
    /// </summary>
    public void UpgradeItem()
    {
        level++;
        //�ӽ÷� ���ĵ�. (PerSec�� �ʱ� ����� �ֱ⶧���� level-1)
        currentCost = startCost * (BigInteger)Mathf.Pow(costPow, level - 1);
        goldPerSec = startGoldPerSec * (BigInteger)Mathf.Pow(upgradePow, level - 1);
    }

    /// <summary>
    /// UI���� ���� �����Ѵ�.
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
    /// �����͵��� �ʱ�ȭ�Ѵ�.
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
