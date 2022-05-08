using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusRewardPopup : PopupWithAds
{
    //기본 보상량
    private double baseRewardAmount;
    //추가 보상량
    private double addRewardAmount;

    //기본 보상량에 곱할 배수
    private float baseMultiply;
    //추가 보상량에 곱할 배수
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
    /// 기본 보상을 적용한다.
    /// </summary>
    private void ApplyBasicReward()
    {
        MoneyManager.Inst.SumJewel(baseRewardAmount);
    }

    /// <summary>
    /// 추가 보상을 적용한다.
    /// </summary>
    private void ApplyAdditionalReward()
    {
        MoneyManager.Inst.SumJewel(addRewardAmount);
    }




    /// <summary>
    /// 데이터를 초기화한다.
    /// </summary>
    private void Initialize()
    {
        baseMultiply = 20.0f;
        addMultiply = 2; //******이부분은 강화시스템과 연관지어 데이터 로드할 것.
        baseRewardAmount = GetBonusReward(baseMultiply);
        addRewardAmount = GetAdditionalBonusReward(addMultiply);

        UpdateUI();
    }

    /// <summary>
    /// 보너스 보상을 가져온다.
    /// </summary>
    /// <returns></returns>
    private double GetBonusReward(float multiply)
    {
        double bonusData = (multiply) * MoneyManager.Inst.JewelPerSec.Value;
        return bonusData;
    }

    /// <summary>
    /// 보너스 보상의 N배를 가져온다.
    /// </summary>
    private double GetAdditionalBonusReward(float multiply)
    {
        double bonusData = baseRewardAmount * (multiply);
        return bonusData;
    }

    /// <summary>
    /// UI를 갱신한다.
    /// </summary>
    private void UpdateUI()
    {
        baseRewardText.text = string.Format("+{0} 획득", CurrencyParser.ToCurrencyString(baseRewardAmount));
        addRewardText.text = string.Format("+{0} 획득", CurrencyParser.ToCurrencyString(addRewardAmount));
    }
}
