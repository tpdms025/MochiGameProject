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
    private const int maxConpareDays = 1;

    //���ͳ� ���� ����
    private bool isConnectedInternet;

    public event Action onDisconnectInternet;


    public static TimerManager Instance { get; private set; }

    private void Awake()
    {
        //�̱���
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
            //���ͳ� ������ �ȵǾ��� �� �ൿ
            if (onDisconnectInternet != null)
                onDisconnectInternet.Invoke();
        }
        else
        {
            isConnectedInternet = false;
            //������ �� �������̷� ���ͳ� ������ �Ǿ��� �� �ൿ

            //StartCoroutine(WebChk());

            //TODO : DB �ε�
            //************************************
            lastDateTime = new DateTime(2022, 02, 20, 0, 0, 0);
            nowDateTime = GetGoogleDateTime();
            Debug.Log("nowDateTime : " + nowDateTime);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        
    }

    private void OnApplicationQuit()
    {
        //TODO :DB����
        //************************************
        lastDateTime = new DateTime(2022, 02, 10, 0, 0, 0);
    }



    /// <summary>
    /// ���� �ð� �����͸� �о�´�.
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
                string date = request.GetResponseHeader("date"); //�̰����� �ݼ۵� �����Ϳ� �ð� �����Ͱ� ����
                Debug.Log("�޾ƿ� �ð�" + date); // GMT�� �޾ƿ´�.
                DateTime dateTime = DateTime.Parse(date).ToLocalTime(); // ToLocalTime() �޼ҵ�� �ѱ��ð����� ��ȯ���� �ش�.
                Debug.Log("�ѱ��ð����κ�ȯ" + dateTime);
            }
        }
    }

    /// <summary>
    /// ���� �ð� �����͸� �о�´�.
    /// </summary>
    /// <returns></returns>
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

        TimeSpan compareDateTime = nowDateTime - lastDateTime;
        
        //�ð��� ���ѵα�
        if(compareDateTime.TotalDays > maxConpareDays)
        {
            compareDateTime = new TimeSpan(maxConpareDays,0,0,0);
        }
        Debug.Log("Offline time calculate is " + (int)compareDateTime.TotalMinutes);
        return compareDateTime;
    }
}
