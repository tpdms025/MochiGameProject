using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowGoodsUI : MonoBehaviour
{
    #region Data
    #endregion

    #region Fields
    [SerializeField] private TextMeshProUGUI jewelText;
    #endregion

    #region Unity methods
    #endregion

    #region Methods
    
    public void OnJewelText(string jewelStr)
    {
        jewelText.text = jewelStr;
    }
    #endregion

    #region Private Methods
    #endregion
}
