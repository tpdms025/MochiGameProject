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
    /// 버프 효과를 적용한다.
    /// </summary>
    protected override void ApplyEffect()
    {
        ability.m_increaseRate = 3; 
    }

    protected override void UpdateEffect(float delta)
    {
    }

    /// <summary>
    /// 버프를 끝낸다.
    /// </summary>
    public override void End()
    {
        ability.m_increaseRate = 1;
    }
}

