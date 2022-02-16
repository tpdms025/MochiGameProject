using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class TimerManager : MonoBehaviour
{
    public const string url = "http://www.google.com";

    //어플리케이션을 종료한 날짜
    public DateTime lastDateTime { get; private set; }

    //어플리케이션을 실행한 날짜 (현재)
    public DateTime nowDateTime { get; private set; }

    //최대 시간차이
    private int maxConpareDays = 1;

    //인터넷 연결 여부
    private bool isConnectedInternet;

    public event Action OnDisconnectInternet;


    public static TimerManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isConnectedInternet = CheckConnectInternet();

        if (isConnectedInternet)
        {
            StartCoroutine(WebChk());

            //TODO : DB 로드
            //************************************
            lastDateTime = new DateTime(2022, 02, 10, 0, 0, 0);
            nowDateTime = GetGoogleDateTime();
            Debug.Log("nowDateTime : " + nowDateTime);
        }

    }

    private void OnApplicationQuit()
    {
        //TODO :DB저장
        //************************************
        lastDateTime = new DateTime(2022, 02, 10, 0, 0, 0);
    }

    /// <summary>
    /// 인터넷이 연결되었는지 확인한다.
    /// </summary>
    private bool CheckConnectInternet()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            //인터넷 연결이 안되었을 때 행동
            OnDisconnectInternet.Invoke();
            return false;
        }
        else
        {
            //데이터 및 와이파이로 인터넷 연결이 되었을 때 행동
            return true;
        }
    }

    private IEnumerator WebChk()
    {
        UnityWebRequest request = new UnityWebRequest();
        using (request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string date = request.GetResponseHeader("date"); //이곳에서 반송된 데이터에 시간 데이터가 존재
                Debug.Log("받아온 시간" + date); // GMT로 받아온다.
                DateTime dateTime = DateTime.Parse(date).ToLocalTime(); // ToLocalTime() 메소드로 한국시간으로 변환시켜 준다.
                Debug.Log("한국시간으로변환" + dateTime);
            }
        }
    }


    public DateTime GetGoogleDateTime()
    {
        //리턴 할 날짜 선언
        DateTime dateTime = DateTime.MinValue;

        try
        {
            //WebRequest 객체로 구글사이트 접속 해당 날짜와 시간을 로컬 형태의 포맷으로 리턴 일자에 담는다.
            using (var response = WebRequest.Create("http://www.google.com").GetResponse())
                dateTime = DateTime.ParseExact(response.Headers["date"],
                    "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                    CultureInfo.InvariantCulture.DateTimeFormat,
                    DateTimeStyles.AssumeUniversal);
        }
        catch (Exception)
        {
            Debug.Log("GoogleDateTime Error");
            //오류 발생시 로컬 날짜 그대로 리턴
            dateTime = DateTime.Now;
        }
        return dateTime;
    }


    /// <summary>
    /// 마지막으로 종료한 시간과 실행한 시간 차이를 계산합니다.
    /// (최대 시간 1일)
    /// </summary>
    public TimeSpan CalculateTimeOffline()
    {
        //DateTime _lastDateTime = new DateTime(2022, 02, 10, 0, 0, 0);
        //DateTime _nowDateTime = GetGoogleDateTime();

        TimeSpan conpareDateTime = nowDateTime - lastDateTime;
        
        //시간차 제한두기
        if(conpareDateTime.TotalDays > maxConpareDays)
        {
            conpareDateTime = new TimeSpan(maxConpareDays,0,0,0);
        }
        return conpareDateTime;
    }
}
