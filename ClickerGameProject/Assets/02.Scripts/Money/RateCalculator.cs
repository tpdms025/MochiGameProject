using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RateCalculator
{
    //���������� ������������ ���� bool�� ����
    public readonly bool isIncrease;

    //����� �ۼ�Ʈ ������
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
