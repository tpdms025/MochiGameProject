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
    /// 효과를 발동한다.
    /// </summary>
    protected override void TriggerEffect()
    {
        ChangeBuffState(true);
    }



    /// <summary>
    /// 버프를 활성화한다.
    /// </summary>
    protected override void Activate()
    {
        StartCoroutine(Cor_ActivateStateUI());
    }

    /// <summary>
    /// 버프를 비활성화한다.
    /// </summary>
    protected override void Deactivate()
    {
        DeactivateStateUI();
    }





    /// <summary>
    /// 버프 활성화 상태의 UI를 갱신한다.
    /// </summary>
    private IEnumerator Cor_ActivateStateUI()
    {
        circularImg.gameObject.SetActive(true);
        fill.transform.gameObject.SetActive(true);
        timeText.transform.gameObject.SetActive(true);
        button.interactable = false;

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

        timeRemaining = buffTime;

        //버프를 끝낸다.
        ChangeBuffState(false);
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
    /// <param name="time"></param>
    /// <param name="maxTime"></param>
    private void UpdateTimeText(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time + 1);    //올림을 위해 +1
        timeText.text = string.Format("{0:mm\\:ss}", timeSpan);
    }

    #endregion
}
