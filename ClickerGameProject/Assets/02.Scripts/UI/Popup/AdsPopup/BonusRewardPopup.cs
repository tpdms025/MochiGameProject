using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusRewardPopup : PopupWithAds
{
    //�⺻ ����
    private double baseRewardAmount;
    //�߰� ����
    private double addRewardAmount;

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
        MoneyManager.Inst.SumJewel(baseRewardAmount);
    }

    /// <summary>
    /// �߰� ������ �����Ѵ�.
    /// </summary>
    private void ApplyAdditionalReward()
    {
        MoneyManager.Inst.SumJewel(addRewardAmount);
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
    private double GetBonusReward(float multiply)
    {
        double bonusData = (multiply) * MoneyManager.Inst.JewelPerSec.Value;
        return bonusData;
    }

    /// <summary>
    /// ���ʽ� ������ N�踦 �����´�.
    /// </summary>
    private double GetAdditionalBonusReward(float multiply)
    {
        double bonusData = baseRewardAmount * (multiply);
        return bonusData;
    }

    /// <summary>
    /// UI�� �����Ѵ�.
    /// </summary>
    private void UpdateUI()
    {
        baseRewardText.text = string.Format("+{0} ȹ��", CurrencyParser.ToCurrencyString(baseRewardAmount));
        addRewardText.text = string.Format("+{0} ȹ��", CurrencyParser.ToCurrencyString(addRewardAmount));
    }
}
