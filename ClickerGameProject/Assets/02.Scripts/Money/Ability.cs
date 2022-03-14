using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;

public class Ability
{
    //원초적인 값
    private BigInteger m_value;

    //생산량을 n배 증가
    public int m_increaseMult = 1;

    //값이 변할 때 호출하는 델리게이트
    public event Action<BigInteger> onValueChanged;

    public BigInteger Value
    {
        get { return m_value * m_increaseMult; }
        set 
        { 
            m_value = value;

            if (onValueChanged != null)
            {
                onValueChanged(m_value);
            }
        }
    }

    public Ability(BigInteger value, int increaseMult = 1)
    {
        m_value = value;
        m_increaseMult = increaseMult;
    }


    public void AddValue(BigInteger _newValue)
    {
        Value += _newValue;
    }

    public void SubValue(BigInteger _newValue)
    {
        Value -= _newValue;
    }

    public void InitValue()
    {
        Value = 0;
    }
}
