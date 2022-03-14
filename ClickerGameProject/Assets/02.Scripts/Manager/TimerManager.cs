using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class TimerManager : MonoBehaviour
{
    public  const string url = "http://www.google.com";

    //���ø����̼��� ������ ��¥
    public DateTime lastDateTime { get; private set; }

    //���ø����̼��� ������ ��¥ (����)
    public DateTime nowDateTime { get; private set; }

    //�������� �ð��� �ִ� ����
    private const int maxConpareDays = 1;

    //�������� �ð� (�ִ� 1��)
    public TimeSpan offlineTimeSpan { get; private set; }



    //���ͳ� ���� ����
    private bool isConnectedInternet = false;

    //���ͳ� ������ ������ ��� ȣ���ϴ� �̺�Ʈ
    public event Action onDisconnectInternet;


    public static TimerManager Instance { get; private set; }

    #region Unity methods

    private void Awake()
    {
        //�̱���
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
                Debug.Log("DontDestroyOnLoad");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(CheckInternet());
    }

    private void OnApplicationQuit()
    {
        //TODO :DB����
        //************************************
        lastDateTime = new DateTime(2022, 02, 10, 0, 0, 0);
    }
    #endregion

    private IEnumerator CheckInternet()
    {
        //TODO :
        //************************************
        //GPGS ������ �Ǵ� �ð����� ��ü�� ��
        yield return new WaitForSeconds(1.0f);


        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //���ͳ� ������ �ȵǾ��� �� �ൿ
            isConnectedInternet = false;

            if (onDisconnectInternet != null)
            {
                onDisconnectInternet.Invoke();
                Debug.Log("onDisconnectInternet");
            }
        }
        else
        {
            //������ �� �������̷� ���ͳ� ������ �Ǿ��� �� �ൿ
            isConnectedInternet = true;

            yield return StartCoroutine(WebChk());

            //TODO : DB �ε�
            //************************************
            lastDateTime = new DateTime(2022, 02, 28, 15, 0, 0);
            //nowDateTime = GetGoogleDateTime();
            //Debug.Log("nowDateTime : " + nowDateTime);

            offlineTimeSpan = CalculateOfflineTime();

            //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        yield return null;
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
                nowDateTime = dateTime;

                //��� �ӽ�
                yield return new WaitForSeconds(2);
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
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
    private TimeSpan CalculateOfflineTime()
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
