using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class SkillButton : MonoBehaviour
{
    protected enum SkillState {None, ApplyBuff, Cooldown};
    //private SkillState state;

    //�ִ� ��ٿ� �ð� (��)
    protected float maxCooldown;

    //���� ��ٿ� �ð� (��)
    protected float cooldownRemaining;

    //���� �ð� (��)
    protected float buffTime;

    //���� ���� �ð� (��)
    protected float buffTimeRemaining;

    //������ �ߵ��Ǿ����� Ȯ���ϴ� bool�� ����
    [SerializeField] protected bool isActivate;




    //������ �ߵ��� ���� �̺�Ʈ ��������Ʈ (float: ��� �ð�)
    public Action<float> onStartedBuff;

    //������ ���� ���� �̺�Ʈ ��������Ʈ
    public Action onFinishedBuff;


    //������ �ߵ���Ű�� ��ư
    [SerializeField]protected Button button;





    /// <summary>
    /// �����͸� �ε��Ͽ� �����Ѵ�.
    /// </summary>
    public abstract void LoadData(float _timeRemaining, float _buffTime, float _cooldownRemaining, float _maxCooldown);


    /// <summary>
    /// ȿ���� �ߵ��Ѵ�.
    /// </summary>
    protected abstract void TriggerEffect();




    /// <summary>
    /// UI�� ����.
    /// </summary>
    protected abstract void TurnOffUI();


    /// <summary>
    /// ������ ����Ѵ�.
    /// </summary>
    protected abstract IEnumerator BuffUsed();
  

    /// <summary>
    /// ��ų ��ٿ��� �Ѵ�.
    /// </summary>
    protected abstract IEnumerator SkillCooldown();
    


    protected void ChangeState(SkillState _state)
    {
        switch (_state)
        {
            case SkillState.None:
                TurnOffUI();
                break;

            case SkillState.ApplyBuff:
                StopCoroutine(BuffUsed());
                StartCoroutine(BuffUsed());
                break;

            case SkillState.Cooldown:
                StopCoroutine(SkillCooldown());
                StartCoroutine(SkillCooldown());
                break;
        }
    }


   

    /// <summary>
    /// ������ ���� �� ��Ÿ���� �ߵ������� �Ǵ��ϸ鼭,
    /// �������� �ð��� ���� �����ð��� ����Ѵ�.
    /// </summary>
    protected float GetRemainingTime(float prevRemainingTime, float maxTime)
    {
        //�����ð��� �ִ�ð��� ���ٸ� ������ �ߵ��� �ȵ� ������ ����
        if (prevRemainingTime == maxTime)
        {
            return maxTime;
        }

        double intervalTime = TimerManager.Instance.offlineTimeSpan.TotalSeconds;
        if (0 <= intervalTime && intervalTime <= prevRemainingTime)
        {
            return prevRemainingTime - (float)intervalTime;
        }
        else
        {
            return maxTime;
        }
    }

}
