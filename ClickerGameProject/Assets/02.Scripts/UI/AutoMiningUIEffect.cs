using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoMiningUIEffect : MonoBehaviour
{
    private RectTransform bg;
    private RectTransform mochikingImg;
    private RectTransform text;

    public Ease ease;

    private void Awake()
    {
        bg = transform.Find("Background").GetComponent<RectTransform>();
        mochikingImg = transform.Find("MochiKingImg").GetComponent<RectTransform>();
        text = transform.Find("Texts").GetComponent<RectTransform>();
    }

    /// <summary>
    /// 이펙트 연출
    /// </summary>
    public void OnEffect()
    {
        Initialize();
        StartCoroutine(Cor_PlayAnimation());
    }



    private void Initialize()
    {
        bg.localPosition = new Vector3(-1080.0f, 0f, 0f);
        mochikingImg.localPosition = new Vector3(-1230.0f, 0f,0f);
        text.localPosition = new Vector3(-855.0f, -45.0f, 0f);
    }


    /// <summary>
    /// DoTween으로 애니메이션을 시작한다.
    /// </summary>
    private IEnumerator Cor_PlayAnimation()
    {
        bg.DOAnchorPosX(0f,0.3f).SetEase(ease);
        mochikingImg.DOAnchorPosX(-150.0f, 0.2f).SetDelay(0.2f).SetEase(ease);
        text.DOAnchorPosX(225.0f, 0.2f).SetDelay(0.4f).SetEase(ease);

        yield return new WaitForSeconds(2.0f);

        bg.DOAnchorPosX(1080f, 0.2f).SetEase(ease);
        mochikingImg.DOAnchorPosX(-150f + 1080f, 0.2f).SetEase(ease);
        text.DOAnchorPosX(225f + 1080f, 0.2f).SetEase(ease);
    }
}
