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
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;
    [SerializeField] private TextMeshProUGUI jewelPerClickText;
    [SerializeField] private TextMeshProUGUI nextJewelPerClickText;
    [SerializeField] private TextMeshProUGUI buyCostText;

    public Button purchaseButton;
    [SerializeField] private GameObject lockPanel;
    #endregion 


    public override void UpdateData(int idx, IReuseCellData _cellData)
    {
        base.UpdateData(idx, _cellData);

        ProductCellData item = _cellData as ProductCellData;
        if (item == null)
            return;

        state = item.purchaseState;
        buyCost = item.cost;

        //UI 갱신
        preview.name = item.imageName;
        titleText.text = item.name;
        levelText.text = string.Format("Lv.{0}", item.level);
        nextLevelText.text = string.Format("Lv.{0}", item.nextLevel);
        jewelPerClickText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(item.jewelPerClick));
        nextJewelPerClickText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(item.nextJewelPerClick));
        buyCostText.text = string.Format("Level Up<br>{0}", CurrencyParser.ToCurrencyString(item.cost));

        ////purchaseButton.onClick.AddListener(delegate { OnPurchase(item.cost, item.jewelPerClick, ref item.isPurchased); });
        SetLockUI(state);
        ChangeStateOfUpgradeBtn(MoneyManager.Instance.Jewel);
    }

    private void OnEnable()
    {
        SubscribeToPurchaseButtonEvents();
    }
    private void OnDisable()
    {
        UnsubscribeFromPurchaseButtonEvents();
    }


    public override void RefreshUI(string ClickUniqueID, IReuseCellData ClickContent)
    {
        base.RefreshUI(ClickUniqueID, ClickContent);
    }


    /// <summary>
    /// 업그레이드 버튼의 상태를 변경한다.
    /// </summary>
    /// <param name="jewel"></param>
    private void ChangeStateOfUpgradeBtn(BigInteger jewel)
    {
        bool result;
        if (state.Equals(ProductCellData.PurchaseState.Lock))
        {
            result = false;
        }
        else
        {
            //구매할 수 있는지 판별
            result = jewel >= buyCost ? true : false;
        }
        purchaseButton.interactable = result;
    }


    /// <summary>
    /// 아이템을 업그레이드한다.
    /// </summary>
    private void PurchaseUpgrade()
    {
        if (state.Equals(ProductCellData.PurchaseState.Unlock))
        {
            MoneyManager.Instance.SubJewel(buyCost);
            MoneyManager.Instance.AddJewelPerClick(jewelPerClick);

            state = ProductCellData.PurchaseState.Select;
            SetLockUI(state);

        }
    }

    private void SetLockUI(ProductCellData.PurchaseState _state)
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
        purchaseButton.onClick.AddListener(PurchaseUpgrade);
        MoneyManager.Instance.onJewelChanged += ChangeStateOfUpgradeBtn;
    }

    private void UnsubscribeFromPurchaseButtonEvents()
    {
        purchaseButton.onClick.RemoveListener(PurchaseUpgrade);
        MoneyManager.Instance.onJewelChanged -= ChangeStateOfUpgradeBtn;
    }

}
