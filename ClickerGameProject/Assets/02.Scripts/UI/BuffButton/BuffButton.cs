using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public abstract class BuffButton : MonoBehaviour
{
    //���� �ð� (��)
    [SerializeField] protected float buffTime;

    //���� ���� �ð� (��)
    public float timeRemaining;

    //������ �ߵ��Ǿ����� Ȯ���ϴ� bool�� ����
    protected bool isActivate;

    //������ ������ �ߵ��Ǿ����� Ȯ���ϴ� bool�� ����
    protected bool prevActivate;



    //������ �ߵ��� ���� �̺�Ʈ ��������Ʈ (float: ��� �ð�)
    public Action<float> onStartedBuff;

    //������ ���� ���� �̺�Ʈ ��������Ʈ
    public Action onFinishedBuff;



    //������ �ߵ���Ű�� ��ư
    [SerializeField] protected Button button;



    /// <summary>
    /// �����͸� �ε��Ͽ� �����Ѵ�.
    /// </summary>
    /// <param name="_timeRemaining"></param>
    /// <param name="_buffTime"></param>
    public abstract void LoadData(float _timeRemaining, float _buffTime);


    /// <summary>
    /// ȿ���� �ߵ��Ѵ�.
    /// </summary>
    protected abstract void TriggerEffect();

    /// <summary>
    /// ������ Ȱ��ȭ�Ѵ�.
    /// </summary>
    protected abstract IEnumerator Activate();

    /// <summary>
    /// ������ ��Ȱ��ȭ�Ѵ�.
    /// </summary>
    protected abstract IEnumerator Deactivate();

    /// <summary>
    /// ���� ���¸� �����Ѵ�.
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
    /// ������ ������ �ߵ������� �Ǵ��ϸ鼭,
    /// �������� �ð��� ���� �����ð��� ����Ѵ�.
    /// </summary>
    /// <returns></returns>
    protected float GetRemainingTime(float prevRemainingTime, float maxTime, out bool isPrevActivate)
    {
        //�����ð��� �ִ�ð��� ���ٸ� ������ �ߵ��� �ȵ� ������ ����
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
