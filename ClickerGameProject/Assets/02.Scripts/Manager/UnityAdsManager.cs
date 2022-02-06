using UnityEngine;
using UnityEngine.Advertisements;


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


    private void Start()
    {
        Initialize();
    }



    public void ShowRewarded()
    {
        if (Advertisement.IsReady())
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show(rewarded_video_id, options);
        }
        else
        {
            Debug.Log("AD FAIL");
        }
    }


    private void HandleShowResult(ShowResult result)
    {
        switch(result)
        {
            case ShowResult.Finished:
                {
#if UNITY_EDITOR
                    Debug.Log("The ad was successfully shown.");
#endif
                    //보상처리
                    //
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
    }
    private void Initialize()
    {
        Advertisement.Initialize(gameId, testMode);
#if UNITY_EDITOR
        Debug.Log("Unity Ads Init completed.");
#endif
    }


}
