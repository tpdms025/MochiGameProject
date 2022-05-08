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
    public override void LoadData(float _timeRemaining, float _buffTime)
    {
        bool _prevActivate;
        timeRemaining = GetRemainingTime(_timeRemaining, _buffTime, out _prevActivate);
        buffTime = _buffTime;
        prevActivate = _prevActivate;

        ChangeBuffState(_prevActivate);
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
    protected override IEnumerator Activate()
    {
        if (!prevActivate)
        {
            timeRemaining = buffTime;
        }
        else
        {
            prevActivate = false;
        }

        //����ȿ���� �ο��Ѵ�.
        if (onStartedBuff != null)
        {
            onStartedBuff.Invoke(timeRemaining);
        }

        circularImg.gameObject.SetActive(true);
        fill.transform.gameObject.SetActive(true);
        timeText.transform.gameObject.SetActive(true);
        button.interactable = false;


        yield return StartCoroutine(BuffTimer());

        //���� ȿ���� ������.
        if (onFinishedBuff != null)
        {
            onFinishedBuff.Invoke();
        }

        //������ ������.
        ChangeBuffState(false);
    }

    /// <summary>
    /// ������ ��Ȱ��ȭ�Ѵ�.
    /// </summary>
    protected override IEnumerator Deactivate()
    {
        DeactivateStateUI();
        yield return null;
    }





    /// <summary>
    /// ���� Ÿ�̸� UI�� �����Ѵ�.
    /// </summary>
    private IEnumerator BuffTimer()
    {
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
        //timeRemaining = buffTime;
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
    private void UpdateTimeText(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time + 1);    //�ø��� ���� +1
        timeText.text = string.Format("{0:mm\\:ss}", timeSpan);
    }

    #endregion
}
