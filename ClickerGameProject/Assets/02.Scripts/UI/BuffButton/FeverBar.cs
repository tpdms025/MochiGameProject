using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FeverBar : BuffButton
{
    #region Fields

    //�ǹ� ȿ���� ������ ��������Ʈ 
    [SerializeField] private Sprite frameActivateSprite;

    //�⺻ ������ ��������Ʈ
    [SerializeField] private Sprite frameDeactivateSprite;

    //�ǹ� ȿ���� ������ ��������Ʈ 
    [SerializeField] private Sprite iconActivableIconSprite;

    //�⺻ ������ ��������Ʈ 
    [SerializeField] private Sprite iconDeactivateSprite;



    //������ �̹���
    [SerializeField] private Image frameImg;

    //������ �̹���
    [SerializeField] private Image iconImg;

    //�ǹ� �ߵ� ǥ�� �ؽ�Ʈ
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
    /// �����͸� �ε��Ͽ� �����Ѵ�.
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
    /// ȿ���� �ߵ��Ѵ�.
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
    /// ������ Ȱ��ȭ�Ѵ�.
    /// </summary>
    protected override void Activate()
    {
        TouchController.onUserTouched -= DeactivateStateUI;

        StartCoroutine(Cor_ActivateStateUI());
    }

    /// <summary>
    /// ������ ��Ȱ��ȭ�Ѵ�.
    /// </summary>
    protected override void Deactivate()
    {
        DeactivateStateUI(0,1);
        TouchController.onUserTouched += DeactivateStateUI;
    }




    /// <summary>
    /// ������ �ߵ��� ������ UI�� �����Ѵ�.
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

        //������ ������.
        ChangeBuffState(false);
    }

    /// <summary>
    /// ���� ��Ȱ��ȭ ������ UI�� �����Ѵ�.
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
            //���� �ߵ� ���ǿ� �ش�� ���
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
