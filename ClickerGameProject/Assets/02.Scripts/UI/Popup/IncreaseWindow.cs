using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseWindow : WindowWithAds
{
    [SerializeField] private Button closeButton;


    protected override void SubscribeToButtonEvents()
    {
        base.SubscribeToButtonEvents();

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(delegate { ToggleOpenOrClose(); });
        }
    }
    protected override void UnsubscribeFromButtonEvents()
    {
        base.UnsubscribeFromButtonEvents();
    }
}
