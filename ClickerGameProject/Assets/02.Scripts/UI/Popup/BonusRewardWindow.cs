using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class BonusRewardWindow : WindowWithAds
{
    [SerializeField] private Button baseButton;
    private float valueMultiply = 10.0f;

    protected override void SubscribeToButtonEvents()
    {
        base.SubscribeToButtonEvents();

        if (baseButton != null)
        {
            baseButton.onClick.AddListener(delegate { GetBonusReward(); });
            baseButton.onClick.AddListener(delegate { ToggleOpenOrClose(); });
        }
        onAdsFinished += Get3xBonusReward;
        BonusBat.onObjectTouched += ToggleOpenOrClose;
    }
    protected override void UnsubscribeFromButtonEvents()
    {
        base.UnsubscribeFromButtonEvents();
    }


    /// <summary>
    /// ���ʽ� ������ �����´�.
    /// </summary>
    /// <returns></returns>
    private BigInteger GetBonusReward()
    {
        BigInteger bonusData = new BigInteger(valueMultiply) * MoneyManager.Instance.JewelPerClick;
        MoneyManager.Instance.AddJewel(bonusData);
        return bonusData;
    }

    /// <summary>
    /// ���ʽ� ������ 3��� ��´�.
    /// </summary>
    private void Get3xBonusReward()
    {
        BigInteger bonusData = GetBonusReward() * new BigInteger(3);
        MoneyManager.Instance.AddJewel(bonusData);
    }
}
