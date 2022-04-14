using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuffCircularBar : BuffButton
{
    //����� �ƿ������� ȸ�� �ӵ�
    private const float rotateSpeed = 30.0f;


    #region Fields
    //ä�� �̹���
    [SerializeField] private Image fill;

    //���� �ð� ǥ�� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI timeText;

    //���� �ƿ����� �̹���
    [SerializeField] private Image circularImg;


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
        //SubscribeToSkillButtonEvents();
    }

    private void OnDisable()
    {
        popup.onAdsFinished -= TriggerEffect;
        //UnsubscribeFromSkillButtonEvents();
    }
    #endregion


    #region Methods

    /// <summary>
    /// �����͸� �ε��Ͽ� �����Ѵ�.
    /// </summary>
    /// <param name="_timeRemaining"></param>
    /// <param name="_buffTime"></param>
    public override void LoadData(float _timeRemaining, float _buffTime)
    {
        timeRemaining = GetRemainingTime(_timeRemaining, _buffTime);
        buffTime = _buffTime;
        bool prevActivate = (timeRemaining == buffTime) ? false : true;

        ChangeBuffState(prevActivate);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// ȿ���� �ߵ��Ѵ�.
    /// </summary>
    protected override void TriggerEffect()
    {
        ChangeBuffState(true);
    }



    /// <summary>
    /// ������ Ȱ��ȭ�Ѵ�.
    /// </summary>
    protected override void Activate()
    {
        StartCoroutine(Cor_ActivateStateUI());
    }

    /// <summary>
    /// ������ ��Ȱ��ȭ�Ѵ�.
    /// </summary>
    protected override void Deactivate()
    {
        DeactivateStateUI();
    }





    /// <summary>
    /// ���� Ȱ��ȭ ������ UI�� �����Ѵ�.
    /// </summary>
    private IEnumerator Cor_ActivateStateUI()
    {
        circularImg.gameObject.SetActive(true);
        fill.transform.gameObject.SetActive(true);
        timeText.transform.gameObject.SetActive(true);
        button.interactable = false;

        //�����ð����� UI�� �����Ѵ�.
        while (timeRemaining >= 0)
        {
            //�ƿ����� ȸ��
            circularImg.transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);
            //circularImage.transform.rotation = Quaternion.Slerp(circularImage.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime);

            //���� �ð� ǥ��
            UpdateTimeText(timeRemaining);

            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        timeRemaining = buffTime;

        //������ ������.
        ChangeBuffState(false);
    }

    /// <summary>
    /// ���� ��Ȱ��ȭ ������ UI�� �����Ѵ�.
    /// </summary>
    private void DeactivateStateUI()
    {
        circularImg.gameObject.SetActive(false);
        fill.transform.gameObject.SetActive(false);
        timeText.transform.gameObject.SetActive(false);
        button.interactable = true;
    }




    /// <summary>
    /// �ð� �ؽ�Ʈ�� �����Ѵ�.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="maxTime"></param>
    private void UpdateTimeText(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time + 1);    //�ø��� ���� +1
        timeText.text = string.Format("{0:mm\\:ss}", timeSpan);
    }

    #endregion
}
