using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowGoodsUI : MonoBehaviour
{
    #region Data


    #endregion

    #region Fields
    [SerializeField] private TextMeshProUGUI JewelText;
    [SerializeField] private TextMeshProUGUI JewelPerSecText;
    [SerializeField] private TextMeshProUGUI MarbleText;
    [SerializeField] private TextMeshProUGUI CubeText;
    #endregion

    #region Unity methods
    private void Start()
    {
        StartCoroutine(Cor_UpdateCurrencyText());
    }
    #endregion

    #region Methods

    /// <summary>
    /// 화폐 UI텍스트를 업데이트한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cor_UpdateCurrencyText()
    {
        while(true)
        {
            string strGold = CurrencyParser.ToCurrencyString(GoldManager.Instance.Gold);
            string strGoldPerSec = CurrencyParser.ToCurrencyString(GoldManager.Instance.GoldPerSec);
            string strMarble = CurrencyParser.ToCurrencyString(GoldManager.Instance.GoldPerSec);
            string strCube = CurrencyParser.ToCurrencyString(GoldManager.Instance.GoldPerSec);

            JewelText.text = string.Format("{0}", strGold);
            JewelPerSecText.text = string.Format("{0}/s", strGoldPerSec);

            yield return null;
        }
    }


    #endregion

    #region Private Methods
    #endregion
}
