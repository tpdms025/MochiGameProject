using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OfflineRewardPopup : PopupWithAds
{
    //�ִ� �ð� (��)
    private readonly int maxTimeInMinute = 240;    //4�ð�
    //�ּ� �ð� (��)
    private readonly int minTimeInMinute = 10;    //10��
    //���� �ð� (��)
    private int currentTimeInMinute;

    //�⺻ ����
    private double baseRewardAmount;
    //�߰� ����
    private double addRewardAmount;

    //�⺻ ���󷮿� ���� ���
    private float basicMultiply;
    //�߰� ���󷮿� ���� ���
    private float addMultiply = 3f;



    #region Fields

    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI maxTimeText;
    [SerializeField] private TextMeshProUGUI curTimeText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private TextMeshProUGUI addRewardText;

    #endregion




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
        baseButton.onClick.RemoveAllListeners();
        onAdsFinished -= ApplyAdditionalReward;
    }

    protected override void Start()
    {
        //�� ó������ �ʱ�ȭ.
        if (DBManager.Inst.firstStart == false)
        {
            Initialize();
            DBManager.Inst.firstStart = true;
        }
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
        currentTimeInMinute = OfflineTimeToMinute();

        basicMultiply = 5.0f;

        //�������� �ð��� �ּҽð����� �۰ų� ������ 0�� ����� ����ó��
        if(currentTimeInMinute < minTimeInMinute || MoneyManager.Inst.JewelPerTouch.Value == 0)
        {
#if UNITY_EDITOR
            Debug.Log("�ð��� ���� ���ϰų� Ȥ�� ������ 0�Դϴ�.");
#endif
            return;
        }
        baseRewardAmount = GetRewardForTime(basicMultiply,currentTimeInMinute);
        addRewardAmount = GetAdditionalRewardForTime(addMultiply);

        ToggleOpenOrClose();
        UpdateUI();
    }

    /// <summary>
    /// �ð��� ����Ͽ� �⺻������ �����´�.
    /// </summary>
    /// <returns></returns>
    private double GetRewardForTime(float multiply, int _curTimeInMinute)
    {
        double rewardData =  (5 * _curTimeInMinute) * MoneyManager.Inst.JewelPerTouch.Value;
        return rewardData;
    }

    /// <summary>
    /// �ð��� ����Ͽ� �߰������� �����´�. (�⺻������ N��)
    /// </summary>
    /// <returns></returns>
    protected double GetAdditionalRewardForTime(float multiply)
    {
        double rewardData = baseRewardAmount * (addMultiply);
        return rewardData;
    }

    /// <summary>
    /// �������� �ð��� �д����� �����´�. (�ִ�ð� ����)
    /// </summary>
    /// <returns></returns>
    private int OfflineTimeToMinute()
    {
        TimeSpan offlineTime = TimerManager.Inst.offlineTimeSpan;

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
        addRewardText.text = string.Format("+{0} ȹ��", CurrencyParser.ToCurrencyString(addRewardAmount));
    }
}
