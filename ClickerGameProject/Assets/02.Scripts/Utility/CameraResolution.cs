//카메라 사이즈를 타겟 해상도의 사이즈, 비율에 맞게 조절한다.
//Sprite 실제 픽셀 사이즈와 타겟 해상도 사이즈를 맞춘다.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    [Tooltip("직접 타겟 해상도를 지정하는지")]
    public bool applyTargetSprite = false;

    public SpriteRenderer targetSprite;
    public Vector2 targetResolution;
    private int PixelPerUnit = 100;

    private void Awake()
    {
        if (targetSprite == null)
            return;

        if (applyTargetSprite == true)
        {
            targetResolution = new Vector2(targetSprite.bounds.size.x, targetSprite.bounds.size.y);
            PixelPerUnit = 1;
        }


        #region 텍스쳐 실제 픽셀 사이즈와 맞추는 버전

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = targetResolution.x / targetResolution.y;
        ////세로길이 고정
        //if (screenRatio >= targetRatio)
        //{
        //    Camera.main.orthographicSize = targetResolution.y * 0.5f / PixelPerUnit;
        //}
        //else
        //{
        //    //가로길이 고정
        float diffInSize = targetRatio / screenRatio;
        Camera.main.orthographicSize = targetResolution.y * diffInSize * 0.5f / PixelPerUnit;
        //    //== targetResolution.x * Screen.height * 0.5f / Screen.width
        //}

        #endregion

        //Camera.main.orthographicSize = targetResolution.x * Screen.height / Screen.width  * 0.5f / PixelPerUnit;

        //카메라의 viewport rect까지 계산
        Camera.main.orthographicSize *= 1.0f-Camera.main.rect.position.y;

        //스프라이트 위치를 카메라 하단에 배치
        Vector3 centerPos = new Vector3(targetSprite.transform.position.x, targetSprite.transform.position.y, transform.position.z);
        Vector3 distDiff = new Vector3(0, Camera.main.orthographicSize - targetSprite.bounds.size.y * 0.5f, 0);
        transform.position = centerPos + distDiff;
    }
}
