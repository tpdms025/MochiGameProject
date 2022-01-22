using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillButton : MonoBehaviour
{
    #region Data
    private float currentCooldown;
    private float currentSkillTime;

    private float maxCooldown;
    private float maxSkillTime;

    public event Action onActivated;
    public event Action onDeactivated;
    public event Action onFinishedBuff;
    public event Action onFinishedCooldown;

    private bool isActivate;

    #endregion


    #region Fields
    [SerializeField] private Image icon;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI cooldownText;
    private Button button;
    #endregion

    #region Unity methods
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        SubscribeToSkillButtonEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromSkillButtonEvents();
    }
    #endregion

    #region Methods
    public void OnClick()
    {
        StartCoroutine(Cor_ActivateToBuff());
    }


    #endregion

    #region Private Methods

    /// <summary>
    /// ������ Ȱ��ȭ�Ѵ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cor_ActivateToBuff()
    {
        while(currentSkillTime < maxSkillTime && isActivate)
        {
            //TODO-��ƼŬ ����
            //

            currentSkillTime += Time.deltaTime;
            yield return null;
        }
        yield return null;

        //TODO-��ƼŬ ��Ȱ��ȭ
        //
        currentSkillTime = 0;

        onFinishedBuff();
    }


    /// <summary>
    /// FillAmount�� ������Ʈ�Ѵ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cor_UpdateFillAmount()
    {
        while (currentCooldown > 0)
        {
            fill.fillAmount = currentCooldown / maxCooldown;
            cooldownText.text = string.Format("{0:0}", currentCooldown);

            currentCooldown -= Time.deltaTime;
            yield return null;
        }
        yield return null;
        currentCooldown = maxCooldown;

        onFinishedCooldown();
    }

    /// <summary>
    /// �����͸� �ҷ��� �ʱ�ȭ�Ѵ�.
    /// </summary>
    private void Init()
    {
        //�� �ӽð�
        maxCooldown = 10.0f;
        maxSkillTime = 5.0f;
        currentCooldown = maxCooldown;
        currentSkillTime = 0;
        isActivate = false;

        onActivated();
    }


    /// <summary>
    /// ��ư�� Ȱ��ȭ�� �� ȣ��ȴ�.
    /// </summary>
    private void OnActivated()
    {
        isActivate = true;
        button.interactable = true;
        fill.transform.gameObject.SetActive(false);
        cooldownText.transform.gameObject.SetActive(false);
    }
    /// <summary>
    /// ��ư�� ��Ȱ��ȭ�� �� ȣ��ȴ�.
    /// </summary>
    private void OnDeactivated()
    {
        isActivate = false;
        button.interactable = false;
        fill.transform.gameObject.SetActive(true);
        cooldownText.transform.gameObject.SetActive(true);
    }

    /// <summary>
    /// ����ȿ���� ���� �� ��Ÿ�� ����� �Ѵ�.
    /// </summary>
    private void OnFinishedBuff()
    {
        onDeactivated();
        StartCoroutine(Cor_UpdateFillAmount());
    }

    /// <summary>
    /// ��Ÿ���� ���� �� ȣ��ȴ�.
    /// </summary>
    private void OnFinishedCooldown()
    {
        onActivated();
    }

    private void SubscribeToSkillButtonEvents()
    {
        onActivated += OnActivated;
        onDeactivated += OnDeactivated;
        onFinishedBuff += OnFinishedBuff;
        onFinishedCooldown += OnFinishedCooldown;
        button.interactable = (isActivate) ? true : false; 
    }
    private void UnsubscribeFromSkillButtonEvents()
    {
        onFinishedBuff -= OnFinishedBuff;
        onFinishedCooldown -= OnFinishedCooldown;
    }

    #endregion
}
