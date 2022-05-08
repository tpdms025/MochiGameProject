using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FeverBar : BuffButton
{


    // 강화수치를 곱한 최종 값
    // 버프 지속시간
    private float BuffTime
    {
        get { return buffTime * MoneyManager.Inst.feverDuration.rateCalc.GetResultRate(); }
    }



    #region Fields

    //피버 효과의 프레임 스프라이트 
    [SerializeField] private Sprite frameActivateSprite;

    //기본 프레임 스프라이트
    [SerializeField] private Sprite frameDeactivateSprite;

    //피버 효과의 아이콘 스프라이트 
    [SerializeField] private Sprite iconActivableIconSprite;

    //기본 아이콘 스프라이트 
    [SerializeField] private Sprite iconDeactivateSprite;



    //프레임 이미지
    [SerializeField] private Image frameImg;

    //아이콘 이미지
    [SerializeField] private Image iconImg;

    //피버 발동 표시 텍스트
    [SerializeField] private TextMeshProUGUI touchText;

    #endregion

    #region Unity methods


    private void OnEnable()
    {
        button.onClick.AddListener(TriggerEffect);
        TouchController.onUserTouched += DeactivateStateUI;
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(TriggerEffect);
        TouchController.onUserTouched -= DeactivateStateUI;
    }
    #endregion


    /// <summary>
    /// 데이터를 로드하여 세팅한다.
    /// </summary>
    public override void LoadData(float _timeRemaining, float _buffTime)
    {
        bool _prevActivate;
        buffTime = _buffTime;
        timeRemaining = GetRemainingTime(_timeRemaining, BuffTime, out _prevActivate);
        prevActivate = _prevActivate;

        base.ChangeBuffState(_prevActivate);
    }

    #region Private Methods

    /// <summary>
    /// 효과를 발동한다.
    /// </summary>
    protected override void TriggerEffect()
    {
        ChangeBuffState(true);
    }

    protected override void ChangeBuffState(bool _isActivate)
    {
        base.ChangeBuffState(_isActivate);
        if (OreWorld.onOreChanged != null)
        {
            OreWorld.onOreChanged(_isActivate);
        }
    }



    /// <summary>
    /// 버프를 활성화한다.
    /// </summary>
    protected override IEnumerator Activate()
    {
        //발동전의 버프시간을 저장해둔다.
        //(발동중에 강화를 할때 실시간으로 적용되는것을 막기위한 변수)
        float buffTime_buffer = BuffTime;

        if (!prevActivate)
        {
            timeRemaining = BuffTime;
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

        frameImg.sprite = frameActivateSprite;
        iconImg.sprite = iconActivableIconSprite;
        touchText.gameObject.SetActive(false);
        button.enabled = false;

        yield return StartCoroutine(BuffTimer(buffTime_buffer));

        //버프 효과를 끝낸다.
        if (onFinishedBuff != null)
        {
            onFinishedBuff.Invoke();
        }

        //버프를 끝내고 상태 변경한다.
        ChangeBuffState(false);

    }

    /// <summary>
    /// 버프를 비활성화한다.
    /// </summary>
    protected override IEnumerator Deactivate()
    {
        DeactivateStateUI(DBManager.Inst.inventory.touchCount, TouchController.maxTouchCount);
            yield return null;
    }




    /// <summary>
    /// 버프 타이머 UI를 갱신한다.
    /// </summary>
    private IEnumerator BuffTimer(float buffTime_buffer)
    {
        while (timeRemaining >= 0)
        {
            UpdateFiilAmount(timeRemaining, buffTime_buffer);
            timeRemaining -= Time.deltaTime;
            yield return null;
        }
        //timeRemaining = BuffTime;
    }

    /// <summary>
    /// 버프 비활성화 상태의 UI를 갱신한다.
    /// </summary>
    private void DeactivateStateUI(int totalCount, int maxCount)
    {
        if (totalCount < maxCount)
        {
            frameImg.sprite = frameDeactivateSprite;
            iconImg.sprite = iconDeactivateSprite;
            touchText.gameObject.SetActive(false);
            button.enabled = false;

            UpdateFiilAmount((float)totalCount, maxCount);
        }
        else
        {
            //버프 발동 조건에 해당될 경우
            frameImg.sprite = frameActivateSprite;
            iconImg.sprite = iconActivableIconSprite;
            touchText.gameObject.SetActive(true);
            button.enabled = true;

            frameImg.fillAmount = 1f;
        }
    }



    private void UpdateFiilAmount(float time,float maxTime)
    {
        frameImg.fillAmount = time / maxTime;
    }

    #endregion
}
