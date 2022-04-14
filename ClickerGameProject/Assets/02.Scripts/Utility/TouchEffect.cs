using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TouchEffect : MonoBehaviour
{
    //이동 속도
    [SerializeField] private float moveSpeed = 2.0f;

    //이동 시간
    [SerializeField] private float moveTime = 0.3f;

    //경과 시간
    private float elapsedTime;

    //이동시킬 오브젝트 위치
    private RectTransform moveObj;

    //캔버스 그룹
    private CanvasGroup group;

    //재화 정보 표시 텍스트
    private TextMeshProUGUI text;

    //부모 파티클
    private ParticleSystem particleParent;


    private void Awake()
    {
        moveObj = transform.Find("JewelText").GetComponent<RectTransform>();
        group = moveObj.GetComponent<CanvasGroup>();
        text = moveObj.GetComponentInChildren<TextMeshProUGUI>();
        particleParent = transform.Find("UIParticle System-Circle").GetComponent<ParticleSystem>();
    }


    /// <summary>
    /// 초기화
    /// </summary>
    /// <param name="jewelStr"></param>
    public void Init(string jewelStr)
    {
        elapsedTime = 0;
        group.alpha = 1;
        moveObj.anchoredPosition = Vector3.zero;
        text.text = string.Format("+{0}", jewelStr);
       
        //파티클 재생
        particleParent.Stop(true);
        if (particleParent.isStopped)
        {
            particleParent.Play(true);
        }
        
        //텍스트 이동
        StartCoroutine(Cor_MoveText());
    }

    private IEnumerator Cor_MoveText()
    {
        //정해진 시간동안 알파 조정과 위치 이동
        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            group.alpha = Mathf.Lerp(1, 0, elapsedTime/moveTime);
            //moveObj.anchoredPosition = Vector2.Lerp(Vector2.zero, new Vector2(0, 70), elapsedTime / moveTime);
            moveObj.anchoredPosition += new Vector2(0, moveSpeed*100*Time.deltaTime);
            yield return null;
        }
        group.alpha = 0;

        //오브젝트 풀에 넣기
        ObjectPool.Instance.PushToPool("TouchEffect",this.gameObject, true);
    }

}
