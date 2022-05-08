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
        //���� ����
        yield return StartCoroutine(versionChecker.LoadLocalVersion());

        //Ÿ��Ʋ ����
        effectAnim.PlayAnimation();
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(tabToPlay.Cor_BlinkText());

        //��ġ�Ҷ����� ���
        yield return new WaitUntil(()=>TabToPlay.activated);

        loadingBar.gameObject.SetActive(true);
        StartCoroutine(UpdateLoadingText());

        //������ �ε�
        LoadAsynData();

        //�� ���ҽ� �ε� �� �̵�
        StartCoroutine(Cor_LoadAsynScene(1));
    }

    private async void LoadAsynData()
    {
        //DB �ε�
        await Task.Run(delegate { DBManager.Inst.LoadAllData(); });
        //yield return StartCoroutine(DBManager.Inst.LoadAllData());

        //���ýð� �о����
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

            //�ִ� �ð� 1��
            if (operation.progress >= 0.9f)
                //if (time > 3f)
            {
                //������ �ε���� �Ϸ�Ǿ��ٸ� �� �̵�
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
    /// �ε��� ���������� Ȯ���ϴ� �ؽ�Ʈ�� �����Ѵ�. (Loading...)
    /// </summary>
    private IEnumerator UpdateLoadingText()
    {
        string dotStr = string.Empty;
        //������ �ε尡 �Ϸ�ɶ����� �ݺ�
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
