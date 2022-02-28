using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;

    public static DialogBox Instance { get; private set; }


    private void Awake()
    {
        //ΩÃ±€≈Ê
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }


        button.onClick.AddListener(delegate { ToggleOpenOrClose(false); });
        ToggleOpenOrClose(false);
    }


    public void Display(string infoString)
    {
        ToggleOpenOrClose(true);
        text.text = infoString;
    }

    public void ToggleOpenOrClose(bool flag)
    {
        if (flag)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
