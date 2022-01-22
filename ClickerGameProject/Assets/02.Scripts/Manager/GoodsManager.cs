using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsManager : MonoBehaviour
{
    #region Fields

    private static GoldManager instance = null;
    public static GoldManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    #endregion

}
