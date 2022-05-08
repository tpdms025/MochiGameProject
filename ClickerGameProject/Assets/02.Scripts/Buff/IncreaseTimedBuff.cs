using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseTimedBuff : TimedBuff
{
    private ValueModifiers valueModifiers;

    public IncreaseTimedBuff(int id, float _duration, ValueModifiers valueModifiers) : base(id,_duration)
    {
        this.valueModifiers = valueModifiers;
    }

    /// <summary>
    /// 버프 효과를 적용한다.
    /// </summary>
    protected override void ApplyEffect()
    {
        valueModifiers.afterMult *= 3d;
    }

    protected override void UpdateEffect(float delta)
    {
    }

    /// <summary>
    /// 버프를 끝낸다.
    /// </summary>
    public override void End()
    {
        valueModifiers.afterMult /= 3d;
    }
}

