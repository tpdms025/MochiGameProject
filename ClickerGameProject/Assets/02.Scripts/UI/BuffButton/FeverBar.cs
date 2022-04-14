using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FeverBar : BuffButton
{
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
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(TriggerEffect);
    }
    #endregion


    /// <summary>
    /// 데이터를 로드하여 세팅한다.
    /// </summary>
    /// <param name="_duration"></param>
    /// <param name="_timeRemaining"></param>
    public override void LoadData(float _timeRemaining, float _buffTime)
    {
        timeRemaining = GetRemainingTime(_timeRemaining, _buffTime);
        buffTime = _buffTime;
        bool prevActivate = (timeRemaining == buffTime) ? false : true;

        ChangeBuffState(prevActivate);
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
        if (Ore.onOreChanged != null)
        {
            Ore.onOreChanged(_isActivate);
        }
    }



    /// <summary>
    /// 버프를 활성화한다.
    /// </summary>
    protected override void Activate()
    {
        TouchController.onUserTouched -= DeactivateStateUI;

        StartCoroutine(Cor_ActivateStateUI());
    }

    /// <summary>
    /// 버프를 비활성화한다.
    /// </summary>
    protected override void Deactivate()
    {
        DeactivateStateUI(0,1);
        TouchController.onUserTouched += DeactivateStateUI;
    }




    /// <summary>
    /// 버프를 발동한 상태의 UI를 갱신한다.
    /// </summary>
    private IEnumerator Cor_ActivateStateUI()
    {
        frameImg.sprite = frameActivateSprite;
        iconImg.sprite = iconActivableIconSprite;
        touchText.gameObject.SetActive(false);
        button.enabled = false;

        while (timeRemaining >= 0)
        {
            UpdateFiilAmount(timeRemaining, buffTime);
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
