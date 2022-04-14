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

    //최대 쿨다운 시간 (초)
    protected float maxCooldown;

    //남은 쿨다운 시간 (초)
    protected float cooldownRemaining;

    //버프 시간 (초)
    protected float buffTime;

    //남은 버프 시간 (초)
    protected float buffTimeRemaining;

    //버프가 발동되었는지 확인하는 bool값 변수
    [SerializeField] protected bool isActivate;




    //버프가 발동할 때의 이벤트 델리게이트 (float: 경과 시간)
    public Action<float> onStartedBuff;

    //버프가 끝날 때의 이벤트 델리게이트
    public Action onFinishedBuff;


    //버프를 발동시키는 버튼
    [SerializeField]protected Button button;





    /// <summary>
    /// 데이터를 로드하여 세팅한다.
    /// </summary>
    public abstract void LoadData(float _timeRemaining, float _buffTime, float _cooldownRemaining, float _maxCooldown);


    /// <summary>
    /// 효과를 발동한다.
    /// </summary>
    protected abstract void TriggerEffect();




    /// <summary>
    /// UI를 끈다.
    /// </summary>
    protected abstract void TurnOffUI();


    /// <summary>
    /// 버프를 사용한다.
    /// </summary>
    protected abstract IEnumerator BuffUsed();
  

    /// <summary>
    /// 스킬 쿨다운을 한다.
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
    /// 이전에 버프 및 쿨타임이 발동중인지 판단하면서,
    /// 오프라인 시간을 비교해 남은시간을 계산한다.
    /// </summary>
    protected float GetRemainingTime(float prevRemainingTime, float maxTime)
    {
        //남은시간이 최대시간과 같다면 이전에 발동이 안된 것으로 간주
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
