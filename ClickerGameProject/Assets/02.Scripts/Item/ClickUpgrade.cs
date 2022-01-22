//Ŭ���� ��尡 �����ϴ� �������� ���׷��̵� ��ư

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickUpgrade : MonoBehaviour
{
    #region Data

    //��ư �̸�
    public string upgradeName;

    //����
    private int level;

    //�ѹ� ���׷��̵� �Ҷ����� goldPerClick ������ ��ŭ ���������� ���� ����
    [SerializeField] private BigInteger goldByUpgrade;

    //ó������ ���׷��̵� �Ҷ� �����ϴ� goldPerClick ��
    private BigInteger startGoldByUpgrade = 1;

    //���� ���׷��̵��� ���
    private BigInteger currentCost;

    //ó�� ���׷��̵��� ���
    private BigInteger startCost = 1;

    //������ �� �ִ� ����
    private int countPurchase = 1;



    //goldPerClick ������ �ʿ��� �����
    public float upgradePow = 25.0f;

    //��� ������ �ʿ��� �����
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

    private void Start()
    {
        LoadDB();
        UpdateUI();
    }

    #endregion

    #region Methods

    /// <summary>
    /// �����ϱ�
    /// </summary>
    public void PurchaseUpgrade()
    {
        //���� �����ϸ� 
        if(GoldManager.Instance.Gold >= currentCost)
        {
            GoldManager.Instance.SubGold(currentCost);
            level++;
            GoldManager.Instance.AddGoldPerClick(goldByUpgrade);

            UpgradeItem();
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
        //�ӽ÷� ���ĵ�.
        goldByUpgrade = startGoldByUpgrade * (BigInteger) Mathf.Pow(upgradePow, level);
        currentCost = startCost * (BigInteger)Mathf.Pow(costPow, level);
    }

    /// <summary>
    /// UI���� ���� �����Ѵ�.
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
    /// DB�� �ҷ��´�.
    /// </summary>
    private void LoadDB()
    {
        level = 1;
        currentCost = startCost;
        goldByUpgrade = startGoldByUpgrade;

    }
    #endregion
}
