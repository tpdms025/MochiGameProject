//Ŭ���� ��尡 �����ϴ� �������� ���׷��̵� ��ư

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestClickUpgrade : MonoBehaviour
{
    #region Data

    //��ư �̸�
    public string upgradeName;

    //����
    private int level;

    //�ѹ� ���׷��̵� �Ҷ����� goldPerClick ������ ��ŭ ���������� ���� ����
    [SerializeField] private BigInteger goldByUpgrade;

    //���� ���׷��̵� ���������� ��
    private BigInteger prevGoldByUpgrade= 0;

    //ó������ ���׷��̵� �Ҷ� �����ϴ� goldPerClick ��
    private BigInteger startGoldByUpgrade = 1;

    //���� ���׷��̵��� ���
    private BigInteger currentCost;

    //ó�� ���׷��̵��� ���
    private BigInteger startCost = 1;

    //������ �� �ִ� ����
    private int countPurchase = 1;



    //goldPerClick ������ �ʿ��� �����
    public float upgradePow = 7.0f;

    //��� ������ �ʿ��� �����
    public float costPow = 16.0f;


    #endregion

    #region Fields

    [SerializeField] private TextMeshProUGUI titleText;


    #endregion

    #region Unity methods

    private void Start()
    {
        LoadDB();

        UpgradeItem();
        prevGoldByUpgrade = 0;
        //MoneyManager.Instance.AddJewelPerClick(goldByUpgrade - prevGoldByUpgrade);
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
        if (MoneyManager.Instance.Jewel >= currentCost)
        {
            MoneyManager.Instance.SubJewel(currentCost);
            level++;

            UpgradeItem();
            MoneyManager.Instance.AddJewelPerClick(goldByUpgrade - prevGoldByUpgrade);
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
        prevGoldByUpgrade = goldByUpgrade;
        //�ӽ÷� ���ĵ�.
        goldByUpgrade = startGoldByUpgrade * (BigInteger)Mathf.Pow(upgradePow, level);
        currentCost = startCost * (BigInteger)Mathf.Pow(costPow, level);
    }

    /// <summary>
    /// UI���� ���� �����Ѵ�.
    /// </summary>
    private void UpdateUI()
    {
        string strGoldPer = CurrencyParser.ToCurrencyString(goldByUpgrade);
        titleText.text = string.Format("{0}/Touch", strGoldPer);
    }

    /// <summary>
    /// DB�� �ҷ��´�.
    /// </summary>
    private void LoadDB()
    {
        level = 0;
        currentCost = startCost;
        goldByUpgrade = startGoldByUpgrade;
    }
    #endregion
}