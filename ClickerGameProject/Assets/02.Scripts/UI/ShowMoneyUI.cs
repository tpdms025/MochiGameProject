using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowMoneyUI : MonoBehaviour
{
    #region Fields

    [SerializeField] private TextMeshProUGUI jewelText;
    [SerializeField] private TextMeshProUGUI jewelPerTouchText;
    [SerializeField] private TextMeshProUGUI jewelPerSecText;
    [SerializeField] private TextMeshProUGUI coinText;

    #endregion

    #region Unity methods
    private void Start()
    {
        StartCoroutine(Cor_LoopUpdateCurrencyText());
    }
    #endregion

    #region Methods

    /// <summary>
    /// 화폐 UI텍스트를 업데이트한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cor_LoopUpdateCurrencyText()
    {
        while (!DBManager.Inst.isGameStop)
        {
            //재화를 문자열로 변환
            MoneyManager.Inst.ConvertToCurrencyString();

            //텍스트 출력
            jewelText.text = string.Format("{0}", MoneyManager.Inst.strJewel);
            jewelPerTouchText.text = string.Format("{0}/Touch", MoneyManager.Inst.strJewelPerTouch);
            jewelPerSecText.text = string.Format("{0}/s", MoneyManager.Inst.strJewelPerSec);
            coinText.text = string.Format("{0}", MoneyManager.Inst.strCoin);

            yield return null;
        }
    }


    #endregion

    #region Private Methods
    #endregion
}
