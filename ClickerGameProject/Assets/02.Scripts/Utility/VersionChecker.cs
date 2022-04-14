using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class VersionChecker : MonoBehaviour
{
    // 버전체크를 위한 URL
    public string URL = "";

    // 현재 빌드버전
    public string curVersion;

    // 최신버전
    string latsetVersion;

    // 버전확인 UI
    public GameObject newVersionAvailable;

    //버전 텍스트
    [SerializeField] private TextMeshProUGUI versionText;


    private void Start()
    {
        StartCoroutine(LoadLocalVersion());
    }

    public IEnumerator LoadLocalVersion()
    {
        curVersion = string.Format("Ver {0}", Application.version);
        versionText.text = curVersion;
        yield return null;
    }

    private bool VersionCheck()
    {
        if(curVersion != latsetVersion)
        {
            newVersionAvailable.SetActive(true);
            return true;
        }
        else
        {
            newVersionAvailable.SetActive(false);
            return false;
        }
    }

    private IEnumerator LoadTxtData(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        // 페이지 요청
        yield return www.SendWebRequest(); 

        if (www.isNetworkError)
        {
            Debug.Log("error get page");
        }
        else
        {
            // 웹에 입력된 최신버전
            latsetVersion = www.downloadHandler.text; 
        }
        VersionCheck();
    }

    // 스토어 열기
    public void OpenURL(string url) 
    {
        Application.OpenURL(url);
    }

}
