using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsController : MonoBehaviour
{
    [HideInInspector]
    [System.NonSerialized]
    public IAdsRewardHandler adsRewardHandler = null;

    public void ViewAds()
    {
        UnityAdsManager.Instance.ShowRewarded(adsRewardHandler.OnAdsFinished, adsRewardHandler.OnAdsEnded);

    }

}
