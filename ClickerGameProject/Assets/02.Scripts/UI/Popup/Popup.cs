//â�� ���� ��ư�� ������ �ݴ� ����� �ֽ��ϴ�.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    //ó���� â�� ������
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
        //â �ݱ�
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
