using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GoldManager : MonoBehaviour
{

    #region Data

    /// <summary>
    /// 보유하고 있는 보석
    /// </summary>
    [SerializeField] private BigInteger m_gold = 0;

    /// <summary>
    /// 클릭할때 증가하는 보석량
    /// </summary>
    [SerializeField] private BigInteger m_goldPerClick = 0;

    /// <summary>
    /// 초당 증가하는 보석량
    /// </summary>
    [SerializeField] private BigInteger m_goldPerSec = 0;

    /// <summary>
    /// m_gold의 프로퍼티
    /// </summary>
    public BigInteger Gold
    {
        get { return m_gold; }
        set
        {
            m_gold = value;
            //DB 저장
            //
        }
    }

    /// <summary>
    /// m_goldPerClick의 프로퍼티
    /// </summary>
    public BigInteger GoldPerClick
    {
        get { return m_goldPerClick; }
        set
        {
            m_goldPerClick = value;
            //DB 저장
            //
        }
    }

    /// <summary>
    /// m_goldPerSec의 프로퍼티
    /// </summary>
    public BigInteger GoldPerSec
    {
        get { return m_goldPerSec; }
        set
        {
            m_goldPerSec = value;
            //DB 저장
            //
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
        //싱글톤
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            InitGold();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(AddGoldPerLoop());
    }

    #endregion


    #region Methods
    public void TestAddGold()
    {
        AddGold((Gold+1)*(Gold+1));
    }

    #region Gold Function

    //public BigInteger GetGold()
    //{
    //    return Gold;
    //}

    //public void SetGold(BigInteger _newGold)
    //{
    //    Gold = _newGold;
    //    //DB 저장
    //    //
    //}

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

    //public BigInteger GetGoldPerClick()
    //{
    //    return m_goldPerClick;
    //}
    
    //public void SetGoldPerClick(BigInteger _newGoldPerClick)
    //{
    //    m_goldPerClick = _newGoldPerClick;

    //    //DB저장
    //    //
    //}

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

    private IEnumerator AddGoldPerLoop()
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

    private void LoadDBCurrency()
    {

    }

    private void SaveDBCurrency()
    {

    }

    #endregion
}
