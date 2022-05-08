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
    /// ȭ�� UI�ؽ�Ʈ�� ������Ʈ�Ѵ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cor_LoopUpdateCurrencyText()
    {
        while (!DBManager.Inst.isGameStop)
        {
            //��ȭ�� ���ڿ��� ��ȯ
            MoneyManager.Inst.ConvertToCurrencyString();

            //�ؽ�Ʈ ���
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
