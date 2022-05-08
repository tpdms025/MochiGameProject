using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RateCalculator
{
    //증가율인지 감소율인지에 대한 bool값 변수
    public readonly bool isIncrease;

    //계산할 퍼센트 비율들
    protected Dictionary<string, float> rates;

    public RateCalculator(bool isIncrease, Dictionary<string, float> rates = null)
    {
        this.isIncrease = isIncrease;
        this.rates = rates;
    }
    public abstract float GetResultRate();

    public void AddRate(string key, float rate)
    {
        if (rates == null)
        {
            rates = new Dictionary<string, float>();
        }
        rates.Add(key, rate);
    }

    public void RemoveRate(string key)
    {
        if (rates == null || rates.Count == 0)
        {
            return;
        }
        if (rates.ContainsKey(key))
        {
            rates.Remove(key);
        }
    }

    public void RefreshRate(string key, float rate)
    {
        if (rates == null)
        {
            rates = new Dictionary<string, float>();
        }

        if (rates.ContainsKey(key))
        {
            rates[key] = rate;
        }
        else
        {
            AddRate(key, rate);
        }
    }

    public void RemoveAllRate()
    {
        if (rates == null || rates.Count == 0)
        {
            return;
        }
        rates.Clear();
    }
}
