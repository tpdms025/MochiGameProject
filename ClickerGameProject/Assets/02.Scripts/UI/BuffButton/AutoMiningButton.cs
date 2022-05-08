using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoMiningButton : SkillButton
{
    //����� �ƿ������� ȸ�� �ӵ�
    private const float rotateSpeed = 30.0f;

 

    // ��ȭ��ġ�� ���� ���� ��
    // ���� ���ӽð�
    private float BuffTime
    {
        get { return buffTime * MoneyManager.Inst.autoMiningDuration.rateCalc.GetResultRate(); }
    }

    #region Fields

    //ä�� �̹���
    [SerializeField] private Image fill;

    //�ð� ǥ�� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI timeText;

    //���� �ƿ����� �̹���
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
    /// �����͸� �ε��Ͽ� �����Ѵ�.
    /// </summary>
    public override void LoadData(float _timeRemaining, float _buffTime, float _cooldownRemaining, float _maxCooldown)
    {
        buffTime = _buffTime;
        maxCooldown = _maxCooldown;

        //bool _prevBuffActivate;
        //bool _prevCooldownActivate;
        //buffTimeRemaining = GetRemainingTime(_timeRemaining, BuffTime, out _prevBuffActivate);
        //cooldownRemaining = GetRemainingTime(_cooldownRemaining, _maxCooldown, out _prevCooldownActivate);
        //prevActivate = _prevBuffActivate;

        bool _prevActivate;
        //������ �����ð��� ��Ÿ������ ���� ���¸� �����Ѵ�.
        SkillState state;

        float totalTime = GetRemainingTime(_timeRemaining + _cooldownRemaining, BuffTime + _maxCooldown, out _prevActivate);
        prevActivate = _prevActivate;
        if(_prevActivate)
        {
            if(totalTime > _cooldownRemaining)
            {
                buffTimeRemaining = totalTime - _cooldownRemaining;
                cooldownRemaining = _maxCooldown;
                state = SkillState.ApplyBuff;
            }
            else
            {
                buffTimeRemaining = 0f;
                cooldownRemaining = totalTime - _timeRemaining;
                state = SkillState.Cooldown;
            }
        }
        else
        {
            state = SkillState.None;
        }


        //if (_prevCooldownActivate)
        //    state = SkillState.Cooldown;
        //else if (_prevBuffActivate)
        //    state = SkillState.ApplyBuff;
        //else
        //    state = SkillState.None;

        //���� ����
        ChangeState(state);
    }

    #endregion




    #region Private Methods

    /// <summary>
    /// ȿ���� �ߵ��Ѵ�.
    /// </summary>
    protected override void TriggerEffect()
    {
        ChangeState(SkillState.ApplyBuff);
    }

    /// <summary>
    /// UI�� ����.
    /// </summary>
    protected override void TurnOffUI()
    {
        button.interactable = true;
        timeText.gameObject.SetActive(false);
        circularImg.gameObject.SetActive(false);
        fill.fillAmount = 0;

        //buffTimeRemaining = BuffTime;
    }

    /// <summary>
    /// ������ ����Ѵ�.
    /// </summary>
    protected override IEnumerator BuffUsed()
    {
        if(!prevActivate)
        {
            buffTimeRemaining = BuffTime;
        }
        else
        {
            prevActivate = false;
        }

        //����ȿ���� �ο��Ѵ�.
        if (onStartedBuff != null)
        {
            onStartedBuff.Invoke(buffTimeRemaining);
        }

        button.interactable = false;
        circularImg.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        fill.fillAmount = 1;

        yield return StartCoroutine(BuffTimer());

        //���� ȿ���� ������.
        if (onFinishedBuff != null)
        {
            onFinishedBuff.Invoke();
        }

        //���� ����
        ChangeState(SkillState.Cooldown);
    }

    /// <summary>
    /// ��ų ��ٿ��� �Ѵ�.
    /// </summary>
    protected override IEnumerator SkillCooldown()
    {
        button.interactable = false;
        circularImg.gameObject.SetActive(false);
        timeText.gameObject.SetActive(true);

        yield return StartCoroutine(CooldownTimer());

        //���� ����
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
        //buffTimeRemaining = BuffTime;
    }


    private IEnumerator CooldownTimer()
    {
        while (cooldownRemaining > 0)
        {
            UpdateFiilAmount(cooldownRemaining, maxCooldown);
            UpdateTimeText(cooldownRemaining);

            cooldownRemaining -= Time.deltaTime;
            yield return null;
        }
        cooldownRemaining = maxCooldown;
    }

    #endregion


    protected void UpdateFiilAmount(float time, float maxTime)
    {
        fill.fillAmount = time / maxTime;
    }

    protected void UpdateTimeText(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time + 1);    //�ø��� ���� +1
        timeText.text = string.Format("{0:mm\\:ss}", timeSpan);
    }

    protected void RotateCircularImg(float time)
    {
        circularImg.transform.Rotate(0, 0, time * rotateSpeed);
    }

    #endregion



}
