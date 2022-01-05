using System.Numerics;
using UnityEngine;

public class GlodController : MonoBehaviour
{
    #region Data

    const string zero = "0";

    /// <summary>
    /// ���� ǥ�� ��Ÿ��
    /// </summary>
    public enum CurrencyType { Default, SI, }

    private static readonly int _asciiA = 65;
    private static readonly int _asciiZ = 90;
    private static readonly int _unitSize = 2;

    static readonly string[] currencyUnits = new string[]
    {
        "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
    };

    BigInteger gold = 0;

    #endregion

    #region Fields
    #endregion

    #region Unity methods

    private void Awake()
    {
        BigInteger intNum = BigInteger.Pow(10, 55);
        string s =ToCurrencyString(intNum);
        Debug.Log(intNum);
        Debug.Log(s);
    }

    private void Start()
    {
        
    }

    #endregion

    #region Methods

    #region Convert
    /// <summary>
    /// double�� �����͸� Ŭ��Ŀ ȭ�� ������ ǥ���Ѵ�.
    /// </summary>
    /// <param name="num"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string ToCurrencyString(BigInteger num,CurrencyType type = CurrencyType.Default)
    {
        //��ȣ ��� ���ڿ�
        string significant = string.Empty;
        //������ ����
        string showNumber = string.Empty;
        //���� ����
        string unitString = string.Empty;

        //�ŵ�����
        int exponent = 0;

        string[] partsSplit = num.ToString("E").Split('+');

        if(partsSplit.Length <2)
        {
            return zero;
        }
        if(int.TryParse(partsSplit[1],out exponent) == false)
        {
            return zero;
        }

        //���� ���� ���ڿ��� �ε���
        int quotient = exponent / _unitSize;

        //����ó�� - ������ ���ڴ������� ���� ��� 99.Z
        if (quotient > currencyUnits.Length - 1)
        {
            unitString = currencyUnits[currencyUnits.Length - 1];
            showNumber = (Mathf.Pow(10, _unitSize) - 1).ToString();
            return string.Format("{0}{1}", showNumber, unitString);
        }

        //�������� ������ �ڸ��� ��꿡 ���
        int remainder = exponent % _unitSize;

        //1A �̸��� �׳� ǥ��
        if(exponent < _unitSize)
        {
            showNumber = num.ToString();
        }
        else
        {
            double temp = double.Parse(partsSplit[0].Replace("E", "")) * Mathf.Pow(10, remainder);
            showNumber = temp.ToString("F2");
        }

        //if(!quotient.Equals(0))
        //{
        //    int _unitCapacity = 0;
        //    if (quotient>26)
        //    {
        //        _unitCapacity = quotient / 26;
        //    }

        //    unitString = ((char)(_asciiA + quotient)).ToString();
        //}

        unitString = currencyUnits[quotient];
        
        return string.Format("{0}{1}", showNumber, unitString);
    }

    #endregion

    public void AddGold(BigInteger add)
    {
        gold += add;
    }


    #endregion

    #region Private Methods

    #endregion

}
