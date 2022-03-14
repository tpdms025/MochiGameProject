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
    /// 버프 효과를 적용한다.
    /// </summary>
    protected override void ApplyEffect()
    {
        ability.m_increaseMult = 3; 
    }

    /// <summary>
    /// 버프를 끝낸다.
    /// </summary>
    public override void End()
    {
        ability.m_increaseMult = 1;
    }
}

