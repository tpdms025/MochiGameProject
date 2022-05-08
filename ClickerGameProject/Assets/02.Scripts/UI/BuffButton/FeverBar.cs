using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FeverBar : BuffButton
{


    // ��ȭ��ġ�� ���� ���� ��
    // ���� ���ӽð�
    private float BuffTime
    {
        get { return buffTime * MoneyManager.Inst.feverDuration.rateCalc.GetResultRate(); }
    }



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
        TouchController.onUserTouched += DeactivateStateUI;
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(TriggerEffect);
        TouchController.onUserTouched -= DeactivateStateUI;
    }
    #endregion


    /// <summary>
    /// �����͸� �ε��Ͽ� �����Ѵ�.
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
    /// ȿ���� �ߵ��Ѵ�.
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
    /// ������ Ȱ��ȭ�Ѵ�.
    /// </summary>
    protected override IEnumerator Activate()
    {
        //�ߵ����� �����ð��� �����صд�.
        //(�ߵ��߿� ��ȭ�� �Ҷ� �ǽð����� ����Ǵ°��� �������� ����)
        float buffTime_buffer = BuffTime;

        if (!prevActivate)
        {
            timeRemaining = BuffTime;
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

        frameImg.sprite = frameActivateSprite;
        iconImg.sprite = iconActivableIconSprite;
        touchText.gameObject.SetActive(false);
        button.enabled = false;

        yield return StartCoroutine(BuffTimer(buffTime_buffer));

        //���� ȿ���� ������.
        if (onFinishedBuff != null)
        {
            onFinishedBuff.Invoke();
        }

        //������ ������ ���� �����Ѵ�.
        ChangeBuffState(false);

    }

    /// <summary>
    /// ������ ��Ȱ��ȭ�Ѵ�.
    /// </summary>
    protected override IEnumerator Deactivate()
    {
        DeactivateStateUI(DBManager.Inst.inventory.touchCount, TouchController.maxTouchCount);
            yield return null;
    }




    /// <summary>
    /// ���� Ÿ�̸� UI�� �����Ѵ�.
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
