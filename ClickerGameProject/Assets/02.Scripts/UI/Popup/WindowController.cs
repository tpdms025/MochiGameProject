using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    [SerializeField] private List<GameObject> windowList;
    private bool isBlocker = false;

    private void Start()
    {
    }

    public void ToggleOpenOrClose(GameObject window)
    {
        if(isBlocker)
        {
            CloseWindow(window);
        }
        else
        {
            OpenWindow(window);
        }
        isBlocker = !isBlocker;
    }

    private void OpenWindow(GameObject window)
    {
        window.SetActive(true);
    }
    private void CloseWindow(GameObject window)
    {
        window.SetActive(false);
    }
}
