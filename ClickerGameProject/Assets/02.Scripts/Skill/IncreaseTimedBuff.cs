using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseTimedBuff : TimedBuff
{
    public Ability ability;
    public IncreaseTimedBuff(int id, float _duration, Ability _ability) : base(id,_duration)
    {
        ability = _ability;
    }

    /// <summary>
    /// ���� ȿ���� �����Ѵ�.
    /// </summary>
    protected override void ApplyEffect()
    {
        ability.m_increaseMult = 3; 
    }

    /// <summary>
    /// ������ ������.
    /// </summary>
    public override void End()
    {
        ability.m_increaseMult = 1;
    }
}

