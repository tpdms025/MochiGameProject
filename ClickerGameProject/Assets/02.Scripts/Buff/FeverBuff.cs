using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverBuff : TimedBuff
{
    private Ability ability;

    public FeverBuff(int id, float _duration, Ability _ability) : base(id, _duration)
    {
        ability = _ability;
    }

    /// <summary>
    /// ���� ȿ���� �����Ѵ�.
    /// </summary>
    protected override void ApplyEffect()
    {
        ability.m_increaseRate = 3;
        Debug.Log("�ǹ� 3�� ����");
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
        if (TouchController.onTouchReset != null)
        {
            TouchController.onTouchReset.Invoke();
        }
    }
}
