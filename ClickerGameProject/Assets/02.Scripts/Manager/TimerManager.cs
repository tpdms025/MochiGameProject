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
    private const int maxConpareDays = 1;

    //인터넷 연결 여부
    private bool isConnectedInternet;

    public event Action onDisconnectInternet;


    public static TimerManager Instance { get; private set; }

    private void Awake()
    {
        //싱글톤
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            isConnectedInternet = false;
            //인터넷 연결이 안되었을 때 행동
            if (onDisconnectInternet != null)
                onDisconnectInternet.Invoke();
        }
        else
        {
            isConnectedInternet = false;
            //데이터 및 와이파이로 인터넷 연결이 되었을 때 행동

            //StartCoroutine(WebChk());

            //TODO : DB 로드
            //************************************
            lastDateTime = new DateTime(2022, 02, 20, 0, 0, 0);
            nowDateTime = GetGoogleDateTime();
            Debug.Log("nowDateTime : " + nowDateTime);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        
    }

    private void OnApplicationQuit()
    {
        //TODO :DB저장
        //************************************
        lastDateTime = new DateTime(2022, 02, 10, 0, 0, 0);
    }



    /// <summary>
    /// 구글 시간 데이터를 읽어온다.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 구글 시간 데이터를 읽어온다.
    /// </summary>
    /// <returns></returns>
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

        TimeSpan compareDateTime = nowDateTime - lastDateTime;
        
        //시간차 제한두기
        if(compareDateTime.TotalDays > maxConpareDays)
        {
            compareDateTime = new TimeSpan(maxConpareDays,0,0,0);
        }
        Debug.Log("Offline time calculate is " + (int)compareDateTime.TotalMinutes);
        return compareDateTime;
    }
}
