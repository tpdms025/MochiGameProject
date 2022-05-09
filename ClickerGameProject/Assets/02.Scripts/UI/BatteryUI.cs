using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatteryUI : MonoBehaviour
{
    // 배터리 충전중 표시 스프라이트
    [SerializeField] private Sprite chargeStateSprite;
    // 배터리가 부족하다는 표시 스프라이트
    [SerializeField] private Sprite fewStateSprite;

    // 배터리 상태 표시 이미지
    [SerializeField] private Image batteryStateImg;      
    // 배터리 모양 프레임 이미지
    [SerializeField] private Image batteryFrameImg;
    // 배터리 잔량 표시 이미지
    [SerializeField] private Image batteryLevelImg;      
    // 배터리 잔량 표시 텍스트
    [SerializeField] private TextMeshProUGUI batteryLevelText;      

    public void UpdateUI()
    {
        float batteryLevel = SystemInfo.batteryLevel;

        switch (SystemInfo.batteryStatus)
        {
            case BatteryStatus.Full:
            case BatteryStatus.Charging:

                batteryStateImg.sprite = chargeStateSprite;
                batteryStateImg.gameObject.SetActive(true);
                batteryLevelText.text = string.Format("{0}%", 100);
                batteryLevelImg.fillAmount = 1f;

                break;

            case BatteryStatus.Discharging:
                // 배터리가 부족한 상태일 경우
                if (batteryLevel < 0.1f) 
                {
                    batteryStateImg.sprite = fewStateSprite;
                    batteryStateImg.gameObject.SetActive(true);
                }
                else
                {
                    batteryStateImg.gameObject.SetActive(false);
                }
                batteryLevelText.text = string.Format("{0}%", batteryLevel*100);
                batteryLevelImg.fillAmount = batteryLevel;
                break;
        }
    }
}
