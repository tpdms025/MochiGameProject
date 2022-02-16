using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class OfflineRewardWindow : WindowWithAds
{
    private float maxTime;
    private float currentTime;

    [SerializeField] private Button baseButton;
    [SerializeField] private Image fill;


    protected override void SubscribeToButtonEvents()
    {
        base.SubscribeToButtonEvents();

        if (baseButton != null)
        {
            baseButton.onClick.AddListener(delegate { GetRewardForTime(); });
            baseButton.onClick.AddListener(delegate { ToggleOpenOrClose(); });
        }
        onAdsFinished += Get3xRewardForTime;
    }
    protected override void UnsubscribeFromButtonEvents()
    {
        base.UnsubscribeFromButtonEvents();
    }



    /// <summary>
    /// 시간에 비례하여 보상을 가져온다.
    /// </summary>
    /// <returns></returns>
    private BigInteger GetRewardForTime()
    {
        BigInteger rewardData =  new BigInteger(OfflineTimeToSeconds()) * MoneyManager.Instance.JewelPerSec;
        MoneyManager.Instance.AddJewel(rewardData);
        return rewardData;
    }

    /// <summary>
    /// 시간에 비례하여 보상을 3배로 얻는다.
    /// </summary>
    /// <returns></returns>
    protected void Get3xRewardForTime()
    {
        BigInteger rewardData = GetRewardForTime() * new BigInteger(3);
        MoneyManager.Instance.AddJewel(rewardData);
    }

    /// <summary>
    /// 오프라인 시간을 초단위로 가져온다.
    /// </summary>
    /// <returns></returns>
    private float OfflineTimeToSeconds()
    {
        TimeSpan offlineTime = TimerManager.Instance.CalculateTimeOffline();

        //최대시간 제한두기
        if (offlineTime.TotalSeconds > maxTime)
        {
            offlineTime = new TimeSpan(0, (int)maxTime / 60, 0, 0);
        }
        return currentTime = (float)offlineTime.TotalSeconds;
    }


    /// <summary>
    /// 타이머 UI를 갱신한다.
    /// </summary>
    private void UpdateTimer()
    {
        fill.fillAmount = currentTime / maxTime;
    }




}
