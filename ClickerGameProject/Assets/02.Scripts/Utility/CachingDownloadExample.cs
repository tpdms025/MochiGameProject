using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using UnityEditor;

public class CachingDownloadExample : MonoBehaviour
{
	// ���� �ٿ� ���� ������ �ּ�
	public string BundleURL;
	// ������ ���
	private string localPath;
	//������ �̸�
	public string assetBundleName = "assetbundle_0";
	// ������ version
	public int version;

	private void Start()
	{
		StartCoroutine(DownloadAndCache());
	}

	private IEnumerator DownloadAndCache()
	{
        yield return null;
        //if (!File.Exists(localPath + assetBundleName)) //������ ���ÿ� �������� ������ => ���ÿ� ���� �ٿ�ε�
        //{
        //	if (!Directory.Exists(localPath)) //������ �������� ������
        //	{
        //		Directory.CreateDirectory(localPath); //���� ����
        //	}
        //	// cache ������ AssetBundle�� ���� ���̹Ƿ� ĳ�̽ý����� �غ� �� ������ ��ٸ�
        //	while (!Caching.ready)
        //	{
        //		yield return null;
        //	}
        //	// ���¹����� ĳ�ÿ� ������ �ε��ϰ�, ������ �ٿ�ε��Ͽ� ĳ�������� �����մϴ�.
        //	using (UnityWebRequest request = UnityWebRequest.Get(BundleURL + "/" + assetBundleName))
        //	{
        //		yield return request.SendWebRequest();  //��û�� �Ϸ�� ������ ���
        //		if (request.error != null)
        //              {
        //			throw new Exception("WWW �ٿ�ε忡 ������ ������ϴ�.:" + request.error);
        //              }
        //		//���� ��������� ������ ������ ���ÿ� ����
        //		File.WriteAllBytes(localPath + "/" + assetBundleName, request.downloadHandler.data);
        //		//Asset ��������
        //		AssetDatabase.Refresh(); 
        //	} // using���� File �� Font ó�� ��ǻ�� ���� �����Ǵ� ���ҽ����� ���� ���� ���� �ڿ��� �ǵ����ټ� �ֵ��� ����� ����
        //}
    }
}



