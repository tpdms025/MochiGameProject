//카메라 사이즈를 타겟 해상도의 사이즈, 비율에 맞게 조절한다.
//Sprite 실제 픽셀 사이즈와 타겟 해상도 사이즈를 맞춘다.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    [Tooltip("직접 타겟 해상도를 지정하는지")]
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

        //세로길이 고정
        //if(screenRatio >= targetRatio)
        //{
        //    Camera.main.orthographicSize = targetResolution.y * 0.5f / PixelPerUnit;
        //}
        //else
        //{
        //가로길이 고정
        float diffInSize = targetRatio / screenRatio;
        Camera.main.orthographicSize = targetResolution.y * 0.5f * diffInSize / PixelPerUnit * Camera.main.rect.position.y;
        // == targetResolution.x * Screen.height * 0.5f / Screen.width
        //}
    }
}
