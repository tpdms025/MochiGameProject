using System.Numerics;
using UnityEngine;

public class GlodController : MonoBehaviour
{
    #region Data

    const string zero = "0";

    /// <summary>
    /// 단위 표현 스타일
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
    /// double형 데이터를 클리커 화폐 단위로 표현한다.
    /// </summary>
    /// <param name="num"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string ToCurrencyString(BigInteger num,CurrencyType type = CurrencyType.Default)
    {
        //부호 출력 문자열
        string significant = string.Empty;
        //보여줄 숫자
        string showNumber = string.Empty;
        //단위 숫자
        string unitString = string.Empty;

        //거듭제곱
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

        //몫은 단위 문자열의 인덱스
        int quotient = exponent / _unitSize;

        //예외처리 - 정해진 숫자단위보다 높을 경우 99.Z
        if (quotient > currencyUnits.Length - 1)
        {
            unitString = currencyUnits[currencyUnits.Length - 1];
            showNumber = (Mathf.Pow(10, _unitSize) - 1).ToString();
            return string.Format("{0}{1}", showNumber, unitString);
        }

        //나머지는 정수부 자릿수 계산에 사용
        int remainder = exponent % _unitSize;

        //1A 미만은 그냥 표현
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
