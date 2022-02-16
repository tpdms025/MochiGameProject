using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsWindowController : MonoBehaviour
{
    [SerializeField] private List<WindowWithAds> windowList;
    private bool isBlocker = false;


    private void Start()
    {
        foreach(WindowWithAds window in windowList)
        {
            //window.ToggleEvent.AddListener(ToggleOpenOrClose(window.transform));
                
        }
    }


    public void Btn_TouchUpWindow()
    {
        ToggleOpenOrClose(windowList[0]);
    }

    public void Btn_JewelUpWindow()
    {
        ToggleOpenOrClose(windowList[1]);
    }


    #region Private Methods

    public void ToggleOpenOrClose(WindowWithAds window)
    {
        if (isBlocker)
        {
            CloseWindow(window.transform);
        }
        else
        {
            OpenWindow(window.transform);
        }
        isBlocker = !isBlocker;
    }

    private void OpenWindow(Transform window)
    {
        window.gameObject.SetActive(true);
    }

    private void CloseWindow(Transform window)
    {
        window.gameObject.SetActive(false);
    }
    #endregion
}
