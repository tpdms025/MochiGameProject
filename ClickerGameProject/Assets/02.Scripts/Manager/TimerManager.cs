using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class TimerManager : MonoBehaviour
{
    //���ø����̼��� ������ ��¥
    public DateTime lastDateTime { get; private set; }

    //���ø����̼��� ������ ��¥ (����)
    public DateTime nowDateTime { get; private set; }

    //�������� �ð��� �ִ� ����
    private readonly int maxConpareDays = 1;

    //�������� �ð� (�ִ� 1��)
    public TimeSpan offlineTimeSpan { get; private set; }





    public readonly string url = "http://www.google.com";

    ////���ͳ� ������ üũ�ϴ� ��� �ð�
    //private readonly float waitingTime = 5.0f;

    //���ͳ� ���� ����
    private bool isConnect;

    ////���ͳ� ������ ������ ��� ȣ���ϴ� �̺�Ʈ
    //public event Action onDisconnectInternet;

    [SerializeField] private NetworkPopup popup;


    public static TimerManager Inst { get; private set; }

    #region Unity methods

    private void Awake()
    {
        //�̱���
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    //private void OnApplicationQuit()
    //{
    //    //TODO :DB����
    //    lastDateTime = new DateTime(2022, 02, 10, 0, 0, 0);
    //}
    #endregion


    public void LoadDateData()
    {
        lastDateTime = DBManager.Inst.playData.lastDateTime;
        nowDateTime = System.DateTime.Now;
        offlineTimeSpan = CalculateOfflineTime();
#if UNITY_EDITOR
        Debug.Log("lastDateTime" + lastDateTime);
        Debug.Log("nowDateTime" + nowDateTime);
        Debug.Log("offlineTimeSpan" + offlineTimeSpan);
#endif
    }

    #region Load Google Date

    private void OnDisconnectInternet()
    {
        //�˾�â ���
        //popup.ToggleOpenOrClose();
    }

    private IEnumerator CheckInternetConnection(Action<bool> syncResult)
    {
        //TODO :
        //************************************
        //GPGS ������ �Ǵ� �ð����� ��ü�� ��
        //yield return new WaitForSeconds(1.0f);


        bool result;
        using (UnityWebRequest request = UnityWebRequest.Head(url))
        {
            request.timeout = 10;
            //URL�� �����Ͽ� ������� �ҷ��ö����� ���
            yield return request.SendWebRequest();
            //������ �߻��ߴ��� üũ
            result = !request.isNetworkError && !request.isHttpError && request.responseCode == 200;
        }
        syncResult(result);


        #region test
        //if (Application.internetReachability == NetworkReachability.NotReachable)
        //{
        //    //���ͳ� ������ �ȵǾ��� �� �ൿ
        //}
        //else
        //{
        //    //������ �� �������̷� ���ͳ� ������ �Ǿ��� �� �ൿ

        //    yield return new WaitForSeconds(2);
        //    UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        //}
        #endregion
        yield return null;
    }



    /// <summary>
    /// ���� �ð� �����͸� �о�´�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadWebTime()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
                isConnect=false;
            }
            else
            {
                isConnect=true;
                string date = request.GetResponseHeader("date"); //�̰����� �ݼ۵� �����Ϳ� �ð� �����Ͱ� ����
                Debug.Log("�޾ƿ� �ð�" + date); // GMT�� �޾ƿ´�.
                DateTime dateTime = DateTime.Parse(date).ToLocalTime(); // ToLocalTime() �޼ҵ�� �ѱ��ð����� ��ȯ���� �ش�.
                Debug.Log("�ѱ��ð����κ�ȯ" + dateTime);
                nowDateTime = dateTime;
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

    #endregion



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
        return compareDateTime;
    }
}
