using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticMiningBuff : TimedBuff
{
    private  Ability ability;
    private float time = 0.0f;
    public AutomaticMiningBuff(int id, float _duration, Ability _ability) : base(id, _duration)
    {
        ability = _ability;
    }

    /// <summary>
    /// 버프 효과를 적용한다.
    /// </summary>
    protected override void ApplyEffect()
    {
        DBManager.Inst.PlayerData.isAutoMining = true;
    }

    protected override void UpdateEffect(float delta)
    {
        time += delta;
        //1초당 3회씩 보석 지급
        if(time > 1.0f)
        {
            MoneyManager.Instance.AddJewel(ability.Value*3);
            time=0.0f;
        }
    }

    /// <summary>
    /// 버프를 끝낸다.
    /// </summary>
    public override void End()
    {
        DBManager.Inst.PlayerData.isAutoMining = false;
    }
}
