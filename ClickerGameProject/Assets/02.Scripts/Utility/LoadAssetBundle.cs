using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LoadAssetBundle : MonoBehaviour
{
    // ������ ���
    private string localPath;
    //������ �̸�
    public string assetBundleName = "assetbundle_0";

    private void Start()
    {
        StartCoroutine(InstantiateObject());    //ȣ�� ����
    }

    private IEnumerator InstantiateObject()
    {
        AssetBundle bundle = AssetBundle.LoadFromFile(localPath+"/"+ assetBundleName);

        AssetBundleRequest request = bundle.LoadAllAssetsAsync<GameObject>(); //���鿡�� ��� ���� �ε�
        yield return request;

        //GameObject[] assets = bundle.LoadAllAssets<GameObject>(); //���鿡�� ��� ���� �ε�
        GameObject[] obj = request.allAssets as GameObject[];

        for (int i=0;i< obj.Length;i++) //��� ������ Ž���ϸ鼭
        {
            obj[i].GetComponent<Transform>().Translate(new Vector3(2, 0f, 1*i));
            Instantiate(obj[i]); //������Ʈ ����
        }

        bundle.Unload(false);
    }
}