using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Ore : MonoBehaviour
{
    //광석이 변경될 때 호출되는 이벤트 델리게이트 함수
    public static System.Action<bool> onOreChanged;

    //광석 스프라이트 렌더러
    private SpriteRenderer spriteRenderer;

    private PolygonCollider2D col;


    //광석 아틀라스 텍스쳐
    [SerializeField] private SpriteAtlas _SA;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<PolygonCollider2D>();
        onOreChanged += ChangeOre;
    }

    private void Start()
    {
        ChangeOre(false);
    }
    private void OnDestroy()
    {
        onOreChanged -= ChangeOre;
    }


    private void ChangeOre(bool isFever)
    {
        //소유한 광석이 없을 경우 리턴
        if (DBManager.Inst.GetLastOreDataOwned() == null)
            return;

        string spriteName;
        if (isFever)
        {
            spriteName = "Ore_Fever";
        }
        else
        {
            spriteName = DBManager.Inst.GetLastOreDataOwned().spriteName;
        }

        ReplaceResources(_SA.GetSprite(spriteName));

        //StartCoroutine(RefreshCollider(col));
        col.TryUpdateShapeToAttachedSprite();
    }

    /// <summary>
    /// 광석 이미지를 변경한다.
    /// </summary>
    private void ReplaceResources(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }

    /// <summary>
    /// 콜라이더를 재설정한다.
    /// </summary>
    private IEnumerator RefreshCollider(Collider2D _col)
    {
        _col.enabled = false;
        yield return null;
        _col.enabled = true;
        yield return null;
    }
}
