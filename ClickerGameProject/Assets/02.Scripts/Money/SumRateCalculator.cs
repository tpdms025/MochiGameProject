using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�տ����� ����ϴ� Ŭ�����Դϴ�.
public class SumRateCalculator : RateCalculator
{
    public SumRateCalculator(bool isIncrease, Dictionary<string, float> rates) : base(isIncrease,rates)
    {
    }

    public override float GetResultRate()
    {
        float value = 1;

        if (rates == null)
        {
            return value;
        }

        //�������� ���
        if(isIncrease)
        {
            foreach(var rate in rates.Values)
            {
                value += rate * 0.01f;
            }
        }
        //�������� ���
        else
        {
            foreach (var rate in rates.Values)
            {
                value -= rate * 0.01f;
            }
            value = 1f - value;
        }

        return value;
    }

}
