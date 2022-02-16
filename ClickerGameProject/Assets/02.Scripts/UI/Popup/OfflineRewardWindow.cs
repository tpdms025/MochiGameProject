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
    /// �ð��� ����Ͽ� ������ �����´�.
    /// </summary>
    /// <returns></returns>
    private BigInteger GetRewardForTime()
    {
        BigInteger rewardData =  new BigInteger(OfflineTimeToSeconds()) * MoneyManager.Instance.JewelPerSec;
        MoneyManager.Instance.AddJewel(rewardData);
        return rewardData;
    }

    /// <summary>
    /// �ð��� ����Ͽ� ������ 3��� ��´�.
    /// </summary>
    /// <returns></returns>
    protected void Get3xRewardForTime()
    {
        BigInteger rewardData = GetRewardForTime() * new BigInteger(3);
        MoneyManager.Instance.AddJewel(rewardData);
    }

    /// <summary>
    /// �������� �ð��� �ʴ����� �����´�.
    /// </summary>
    /// <returns></returns>
    private float OfflineTimeToSeconds()
    {
        TimeSpan offlineTime = TimerManager.Instance.CalculateTimeOffline();

        //�ִ�ð� ���ѵα�
        if (offlineTime.TotalSeconds > maxTime)
        {
            offlineTime = new TimeSpan(0, (int)maxTime / 60, 0, 0);
        }
        return currentTime = (float)offlineTime.TotalSeconds;
    }


    /// <summary>
    /// Ÿ�̸� UI�� �����Ѵ�.
    /// </summary>
    private void UpdateTimer()
    {
        fill.fillAmount = currentTime / maxTime;
    }




}
