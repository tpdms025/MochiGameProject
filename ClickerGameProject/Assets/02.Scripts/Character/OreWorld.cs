using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class OreWorld : MonoBehaviour
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
        //DB를 읽어와 소유한 광석으로 교체한다.
        ChangeOre(false);
    }

    private void OnDestroy()
    {
        onOreChanged -= ChangeOre;
    }


    /// <summary>
    /// 광석을 교체한다.
    /// </summary>
    private void ChangeOre(bool isFever)
    {
        Database.ProductOriginData ownedData = DBManager.Inst.GetLastOreDataOwned();

        //소유한 광석이 없을 경우 리턴
        if (ownedData == null)
            return;

        string spriteName;
        if (isFever)
        {
            spriteName = "Ore_Fever";
        }
        else
        {
            spriteName = ownedData.spriteName;
        }

        //이미지 변경
        ReplaceResources(_SA.GetSprite(spriteName));

        //폴리곤 콜라이더의 영역을 다시 갱신한다.
        col.TryUpdateShapeToAttachedSprite();
    }

    /// <summary>
    /// 광석 이미지를 변경한다.
    /// </summary>
    private void ReplaceResources(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }

}
