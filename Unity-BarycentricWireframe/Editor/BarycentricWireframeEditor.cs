#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class BarycentricWireframeEditor : EditorWindow
{
	private string cachePath = "Assets/ModelsCache";
	
	[MenuItem("Tools/Barycentric Wireframe Editor")]
	public static void ShowWindow()
	{
		GetWindow<BarycentricWireframeEditor>("Barycentric Wireframe");
	}
	
	void OnGUI()
	{
		GUILayout.Space(10);
		
		cachePath = EditorGUILayout.TextField("Cache Path", cachePath);
		
		GUILayout.Space(10);
		
		if (Selection.gameObjects != null)
		{
			float objectsCount = Selection.gameObjects.Length;
			if (objectsCount > 0)
			{
				string buttonTitle = (objectsCount == 1)? "Process 1 Object": "Process " + objectsCount + " Objects";
				if (GUILayout.Button(buttonTitle))
				{
					BakeSelectedMeshes();
				}
			}
			else
			{
				GUILayout.Label("Please select one or more mesh objects");
			}
		}
		
		GUILayout.Space(10);
		
		GUILayout.Label("Creates cached mesh assets with barycentric coordinates stored as tangent vectors for use in wireframe shaders.", EditorStyles.wordWrappedLabel);
	}
	
	void BakeSelectedMeshes()
	{
		if (!Directory.Exists(cachePath))
			Directory.CreateDirectory(cachePath);
		
		foreach (GameObject go in Selection.gameObjects)
		{
			MeshFilter mf = go.GetComponent<MeshFilter>();
			if (mf == null)
			{
				Debug.LogWarning($"Skipping '{go.name}' — no MeshFilter.");
				continue;
			}
			
			Mesh meshSource;
			var barycentricComp = go.GetComponent<BarycentricWireframe>();
			
			if (barycentricComp == null)
			{
				barycentricComp = go.AddComponent<BarycentricWireframe>();
				barycentricComp.meshSource = mf.sharedMesh;
				meshSource = barycentricComp.meshSource;
				Debug.Log($"[{go.name}] Added Barycentric Wireframe component and set Mesh Source.");
			}
			else
			{
				meshSource = barycentricComp.meshSource;
				if (meshSource == null)
				{
					Debug.LogWarning($"[{go.name}] has Barycentric Wireframe component but no Mesh Source set.");
					continue;
				}
			}
			
			string filename = Path.Combine(cachePath, meshSource.name + "_barycentric.asset");
			bool shouldRebake = true;
			
			if (File.Exists(filename))
			{
				string assetPath = filename.Replace(Application.dataPath, "Assets");
				Mesh existing = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
				string fullSourcePath = AssetDatabase.GetAssetPath(meshSource);
				if (!string.IsNullOrEmpty(fullSourcePath))
				{
					string sourceFullPath = Path.Combine(Directory.GetCurrentDirectory(), fullSourcePath);
					string existingFullPath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);
					
					if (File.GetLastWriteTimeUtc(sourceFullPath) <= File.GetLastWriteTimeUtc(existingFullPath))
					{
						mf.sharedMesh = existing;
						Debug.Log($"[{go.name}] Already cached: {assetPath}");
						shouldRebake = false;
					}
				}
			}
			
			if (shouldRebake)
			{
				Mesh processed = BarycentricWireframeUtility.CreateBarycentricMesh(meshSource);
				string assetPath = filename.Replace(Application.dataPath, "Assets");
				AssetDatabase.CreateAsset(processed, assetPath);
				AssetDatabase.SaveAssets();
				mf.sharedMesh = processed;
				Debug.Log($"[{go.name}] Barycentric mesh generated and saved: {assetPath}");
			}
		}
	}
}
#endif
