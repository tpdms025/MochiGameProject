using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedBuff 
{
    public int Id;
    public bool isFinished { get; protected set; }
    protected float duration;


    #region Methods

    public TimedBuff(int id, float _duration)
    {
        Id = id;
        duration = _duration;
        if(duration <= 0)
        {
            isFinished = false;
        }
    }

    public void Tick(float delta)
    {
        duration -= delta;
        if(duration <= 0)
        {
            End();
            isFinished = true;
        }
    }

    /// <summary>
    /// ������ �ߵ���Ų��.
    /// </summary>
    public void Activate()
    {
        ApplyEffect();
    }



    /// <summary>
    /// ���� ȿ���� �����Ѵ�.
    /// </summary>
    protected abstract void ApplyEffect();

    /// <summary>
    /// ������ ������.
    /// </summary>
    public abstract void End();
  
    #endregion

}
