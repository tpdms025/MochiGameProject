using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(AdsController))]
public class PopupWithAds : Popup , IAdsRewardHandler
{
    //광고 시청 후 이벤트
    public event Action onAdsFinished;
    //광고가 끝난 후 이벤트 (취소 포함)
    public event Action onAdsEnded;

    //광고 버튼
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

    public void OnAdsFinished()
    {
        if(onAdsFinished != null)
        {
            onAdsFinished.Invoke();
        }
    }
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



    //protected void ViewAds()
    //{
    //    UnityAdsManager.Instance.ShowRewarded(onTriggerEffect, onAdsEnded);
    //}

  

}
