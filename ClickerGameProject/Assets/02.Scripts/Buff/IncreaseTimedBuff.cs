using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseTimedBuff : TimedBuff
{
    private Ability ability;

    public IncreaseTimedBuff(int id, float _duration, Ability _ability) : base(id,_duration)
    {
        ability = _ability;
    }

    /// <summary>
    /// ���� ȿ���� �����Ѵ�.
    /// </summary>
    protected override void ApplyEffect()
    {
        ability.m_increaseRate = 3; 
    }

    protected override void UpdateEffect(float delta)
    {
    }

    /// <summary>
    /// ������ ������.
    /// </summary>
    public override void End()
    {
        ability.m_increaseRate = 1;
    }
}

