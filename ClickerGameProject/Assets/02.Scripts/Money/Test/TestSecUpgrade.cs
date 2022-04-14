using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class TestSecUpgrade : MonoBehaviour
{
    //����
    private int level;

    //*********************����� �ӽ�. ���߿� ���̺� ������ �޾ƿ� ��.*****************
    //ó������ ���׷��̵� �Ҷ� �����ϴ� goldPerClick ��
    private BigInteger startGoldByUpgrade = 1;

    //�ѹ� ���׷��̵� �Ҷ����� goldPerSec ������ ��ŭ ���������� ���� ����
    private BigInteger goldByUpgrade;

    //���� ���׷��̵� ���������� ��
    private BigInteger prevGoldByUpgrade = 0;

    //ó�� ���׷��̵��� ���
    private BigInteger startCost = 1;

    //���� ���׷��̵��� ���
    private BigInteger currentCost;

    //goldPerClick ������ �ʿ��� �����
    public float upgradePow = 7.0f;

    //��� ������ �ʿ��� �����
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
        //���� �����ϸ� 
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
    /// ���� �ѹ� ���׷��̵� �Ǵ� ����� ���� �����Ѵ�.
    /// </summary>
    private void UpgradeItem()
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
        titleText.text = string.Format("{0}/s", strGoldPer);
    }
}
