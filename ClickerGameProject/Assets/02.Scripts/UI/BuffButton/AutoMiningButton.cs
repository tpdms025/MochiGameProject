using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoMiningButton : SkillButton
{
    //노란색 아웃라인의 회전 속도
    private const float rotateSpeed = 30.0f;


    #region Fields

    //채울 이미지
    [SerializeField] private Image fill;

    //시간 표시 텍스트
    [SerializeField] private TextMeshProUGUI timeText;

    //원형 아웃라인 이미지
    [SerializeField] private Image circularImg;

    private Transform mochiKing;

    #endregion



    #region Unity methods

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TriggerEffect);

        mochiKing = GameObject.FindGameObjectWithTag("MochiKing").transform;
        mochiKing.gameObject.SetActive(false);
        onStartedBuff += (t) => mochiKing.gameObject.SetActive(true);
        onFinishedBuff += () => mochiKing.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        onStartedBuff -= (t) => mochiKing.gameObject.SetActive(true);
        onFinishedBuff -= () => mochiKing.gameObject.SetActive(false);
    }

    #endregion




    #region Methods

    /// <summary>
    /// 데이터를 로드하여 세팅한다.
    /// </summary>
    public override void LoadData(float _timeRemaining, float _buffTime, float _cooldownRemaining, float _maxCooldown)
    {
        buffTime = _buffTime;
        maxCooldown = _maxCooldown;
        buffTimeRemaining = GetRemainingTime(_timeRemaining, _buffTime);
        cooldownRemaining = GetRemainingTime(_cooldownRemaining, _maxCooldown);
        bool prevBuffActivate = (buffTimeRemaining == buffTime) ? false : true;
        bool prevCooldownActivate = (_cooldownRemaining == _maxCooldown) ? false : true;

        //버프의 남은시간과 쿨타임으로 현재 상태를 설정한다.
        SkillState state;
        if (prevBuffActivate)
            state = SkillState.ApplyBuff;
        else if (prevCooldownActivate)
            state = SkillState.Cooldown;
        else
            state = SkillState.None;

        //상태 변경
        ChangeState(state);
    }

    #endregion




    #region Private Methods

    /// <summary>
    /// 효과를 발동한다.
    /// </summary>
    protected override void TriggerEffect()
    {
        ChangeState(SkillState.ApplyBuff);
    }

    /// <summary>
    /// UI를 끈다.
    /// </summary>
    protected override void TurnOffUI()
    {
        button.interactable = true;
        timeText.gameObject.SetActive(false);
        circularImg.gameObject.SetActive(false);
        fill.fillAmount = 0;
    }

    /// <summary>
    /// 버프를 사용한다.
    /// </summary>
    protected override IEnumerator BuffUsed()
    {
        //버프효과를 부여한다.
        if (onStartedBuff != null)
        {
            onStartedBuff.Invoke(buffTimeRemaining);
        }

        button.interactable = false;
        circularImg.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        fill.fillAmount = 1;

        yield return StartCoroutine(BuffTimer());

        //버프 효과를 끝낸다.
        if (onFinishedBuff != null)
        {
            onFinishedBuff.Invoke();
        }

        //상태 변경
        ChangeState(SkillState.Cooldown);
    }

    /// <summary>
    /// 스킬 쿨다운을 한다.
    /// </summary>
    protected override IEnumerator SkillCooldown()
    {
        button.interactable = false;
        circularImg.gameObject.SetActive(false);
        timeText.gameObject.SetActive(true);

        yield return StartCoroutine(CooldownTimer());

        //상태 변경
        ChangeState(SkillState.None);
    }



    #region Timer



    private IEnumerator BuffTimer()
    {
        while (buffTimeRemaining > 0)
        {
            UpdateTimeText(buffTimeRemaining);
            RotateCircularImg(Time.deltaTime);

            buffTimeRemaining -= Time.deltaTime;
            yield return null;
        }
        buffTimeRemaining = buffTime;
    }


    private IEnumerator CooldownTimer()
    {
        while (cooldownRemaining > 0)
        {
            UpdateFiilAmount(cooldownRemaining);
            UpdateTimeText(cooldownRemaining);

            cooldownRemaining -= Time.deltaTime;
            yield return null;
        }
        cooldownRemaining = maxCooldown;
    }

    #endregion


    protected void UpdateFiilAmount(float time)
    {
        fill.fillAmount = time / maxCooldown;
    }

    protected void UpdateTimeText(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time + 1);    //올림을 위해 +1
        timeText.text = string.Format("{0:mm\\:ss}", timeSpan);
    }

    protected void RotateCircularImg(float time)
    {
        circularImg.transform.Rotate(0, 0, time * rotateSpeed);
    }

    #endregion



}
