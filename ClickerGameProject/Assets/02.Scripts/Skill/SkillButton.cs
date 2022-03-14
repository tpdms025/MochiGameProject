using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillButton : MonoBehaviour
{
    #region Data

    //��ų �ð� (��)
    [SerializeField] private float skillTime;

    //��� �ð� (��)
    [SerializeField] private float elapsedTime;


    //��ų�� ������ �� �߻��ϴ� �̺�Ʈ ��������Ʈ
    //float: ��� �ð�
    public event Action<float> OnStartedSkill;         

    //��ų�� ���� �� �߻��ϴ� �̺�Ʈ ��������Ʈ
    public event Action OnFinishedSkill;


    private const float rotateSpeed = 30.0f;
    [SerializeField] private bool isSkillActivate;

    #endregion


    #region Fields
    [SerializeField] private Image icon;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI cooldownText;

    private Button button;

    //�ӽ�
    [Header("[Temporary]")]
    [SerializeField] private Image circularImage;

    //�˾�â ������Ʈ
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
    /// ��ų�� �ߵ��Ѵ�.
    /// </summary>
    public void TriggerEffect()
    {
        ActivateStateUI();
    }

    /// <summary>
    /// �����͸� �ε��Ͽ� �����Ѵ�.
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
    /// ��ų Ȱ��ȭ ������ UI
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
    /// �⺻ ������ UI
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
    /// ��ų�� Ȱ��ȭ�Ͽ� ������ �ߵ���Ų��.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cor_SkillsActivation()
    {
        //���� ���� �̺�Ʈ ȣ��
        if (OnStartedSkill != null)
        {
            OnStartedSkill.Invoke(elapsedTime);
        }

        //UI ������Ʈ
        while (elapsedTime > 0)
        {
            fill.fillAmount = elapsedTime / skillTime;
            TimeSpan duration = TimeSpan.FromSeconds(elapsedTime+1);    //�ø��� ���� +1
            cooldownText.text = string.Format("{0:mm\\:ss}", duration);
            //cooldownText.text = string.Format("{0:0}", currentSkillTime);

            elapsedTime -= Time.deltaTime;
            //Debug.Log("currentSkillTime is " + currentSkillTime);
            yield return null;
        }

        elapsedTime = skillTime;

        //���� ���� �̺�Ʈ ȣ��
        if (OnFinishedSkill != null)
        {
            OnFinishedSkill.Invoke();
        }

        yield return null;

        //�⺻���� UI�� ����
        DefaultStateUI();
    }

    /// <summary>
    /// �⺻ UI�� �����ش�.
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
