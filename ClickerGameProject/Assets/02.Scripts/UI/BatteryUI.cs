using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatteryUI : MonoBehaviour
{
    // ���͸� ������ ǥ�� ��������Ʈ
    [SerializeField] private Sprite chargeStateSprite;
    // ���͸��� �����ϴٴ� ǥ�� ��������Ʈ
    [SerializeField] private Sprite fewStateSprite;

    // ���͸� ���� ǥ�� �̹���
    [SerializeField] private Image batteryStateImg;      
    // ���͸� ��� ������ �̹���
    [SerializeField] private Image batteryFrameImg;
    // ���͸� �ܷ� ǥ�� �̹���
    [SerializeField] private Image batteryLevelImg;      
    // ���͸� �ܷ� ǥ�� �ؽ�Ʈ
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
                // ���͸��� ������ ������ ���
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
