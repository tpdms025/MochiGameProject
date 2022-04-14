using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OfflineRewardPopup : PopupWithAds
{
    private int maxTimeInMinute;
    private int currentTimeInMinute;
    private BigInteger baseRewardAmount;
    private BigInteger addRewardAmount;

    //기본 보상량에 곱할 배수
    private float basicMultiply;
    //추가 보상량에 곱할 배수
    private float addMultiply = 3;


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
            baseButton.onClick.AddListener(delegate { ApplyBasicReward(); });
        }
        onAdsFinished += ApplyAdditionalReward;
    }
    protected override void UnsubscribeFromButtonEvents()
    {
        base.UnsubscribeFromButtonEvents();
    }

    protected override void Start()
    {
        //맨 처음에만 초기화.
        Initialize();
        base.Start();
    }

    /// <summary>
    /// 기본 보상을 적용한다.
    /// </summary>
    private void ApplyBasicReward()
    {
        MoneyManager.Instance.AddJewel(baseRewardAmount);
    }

    /// <summary>
    /// 추가 보상을 적용한다.
    /// </summary>
    private void ApplyAdditionalReward()
    {
        MoneyManager.Instance.AddJewel(baseRewardAmount+addRewardAmount);
    }

    /// <summary>
    /// 데이터를 초기화한다.
    /// </summary>
    private void Initialize()
    {
        basicMultiply = 5.0f;
        addMultiply = 3.0f;
        maxTimeInMinute = 240; // 4시간
        currentTimeInMinute = OfflineTimeToMinute();

        //오프라인 시간이 0이거나 보상이 0일 경우의 예외처리
        if(currentTimeInMinute == 0 || MoneyManager.Instance.JewelPerTouch == 0)
        {
#if UNITY_EDITOR
            Debug.Log("시간을 받지 못하거나 혹은 보상이 0입니다.");
#endif
            ToggleOpenOrClose();
            return;
        }
        baseRewardAmount = GetRewardForTime(basicMultiply,currentTimeInMinute);
        addRewardAmount = GetAdditionalRewardForTime(addMultiply);

        UpdateUI();
    }

    /// <summary>
    /// 시간에 비례하여 기본보상을 가져온다.
    /// </summary>
    /// <returns></returns>
    private BigInteger GetRewardForTime(float multiply, int _curTimeInMinute)
    {
        BigInteger rewardData =  new BigInteger(5 * _curTimeInMinute) * MoneyManager.Instance.JewelPerTouch;
        return rewardData;
    }

    /// <summary>
    /// 시간에 비례하여 추가보상을 가져온다. (기본보상의 N배)
    /// </summary>
    /// <returns></returns>
    protected BigInteger GetAdditionalRewardForTime(float multiply)
    {
        BigInteger rewardData = baseRewardAmount * new BigInteger(addMultiply);
        return rewardData;
    }

    /// <summary>
    /// 오프라인 시간을 분단위로 가져온다. (최대시간 제한)
    /// </summary>
    /// <returns></returns>
    private int OfflineTimeToMinute()
    {
        TimeSpan offlineTime = TimerManager.Instance.offlineTimeSpan;

        //최대시간 제한두기
        if (offlineTime.TotalMinutes > maxTimeInMinute)
        {
            offlineTime = new TimeSpan(0, (int)maxTimeInMinute / 60, 0, 0);
        }
        return (int)offlineTime.TotalMinutes;
    }


    /// <summary>
    /// UI를 갱신한다.
    /// </summary>
    private void UpdateUI()
    {
        //타이머 시간
        curTimeText.text = string.Empty;

        //시간 출력
        if (currentTimeInMinute >= 60 )
            curTimeText.text += string.Format("{0}시간", currentTimeInMinute / 60);
        //분 출력
        if (currentTimeInMinute % 60 != 0)
            curTimeText.text += string.Format(" {0}분", currentTimeInMinute % 60);

        fill.fillAmount = (float)currentTimeInMinute / maxTimeInMinute;
        maxTimeText.text = string.Format("최대 {0}시간", maxTimeInMinute/60);
        rewardText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(baseRewardAmount));
        addRewardText.text = string.Format("+{0} 추가 획득", CurrencyParser.ToCurrencyString(addRewardAmount));
    }
}
