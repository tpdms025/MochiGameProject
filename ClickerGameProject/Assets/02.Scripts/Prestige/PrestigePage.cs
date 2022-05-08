using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PrestigePage : MonoBehaviour
{
    private PrestigeSystem _system;


    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI STRText;
    [SerializeField] private TextMeshProUGUI OreText;
    [SerializeField] private TextMeshProUGUI WorkmanText;
    [SerializeField] private TextMeshProUGUI PrestigeCountText;

    [SerializeField] private Button button;
    [SerializeField] private PrestigePopup popup;


    private void Awake()
    {
        _system = GetComponent<PrestigeSystem>();

        //��ư �̺�Ʈ ���
        button.onClick.AddListener(popup.ToggleOpenOrClose);
        popup.prestigeButton.onClick.AddListener(_system.Replay);
    }

    private void OnEnable()
    {
        //ȭ�鿡 ���϶����� ���ʽ� UI�� �����Ѵ�.
        _system.AwardBonus();
        UpdateUI(_system);
    }

    /// <summary>
    /// UI�� ������Ʈ�Ѵ�.
    /// </summary>
    private void UpdateUI(PrestigeSystem _system)
    {
        titleText.text = string.Format("ȯ�� ���ʽ� : ��ġ�� ȹ�淮 <#ffdc64>{0}</color>%", _system.totalBonusPercent);
        STRText.text = string.Format("�ٷ� ���� ���ʽ�: + <#ffdc64>{0}</color>/{1}(%)", _system.STRLevelBonus, _system.STR_MaxBonus);
        OreText.text = string.Format("���� ���� ���ʽ�: + <#ffdc64>{0}</color>/{1}(%)", _system.oreCollectionBonus, _system.ore_MaxBonus);
        WorkmanText.text = string.Format("�ϲ� ���� ���ʽ�: + <#ffdc64>{0}</color>/{1}(%)", _system.workmanCollectionBonus, _system.workman_MaxBonus);
        PrestigeCountText.text = string.Format("ȯ�� Ƚ�� ���ʽ�: + <#ffdc64>{0}</color>(%)", _system.prestigeCountBonus);

        button.interactable = _system.IsPrestigeable();

        if(button.interactable)
        {
            popup.UpdateUI(_system.totalBonusPercent);
        }
    }


}
