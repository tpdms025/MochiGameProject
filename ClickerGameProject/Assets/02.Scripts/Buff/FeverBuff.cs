using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverBuff : TimedBuff
{
    private ValueModifiers valueModifiers;
    private double bufferMult; 

    public FeverBuff(int id, float _duration, ValueModifiers valueModifiers) : base(id, _duration)
    {
        this.valueModifiers = valueModifiers;
    }

    /// <summary>
    /// 버프 효과를 적용한다.
    /// </summary>
    protected override void ApplyEffect()
    {
        bufferMult = MoneyManager.Inst.feverAmount.Value;
        valueModifiers.afterMult *= bufferMult;
    }

    protected override void UpdateEffect(float delta)
    {
    }

    /// <summary>
    /// 버프를 끝낸다.
    /// </summary>
    public override void End()
    {
        valueModifiers.afterMult /= bufferMult;
        if (TouchController.onTouchReset != null)
        {
            TouchController.onTouchReset.Invoke();
        }
    }
}
