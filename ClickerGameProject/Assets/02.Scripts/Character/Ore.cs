using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Ore : MonoBehaviour
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
        ChangeOre(false);
    }
    private void OnDestroy()
    {
        onOreChanged -= ChangeOre;
    }


    private void ChangeOre(bool isFever)
    {
        //������ ������ ���� ��� ����
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
    /// ���� �̹����� �����Ѵ�.
    /// </summary>
    private void ReplaceResources(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }

    /// <summary>
    /// �ݶ��̴��� �缳���Ѵ�.
    /// </summary>
    private IEnumerator RefreshCollider(Collider2D _col)
    {
        _col.enabled = false;
        yield return null;
        _col.enabled = true;
        yield return null;
    }
}
