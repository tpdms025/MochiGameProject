//창을 열고 버튼을 누르면 닫는 기능이 있습니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    //처음에 창을 여는지
    [SerializeField] private bool firstOpen = false;

    [SerializeField] protected Button baseButton;



    protected virtual void Awake()
    {
        if (baseButton != null)
        {
            baseButton.onClick.AddListener(delegate { ToggleOpenOrClose(); });
        }
    }

    protected virtual void Start()
    {
        //창 닫기
        if(firstOpen == false)
        {
            ToggleOpenOrClose();
        }
    }

    public void ToggleOpenOrClose()
    {
        if (gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }


}
