using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{

    #region Data

    /// �����ϰ� �ִ� ����
    private BigInteger m_jewel;

    /// �����ϰ� �ִ� ����
    private BigInteger m_marble;

    /// �����ϰ� �ִ� ����
    private BigInteger m_coin;




    ///// ��ġ�� �� �����ϴ� ������
    //private BigInteger m_jewelPerTouch;

    ///// �ʴ� �����ϴ� ������
    //private BigInteger m_JewelPerSec;




    /// ��ġ�� �� �����ϴ� ��ü�� ������
    public Ability totalJewelPerTouch;
    /// �ʴ� �����ϴ� ��ü�� ������
    public Ability totalJewelPerSec;



    //���� ���� �� ȣ���ϴ� ��������Ʈ
    public event Action<BigInteger> onJewelChanged;
    public event Action<BigInteger> onMableChanged;
    public event Action<BigInteger> onCoinChanged;

    #endregion

    #region Property

    /// <summary>
    /// m_jewel�� ������Ƽ
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
    /// m_marble�� ������Ƽ
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
    /// m_coin�� ������Ƽ
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
    /// m_jewelPerTouch�� ������Ƽ
    /// </summary>
    public BigInteger JewelPerTouch
    {
        get { return totalJewelPerTouch.Value; }
        private set{
            totalJewelPerTouch.Value = value;
        }
    }

    /// <summary>
    /// m_jewelPerSec�� ������Ƽ
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
        //TODO: DB �о����
        //���� �ӽ�
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
    /// 1�ʴ� ��带 ������Ű�� �ݺ���
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
    ///// �ʴ� ������ ��ųʸ��� �����͸� �����Ѵ�.
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
    ///// �ʴ� ������ �񼭳ฮ�� �����͸� �߰��Ѵ�.
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
