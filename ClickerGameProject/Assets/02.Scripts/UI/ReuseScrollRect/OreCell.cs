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

        //UI ����
        preview_icon.sprite = _iconAtlas.GetSprite("Icon_"+item.imageName);
        ChangeUI(item);


        //������ �� ���� ����
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
        if (state == CellState.Lock)
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

    private void ChangeUI(ProductCellData item)
    {
        //������ 0 (���� �� ����)
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
        //������ max
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
    /// ���¿� ���� ���� UI�� �����ش�.
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
    /// ���� ������ UI ������Ʈ�� Ű�ų� ����.
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
