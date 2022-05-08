using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkmanSpawner : MonoBehaviour
{
    [Header("Position")]

    //���� ��ġ��
    [SerializeField] private Vector3[] spawnPoints;

    //����ü�� �ְ��� �迭
    [SerializeField] private Vector3[] maxPoints;

    //����ü�� ��ǥ�� �迭
    [SerializeField] private Vector3[] targetPoints;



    [Space(10)]
    [Header("Time")]

    //idle->attack ������ ���� �ð�
    //(��� �ϲ۵��� ������ �����ϰ� �ϰ��� ��)
    [SerializeField] private readonly float patternTime = 1.216667f;

    //���� �ϲ��� ���� �ð�
    [SerializeField] private float curPatternTime = 0f;


    [Space(10)]

    //������ ������
    private GameObject clonePrefab;

    private LoadAnimator loadAnimator;

    //�ϲ��� ������ �� ȣ��Ǵ� �̺�Ʈ ��������Ʈ �Լ�
    public static System.Action<int> onSpawned;





    private void Awake()
    {
        //spawnPoints = new Vector3[]
        //{
        //    new Vector3(-3.27f,-0.08f), new Vector3(2.57f,-0.17f), 
        //    new Vector3(4.62f,0.68f), new Vector3(-4.27f,1.59f), 
        //    new Vector3(2.54f,1.62f),new Vector3(-2.62f,2.27f), 
        //    new Vector3(4.49f,2.84f), new Vector3(-4.7f,3.14f)
        //};
        spawnPoints = new Vector3[transform.Find("SpawnPoints").childCount];
        maxPoints = new Vector3[transform.Find("ProjectileMaxPoints").childCount];
        targetPoints = new Vector3[transform.Find("ProjectileTargetPoints").childCount];

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = transform.Find("SpawnPoints").GetChild(i).transform.position;
        }
        for (int i = 0; i < maxPoints.Length; i++)
        {
            maxPoints[i] = transform.Find("ProjectileMaxPoints").GetChild(i).transform.position;
        }
        for (int i = 0; i < targetPoints.Length; i++)
        {
            targetPoints[i] = transform.Find("ProjectileTargetPoints").GetChild(i).transform.position;
        }

        clonePrefab = Resources.Load("Prefabs/Workman") as GameObject;
        loadAnimator = GetComponent<LoadAnimator>();


        onSpawned += SpawnWorkman;
    }

    private void Start()
    {
        //�ϲ��� �ϳ��� �����ϰ� �ִٸ�
        if (DBManager.Inst.inventory.workmanCount > 0)
        {
            //������ �ϲ۵鸸 ����
            if (onSpawned != null)
            {
                for (int i = 0; i < DBManager.Inst.inventory.workmanLevels_owned.Length; i++)
                {
                    //������ 0���� ũ�� ������ ������ ����
                    if (DBManager.Inst.inventory.workmanLevels_owned[i] > 0)
                    {
                        onSpawned.Invoke(i);
                    }
                }
            }
        }
        StartCoroutine(Cor_CheckPatternTime());
    }


    private void OnDestroy()
    {
        onSpawned -= SpawnWorkman;
    }


    /// <summary>
    /// ������ id�� �ϲ��� �����Ѵ�.
    /// </summary>
    public void SpawnWorkman(int idSelected)
    {
        Vector2 pointToSpawn = spawnPoints[idSelected];
        Workman workmanObj = CreateWorkman(pointToSpawn).GetComponentInChildren<Workman>();

        //�����ʿ� ������ �¿����
        if (pointToSpawn.x < 0)
        {
            workmanObj.GetComponent<SpriteRenderer>().flipX = true;
        }

        workmanObj.SetAnimator(loadAnimator.GetAnimatorController(idSelected));
        workmanObj.Init(maxPoints[idSelected], targetPoints[idSelected],loadAnimator.GetSprite(idSelected));
        StartCoroutine(workmanObj.FSM(curPatternTime));
    }




    /// <summary>
    /// �ϲۿ�����Ʈ�� �����Ѵ�.
    /// </summary>
    private GameObject CreateWorkman(Vector3 pos)
    {
        GameObject obj = Object.Instantiate(clonePrefab) as GameObject;
        obj.transform.SetParent(transform);
        obj.transform.position = pos;
        obj.SetActive(true);

        return obj;
    }


    /// <summary>
    /// �ϲ� ���� ������ �ð��� Ȯ���Ѵ�.
    /// </summary>
    private IEnumerator Cor_CheckPatternTime()
    {
        //�ϲ� ���Ű� �ѹ��̻��� ���������� ���
        yield return new WaitUntil(() => DBManager.Inst.inventory.workmanCount > 0);

        yield return null;
        while (!DBManager.Inst.isGameStop)
        {
            curPatternTime += Time.deltaTime;
            if (curPatternTime >= patternTime)
            {
                curPatternTime = 0f;
            }
            yield return null;
        }
    }
}
