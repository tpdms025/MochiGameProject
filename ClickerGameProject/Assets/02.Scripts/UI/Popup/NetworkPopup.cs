using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPopup : Popup
{
    protected override void Awake()
    {
        base.Awake();
     
        baseButton.onClick.AddListener(delegate { ApplicationQuit(); });

    }
    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        //TimerManager.Inst.onDisconnectInternet += ToggleOpenOrClose;
        //Debug.Log("OnEnable");
    }

    private void OnDisable()
    {
        //TimerManager.Inst.onDisconnectInternet -= ToggleOpenOrClose;

    }

    private void ApplicationQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
