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

        //버튼 이벤트 등록
        button.onClick.AddListener(popup.ToggleOpenOrClose);
        popup.prestigeButton.onClick.AddListener(_system.Replay);
    }

    private void OnEnable()
    {
        //화면에 보일때마다 보너스 UI를 갱신한다.
        _system.AwardBonus();
        UpdateUI(_system);
    }

    /// <summary>
    /// UI를 업데이트한다.
    /// </summary>
    private void UpdateUI(PrestigeSystem _system)
    {
        titleText.text = string.Format("환생 보너스 : 터치당 획득량 <#ffdc64>{0}</color>%", _system.totalBonusPercent);
        STRText.text = string.Format("근력 레벨 보너스: + <#ffdc64>{0}</color>/{1}(%)", _system.STRLevelBonus, _system.STR_MaxBonus);
        OreText.text = string.Format("광석 수집 보너스: + <#ffdc64>{0}</color>/{1}(%)", _system.oreCollectionBonus, _system.ore_MaxBonus);
        WorkmanText.text = string.Format("일꾼 수집 보너스: + <#ffdc64>{0}</color>/{1}(%)", _system.workmanCollectionBonus, _system.workman_MaxBonus);
        PrestigeCountText.text = string.Format("환생 횟수 보너스: + <#ffdc64>{0}</color>(%)", _system.prestigeCountBonus);

        button.interactable = _system.IsPrestigeable();

        if(button.interactable)
        {
            popup.UpdateUI(_system.totalBonusPercent);
        }
    }


}
