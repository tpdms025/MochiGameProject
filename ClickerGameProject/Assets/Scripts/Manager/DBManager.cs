using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    #region Data
    #endregion

    #region Fields

    private static DBManager instance = null;
    public static DBManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    #endregion

    #region Unity methods

    private void Awake()
    {
        //ΩÃ±€≈Ê
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        Debug.Log(this.gameObject.name+ "Awake");
    }

    private void Start()
    {
        GoldManager.Instance.onGoldChanged += SaveGold;
    }

    #endregion

    #region Methods

    private void LoadDBCurrency()
    {

    }

    private void SaveDBCurrency()
    {

    }

    #endregion

    #region Private Methods
    private void SaveGold(BigInteger gold)
    {
        //Debug.Log("SaveGold: " + gold);
    }
    #endregion
}
