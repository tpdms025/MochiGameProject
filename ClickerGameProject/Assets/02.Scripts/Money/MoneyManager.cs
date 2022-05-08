using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    #region Data

    //�����ϰ� �ִ� ����
    private ValueModifiers jewel;

    //�����ϰ� �ִ� ����
    private ValueModifiers coin;

    // ��ġ�� ȹ�淮
    private ValueModifiers totalJewelPerTouch;

    // �ʴ� ȹ�淮
    private ValueModifiers totalJewelPerSec;
    


    //���� ������ ���ڿ�
    public string strJewel;

    //���� ������ ���ڿ�
    public string strCoin;

    //��ġ�� �������� ���ڿ�
    public string strJewelPerTouch;

    //�ʴ� �������� ���ڿ�
    public string strJewelPerSec;


    //==============Ability================
    // �ϲ� ���귮 (Rate�� ����ϱ�)
    public List<ValueModifiers> workmansAmount;

    //�ڵ�ä���� ���ӽð� (Rate�� ����ϱ�)
    public ValueModifiers autoMiningDuration;   

    //�ڵ�ä���� ��ġȽ��
    public ValueModifiers autoMiningTouchCnt;

    //�ǹ� ���ӽð� (Rate�� ����ϱ�)
    public ValueModifiers feverDuration;   

    //�ǹ� ������ ȹ�淮
    public ValueModifiers feverAmount;
    //======================================

    #endregion

    #region Property


    /// <summary>
    /// m_jewel�� ������Ƽ
    /// </summary>
    public ValueModifiers Jewel
    {
        get { return jewel; }
    }

    /// <summary>
    /// m_coin�� ������Ƽ
    /// </summary>
    public ValueModifiers Coin
    {
        get { return coin; }
    }

    /// <summary>
    /// m_jewelPerTouch�� ������Ƽ
    /// </summary>
    public ValueModifiers JewelPerTouch
    {
        get { return totalJewelPerTouch; }
    }

    /// <summary>
    /// m_jewelPerSec�� ������Ƽ
    /// </summary>
    public ValueModifiers JewelPerSec
    {
        get { return totalJewelPerSec; }
    }


    #endregion

    #region Fields

    private static MoneyManager instance = null;
    public static MoneyManager Inst
    {
        get { return instance; }
        set { instance = value;  }
    }

    #endregion

    #region Unity methods

    private void Awake()
    {
        //�̱���
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(Loop_IncreaseJewelPerSec());
    }

    #endregion

    public void SetMoneyData(double _jewel, double _coin, double _jewelPerTouch, double _jewelPerSec)
    {
        jewel = new ValueModifiers(_jewel, true, true);
        coin = new ValueModifiers(_coin, true, true);
        totalJewelPerTouch = new ValueModifiers(_jewelPerTouch, true, true);
        totalJewelPerSec = new ValueModifiers(_jewelPerSec, true, true);
    }

    public void SetAbilityData(int[] levelList)
    {
        //��ȭ ����
        workmansAmount = new List<ValueModifiers>();
        for (int i = 0; i < levelList.Length; i++)
        {
            double amount;
            int level = levelList[i];
            if(level == 0)
            {
                amount = 0d;
            }
            amount = DBManager.Inst.GetWorkmanOriginData(i, level).amount;
            workmansAmount.Add(new ValueModifiers(amount, true, true));
        }
        autoMiningTouchCnt = new ValueModifiers(3, true, true);
        autoMiningDuration = new ValueModifiers(60, true, true);
        feverAmount = new ValueModifiers(3, true, true);
        feverDuration = new ValueModifiers(5, true, true);

    }

    public void InitializeAll()
    {
        InitJewel();
        InitCoin();
        InitJewelPerTouch();
        InitJewelPerSec();
    }


    /// <summary>
    /// ���� ��ȭ�� 0���� ȯ���� �ʱ�ȭ�Ѵ�.
    /// </summary>
    public void ResetOnPrestige()
    {
        //������ ����
        InitJewel();
        InitJewelPerTouch();
        InitJewelPerSec();

        InitAbility();
    }

    public void InitAbility()
    {
        InitWorkmansEnhance();
        InitAutoMiningDurationEnhance();
        InitAutoMiningTouchCntEnhance();
        InitFeverAmountEnhance();
        InitFeverDurationEnhance();
    }


    #region Methods

    #region Modify Jewel Function

    public void SumJewel(double _newValue)
    {
        jewel.SumValue(_newValue);
    }

    public void SubJewel(double _newValue)
    {
        jewel.SubValue(_newValue);
    }

    public void InitJewel()
    {
        jewel.InitValue();
    }


    #endregion

    #region Modify JewelPerTouch Function

    public void SumJewelPerTouch(double _newValue)
    {
        totalJewelPerTouch.SumValue(_newValue);
    }

    public void SubJewelPerTouch(double _newValue)
    {
        totalJewelPerTouch.SubValue(_newValue);
    }

    public void InitJewelPerTouch()
    {
        //1���� ������ �ʱ�ȭ
        totalJewelPerTouch.InitValue();
        //totalJewelPerTouch.SumValue(2);
        totalJewelPerTouch.SumValue(DBManager.Inst.GetOreOriginData(0, 0).amount);
    }


    #endregion

    #region Modify JewelPerSec Function

    public void SumJewelPerSec(double _newValue)
    {
        totalJewelPerSec.SumValue(_newValue);
    }

    public void SubJewelPerSec(double _newValue)
    {
        totalJewelPerSec.SubValue(_newValue);
    }

    public void InitJewelPerSec()
    {
        totalJewelPerSec.InitValue();
    }

    #endregion

    #region Modify Coin Function

    public void SumCoin(double _newValue)
    {
        coin.SumValue(_newValue);
    }

    public void SubCoin(double _newValue)
    {
        coin.SubValue(_newValue);
    }

    public void InitCoin()
    {
        coin.InitValue();
    }

    #endregion

    #region Workman Enhance Function
    private void InitWorkmansEnhance()
    {
        foreach(var workman in workmansAmount)
        {
            workman.InitValue();
        }
    }
    
    /// <summary>
    /// ��� �ϲ� ���� �ʴ�ȹ�淮�� ���Ѵ�.
    /// </summary>
    public void SumAllWorkmanAmountToJewelPerSec()
    {
        totalJewelPerSec.InitBaseValue();
        for(int i=0; i<workmansAmount.Count; i++)
        {
            SumJewelPerSec(workmansAmount[i].Value);
        }
    }
    #endregion

    #region Automatic Mining Enhance Function
    private void InitAutoMiningTouchCntEnhance()
    {
        autoMiningTouchCnt.InitValue();
    }
    private void InitAutoMiningDurationEnhance()
    {
        autoMiningDuration.InitValue();
    }
    #endregion

    #region Fever Enhance Function
    private void InitFeverAmountEnhance()
    {
        feverAmount.InitValue();
    }
    private void InitFeverDurationEnhance()
    {
        feverDuration.InitValue();
    }
    #endregion

    #endregion


    #region Private Methods

    /// <summary>
    /// 1�ʸ��� �ʴ� ȹ�淮��ŭ ������ �߰��ϴ� �ݺ� �Լ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator Loop_IncreaseJewelPerSec()
    {
        yield return new WaitUntil(() => DBManager.Inst.loadAllCompleted);

        while (!DBManager.Inst.isGameStop)
        {
            if(JewelPerSec.Value != 0)
            {
                SumJewel(JewelPerSec.Value);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }


    /// <summary>
    /// ��ȭ�� A-Z���� ���ڿ��� ��ȯ�Ѵ�.
    /// </summary>
    public void ConvertToCurrencyString()
    {
        strJewel = CurrencyParser.ToCurrencyString(Jewel.Value);
        strJewelPerTouch = CurrencyParser.ToCurrencyString(JewelPerTouch.Value);
        strJewelPerSec = CurrencyParser.ToCurrencyString(JewelPerSec.Value);
        strCoin = CurrencyParser.ToCurrencyString(Coin.Value);
    }

 

    #endregion
}
