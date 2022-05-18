using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

public class UnityAdsManager : MonoBehaviour
{
#if UNITY_ANDROID || UNITY_EDITOR
    public const string gameId = "4549213";
    private const string rewarded_video_id = "Rewarded_Android";
#elif UNITY_IOS
    public const string gameId = "4549212";
    private const string rewarded_video_id = "Rewarded_iOS";
#endif

    public bool testMode = true;

    public event Action onAdsFinished;
    public event Action onAdsEnded;

    public static UnityAdsManager Instance { get; private set; }



    #region Unity methods

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    #endregion


    /// <summary>
    /// 보상광고를 보여주다.
    /// </summary>
    /// <param name="_onAdFinished">광고시청을 다 했을 때의 이벤트</param>
    /// <param name="_onAdCompleted">광고가 끝날 때(오류 등..)</param>
    public void ShowRewarded(Action _onAdsFinished, Action _onAdsEnded)
    {
        if (Advertisement.IsReady())
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show(rewarded_video_id, options);

            onAdsFinished += _onAdsFinished;
            onAdsEnded += _onAdsEnded; 
        }
        else
        {
            Debug.Log("AD FAIL");
        }
    }


    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                {
#if UNITY_EDITOR
                    Debug.Log("The ad was successfully shown.");
#endif
                    //보상처리
                    if (onAdsFinished != null)
                    {
                        Debug.Log("onAdFinished");
                        onAdsFinished.Invoke();
                    }
                }
                break;
            case ShowResult.Skipped:
#if UNITY_EDITOR
                Debug.Log("The ad was skipped before reaching the end.");
#endif
                break;
            case ShowResult.Failed:
#if UNITY_EDITOR
                Debug.LogError("The ad failed to be shown.");
#endif
                break;
        }

        if (onAdsEnded != null)
        {
            Debug.Log("onAdsEnded");
            onAdsEnded.Invoke();
        }
        onAdsEnded = null;
        onAdsFinished = null;
    }

    private void Initialize()
    {
        Advertisement.Initialize(gameId, testMode);
#if UNITY_EDITOR
        Debug.Log("Unity Ads Init completed.");
#endif
    }


}
