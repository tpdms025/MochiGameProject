using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using UnityEditor;

public class CachingDownloadExample : MonoBehaviour
{
	// 번들 다운 받을 서버의 주소
	public string BundleURL;
	// 로컬의 경로
	private string localPath;
	//번들의 이름
	public string assetBundleName = "assetbundle_0";
	// 번들의 version
	public int version;

	private void Start()
	{
		StartCoroutine(DownloadAndCache());
	}

	private IEnumerator DownloadAndCache()
	{
        yield return null;
        //if (!File.Exists(localPath + assetBundleName)) //번들이 로컬에 존재하지 않으면 => 로컬에 번들 다운로드
        //{
        //	if (!Directory.Exists(localPath)) //폴더가 존재하지 않으면
        //	{
        //		Directory.CreateDirectory(localPath); //폴더 생성
        //	}
        //	// cache 폴더에 AssetBundle을 담을 것이므로 캐싱시스템이 준비 될 때까지 기다림
        //	while (!Caching.ready)
        //	{
        //		yield return null;
        //	}
        //	// 에셋번들을 캐시에 있으면 로드하고, 없으면 다운로드하여 캐시폴더에 저장합니다.
        //	using (UnityWebRequest request = UnityWebRequest.Get(BundleURL + "/" + assetBundleName))
        //	{
        //		yield return request.SendWebRequest();  //요청이 완료될 때까지 대기
        //		if (request.error != null)
        //              {
        //			throw new Exception("WWW 다운로드에 에러가 생겼습니다.:" + request.error);
        //              }
        //		//파일 입출력으로 서버의 번들을 로컬에 저장
        //		File.WriteAllBytes(localPath + "/" + assetBundleName, request.downloadHandler.data);
        //		//Asset 리프레쉬
        //		AssetDatabase.Refresh(); 
        //	} // using문은 File 및 Font 처럼 컴퓨터 에서 관리되는 리소스들을 쓰고 나서 쉽게 자원을 되돌려줄수 있도록 기능을 제공
        //}
    }
}



