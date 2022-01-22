using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CanvasGroup))]
public class FlotingGold : MonoBehaviour
{
    private float moveYSpeed = 2.0f;
    private float alphaSpeed = 5.0f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private float posY;
    private float alpha;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        posY = 0;
        alpha = 1;
        canvasGroup.alpha = alpha;
    }

    private void Update()
    {
        //오브젝트 위치
        MoveYAxis(rectTransform);

        //알파값
        alpha = Mathf.Lerp(alpha, 0, alphaSpeed * Time.deltaTime);
        canvasGroup.alpha = alpha;
    }

    private void MoveYAxis(RectTransform rect)
    {
        posY = Mathf.Lerp(posY,1,moveYSpeed * Time.deltaTime);
        rect.localPosition += new Vector3(0, posY *10.0f, 0);
    }
}
