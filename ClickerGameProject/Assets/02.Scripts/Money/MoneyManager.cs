using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{

    #region Data

    /// <summary>
    /// �����ϰ� �ִ� ����
    /// </summary>
    private BigInteger m_jewel = 0;

    /// <summary>
    /// Ŭ���� �� �����ϴ� ������
    /// </summary>
    private BigInteger m_jewelPerClick = 0;

    /// <summary>
    /// �ʴ� �����ϴ� ��ü ������
    /// </summary>
    private BigInteger m_totalJewelPerSec = 0;



    //��������Ʈ
    public event Action<BigInteger> onJewelChanged;
    public event Action<BigInteger> onJewelPerClickChanged;
    public event Action<BigInteger> onJewelPerSecChanged;

    #endregion

    #region Property

    /// <summary>
    /// m_jewel�� ������Ƽ
    /// </summary>
    public BigInteger Jewel
    {
        get { return m_jewel; } 
        private set{ m_jewel = value;
            if (onJewelChanged != null)
            {
                onJewelChanged(m_jewel);
            }
        }
    }

    /// <summary>
    /// m_jewelPerClick�� ������Ƽ
    /// </summary>
    public BigInteger JewelPerClick
    {
        get { return m_jewelPerClick; }
        private set{ m_jewelPerClick = value;
            if (onJewelPerClickChanged != null)
            {
                onJewelPerClickChanged(m_jewelPerClick);
            }
        }
    }

    /// <summary>
    /// m_jewelPerSec�� ������Ƽ
    /// </summary>
    public BigInteger JewelPerSec
    {
        get { return m_totalJewelPerSec; }
        private set { m_totalJewelPerSec = value;
            if (onJewelPerSecChanged != null)
            {
                onJewelPerSecChanged(m_totalJewelPerSec);
            }
        }
    }

    /// <summary>
    /// �����ϰ� �ִ� ť��
    /// </summary>
    public BigInteger cube { get; private set; }

    /// <summary>
    /// �����ϰ� �ִ� ����
    /// </summary>
    public BigInteger marble { get; private set; }

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
        //
        InitJewel();
        InitCube();
        InitMarble();

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
        JewelPerClick += _newValue;
    }

    public void SubJewelPerClick(BigInteger _newValue)
    {
        JewelPerClick -= _newValue;
    }

    public void InitJewelPerClick()
    {
        JewelPerClick = 0;
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

    public void AddCube(BigInteger _newValue)
    {
        cube += _newValue;
    }

    public void SubCube(BigInteger _newValue)
    {
        cube -= _newValue;
    }

    public void InitCube()
    {
        cube = 0;
    }

    #endregion

    #region Modify Marble Function

    public void AddMarble(BigInteger _newValue)
    {
        marble += _newValue;
    }

    public void SubMarble(BigInteger _newValue)
    {
        marble -= _newValue;
    }

    public void InitMarble()
    {
        marble = 0;
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
