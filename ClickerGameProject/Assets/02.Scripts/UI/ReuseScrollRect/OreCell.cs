using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;



public class OreCell : UIReuseItemCell
{
    private BigInteger buyCost;

    [SerializeField] private CellState state;

    #region Fields

    [SerializeField] private SpriteAtlas _iconAtlas;
    [SerializeField] private Image bgImage;
    [SerializeField] private Image preview_border;
    [SerializeField] private Image preview_icon;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;
    [SerializeField] private TextMeshProUGUI jewelPerClickText;
    [SerializeField] private TextMeshProUGUI nextJewelPerClickText;
    [SerializeField] private TextMeshProUGUI buyCostText;

    [Header("Color Select")]
    [Space(10)]
    private Color32 normalTextColor;
    [SerializeField] private Color32 maxTextColor;
    [SerializeField] private Sprite normalBGSprite;
    [SerializeField] private Sprite normalPreviewSprite;
    [SerializeField] private Sprite selectedBGSprite;
    [SerializeField] private Sprite selectedPreviewSprite;
    [SerializeField] private Transform arrowObj1;
    [SerializeField] private Transform arrowObj2;
    private Transform silhouetteSprite;
    [SerializeField] private Transform lockPanel;

    #endregion

    #region Unity methods

    protected override void Awake()
    {
        base.Awake();
        SubscribeToUpgradeButtonEvents();
        normalTextColor = levelText.color;
        silhouetteSprite = preview_icon.transform.Find("Silhouette").transform;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnsubscribeFromUpgradeButtonEvents();
    }

    #endregion


    public override void UpdateData(int idx, IReuseCellData _cellData, int ClickIndexID = -1)
    {
        base.UpdateData(idx, _cellData);

        ProductCellData item = _cellData as ProductCellData;
        if (item == null)
            return;


        buyCost = item.cost;

        //UI 갱신
        preview_icon.sprite = _iconAtlas.GetSprite("Icon_"+item.imageName);
        ChangeUI(item);


        //선택한 셀 색상 변경
        if (m_Index == ClickIndexID)
        {
            bgImage.sprite = selectedBGSprite;
            preview_border.sprite = selectedPreviewSprite;
        }
        else
        {
            bgImage.sprite = normalBGSprite;
            preview_border.sprite = normalPreviewSprite;
        }

        state = item.cellState;
        ShowSlotUIForState();

        ChangeUpgradeBtnState(MoneyManager.Instance.Jewel);
    }



    /// <summary>
    /// 다른 셀이 터치될 때 호출되며, UI를 다시 갱신한다.
    /// </summary>
    /// <param name="ClickIndexID"></param>
    /// <param name="ClickContent"></param>
    public override void RefreshUI(int ClickIndexID, IReuseCellData ClickContent)
    {
        base.RefreshUI(ClickIndexID, ClickContent);

        //Debug.Log(m_Index + "의 셀이 Refresh되었고 선택한 셀은 "+ ClickIndexID);
        ProductCellData item = ClickContent as ProductCellData;
        if (item == null)
            return;

        //선택한 셀만 정보 갱신하기
        if (m_Index == ClickIndexID)
        {
            buyCost = item.cost;

            //UI 갱신
            ChangeUI(item);

            bgImage.sprite = selectedBGSprite;
            preview_border.sprite = selectedPreviewSprite;
        }
        else
        {
            bgImage.sprite = normalBGSprite;
            preview_border.sprite = normalPreviewSprite;
        }

        state = item.cellState;
        ShowSlotUIForState();

        ChangeUpgradeBtnState(MoneyManager.Instance.Jewel);
    }




    /// <summary>
    /// 버튼이 눌릴 때 호출되는 함수
    /// </summary>
    public void OnButtonIndexCellCallbackClick()
    {
        if (onClick_Index != null)
        {
            onClick_Index.Invoke(m_Index);
        }
    }


    #region UI 갱신

    /// <summary>
    /// 소유한 재화에 맞게 업그레이드 버튼의 상태를 변경한다.
    /// </summary>
    /// <param name="jewel"></param>
    private void ChangeUpgradeBtnState(BigInteger jewel)
    {
        bool result;
        if (state == CellState.Lock)
        {
            result = false;
        }
        else
        {
            //구매할 수 있는지 판별
            result = jewel >= buyCost ? true : false;
        }
        m_Button.interactable = result;
    }

    private void ChangeUI(ProductCellData item)
    {
        //레벨이 0 (구매 전 상태)
        if (item.level == 0)
        {
            ToggleUI_NextInfo(false);
            m_Button.gameObject.SetActive(true);
            silhouetteSprite.gameObject.SetActive(true);

            titleText.text = string.Format("???");
            levelText.text = string.Format("Lv.{0}", item.level);
            jewelPerClickText.text = string.Format("???");
            buyCostText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(item.cost));
        }
        //레벨이 max
        else if (item.nextLevel == item.level)
        {
            ToggleUI_NextInfo(false);
            m_Button.gameObject.SetActive(false);
            silhouetteSprite.gameObject.SetActive(false);

            titleText.text = item.name;
            levelText.text = string.Format("Lv.MAX");
            jewelPerClickText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(item.currentAmount));
        }
        else
        {
            ToggleUI_NextInfo(true);
            m_Button.gameObject.SetActive(true);
            silhouetteSprite.gameObject.SetActive(false);

            titleText.text = item.name;
            levelText.text = string.Format("Lv.{0}", item.level);
            jewelPerClickText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(item.currentAmount));
            nextLevelText.text = string.Format("Lv.{0}", item.nextLevel);
            nextJewelPerClickText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(item.nextAmount));
            buyCostText.text = string.Format("{0}", CurrencyParser.ToCurrencyString(item.cost));
        }
    }

    /// <summary>
    /// 상태에 대한 슬롯 UI를 보여준다.
    /// </summary>
    /// <param name="_state"></param>
    private void ShowSlotUIForState()
    {
        switch (state)
        {
            case CellState.Lock:
                lockPanel.gameObject.SetActive(true);
                levelText.color = normalTextColor;
                jewelPerClickText.color = normalTextColor;
                break;

            case CellState.Unlock:
                lockPanel.gameObject.SetActive(false);
                levelText.color = normalTextColor;
                jewelPerClickText.color = normalTextColor;
                break;

            case CellState.MaxCompletion:
                lockPanel.gameObject.SetActive(false);
                levelText.color = maxTextColor;
                jewelPerClickText.color = maxTextColor;
                break;
        }
    }


    /// <summary>
    /// 다음 정보의 UI 오브젝트를 키거나 끈다.
    /// </summary>
    private void ToggleUI_NextInfo(bool turnOn)
    {
        nextLevelText.gameObject.SetActive(turnOn);
        nextJewelPerClickText.gameObject.SetActive(turnOn);
        arrowObj1.gameObject.SetActive(turnOn);
        arrowObj2.gameObject.SetActive(turnOn);
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
