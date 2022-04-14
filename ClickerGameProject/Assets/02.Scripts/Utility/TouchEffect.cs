using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TouchEffect : MonoBehaviour
{
    //�̵� �ӵ�
    [SerializeField] private float moveSpeed = 2.0f;

    //�̵� �ð�
    [SerializeField] private float moveTime = 0.3f;

    //��� �ð�
    private float elapsedTime;

    //�̵���ų ������Ʈ ��ġ
    private RectTransform moveObj;

    //ĵ���� �׷�
    private CanvasGroup group;

    //��ȭ ���� ǥ�� �ؽ�Ʈ
    private TextMeshProUGUI text;

    //�θ� ��ƼŬ
    private ParticleSystem particleParent;


    private void Awake()
    {
        moveObj = transform.Find("JewelText").GetComponent<RectTransform>();
        group = moveObj.GetComponent<CanvasGroup>();
        text = moveObj.GetComponentInChildren<TextMeshProUGUI>();
        particleParent = transform.Find("UIParticle System-Circle").GetComponent<ParticleSystem>();
    }


    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    /// <param name="jewelStr"></param>
    public void Init(string jewelStr)
    {
        elapsedTime = 0;
        group.alpha = 1;
        moveObj.anchoredPosition = Vector3.zero;
        text.text = string.Format("+{0}", jewelStr);
       
        //��ƼŬ ���
        particleParent.Stop(true);
        if (particleParent.isStopped)
        {
            particleParent.Play(true);
        }
        
        //�ؽ�Ʈ �̵�
        StartCoroutine(Cor_MoveText());
    }

    private IEnumerator Cor_MoveText()
    {
        //������ �ð����� ���� ������ ��ġ �̵�
        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            group.alpha = Mathf.Lerp(1, 0, elapsedTime/moveTime);
            //moveObj.anchoredPosition = Vector2.Lerp(Vector2.zero, new Vector2(0, 70), elapsedTime / moveTime);
            moveObj.anchoredPosition += new Vector2(0, moveSpeed*100*Time.deltaTime);
            yield return null;
        }
        group.alpha = 0;

        //������Ʈ Ǯ�� �ֱ�
        ObjectPool.Instance.PushToPool("TouchEffect",this.gameObject, true);
    }

}
