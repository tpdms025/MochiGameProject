using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//합연산을 계산하는 클래스입니다.
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

        //증가율일 경우
        if(isIncrease)
        {
            foreach(var rate in rates.Values)
            {
                value += rate * 0.01f;
            }
        }
        //감소율일 경우
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
