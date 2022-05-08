using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkmanSpawner : MonoBehaviour
{
    [Header("Position")]

    //스폰 위치들
    [SerializeField] private Vector3[] spawnPoints;

    //투사체의 최고점 배열
    [SerializeField] private Vector3[] maxPoints;

    //투사체의 목표점 배열
    [SerializeField] private Vector3[] targetPoints;



    [Space(10)]
    [Header("Time")]

    //idle->attack 까지의 패턴 시간
    //(모든 일꾼들의 패턴을 동일하게 하고자 함)
    [SerializeField] private readonly float patternTime = 1.216667f;

    //현재 일꾼의 패턴 시간
    [SerializeField] private float curPatternTime = 0f;


    [Space(10)]

    //복사할 프리팹
    private GameObject clonePrefab;

    private LoadAnimator loadAnimator;

    //일꾼이 스폰할 때 호출되는 이벤트 델리게이트 함수
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
        //일꾼을 하나라도 소유하고 있다면
        if (DBManager.Inst.inventory.workmanCount > 0)
        {
            //소유한 일꾼들만 스폰
            if (onSpawned != null)
            {
                for (int i = 0; i < DBManager.Inst.inventory.workmanLevels_owned.Length; i++)
                {
                    //레벨이 0보다 크면 구매한 것으로 간주
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
    /// 정해진 id의 일꾼을 스폰한다.
    /// </summary>
    public void SpawnWorkman(int idSelected)
    {
        Vector2 pointToSpawn = spawnPoints[idSelected];
        Workman workmanObj = CreateWorkman(pointToSpawn).GetComponentInChildren<Workman>();

        //오른쪽에 있으면 좌우반전
        if (pointToSpawn.x < 0)
        {
            workmanObj.GetComponent<SpriteRenderer>().flipX = true;
        }

        workmanObj.SetAnimator(loadAnimator.GetAnimatorController(idSelected));
        workmanObj.Init(maxPoints[idSelected], targetPoints[idSelected],loadAnimator.GetSprite(idSelected));
        StartCoroutine(workmanObj.FSM(curPatternTime));
    }




    /// <summary>
    /// 일꾼오브젝트를 생성한다.
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
    /// 일꾼 상태 패턴의 시간을 확인한다.
    /// </summary>
    private IEnumerator Cor_CheckPatternTime()
    {
        //일꾼 구매가 한번이상이 있을때까지 대기
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
