using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;



[System.Serializable]
public class ValueModifiers
{
    //�������� ��
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

    //���� ���� �� ȣ���ϴ� ��������Ʈ (double: ��ȭ��)
    public Action<double> onValueChanged;

    //���귮 ����/���� ����
    public RateCalculator rateCalc;

    //�Ŀ����� ���
    public double afterMult = 1;


    //��� ������ �� ���� ��
    public double Value
    {
        // �⺻ �� * ��ȭ��ġ ���� * n���
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
