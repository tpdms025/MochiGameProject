using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowMoneyUI : MonoBehaviour
{
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
    /// ȭ�� UI�ؽ�Ʈ�� ������Ʈ�Ѵ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cor_UpdateCurrencyText()
    {
        while (true)
        {
            //��ȭ�� ���ڿ��� ��ȯ
            MoneyManager.Instance.ConvertToCurrencyString();

            //�ؽ�Ʈ ���
            jewelText.text = string.Format("{0}", MoneyManager.Instance.strJewel);
            jewelPerTouchText.text = string.Format("{0}/Touch", MoneyManager.Instance.strJewelPerTouch);
            jewelPerSecText.text = string.Format("{0}/s", MoneyManager.Instance.strJewelPerSec);
            marbleText.text = string.Format("{0}", MoneyManager.Instance.strMarble);
            coinText.text = string.Format("{0}", MoneyManager.Instance.strCoin);

            yield return null;
        }
    }


    #endregion

    #region Private Methods
    #endregion
}
