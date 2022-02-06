using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowMoneyUI : MonoBehaviour
{
    #region Data


    #endregion

    #region Fields

    [SerializeField] private TextMeshProUGUI JewelText;
    [SerializeField] private TextMeshProUGUI JewelPerClickText;
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
            string strGold = CurrencyParser.ToCurrencyString(MoneyManager.Instance.Jewel);
            string strGoldPerClick = CurrencyParser.ToCurrencyString(MoneyManager.Instance.JewelPerClick);
            string strGoldPerSec = CurrencyParser.ToCurrencyString(MoneyManager.Instance.JewelPerSec);
            string strMarble = CurrencyParser.ToCurrencyString(MoneyManager.Instance.marble);
            string strCube = CurrencyParser.ToCurrencyString(MoneyManager.Instance.cube);

            JewelText.text = string.Format("{0}", strGold);
            JewelPerClickText.text = string.Format("{0}/Touch", strGoldPerClick);
            JewelPerSecText.text = string.Format("{0}/s", strGoldPerSec);
            MarbleText.text = string.Format("{0}", strMarble);
            CubeText.text = string.Format("{0}", strCube);

            yield return null;
        }
    }


    #endregion

    #region Private Methods
    #endregion
}
