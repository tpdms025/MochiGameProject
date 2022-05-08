using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;



[System.Serializable]
public class ValueModifiers
{
    //원초적인 값
    private double m_value;
    public double baseValue
    {
        get { return m_value; }
        private set
        {
            m_value = value;
            if (onValueChanged != null)
            {
                onValueChanged(m_value);
            }
        }
    }

    //값이 변할 때 호출하는 델리게이트 (double: 변화값)
    public Action<double> onValueChanged;

    //생산량 증가/감소 비율
    public RateCalculator rateCalc;

    //후연산의 배수
    public double afterMult = 1;


    //모든 연산을 한 최종 값
    public double Value
    {
        // 기본 값 * 강화수치 비율 * n배수
        get { return m_value * rateCalc.GetResultRate()* afterMult; }
    }



    public ValueModifiers(double value, bool isIncrease,bool isMult, Dictionary<string, float> rates = null)
    {
        m_value = value;
        if(isMult)
            rateCalc = new MultRateCalculator(isIncrease,rates);
        else
            rateCalc = new SumRateCalculator(isIncrease, rates);

    }


    public void SumValue(double _newValue)
    {
        baseValue += _newValue;
    }

    public void SubValue(double _newValue)
    {
        baseValue -= _newValue;
    }

    public void InitValue()
    {
        InitBaseValue();
        rateCalc.RemoveAllRate();
    }


    public void InitBaseValue()
    {
        m_value = 0;
    }
    public void SetBaseValue(double _newValue)
    {
        m_value = _newValue;
    }

}
