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

    //���ø����̼��� ������ ��¥
    public DateTime lastDateTime { get; private set; }

    //���ø����̼��� ������ ��¥ (����)
    public DateTime nowDateTime { get; private set; }

    //�ִ� �ð�����
    private int maxConpareDays = 1;

    //���ͳ� ���� ����
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

            //TODO : DB �ε�
            //************************************
            lastDateTime = new DateTime(2022, 02, 10, 0, 0, 0);
            nowDateTime = GetGoogleDateTime();
            Debug.Log("nowDateTime : " + nowDateTime);
        }

    }

    private void OnApplicationQuit()
    {
        //TODO :DB����
        //************************************
        lastDateTime = new DateTime(2022, 02, 10, 0, 0, 0);
    }

    /// <summary>
    /// ���ͳ��� ����Ǿ����� Ȯ���Ѵ�.
    /// </summary>
    private bool CheckConnectInternet()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            //���ͳ� ������ �ȵǾ��� �� �ൿ
            OnDisconnectInternet.Invoke();
            return false;
        }
        else
        {
            //������ �� �������̷� ���ͳ� ������ �Ǿ��� �� �ൿ
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
                string date = request.GetResponseHeader("date"); //�̰����� �ݼ۵� �����Ϳ� �ð� �����Ͱ� ����
                Debug.Log("�޾ƿ� �ð�" + date); // GMT�� �޾ƿ´�.
                DateTime dateTime = DateTime.Parse(date).ToLocalTime(); // ToLocalTime() �޼ҵ�� �ѱ��ð����� ��ȯ���� �ش�.
                Debug.Log("�ѱ��ð����κ�ȯ" + dateTime);
            }
        }
    }


    public DateTime GetGoogleDateTime()
    {
        //���� �� ��¥ ����
        DateTime dateTime = DateTime.MinValue;

        try
        {
            //WebRequest ��ü�� ���ۻ���Ʈ ���� �ش� ��¥�� �ð��� ���� ������ �������� ���� ���ڿ� ��´�.
            using (var response = WebRequest.Create("http://www.google.com").GetResponse())
                dateTime = DateTime.ParseExact(response.Headers["date"],
                    "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                    CultureInfo.InvariantCulture.DateTimeFormat,
                    DateTimeStyles.AssumeUniversal);
        }
        catch (Exception)
        {
            Debug.Log("GoogleDateTime Error");
            //���� �߻��� ���� ��¥ �״�� ����
            dateTime = DateTime.Now;
        }
        return dateTime;
    }


    /// <summary>
    /// ���������� ������ �ð��� ������ �ð� ���̸� ����մϴ�.
    /// (�ִ� �ð� 1��)
    /// </summary>
    public TimeSpan CalculateTimeOffline()
    {
        //DateTime _lastDateTime = new DateTime(2022, 02, 10, 0, 0, 0);
        //DateTime _nowDateTime = GetGoogleDateTime();

        TimeSpan conpareDateTime = nowDateTime - lastDateTime;
        
        //�ð��� ���ѵα�
        if(conpareDateTime.TotalDays > maxConpareDays)
        {
            conpareDateTime = new TimeSpan(maxConpareDays,0,0,0);
        }
        return conpareDateTime;
    }
}
