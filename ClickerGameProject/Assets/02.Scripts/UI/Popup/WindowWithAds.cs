using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(AdsController))]
public class WindowWithAds : MonoBehaviour , IAdsRewardHandler
{
    //광고 시청 후 이벤트
    public event Action onAdsFinished;
    //광고가 끝난 후 이벤트 (취소 포함)
    public event Action onAdsEnded;

    //해당 창이 열리거나 닫혀있는지
    [SerializeField] private bool isOpen = true;

    //광고 버튼
    [SerializeField] private Button adsButton;

    private AdsController adsController;

    protected void Awake()
    {
        adsController = GetComponent<AdsController>();
        adsController.adsRewardHandler = this;

        SubscribeToButtonEvents();
        ToggleOpenOrClose();
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


    public void ToggleOpenOrClose()
    {
        if(isOpen)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
        isOpen = !isOpen;
    }

  

}
