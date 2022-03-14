using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowMoneyUI : MonoBehaviour
{
    #region Data


    #endregion

    #region Fields

    [SerializeField] private TextMeshProUGUI jewelText;
    [SerializeField] private TextMeshProUGUI jewelPerTouchText;
    [SerializeField] private TextMeshProUGUI jewelPerSecText;
    [SerializeField] private TextMeshProUGUI marbleText;
    [SerializeField] private TextMeshProUGUI coinText;

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
            string strJewel = CurrencyParser.ToCurrencyString(MoneyManager.Instance.Jewel);
            string strJewelPerTouch = CurrencyParser.ToCurrencyString(MoneyManager.Instance.JewelPerTouch);
            string strJewelPerSec = CurrencyParser.ToCurrencyString(MoneyManager.Instance.JewelPerSec);
            string strMarble = CurrencyParser.ToCurrencyString(MoneyManager.Instance.Marble);
            string strCoin = CurrencyParser.ToCurrencyString(MoneyManager.Instance.Coin);

            jewelText.text = string.Format("{0}", strJewel);
            jewelPerTouchText.text = string.Format("{0}/Touch", strJewelPerTouch);
            jewelPerSecText.text = string.Format("{0}/s", strJewelPerSec);
            marbleText.text = string.Format("{0}", strMarble);
            coinText.text = string.Format("{0}", strCoin);

            yield return null;
        }
    }


    #endregion

    #region Private Methods
    #endregion
}
