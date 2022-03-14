using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusRewardPopup : PopupWithAds
{
    private BigInteger baseRewardAmount;
    private BigInteger addRewardAmount;

    //�⺻ ���󷮿� ���� ���
    private float baseMultiply;
    //�߰� ���󷮿� ���� ���
    private float addMultiply;


    [SerializeField] private TextMeshProUGUI baseRewardText;
    [SerializeField] private TextMeshProUGUI addRewardText;

    protected override void SubscribeToButtonEvents()
    {
        base.SubscribeToButtonEvents();

        if (baseButton != null)
        {
            baseButton.onClick.AddListener(delegate { ApplyBasicReward(); });
        }
        onAdsFinished += ApplyAdditionalReward;
        BonusBat.onObjectTouched += ToggleOpenOrClose;
        BonusBat.onObjectTouched += Initialize;
    }
    protected override void UnsubscribeFromButtonEvents()
    {
        base.UnsubscribeFromButtonEvents();
    }

    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// �⺻ ������ �����Ѵ�.
    /// </summary>
    private void ApplyBasicReward()
    {
        MoneyManager.Instance.AddJewel(baseRewardAmount);
    }

    /// <summary>
    /// �߰� ������ �����Ѵ�.
    /// </summary>
    private void ApplyAdditionalReward()
    {
        MoneyManager.Instance.AddJewel(addRewardAmount);
    }




    /// <summary>
    /// �����͸� �ʱ�ȭ�Ѵ�.
    /// </summary>
    private void Initialize()
    {
        baseMultiply = 20.0f;
        addMultiply = 2; //******�̺κ��� ��ȭ�ý��۰� �������� ������ �ε��� ��.
        baseRewardAmount = GetBonusReward(baseMultiply);
        addRewardAmount = GetAdditionalBonusReward(addMultiply);

        UpdateUI();
    }

    /// <summary>
    /// ���ʽ� ������ �����´�.
    /// </summary>
    /// <returns></returns>
    private BigInteger GetBonusReward(float multiply)
    {
        BigInteger bonusData = new BigInteger(multiply) * MoneyManager.Instance.JewelPerSec;
        return bonusData;
    }

    /// <summary>
    /// ���ʽ� ������ N�踦 �����´�.
    /// </summary>
    private BigInteger GetAdditionalBonusReward(float multiply)
    {
        BigInteger bonusData = baseRewardAmount * new BigInteger(multiply);
        return bonusData;
    }

    /// <summary>
    /// UI�� �����Ѵ�.
    /// </summary>
    private void UpdateUI()
    {
        baseRewardText.text = string.Format("+{0} Gain", CurrencyParser.ToCurrencyString(baseRewardAmount));
        addRewardText.text = string.Format("+{0} Gain", CurrencyParser.ToCurrencyString(addRewardAmount));
    }
}
