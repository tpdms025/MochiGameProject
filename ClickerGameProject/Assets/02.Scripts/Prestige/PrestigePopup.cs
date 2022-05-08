using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PrestigePopup : Popup
{
    [SerializeField] private TextMeshProUGUI titleText;
    public Button prestigeButton;
    [SerializeField] private Transform effectTrans;

    protected override void Awake()
    {
        base.Awake();
        prestigeButton.onClick.AddListener(() => effectTrans.gameObject.SetActive(true));
    }

    public void UpdateUI(int totalBonusPercent)
    {
        effectTrans.gameObject.SetActive(false);
        titleText.text = string.Format("ÅÍÄ¡´ç È¹µæ·® <#FBD253>{0}</color>%", totalBonusPercent);
    }



}
