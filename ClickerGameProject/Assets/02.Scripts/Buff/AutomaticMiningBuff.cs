using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticMiningBuff : TimedBuff
{
    private ValueModifiers valueModifiers;
    private float time = 0.0f;
    private double bufferCnt;
    public AutomaticMiningBuff(int id, float _duration, ValueModifiers valueModifiers) : base(id, _duration)
    {
        this.valueModifiers = valueModifiers;
    }

    /// <summary>
    /// 버프 효과를 적용한다.
    /// </summary>
    protected override void ApplyEffect()
    {
        DBManager.Inst.inventory.isAutoMining = true;
        bufferCnt = MoneyManager.Inst.autoMiningTouchCnt.Value;
    }

    protected override void UpdateEffect(float delta)
    {
        if (!DBManager.Inst.isGameStop)
        {
            time += delta;
            //1초당 n회씩 터치당 획득량만큼의 보석 지급
            if (time > 1.0f)
            {
                MoneyManager.Inst.SumJewel(valueModifiers.Value * bufferCnt);
                time = 0.0f;
            }
        }
    }

    /// <summary>
    /// 버프를 끝낸다.
    /// </summary>
    public override void End()
    {
        DBManager.Inst.inventory.isAutoMining = false;
    }
}
