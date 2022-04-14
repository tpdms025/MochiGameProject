using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    private VersionChecker versionChecker;

    private void Awake()
    {
        versionChecker = GetComponent<VersionChecker>();
    }
    private void Start()
    {
        StartCoroutine(OnStart());
    }
    private IEnumerator OnStart()
    {
        //���� ����
        yield return StartCoroutine(versionChecker.LoadLocalVersion());

        //���ýð� �о����
        StartCoroutine(TimerManager.Instance.LoadDateData());

        //DB �ε�
        DBManager.Inst.LoadData();

        yield return new WaitForSeconds(1);

        //�� �̵�
        LoadScene();
    }

    private void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

}
