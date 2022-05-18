using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    private VersionChecker versionChecker;
    [SerializeField] private TabToPlay tabToPlay;
    [SerializeField] private TitleEffectAnim effectAnim;

    [SerializeField] private GameObject loadingBar;
    [SerializeField] private Image loadingFill;
    [SerializeField] private TextMeshProUGUI loadingPercentText;
    [SerializeField] private TextMeshProUGUI loadingText;




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

        //타이틀 연출
        effectAnim.PlayAnimation();
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(tabToPlay.Cor_BlinkText());

        //터치할때까지 대기
        yield return new WaitUntil(()=>TabToPlay.activated);

        loadingBar.gameObject.SetActive(true);
        StartCoroutine(UpdateLoadingText());

        //데이터 로드
        LoadAsynData();

        //씬 리소스 로드 후 이동
        StartCoroutine(Cor_LoadAsynScene(1));
    }

    private async void LoadAsynData()
    {
        //DB 로드
        await Task.Run(delegate { DBManager.Inst.LoadAllData(); });
        //yield return StartCoroutine(DBManager.Inst.LoadAllData());

        //로컬시간 읽어오기
        await Task.Run(delegate { TimerManager.Inst.LoadDateData(); });
        //yield return StartCoroutine(TimerManager.Inst.LoadDateData());

        await Task.Run(() => DBManager.Inst.loadDataCompleted = true); 
    }

    private IEnumerator Cor_LoadAsynScene(int nextSceneNum)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneNum);
        operation.allowSceneActivation = false;

        float time = 0f;
        while (!operation.isDone)
        {
            time += Time.deltaTime;
            loadingFill.fillAmount = operation.progress;
            //loadingFill.fillAmount = time / 1f;
            loadingPercentText.text = string.Format("{0:F0}%", loadingFill.fillAmount * 100);

            //최대 시간 1초
            if (operation.progress >= 0.9f)
                //if (time > 3f)
            {
                //데이터 로드까지 완료되었다며 씬 이동
                if (DBManager.Inst.loadDataCompleted)
                {
                    loadingFill.fillAmount = 1f;
                    loadingPercentText.text = string.Format("{0}%", 100);
                    DBManager.Inst.loadAllCompleted = true;
                    operation.allowSceneActivation = true;
                    yield break;
                }
            }
            yield return null;
        }
    }


    /// <summary>
    /// 로딩이 진행중인지 확인하는 텍스트를 갱신한다. (Loading...)
    /// </summary>
    private IEnumerator UpdateLoadingText()
    {
        string dotStr = string.Empty;
        //데이터 로드가 완료될때까지 반복
        while (!DBManager.Inst.loadAllCompleted)
        {
            if (dotStr.Length >= 3)
            {
                dotStr = ".";
            }
            else
            {
                dotStr += ".";
            }
             loadingText.text = string.Format("Loading{0}",dotStr);

            yield return new WaitForSeconds(0.5f);
        }
    }

}
