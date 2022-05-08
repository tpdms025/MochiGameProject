using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public abstract class BuffButton : MonoBehaviour
{
    //버프 시간 (초)
    [SerializeField] protected float buffTime;

    //남은 버프 시간 (초)
    public float timeRemaining;

    //버프가 발동되었는지 확인하는 bool값 변수
    protected bool isActivate;

    //버프가 이전에 발동되었는지 확인하는 bool값 변수
    protected bool prevActivate;



    //버프가 발동할 때의 이벤트 델리게이트 (float: 경과 시간)
    public Action<float> onStartedBuff;

    //버프가 끝날 때의 이벤트 델리게이트
    public Action onFinishedBuff;



    //버프를 발동시키는 버튼
    [SerializeField] protected Button button;



    /// <summary>
    /// 데이터를 로드하여 세팅한다.
    /// </summary>
    /// <param name="_timeRemaining"></param>
    /// <param name="_buffTime"></param>
    public abstract void LoadData(float _timeRemaining, float _buffTime);


    /// <summary>
    /// 효과를 발동한다.
    /// </summary>
    protected abstract void TriggerEffect();

    /// <summary>
    /// 버프를 활성화한다.
    /// </summary>
    protected abstract IEnumerator Activate();

    /// <summary>
    /// 버프를 비활성화한다.
    /// </summary>
    protected abstract IEnumerator Deactivate();

    /// <summary>
    /// 버프 상태를 변경한다.
    /// </summary>
    /// <param name="_isActivate"></param>
    protected virtual void ChangeBuffState(bool _isActivate)
    {
        isActivate = _isActivate;

        if (isActivate)
        {
            StartCoroutine(Activate());
        }
        else
        {
            StartCoroutine(Deactivate());
        }
    }


    /// <summary>
    /// 이전에 버프가 발동중인지 판단하면서,
    /// 오프라인 시간을 비교해 남은시간을 계산한다.
    /// </summary>
    /// <returns></returns>
    protected float GetRemainingTime(float prevRemainingTime, float maxTime, out bool isPrevActivate)
    {
        //남은시간이 최대시간과 같다면 이전에 발동이 안된 것으로 간주
        if (prevRemainingTime == maxTime || prevRemainingTime == 0)
        {
            isPrevActivate = false;
            return maxTime;
        }

        var intervalTime = TimerManager.Inst.offlineTimeSpan.TotalSeconds;
        if (0 <= intervalTime && intervalTime < prevRemainingTime)
        {
            isPrevActivate = true;
            return prevRemainingTime - (float)intervalTime;
        }
        else
        {
            isPrevActivate = false;
            return maxTime;
        }
    }
}
