using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class OreWorld : MonoBehaviour
{
    //������ ����� �� ȣ��Ǵ� �̺�Ʈ ��������Ʈ �Լ�
    public static System.Action<bool> onOreChanged;

    //���� ��������Ʈ ������
    private SpriteRenderer spriteRenderer;

    private PolygonCollider2D col;


    //���� ��Ʋ�� �ؽ���
    [SerializeField] private SpriteAtlas _SA;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<PolygonCollider2D>();
        onOreChanged += ChangeOre;
    }

    private void Start()
    {
        //DB�� �о�� ������ �������� ��ü�Ѵ�.
        ChangeOre(false);
    }

    private void OnDestroy()
    {
        onOreChanged -= ChangeOre;
    }


    /// <summary>
    /// ������ ��ü�Ѵ�.
    /// </summary>
    private void ChangeOre(bool isFever)
    {
        Database.ProductOriginData ownedData = DBManager.Inst.GetLastOreDataOwned();

        //������ ������ ���� ��� ����
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

        //�̹��� ����
        ReplaceResources(_SA.GetSprite(spriteName));

        //������ �ݶ��̴��� ������ �ٽ� �����Ѵ�.
        col.TryUpdateShapeToAttachedSprite();
    }

    /// <summary>
    /// ���� �̹����� �����Ѵ�.
    /// </summary>
    private void ReplaceResources(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }

}
