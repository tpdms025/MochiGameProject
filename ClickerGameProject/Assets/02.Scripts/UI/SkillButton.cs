using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillButton : MonoBehaviour
{
    #region Data

    //스킬 지속시간
    private float maxSkillTime;

    private float currentSkillTime;

    private float rotateSpeed = 30.0f;

    public event Action OnStartedSkill;
    public event Action OnFinishedSkill;

    [SerializeField]  private bool isSkillActivate;

    #endregion


    #region Fields
    [SerializeField] private Image icon;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI cooldownText;

    private Button button;

    //임시
    [Header("[Temporary]")]
    [SerializeField] private Image circularImage;

    [SerializeField] private PopupWithAds popup;

    #endregion

    #region Unity methods
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(popup.ToggleOpenOrClose);
    }

    private void Start()
    {
        InitData();
        InitFunc();
    }

    private void OnEnable()
    {
        popup.onAdsFinished += TriggerEffect;
        SubscribeToSkillButtonEvents();
    }

    private void OnDisable()
    {
        popup.onAdsFinished -= TriggerEffect;
        UnsubscribeFromSkillButtonEvents();
    }
    #endregion

    #region Methods


    /// <summary>
    /// 스킬을 발동한다.
    /// </summary>
    public void TriggerEffect()
    {
        Debug.Log("TriggerEffect");
        ActivateStateUI();
    }



    #endregion

    #region Private Methods

    /// <summary>
    /// 스킬 활성화 상태의 UI
    /// </summary>
    private void ActivateStateUI()
    {
        isSkillActivate = true;
        circularImage.transform.gameObject.SetActive(false);
        fill.transform.gameObject.SetActive(true);
        cooldownText.transform.gameObject.SetActive(true);
        button.enabled = false;

        StartCoroutine(Cor_SkillsActivation());
    }

    /// <summary>
    /// 기본 상태의 UI
    /// </summary>
    private void DefaultStateUI()
    {
        isSkillActivate = false;
        circularImage.transform.gameObject.SetActive(true);
        fill.transform.gameObject.SetActive(false);
        cooldownText.transform.gameObject.SetActive(false);
        button.enabled = true;

        StartCoroutine(Cor_ShowDefaultUI());
    }


    /// <summary>
    /// 스킬을 활성화한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cor_SkillsActivation()
    {
        //버프시작 이벤트 호출
        if (OnStartedSkill != null)
        {
            OnStartedSkill.Invoke();
        }
        Debug.Log("currentSkillTime"+ currentSkillTime);

        //UI 업데이트
        while (currentSkillTime > 0)
        {
            Debug.Log("Cor_SkillsActivation");
            fill.fillAmount = currentSkillTime / maxSkillTime;
            TimeSpan duration = TimeSpan.FromSeconds(currentSkillTime);
            cooldownText.text = string.Format("{0:mm\\:ss}", duration);
            //cooldownText.text = string.Format("{0:0}", currentSkillTime);

            currentSkillTime -= Time.deltaTime;
            yield return null;
        }

        currentSkillTime = maxSkillTime;

        //버프 종료 이벤트 호출
        if (OnFinishedSkill != null)
        {
            OnFinishedSkill.Invoke();
        }

        yield return null;

        //기본상태 UI로 변경
        DefaultStateUI();
    }

    /// <summary>
    /// 기본 UI를 보여준다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cor_ShowDefaultUI()
    {
        while (!isSkillActivate)
        {
            //TODO : Show the particles
            //************************************
            circularImage.transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);
            //circularImage.transform.rotation = Quaternion.Slerp(circularImage.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime);
            yield return null;
        }
    }


    /// <summary>
    /// 오프라인시간을 계산하며 어플리케이션을 처음 실행할 때
    /// 스킬이 발동 중인지 확인한다.
    /// </summary>
    /// <returns></returns>
    private bool CheckSkillActivated()
    {
        //TODO : DB로드하여 마지막으로 종료할 때 스킬이 발동중인지 확인하는 변수를 가져온다.
        //************************************
        //현재는 임시로 true함.
        bool isPrevSkillActivate = true;
        if(isPrevSkillActivate.Equals(false))
        {
            return false;
        }

        bool _isSkillActivate;
        double intervalTime = TimerManager.Instance.CalculateTimeOffline().TotalSeconds;
        if (0 < intervalTime && intervalTime < maxSkillTime)
        {
            //스킬 활성화
            _isSkillActivate = true;
            currentSkillTime = maxSkillTime - (float)intervalTime;
        }
        else
        {
            //스킬 비활성화
            _isSkillActivate = false;
            currentSkillTime = maxSkillTime;
        }

        return _isSkillActivate;
    }


    /// <summary>
    /// 데이터를 불러와 세팅한다.
    /// </summary>
    private void InitData()
    {
        //임시값
        maxSkillTime = 5.0f;

        isSkillActivate = CheckSkillActivated();
    }

    /// <summary>
    /// 데이터를 불러와 초기함수를 호출한다.
    /// </summary>
    private void InitFunc()
    {
        if (isSkillActivate)
        {
            ActivateStateUI();
        }
        else
        {
            DefaultStateUI();
        }
    }



    private void SubscribeToSkillButtonEvents()
    {
    }
    private void UnsubscribeFromSkillButtonEvents()
    {
    }

    #endregion
}
