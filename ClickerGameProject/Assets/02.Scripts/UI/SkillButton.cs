using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillButton : MonoBehaviour
{
    #region Data

    //��ų ���ӽð�
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

    //�ӽ�
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
    /// ��ų�� �ߵ��Ѵ�.
    /// </summary>
    public void TriggerEffect()
    {
        Debug.Log("TriggerEffect");
        ActivateStateUI();
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
    /// ��ų�� Ȱ��ȭ�Ѵ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cor_SkillsActivation()
    {
        //�������� �̺�Ʈ ȣ��
        if (OnStartedSkill != null)
        {
            OnStartedSkill.Invoke();
        }
        Debug.Log("currentSkillTime"+ currentSkillTime);

        //UI ������Ʈ
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


    /// <summary>
    /// �������νð��� ����ϸ� ���ø����̼��� ó�� ������ ��
    /// ��ų�� �ߵ� ������ Ȯ���Ѵ�.
    /// </summary>
    /// <returns></returns>
    private bool CheckSkillActivated()
    {
        //TODO : DB�ε��Ͽ� ���������� ������ �� ��ų�� �ߵ������� Ȯ���ϴ� ������ �����´�.
        //************************************
        //����� �ӽ÷� true��.
        bool isPrevSkillActivate = true;
        if(isPrevSkillActivate.Equals(false))
        {
            return false;
        }

        bool _isSkillActivate;
        double intervalTime = TimerManager.Instance.CalculateTimeOffline().TotalSeconds;
        if (0 < intervalTime && intervalTime < maxSkillTime)
        {
            //��ų Ȱ��ȭ
            _isSkillActivate = true;
            currentSkillTime = maxSkillTime - (float)intervalTime;
        }
        else
        {
            //��ų ��Ȱ��ȭ
            _isSkillActivate = false;
            currentSkillTime = maxSkillTime;
        }

        return _isSkillActivate;
    }


    /// <summary>
    /// �����͸� �ҷ��� �����Ѵ�.
    /// </summary>
    private void InitData()
    {
        //�ӽð�
        maxSkillTime = 5.0f;

        isSkillActivate = CheckSkillActivated();
    }

    /// <summary>
    /// �����͸� �ҷ��� �ʱ��Լ��� ȣ���Ѵ�.
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
