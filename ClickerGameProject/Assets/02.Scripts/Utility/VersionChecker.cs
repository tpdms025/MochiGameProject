using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class VersionChecker : MonoBehaviour
{
    // ����üũ�� ���� URL
    public string URL = "";

    // ���� �������
    public string curVersion;

    // �ֽŹ���
    string latsetVersion;

    // ����Ȯ�� UI
    public GameObject newVersionAvailable;

    //���� �ؽ�Ʈ
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

        // ������ ��û
        yield return www.SendWebRequest(); 

        if (www.isNetworkError)
        {
            Debug.Log("error get page");
        }
        else
        {
            // ���� �Էµ� �ֽŹ���
            latsetVersion = www.downloadHandler.text; 
        }
        VersionCheck();
    }

    // ����� ����
    public void OpenURL(string url) 
    {
        Application.OpenURL(url);
    }

}
