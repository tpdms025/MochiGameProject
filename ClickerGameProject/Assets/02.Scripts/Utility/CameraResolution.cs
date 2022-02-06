//ī�޶� ����� Ÿ�� �ػ��� ������, ������ �°� �����Ѵ�.
//Sprite ���� �ȼ� ������� Ÿ�� �ػ� ����� �����.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    [Tooltip("���� Ÿ�� �ػ󵵸� �����ϴ���")]
    public bool isCustom = false;

    public SpriteRenderer targetSprite;
    public Vector2 targetResolution;
    private int PixelPerUnit = 100;

    private void Start()
    {
        if (!isCustom && targetSprite != null)
        {
            targetResolution = new Vector2(targetSprite.bounds.size.x, targetSprite.bounds.size.y);
            PixelPerUnit = 1;
        }


        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = targetResolution.x / targetResolution.y;

        //���α��� ����
        //if(screenRatio >= targetRatio)
        //{
        //    Camera.main.orthographicSize = targetResolution.y * 0.5f / PixelPerUnit;
        //}
        //else
        //{
        //���α��� ����
        float diffInSize = targetRatio / screenRatio;
        Camera.main.orthographicSize = targetResolution.y * 0.5f * diffInSize / PixelPerUnit * Camera.main.rect.position.y;
        // == targetResolution.x * Screen.height * 0.5f / Screen.width
        //}
    }
}
