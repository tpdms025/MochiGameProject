using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GoldManager : MonoBehaviour
{

    #region Data

    /// <summary>
    /// �����ϰ� �ִ� ����
    /// </summary>
    [SerializeField] private BigInteger m_gold = 0;

    /// <summary>
    /// Ŭ���Ҷ� �����ϴ� ������
    /// </summary>
    [SerializeField] private BigInteger m_goldPerClick = 0;

    /// <summary>
    /// �ʴ� �����ϴ� ������
    /// </summary>
    [SerializeField] private BigInteger m_goldPerSec = 0;


    //DB ���� �� UI����
    public event Action<BigInteger> onGoldChanged;
    public event Action<BigInteger> onGoldPerClickChanged;
    public event Action<BigInteger> onGoldPerSecChanged;

    #endregion

    #region Property

    /// <summary>
    /// m_gold�� ������Ƽ
    /// </summary>
    public BigInteger Gold
    {
        get { return m_gold; }
        set
        {
            m_gold = value;

            if (onGoldChanged != null)
            {
                onGoldChanged(m_gold);
            }
        }
    }

    /// <summary>
    /// m_goldPerClick�� ������Ƽ
    /// </summary>
    public BigInteger GoldPerClick
    {
        get { return m_goldPerClick; }
        set
        {
            m_goldPerClick = value;

            if (onGoldChanged != null)
            {
                onGoldChanged(m_gold);
            }
        }
    }

    /// <summary>
    /// m_goldPerSec�� ������Ƽ
    /// </summary>
    public BigInteger GoldPerSec
    {
        get { return m_goldPerSec; }
        set
        {
            m_goldPerSec = value;

            if (onGoldChanged != null)
            {
                onGoldChanged(m_gold);
            }
        }
    }
    #endregion

    #region Fields

    private static GoldManager instance = null;
    public static GoldManager Instance
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
        InitGold();
    }

    private void Start()
    {
        StartCoroutine(AddGoldPerSecLoop());
    }

    #endregion


    #region Methods

    #region Gold Function

    public void AddGold(BigInteger _newGold)
    {
        Gold += _newGold;
    }

    public void SubGold(BigInteger _newGold)
    {
        Gold -= _newGold;
    }

    public void InitGold()
    {
        Gold = 0;
    }
 

    #endregion

    #region GoldPerClick Function

    public void AddGoldPerClick(BigInteger _newGoldPerClick)
    {
        m_goldPerClick += _newGoldPerClick;

    }

    public void SubGoldPerClick(BigInteger _newGoldPerClick)
    {
        m_goldPerClick -= _newGoldPerClick;
    }

    public void InitGoldPerClick()
    {
        m_goldPerClick = 0;
    }


    #endregion

    #region GoldPerSec Function

    public void AddGoldPerSec(BigInteger _newGoldPerSec)
    {
        m_goldPerSec += _newGoldPerSec;

    }

    public void SubGoldPerSec(BigInteger _newGoldPerSec)
    {
        m_goldPerSec -= _newGoldPerSec;
    }

    public void InitGoldPerSec()
    {
        m_goldPerSec = 0;
    }


    #endregion

    #endregion

    #region Private Methods

    /// <summary>
    /// 1�ʴ� ��带 ������Ű�� �ݺ���
    /// </summary>
    /// <returns></returns>
    private IEnumerator AddGoldPerSecLoop()
    {
        while (true)
        {
            if(GoldPerSec != 0)
            {
                AddGold(GoldPerSec);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

 

    #endregion
}
