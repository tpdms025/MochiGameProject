//ī�޶� ����� Ÿ�� �ػ��� ������, ������ �°� �����Ѵ�.
//Sprite ���� �ȼ� ������� Ÿ�� �ػ� ����� �����.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    [Tooltip("���� Ÿ�� �ػ󵵸� �����ϴ���")]
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


        #region �ؽ��� ���� �ȼ� ������� ���ߴ� ����

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = targetResolution.x / targetResolution.y;
        ////���α��� ����
        //if (screenRatio >= targetRatio)
        //{
        //    Camera.main.orthographicSize = targetResolution.y * 0.5f / PixelPerUnit;
        //}
        //else
        //{
        //    //���α��� ����
        float diffInSize = targetRatio / screenRatio;
        Camera.main.orthographicSize = targetResolution.y * diffInSize * 0.5f / PixelPerUnit;
        //    //== targetResolution.x * Screen.height * 0.5f / Screen.width
        //}

        #endregion

        //Camera.main.orthographicSize = targetResolution.x * Screen.height / Screen.width  * 0.5f / PixelPerUnit;

        //ī�޶��� viewport rect���� ���
        Camera.main.orthographicSize *= 1.0f-Camera.main.rect.position.y;

        //��������Ʈ ��ġ�� ī�޶� �ϴܿ� ��ġ
        Vector3 centerPos = new Vector3(targetSprite.transform.position.x, targetSprite.transform.position.y, transform.position.z);
        Vector3 distDiff = new Vector3(0, Camera.main.orthographicSize - targetSprite.bounds.size.y * 0.5f, 0);
        transform.position = centerPos + distDiff;
    }
}
