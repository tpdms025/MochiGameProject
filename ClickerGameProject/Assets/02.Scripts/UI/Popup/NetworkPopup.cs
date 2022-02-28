using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPopup : Popup
{

    private static NetworkPopup instance = null;
    public static NetworkPopup Instance
    {
        get { return instance; }
        set { instance = value; }
    }
    protected override void Awake()
    {
        base.Awake();
        //ΩÃ±€≈Ê
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        baseButton.onClick.AddListener(delegate { Application.Quit(); });

    }
    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        //TimerManager.Instance.onDisconnectInternet += ToggleOpenOrClose;
        //Debug.Log("OnEnable");
    }

    private void OnDisable()
    {
        //TimerManager.Instance.onDisconnectInternet -= ToggleOpenOrClose;

    }
}
