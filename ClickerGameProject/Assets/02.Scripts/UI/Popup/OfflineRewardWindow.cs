using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OfflineRewardWindow : WindowWithAds
{
    private int maxTimeInMinute;
    private int currentTimeInMinute;
    private BigInteger rewardAmount;
    private BigInteger addRewardAmount;
    private const int additionalMultiply = 3;

    [SerializeField] private Button baseButton;
    [SerializeField] private Image fill;

    [SerializeField] private TextMeshProUGUI maxTimeText;
    [SerializeField] private TextMeshProUGUI curTimeText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private TextMeshProUGUI addRewardText;


    protected override void SubscribeToButtonEvents()
    {
        base.SubscribeToButtonEvents();

        if (baseButton != null)
        {
            baseButton.onClick.AddListener(delegate { ApplyReward(); });
            baseButton.onClick.AddListener(delegate { ToggleOpenOrClose(); });
        }
        onAdsFinished += ApplyAdditionalReward;
    }
    protected override void UnsubscribeFromButtonEvents()
    {
        base.UnsubscribeFromButtonEvents();
    }

    private void Start()
    {
        Initialize();
    }


    private void ApplyReward()
    {
        MoneyManager.Instance.AddJewel(rewardAmount);
    }
    private void ApplyAdditionalReward()
    {
        MoneyManager.Instance.AddJewel(addRewardAmount);
    }

    /// <summary>
    /// 데이터를 초기화한다.
    /// </summary>
    private void Initialize()
    {
        maxTimeInMinute = 240; // 4시간
        currentTimeInMinute = OfflineTimeToMinute();
        if(currentTimeInMinute == 0 /*|| MoneyManager.Instance.JewelPerClick == 0*/)
        {
            Debug.Log("시간을 받지 못하거나 혹은 보상이 0입니다.");
            ToggleOpenOrClose();
            return;
        }
        rewardAmount = GetRewardForTime(currentTimeInMinute);
        addRewardAmount = GetAdditionalRewardForTime(currentTimeInMinute);

        UpdateUI();
    }

    /// <summary>
    /// 시간에 비례하여 보상을 가져온다.
    /// </summary>
    /// <returns></returns>
    private BigInteger GetRewardForTime(int _curTimeInMinute)
    {
        BigInteger rewardData =  new BigInteger(1 * _curTimeInMinute) * MoneyManager.Instance.JewelPerClick;
        return rewardData;
    }

    /// <summary>
    /// 시간에 비례하여 보상을 추가로 가져온다.
    /// </summary>
    /// <returns></returns>
    protected BigInteger GetAdditionalRewardForTime(int _curTimeInMinute)
    {
        BigInteger rewardData = GetRewardForTime(_curTimeInMinute) * new BigInteger(additionalMultiply);
        return rewardData;
    }

    /// <summary>
    /// 오프라인 시간을 분단위로 가져온다. (최대시간 제한)
    /// </summary>
    /// <returns></returns>
    private int OfflineTimeToMinute()
    {
        TimeSpan offlineTime = TimerManager.Instance.CalculateTimeOffline();

        //최대시간 제한두기
        if (offlineTime.TotalMinutes > maxTimeInMinute)
        {
            offlineTime = new TimeSpan(0, (int)maxTimeInMinute / 60, 0, 0);
        }
        Debug.Log("Offline time is " + (int)offlineTime.TotalMinutes);
        return (int)offlineTime.TotalMinutes;
    }


    /// <summary>
    /// UI를 갱신한다.
    /// </summary>
    private void UpdateUI()
    {
        Debug.Log("currentTimeInMinute? "+ currentTimeInMinute);

        fill.fillAmount = (float)currentTimeInMinute / maxTimeInMinute;
        maxTimeText.text = string.Format("Max {0}Time", maxTimeInMinute/60);
        curTimeText.text = string.Format("{0}Time", currentTimeInMinute / 60);
        //시간 분 단위가 0이 아닐경우 추가
        if (currentTimeInMinute % 60 != 0)
        {
            curTimeText.text += string.Format(" {0}Minutes", currentTimeInMinute % 60);
        }

        rewardText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(rewardAmount));
        addRewardText.text = string.Format("+{0} Add Jewel", CurrencyParser.ToCurrencyString(addRewardAmount));
    }
}
