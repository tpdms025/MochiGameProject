using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkmanSpawner : MonoBehaviour
{
    //스폰 위치들
    [SerializeField] private Vector3[] spawnPoints;

    //투사체의 최고점 배열
    [SerializeField] private Vector3[] maxPoints;

    //투사체의 목표점 배열
    [SerializeField] private Vector3[] targetPoints;


    //복사할 프리팹
    private GameObject clonePrefab;

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

        for (int i=0; i< spawnPoints.Length;i++)
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

        onSpawned += SpawnWorkman;
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
        Workman workmanObj= CreateWorkman(pointToSpawn).GetComponent<Workman>();

        //짝수번호라면 좌우 반전
        if(idSelected >> 1 == 0)
        {
            workmanObj.GetComponent<SpriteRenderer>().flipX = true;
        }
        //Vector3 targetPoint = localToWorldPoint(spawnPoints[idSelected] + new Vector3(3.0f, 0f, 0f));
        //Vector3 maxPoint = localToWorldPoint(spawnPoints[idSelected] + new Vector3(1.5f, 2.0f, 0f));
        Vector3 targetPoint = spawnPoints[idSelected] + new Vector3(3.0f, 0f, 0f);
        Vector3 maxPoint = spawnPoints[idSelected] + new Vector3(1.5f, 2.0f, 0f);

        workmanObj.Init(maxPoints[idSelected], targetPoints[idSelected]);
        StartCoroutine(workmanObj.FSM());
    }

    private Vector3 localToWorldPoint(Vector3 localPoint)
    {
        return transform.position + localPoint;
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
}
