//1�ʴ� �ڵ����� ��尡 �����ϴ� �������� ���׷��̵� ��ư

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    #region Data

    //��ư �̸�
    public string itemName;

    //����
    private int level = 1;

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
    [HideInInspector] public bool isPurchased = false;



    //goldPerClick ������ �ʿ��� �����
    private float upgradePow = 5f;

    //��� ������ �ʿ��� �����
    private float costPow = 50.0f;


    #endregion

    #region Fields

    [SerializeField] private Image preview;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI goldPerSecText;
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
    /// ������ �����ϱ�
    /// </summary>
    public void PurchaseUpgrade()
    {
        //���� �����ϸ� 
        if (GoldManager.Instance.Gold >= currentCost)
        {
            isPurchased = true;
            GoldManager.Instance.SubGold(currentCost);
            level++;

            UpdateItem();
            UpdateUI();

            GoldManager.Instance.AddGoldPerSec(goldPerSec);
        }
    }


    #endregion

    #region Private Methods

    /// <summary>
    /// ���� �ѹ� ���׷��̵� �Ǵ� ����� ���� �����Ѵ�.
    /// </summary>
    public void UpdateItem()
    {
        //�ӽ÷� ���ĵ�.
        goldPerSec = startGoldPerSec * (BigInteger)Mathf.Pow(upgradePow, level);
        currentCost = startCost * (BigInteger)Mathf.Pow(costPow, level);
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
    private void Init()
    {
        level = 1;
        currentCost = startCost;
        goldPerSec = startGoldPerSec;
    }
    #endregion
}
