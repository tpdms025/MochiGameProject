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

    [SerializeField] private CellState state;

    #region Fields

    [SerializeField] private Image bgImage;
    [SerializeField] private Image preview;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;
    [SerializeField] private TextMeshProUGUI jewelPerClickText;
    [SerializeField] private TextMeshProUGUI nextJewelPerClickText;
    [SerializeField] private TextMeshProUGUI buyCostText;

    [SerializeField] private GameObject lockPanel;

    #endregion 


    public override void UpdateData(int idx, IReuseCellData _cellData , int ClickIndexID = -1)
    {
        base.UpdateData(idx, _cellData);

        ProductCellData item = _cellData as ProductCellData;
        if (item == null)
            return;

        buyCost = item.cost;

        //UI ����
        preview.name = item.imageName;
        titleText.text = item.name;

        bool isMaxlevel = item.nextLevel.Equals(item.level);
        ToggleUIInfo(isMaxlevel);
        if (isMaxlevel)
        {
            levelText.text = String.Format("Lv.MAX");
            jewelPerClickText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(item.jewelPerClick));
        }
        else
        {
            levelText.text = string.Format("Lv.{0}", item.level);
            nextLevelText.text = string.Format("Lv.{0}", item.nextLevel);
            jewelPerClickText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(item.jewelPerClick));
            nextJewelPerClickText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(item.nextJewelPerClick));
            buyCostText.text = string.Format("Level Up<br>{0}", CurrencyParser.ToCurrencyString(item.cost));
        }


        //Debug.Log("ClickIndexID:" + ClickIndexID + " m_Index" + m_Index);

        if (m_Index == ClickIndexID)
        {
            bgImage.color = Color.yellow;
        }
        else
        {
            bgImage.color = Color.white;
        }

        state = item.cellState;
        ShowSlotUIForState();

        ChangeUpgradeBtnState(MoneyManager.Instance.Jewel);
    }

    #region Unity methods

    protected override void Awake()
    {
        base.Awake();
        SubscribeToUpgradeButtonEvents();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnsubscribeFromUpgradeButtonEvents();
    }

    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }
    #endregion


    /// <summary>
    /// �ٸ� ���� ��ġ�� �� ȣ��Ǹ�, UI�� �ٽ� �����Ѵ�.
    /// </summary>
    /// <param name="ClickIndexID"></param>
    /// <param name="ClickContent"></param>
    public override void RefreshUI(int ClickIndexID, IReuseCellData ClickContent)
    {
        base.RefreshUI(ClickIndexID, ClickContent);

        //Debug.Log(m_Index + "�� ���� Refresh�Ǿ��� ������ ���� "+ ClickIndexID);
        ProductCellData item = ClickContent as ProductCellData;
        if (item == null)
            return;

        //������ ���� ���� �����ϱ�
        if (m_Index == ClickIndexID)
        {
            buyCost = item.cost;
            //UI ����
            bool isMaxlevel = item.nextLevel.Equals(item.level);
            ToggleUIInfo(isMaxlevel);
            if (isMaxlevel)
            {
                levelText.text = string.Format("Lv.MAX");
                jewelPerClickText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(item.jewelPerClick));
            }
            else
            {
                levelText.text = string.Format("Lv.{0}", item.level);
                nextLevelText.text = string.Format("Lv.{0}", item.nextLevel);
                jewelPerClickText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(item.jewelPerClick));
                nextJewelPerClickText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(item.nextJewelPerClick));
                buyCostText.text = string.Format("Level Up<br>{0}", CurrencyParser.ToCurrencyString(item.cost));
            }

            bgImage.color = Color.yellow;
        }
        else
        {
            bgImage.color = Color.white;
        }

        state = item.cellState;
        ShowSlotUIForState();

        ChangeUpgradeBtnState(MoneyManager.Instance.Jewel);
    }




    /// <summary>
    /// ��ư�� ���� �� ȣ��Ǵ� �Լ�
    /// </summary>
    public void OnButtonIndexCellCallbackClick()
    {
        if (onClick_Index != null)
        {
            onClick_Index.Invoke(m_Index);
        }
    }


    #region UI ����

    /// <summary>
    /// ������ ��ȭ�� �°� ���׷��̵� ��ư�� ���¸� �����Ѵ�.
    /// </summary>
    /// <param name="jewel"></param>
    private void ChangeUpgradeBtnState(BigInteger jewel)
    {
        bool result;
        if (state.Equals(CellState.Lock))
        {
            result = false;
        }
        else
        {
            //������ �� �ִ��� �Ǻ�
            result = jewel >= buyCost ? true : false;
        }
        m_Button.interactable = result;
    }

    /// <summary>
    /// ���¿� ���� ���� UI�� �����ش�.
    /// </summary>
    /// <param name="_state"></param>
    private void ShowSlotUIForState()
    {

        switch (state)
        {
            case CellState.Lock:
                lockPanel.SetActive(true);
                break;

            case CellState.Unlock:
                lockPanel.SetActive(false);
                break;

            case CellState.MaxCompletion:
                lockPanel.SetActive(false);
                break;
        }
    }


    /// <summary>
    /// ���� UI ������Ʈ�� Ű�ų� ����.
    /// </summary>
    private void ToggleUIInfo(bool isMaxlevel)
    {
        m_Button.gameObject.SetActive(!isMaxlevel);
        nextLevelText.gameObject.SetActive(!isMaxlevel);
        nextJewelPerClickText.gameObject.SetActive(!isMaxlevel);
    }


    #endregion



    private void SubscribeToUpgradeButtonEvents()
    {
        onClick_Custom.AddListener(() => OnButtonIndexCellCallbackClick());
        //m_Button.onClick.AddListener(PurchaseUpgrade);
        MoneyManager.Instance.onJewelChanged += ChangeUpgradeBtnState;
    }

    private void UnsubscribeFromUpgradeButtonEvents()
    {
        onClick_Custom.RemoveListener(() => OnButtonIndexCellCallbackClick());
        //m_Button.onClick.RemoveListener(PurchaseUpgrade);
        MoneyManager.Instance.onJewelChanged -= ChangeUpgradeBtnState;
    }

}
