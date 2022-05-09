using UnityEngine;
using UnityEditor;
using System.IO;

public class BuildAssetBundles : MonoBehaviour
{

	/***********************************************************************
	 * �뵵 : MenuItem�� ����ϸ� �޴�â�� ���ο� �޴��� �߰��� �� �ֽ��ϴ�.		      
	 * (�Ʒ��� �ڵ忡���� Bundles �׸� ���� �׸����� Build AssetBundles �׸��� �߰�.)  
	 ***********************************************************************/
	[MenuItem("Bundles/Build AssetBundles")]
	static void BuildAllAssetBundles()
	{
		string assetBundleDirectory = "Assets/AssetBundles";//에셋번들의 파일경로.
		if (!Directory.Exists(assetBundleDirectory))        //해당 파일이 있는지 확인하고 없다면 새롭게 생성.
		{
			Directory.CreateDirectory(assetBundleDirectory);
		}

		/***********************************************************************
		* �̸� : BuildPipeLine.BuildAssetBundles()
	    * �뵵 : BuildPipeLine Ŭ������ �Լ� BuildAssetBundles()�� ���¹����� ������ݴϴ�.     
	    * �Ű��������� String ���� �ѱ�� �Ǹ�, ����� ���� ������ ������ ����Դϴ�. 
	    * ���� ��� Assets ���� ������ �����Ϸ��� "Assets/AssetBundles"�� �Է��ؾ��մϴ�.
	    ***********************************************************************/
		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
	}
}