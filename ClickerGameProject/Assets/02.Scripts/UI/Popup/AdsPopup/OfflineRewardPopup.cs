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

    //�⺻ ���󷮿� ���� ���
    private float basicMultiply;
    //�߰� ���󷮿� ���� ���
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
        //�� ó������ �ʱ�ȭ.
        Initialize();
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
        MoneyManager.Instance.AddJewel(baseRewardAmount+addRewardAmount);
    }

    /// <summary>
    /// �����͸� �ʱ�ȭ�Ѵ�.
    /// </summary>
    private void Initialize()
    {
        basicMultiply = 5.0f;
        addMultiply = 3.0f;
        maxTimeInMinute = 240; // 4�ð�
        currentTimeInMinute = OfflineTimeToMinute();

        //�������� �ð��� 0�̰ų� ������ 0�� ����� ����ó��
        if(currentTimeInMinute == 0 || MoneyManager.Instance.JewelPerTouch == 0)
        {
#if UNITY_EDITOR
            Debug.Log("�ð��� ���� ���ϰų� Ȥ�� ������ 0�Դϴ�.");
#endif
            ToggleOpenOrClose();
            return;
        }
        baseRewardAmount = GetRewardForTime(basicMultiply,currentTimeInMinute);
        addRewardAmount = GetAdditionalRewardForTime(addMultiply);

        UpdateUI();
    }

    /// <summary>
    /// �ð��� ����Ͽ� �⺻������ �����´�.
    /// </summary>
    /// <returns></returns>
    private BigInteger GetRewardForTime(float multiply, int _curTimeInMinute)
    {
        BigInteger rewardData =  new BigInteger(5 * _curTimeInMinute) * MoneyManager.Instance.JewelPerTouch;
        return rewardData;
    }

    /// <summary>
    /// �ð��� ����Ͽ� �߰������� �����´�. (�⺻������ N��)
    /// </summary>
    /// <returns></returns>
    protected BigInteger GetAdditionalRewardForTime(float multiply)
    {
        BigInteger rewardData = baseRewardAmount * new BigInteger(addMultiply);
        return rewardData;
    }

    /// <summary>
    /// �������� �ð��� �д����� �����´�. (�ִ�ð� ����)
    /// </summary>
    /// <returns></returns>
    private int OfflineTimeToMinute()
    {
        TimeSpan offlineTime = TimerManager.Instance.offlineTimeSpan;

        //�ִ�ð� ���ѵα�
        if (offlineTime.TotalMinutes > maxTimeInMinute)
        {
            offlineTime = new TimeSpan(0, (int)maxTimeInMinute / 60, 0, 0);
        }
        return (int)offlineTime.TotalMinutes;
    }


    /// <summary>
    /// UI�� �����Ѵ�.
    /// </summary>
    private void UpdateUI()
    {
        //Ÿ�̸� �ð�
        curTimeText.text = string.Empty;

        //�ð� ���
        if (currentTimeInMinute >= 60 )
            curTimeText.text += string.Format("{0}�ð�", currentTimeInMinute / 60);
        //�� ���
        if (currentTimeInMinute % 60 != 0)
            curTimeText.text += string.Format(" {0}��", currentTimeInMinute % 60);

        fill.fillAmount = (float)currentTimeInMinute / maxTimeInMinute;
        maxTimeText.text = string.Format("�ִ� {0}�ð�", maxTimeInMinute/60);
        rewardText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(baseRewardAmount));
        addRewardText.text = string.Format("+{0} �߰� ȹ��", CurrencyParser.ToCurrencyString(addRewardAmount));
    }
}
