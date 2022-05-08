using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuffCircularBar : BuffButton
{
    //노란색 아웃라인의 회전 속도
    private const float rotateSpeed = 30.0f;


    #region Fields
    //채울 이미지
    [SerializeField] private Image fill;

    //남은 시간 표시 텍스트
    [SerializeField] private TextMeshProUGUI timeText;

    //원형 아웃라인 이미지
    [SerializeField] private Image circularImg;


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
    /// 데이터를 로드하여 세팅한다.
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
    /// 효과를 발동한다.
    /// </summary>
    protected override void TriggerEffect()
    {
        ChangeBuffState(true);
    }



    /// <summary>
    /// 버프를 활성화한다.
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

        //버프효과를 부여한다.
        if (onStartedBuff != null)
        {
            onStartedBuff.Invoke(timeRemaining);
        }

        circularImg.gameObject.SetActive(true);
        fill.transform.gameObject.SetActive(true);
        timeText.transform.gameObject.SetActive(true);
        button.interactable = false;


        yield return StartCoroutine(BuffTimer());

        //버프 효과를 끝낸다.
        if (onFinishedBuff != null)
        {
            onFinishedBuff.Invoke();
        }

        //버프를 끝낸다.
        ChangeBuffState(false);
    }

    /// <summary>
    /// 버프를 비활성화한다.
    /// </summary>
    protected override IEnumerator Deactivate()
    {
        DeactivateStateUI();
        yield return null;
    }





    /// <summary>
    /// 버프 타이머 UI를 갱신한다.
    /// </summary>
    private IEnumerator BuffTimer()
    {
        //버프시간동안 UI를 갱신한다.
        while (timeRemaining >= 0)
        {
            //아웃라인 회전
            circularImg.transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);
            //circularImage.transform.rotation = Quaternion.Slerp(circularImage.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime);

            //남은 시간 표시
            UpdateTimeText(timeRemaining);

            timeRemaining -= Time.deltaTime;
            yield return null;
        }
        //timeRemaining = buffTime;
    }

    /// <summary>
    /// 버프 비활성화 상태의 UI를 갱신한다.
    /// </summary>
    private void DeactivateStateUI()
    {
        circularImg.gameObject.SetActive(false);
        fill.transform.gameObject.SetActive(false);
        timeText.transform.gameObject.SetActive(false);
        button.interactable = true;
    }




    /// <summary>
    /// 시간 텍스트를 갱신한다.
    /// </summary>
    private void UpdateTimeText(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time + 1);    //올림을 위해 +1
        timeText.text = string.Format("{0:mm\\:ss}", timeSpan);
    }

    #endregion
}
