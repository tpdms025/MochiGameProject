using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LoadAssetBundle : MonoBehaviour
{
    // 로컬의 경로
    private string localPath;
    //번들의 이름
    public string assetBundleName = "assetbundle_0";

    private void Start()
    {
        StartCoroutine(InstantiateObject());    //호출 시작
    }

    private IEnumerator InstantiateObject()
    {
        AssetBundle bundle = AssetBundle.LoadFromFile(localPath+"/"+ assetBundleName);

        AssetBundleRequest request = bundle.LoadAllAssetsAsync<GameObject>(); //번들에서 모든 에셋 로드
        yield return request;

        //GameObject[] assets = bundle.LoadAllAssets<GameObject>(); //번들에서 모든 에셋 로드
        GameObject[] obj = request.allAssets as GameObject[];

        for (int i=0;i< obj.Length;i++) //모든 에셋을 탐색하면서
        {
            obj[i].GetComponent<Transform>().Translate(new Vector3(2, 0f, 1*i));
            Instantiate(obj[i]); //오브젝트 생성
        }

        bundle.Unload(false);
    }
}