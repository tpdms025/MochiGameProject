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
    [SerializeField] protected float timeRemaining;

    //������ �ߵ��Ǿ����� Ȯ���ϴ� bool�� ����
    [SerializeField] protected bool isActivate;




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
    protected abstract void Activate();

    /// <summary>
    /// ������ ��Ȱ��ȭ�Ѵ�.
    /// </summary>
    protected abstract void Deactivate();

    /// <summary>
    /// ���� ���¸� �����Ѵ�.
    /// </summary>
    /// <param name="_isActivate"></param>
    protected virtual void ChangeBuffState(bool _isActivate)
    {
        isActivate = _isActivate;

        if (isActivate)
        {
            //����ȿ���� �ο��Ѵ�.
            if (onStartedBuff != null)
            {
                onStartedBuff.Invoke(timeRemaining);
            }
            Activate();
        }
        else
        {
            //���� ȿ���� ������.
            if (onFinishedBuff != null)
            {
                onFinishedBuff.Invoke();
            }
            Deactivate();
        }
    }


    /// <summary>
    /// ������ ������ �ߵ������� �Ǵ��ϸ鼭,
    /// �������� �ð��� ���� �����ð��� ����Ѵ�.
    /// </summary>
    /// <returns></returns>
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
