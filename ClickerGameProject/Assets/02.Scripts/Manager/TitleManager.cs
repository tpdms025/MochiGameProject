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
        //버전 관리
        yield return StartCoroutine(versionChecker.LoadLocalVersion());

        //로컬시간 읽어오기
        StartCoroutine(TimerManager.Instance.LoadDateData());

        //DB 로드
        DBManager.Inst.LoadData();

        yield return new WaitForSeconds(1);

        //씬 이동
        LoadScene();
    }

    private void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

}
