using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum CellType { OnePurchase, Upgrade, Observation }



public class ProductCell : UIReuseItemCell
{
    private BigInteger buyCost;
    private BigInteger jewelPerClick;
    [SerializeField] private ProductCellData.PurchaseState state;
    #region Fields
    [SerializeField] private Image bgImage;
    [SerializeField] private Image preview;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI jewelPerClickText;
    [SerializeField] private TextMeshProUGUI buyCostText;

    public Button purchaseButton;
    [SerializeField] private GameObject lockPanel;
    #endregion 


    public override void UpdateData(int idx, IReuseCellData _CellData)
    {
        base.UpdateData(idx, _CellData);

        ProductCellData item = _CellData as ProductCellData;
        if (item == null)
            return;

        state = item.purchaseState;
        buyCost = item.cost;

        //UI 갱신
        preview.name = item.imageName;
        titleText.text = item.name;
        jewelPerClickText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(item.jewelPerClick));
        buyCostText.text = string.Format("Cost {0}", CurrencyParser.ToCurrencyString(item.cost));

        ////purchaseButton.onClick.AddListener(delegate { OnPurchase(item.cost, item.jewelPerClick, ref item.isPurchased); });
        UpdateUI(state);
        OnPurchaseBtnChanged(MoneyManager.Instance.Jewel);
    }

    private void Awake()
    {
        SubscribeToPurchaseButtonEvents();
    }
    private void OnDestroy()
    {
        UnsubscribeFromPurchaseButtonEvents();
    }


    public override void RefreshUI(string ClickUniqueID, IReuseCellData ClickContent)
    {
        base.RefreshUI(ClickUniqueID, ClickContent);
    }


    private void OnPurchaseBtnChanged(BigInteger jewel)
    {
        bool result;
        if (state.Equals(ProductCellData.PurchaseState.Lock))
        {
            result = true;
        }
        else
        {
            result = jewel >= buyCost ? true : false;
        }
        purchaseButton.interactable = result;
    }


    /// <summary>
    /// 아이템을 구입한다.
    /// </summary>
    //public void OnPurchase(BigInteger needCost, BigInteger jewelPerClick, ref bool _isPurchased)
    private void OnPurchase()
    {
        if (state.Equals(ProductCellData.PurchaseState.Unlock))
        {
            state = ProductCellData.PurchaseState.Select;
            UpdateUI(state);


            MoneyManager.Instance.SubJewel(buyCost);
            MoneyManager.Instance.AddJewelPerClick(jewelPerClick);
        }
    }

    private void UpdateUI(ProductCellData.PurchaseState _state)
    {
        bgImage.color = Color.white;
        switch (state)
        {
            case ProductCellData.PurchaseState.Lock:
                OnDisableCell();
                break;
            case ProductCellData.PurchaseState.Unlock:
                OnEnableCell();
                break;
            case ProductCellData.PurchaseState.Select:
                bgImage.color = Color.yellow;
                OnEnableCell();
                break;
            case ProductCellData.PurchaseState.Have:
                OnEnableCell();
                break;
        }
    }



    private void OnDisableCell()
    {
        lockPanel.SetActive(true);
    }
    private void OnEnableCell()
    {
        lockPanel.SetActive(false);
    }

    private void SubscribeToPurchaseButtonEvents()
    {
        purchaseButton.onClick.AddListener(OnPurchase);
        MoneyManager.Instance.onJewelChanged += OnPurchaseBtnChanged;
    }

    private void UnsubscribeFromPurchaseButtonEvents()
    {
        purchaseButton.onClick.RemoveListener(OnPurchase);
        MoneyManager.Instance.onJewelChanged -= OnPurchaseBtnChanged;
    }

}
