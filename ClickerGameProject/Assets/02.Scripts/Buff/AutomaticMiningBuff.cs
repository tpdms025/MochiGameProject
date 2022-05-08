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
    /// ���� ȿ���� �����Ѵ�.
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
            //1�ʴ� nȸ�� ��ġ�� ȹ�淮��ŭ�� ���� ����
            if (time > 1.0f)
            {
                MoneyManager.Inst.SumJewel(valueModifiers.Value * bufferCnt);
                time = 0.0f;
            }
        }
    }

    /// <summary>
    /// ������ ������.
    /// </summary>
    public override void End()
    {
        DBManager.Inst.inventory.isAutoMining = false;
    }
}
