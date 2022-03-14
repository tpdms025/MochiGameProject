using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{

    #region Data

    /// 보유하고 있는 보석
    private BigInteger m_jewel;

    /// 보유하고 있는 구슬
    private BigInteger m_marble;

    /// 보유하고 있는 코인
    private BigInteger m_coin;




    ///// 터치할 때 증가하는 보석량
    //private BigInteger m_jewelPerTouch;

    ///// 초당 증가하는 보석량
    //private BigInteger m_JewelPerSec;




    /// 터치할 때 증가하는 전체의 보석량
    public Ability totalJewelPerTouch;
    /// 초당 증가하는 전체의 보석량
    public Ability totalJewelPerSec;



    //값이 변할 때 호출하는 델리게이트
    public event Action<BigInteger> onJewelChanged;
    public event Action<BigInteger> onMableChanged;
    public event Action<BigInteger> onCoinChanged;

    #endregion

    #region Property

    /// <summary>
    /// m_jewel의 프로퍼티
    /// </summary>
    public BigInteger Jewel
    {
        get { return m_jewel; } 
        private set{ 
            m_jewel = value;
            if (onJewelChanged != null)
            {
                onJewelChanged(m_jewel);
            }
        }
    }

    /// <summary>
    /// m_marble의 프로퍼티
    /// </summary>
    public BigInteger Marble
    {
        get { return m_marble; }
        private set
        {
            m_marble = value;
            if (onMableChanged != null)
            {
                onMableChanged(m_marble);
            }
        }
    }

    /// <summary>
    /// m_coin의 프로퍼티
    /// </summary>
    public BigInteger Coin
    {
        get { return m_coin; }
        private set
        {
            m_coin = value;
            if (onCoinChanged != null)
            {
                onCoinChanged(m_coin);
            }
        }
    }



    /// <summary>
    /// m_jewelPerTouch의 프로퍼티
    /// </summary>
    public BigInteger JewelPerTouch
    {
        get { return totalJewelPerTouch.Value; }
        private set{
            totalJewelPerTouch.Value = value;
        }
    }

    /// <summary>
    /// m_jewelPerSec의 프로퍼티
    /// </summary>
    public BigInteger JewelPerSec
    {
        get { return totalJewelPerSec.Value; }
        private set {
            totalJewelPerSec.Value = value;
        }
    }

  

    #endregion

    #region Fields

    private static MoneyManager instance = null;
    public static MoneyManager Instance
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
        //TODO: DB 읽어오기
        //현재 임시
        //
        InitJewel();
        InitMarble();
        InitCoin();

        totalJewelPerTouch = new Ability(1);
        totalJewelPerSec = new Ability(1);

        StartCoroutine(Loop_IncreaseJewelPerSec());
    }

    #endregion


    #region Methods

    #region Modify Jewel Function

    public void AddJewel(BigInteger _newValue)
    {
        Jewel += _newValue;
    }

    public void SubJewel(BigInteger _newValue)
    {
        Jewel -= _newValue;
    }

    public void InitJewel()
    {
        Jewel = 0;
    }


    #endregion

    #region Modify JewelPerClick Function

    public void AddJewelPerClick(BigInteger _newValue)
    {
        JewelPerTouch += _newValue;
    }

    public void SubJewelPerClick(BigInteger _newValue)
    {
        JewelPerTouch -= _newValue;
    }

    public void InitJewelPerClick()
    {
        JewelPerTouch = 0;
    }


    #endregion

    #region Modify JewelPerSec Function

    public void AddJewelPerSec(BigInteger _newValue)
    {
        JewelPerSec += _newValue;
    }

    public void SubJewelPerSec(BigInteger _newValue)
    {
        JewelPerSec -= _newValue;
    }

    public void InitJewelPerSec()
    {
        JewelPerSec = 0;
    }

    #endregion

    #region Modify Cube Function

    public void AddCoin(BigInteger _newValue)
    {
        Coin += _newValue;
    }

    public void SubCoin(BigInteger _newValue)
    {
        Coin -= _newValue;
    }

    public void InitCoin()
    {
        Coin = 0;
    }

    #endregion

    #region Modify Marble Function

    public void AddMarble(BigInteger _newValue)
    {
        Marble += _newValue;
    }

    public void SubMarble(BigInteger _newValue)
    {
        Marble -= _newValue;
    }

    public void InitMarble()
    {
        Marble = 0;
    }

    #endregion

    #endregion

    #region Private Methods

    /// <summary>
    /// 1초당 골드를 증가시키는 반복문
    /// </summary>
    /// <returns></returns>
    private IEnumerator Loop_IncreaseJewelPerSec()
    {
        while (true)
        {
            if(JewelPerSec != 0)
            {
                AddJewel(JewelPerSec);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    ///// <summary>
    ///// 초당 증가량 딕셔너리의 데이터를 변경한다.
    ///// </summary>
    ///// <param name="perSecList"></param>
    ///// <param name="_id"></param>
    ///// <param name="newValue"></param>
    //public void ChangedPerSec(Dictionary<int,BigInteger> perSecList,int _id, BigInteger newValue)
    //{
    //    BigInteger interval = newValue - perSecList[_id];
    //    JewelPerSec += interval;
    //    perSecList[_id] = newValue;
    //}

    ///// <summary>
    ///// 초당 증가량 딕서녀리에 데이터를 추가한다.
    ///// </summary>
    ///// <param name="perSecList"></param>
    ///// <param name="_id"></param>
    ///// <param name="newValue"></param>
    //public void AddJewelPerSec(Dictionary<int, BigInteger> perSecList, int _id, BigInteger newValue)
    //{
    //    if (perSecList[_id] != null)
    //        return;
    //    perSecList.Add(_id, newValue);
    //    JewelPerSec += newValue;
    //}

 

    #endregion
}
