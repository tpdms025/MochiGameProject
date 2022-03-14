using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillButton : MonoBehaviour
{
    #region Data

    //스킬 시간 (초)
    [SerializeField] private float skillTime;

    //경과 시간 (초)
    [SerializeField] private float elapsedTime;


    //스킬이 시작할 때 발생하는 이벤트 델리게이트
    //float: 경과 시간
    public event Action<float> OnStartedSkill;         

    //스킬이 끝날 때 발생하는 이벤트 델리게이트
    public event Action OnFinishedSkill;


    private const float rotateSpeed = 30.0f;
    [SerializeField] private bool isSkillActivate;

    #endregion


    #region Fields
    [SerializeField] private Image icon;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI cooldownText;

    private Button button;

    //임시
    [Header("[Temporary]")]
    [SerializeField] private Image circularImage;

    //팝업창 오브젝트
    [SerializeField] private PopupWithAds popup;

    #endregion

    #region Unity methods
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(popup.ToggleOpenOrClose);
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
        ActivateStateUI();
    }

    /// <summary>
    /// 데이터를 로드하여 세팅한다.
    /// </summary>
    /// <param name="_skillTime"></param>
    public void LoadData(float curSkillTime, float _skillTime)
    {
        elapsedTime = curSkillTime;
        skillTime = _skillTime;
        bool prevActivate = elapsedTime == skillTime ? false : true;
        isSkillActivate = prevActivate;

        Initialize();
    }
  
    private void Initialize()
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
    /// 스킬을 활성화하여 버프를 발동시킨다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cor_SkillsActivation()
    {
        //버프 시작 이벤트 호출
        if (OnStartedSkill != null)
        {
            OnStartedSkill.Invoke(elapsedTime);
        }

        //UI 업데이트
        while (elapsedTime > 0)
        {
            fill.fillAmount = elapsedTime / skillTime;
            TimeSpan duration = TimeSpan.FromSeconds(elapsedTime+1);    //올림을 위해 +1
            cooldownText.text = string.Format("{0:mm\\:ss}", duration);
            //cooldownText.text = string.Format("{0:0}", currentSkillTime);

            elapsedTime -= Time.deltaTime;
            //Debug.Log("currentSkillTime is " + currentSkillTime);
            yield return null;
        }

        elapsedTime = skillTime;

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


    



    private void SubscribeToSkillButtonEvents()
    {
    }
    private void UnsubscribeFromSkillButtonEvents()
    {
    }

    #endregion
}
