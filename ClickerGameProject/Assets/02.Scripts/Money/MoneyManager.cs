using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    #region Data

    //보유하고 있는 보석
    private ValueModifiers jewel;

    //보유하고 있는 코인
    private ValueModifiers coin;

    // 터치당 획득량
    private ValueModifiers totalJewelPerTouch;

    // 초당 획득량
    private ValueModifiers totalJewelPerSec;
    


    //보유 보석의 문자열
    public string strJewel;

    //보유 코인의 문자열
    public string strCoin;

    //터치당 보석량의 문자열
    public string strJewelPerTouch;

    //초당 보석량의 문자열
    public string strJewelPerSec;


    //==============Ability================
    // 일꾼 생산량 (Rate만 사용하기)
    public List<ValueModifiers> workmansAmount;

    //자동채굴의 지속시간 (Rate만 사용하기)
    public ValueModifiers autoMiningDuration;   

    //자동채굴의 터치횟수
    public ValueModifiers autoMiningTouchCnt;

    //피버 지속시간 (Rate만 사용하기)
    public ValueModifiers feverDuration;   

    //피버 광석의 획득량
    public ValueModifiers feverAmount;
    //======================================

    #endregion

    #region Property


    /// <summary>
    /// m_jewel의 프로퍼티
    /// </summary>
    public ValueModifiers Jewel
    {
        get { return jewel; }
    }

    /// <summary>
    /// m_coin의 프로퍼티
    /// </summary>
    public ValueModifiers Coin
    {
        get { return coin; }
    }

    /// <summary>
    /// m_jewelPerTouch의 프로퍼티
    /// </summary>
    public ValueModifiers JewelPerTouch
    {
        get { return totalJewelPerTouch; }
    }

    /// <summary>
    /// m_jewelPerSec의 프로퍼티
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
        //싱글톤
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
        //강화 정보
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
    /// 허용된 재화만 0으로 환생때 초기화한다.
    /// </summary>
    public void ResetOnPrestige()
    {
        //코인은 제외
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
        //1레벨 값으로 초기화
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
    /// 모든 일꾼 값을 초당획득량에 더한다.
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
    /// 1초마다 초당 획득량만큼 보석을 추가하는 반복 함수
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
    /// 재화를 A-Z단위 문자열로 변환한다.
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
