using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(AdsController))]
public class WindowWithAds : MonoBehaviour , IAdsRewardHandler
{
    //���� ��û �� �̺�Ʈ
    public event Action onAdsFinished;
    //������ ���� �� �̺�Ʈ (��� ����)
    public event Action onAdsEnded;

    //�ش� â�� �����ų� �����ִ���
    [SerializeField] private bool isOpen = true;

    //���� ��ư
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