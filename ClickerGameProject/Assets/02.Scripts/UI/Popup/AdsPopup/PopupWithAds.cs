using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(AdsController))]
public class PopupWithAds : Popup , IAdsRewardHandler
{
    //���� ��û �� �̺�Ʈ
    public event Action onAdsFinished;
    //���� ���� �� �̺�Ʈ (��� ����)
    public event Action onAdsEnded;

    //���� ��ư
    [SerializeField] private Button adsButton;

    private AdsController adsController;

    protected override void Awake()
    {
        base.Awake();
        adsController = GetComponent<AdsController>();
        adsController.adsRewardHandler = this;

        SubscribeToButtonEvents();
    }

    protected void OnDestroy()
    {
        UnsubscribeFromButtonEvents();
    }


    #region interface Method

    //@Override
    public void OnAdsFinished()
    {
        if(onAdsFinished != null)
        {
            onAdsFinished.Invoke();
        }
    }
    //@Override
    public void OnAdsEnded()
    {
        if(onAdsEnded != null)
        {
            onAdsEnded.Invoke();
        }
        ToggleOpenOrClose();
    }
    #endregion



    protected virtual void SubscribeToButtonEvents()
    {
        //if (adsButton != null)
        //{
        //    adsButton.onClick.AddListener(delegate { ViewAds(); });
        //}
        //onTriggerEffect += ProvideRewardData;
        //onAdsEnded += EndAnds;

        if (adsButton != null)
        {
            adsButton.onClick.AddListener(delegate { adsController.ViewAds(); });
        }
    }
    protected virtual void UnsubscribeFromButtonEvents()
    {
    }

    public void SetAction(Action onAdsFinished, Action onAdsEnded)
    {
        this.onAdsFinished = onAdsFinished; 
        this.onAdsEnded = onAdsEnded;
    }


    //protected void ViewAds()
    //{
    //    UnityAdsManager.Instance.ShowRewarded(onTriggerEffect, onAdsEnded);
    //}



}
