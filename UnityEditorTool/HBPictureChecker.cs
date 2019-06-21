using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using AL_Effect;
using behaviac;
using DG.DemiEditor;
using Google.Protobuf.WellKnownTypes;
using LitJson;
//using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;


public class HBPictureChecker : EditorWindow {
#if MAPEDITOR
	[MenuItem("Custom/HBPictureChecker", priority = 0)]
	static void OpenMapEditor()
	{
		GetWindow(typeof(HBPictureChecker));
	}
#endif
	
	
	HBPictureChecker()
	{
		titleContent = new GUIContent("Check Resources");
	}

	private enum PixelSize
	{
		Size_64=64,
		Size_128=128,
		Size_256=256,
		Size_512=512,
		Size_1024=1024,
		
	}

	private PixelSize chooseSize;
	private string wantFileName;
	private string dontWantFileName;
	private string wantMaterialName;

	void OnGUI()
	{
		GUILayout.BeginVertical();
		
		
		
		GUILayout.Space(10);
		GUILayout.Label("图片与材质检查器", new GUIStyle()
		{
			alignment = TextAnchor.MiddleCenter,
			fontSize = 20,
			normal = new GUIStyleState()
			{
				textColor = Color.red
			}
		});
		
		chooseSize = (PixelSize) EditorGUILayout.EnumPopup("图片分辨率大小", chooseSize);
		if (chooseSize == null)
			chooseSize = (PixelSize) EditorGUILayout.EnumPopup("图片分辨率大小", PixelSize.Size_256);
		GUILayout.Space(15);
		wantFileName = EditorGUILayout.TextField("想要查询的文件夹名,以;号分隔", wantFileName);
		GUILayout.Space(15);
		dontWantFileName = EditorGUILayout.TextField("不想查询的文件夹名,以;号分隔", dontWantFileName);
		GUILayout.Space(15);
		wantMaterialName = EditorGUILayout.TextField("想查询的材质名", wantMaterialName);
		GUILayout.Space(15);
		if (wantMaterialName == null)
		{
			wantMaterialName =	EditorGUILayout.TextField("想查询的材质名", "Matcap"); ;
		}
		
		
		GUILayout.Label("开始遍历Resources文件夹下的所有图片与材质文件！");
		if (GUILayout.Button("开始遍历", GUILayout.Height(40)))
		{
			StartCheck();
		}
		
		GUILayout.EndVertical();
	}

	void StartCheck()
	{
		DirectoryInfo direction=new DirectoryInfo(Application.dataPath + "/Resources");

		List<string> pictures = new List<string>();
		List<string> materials = new List<string>();

		FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
		int count = files.Length;
		int i = 0;

		foreach (var file in files)
		{
			i += 1;
			EditorUtility.DisplayProgressBar("进度", "查询进度:" + i + "/" + (float) count, i / (float) count);

			 if (file.Extension == ".png" || file.Extension == ".jpg" || file.Extension == ".tga")
			{
				string path = file.DirectoryName;
				string oppositePath = path.Remove(0, path.IndexOf("Assets"));
				string allPath = oppositePath + "\\" + file.Name;
				Texture tex = AssetDatabase.LoadAssetAtPath<Texture>(oppositePath + "\\" + file.Name);

				bool find = false;
				if (dontWantFileName != null)
				{
					
					string[] dontWant = dontWantFileName.Split(';');
					if (dontWantFileName.Length>1)
					{
						foreach (string  str in dontWant)
						{
							if (path.Contains(str))
								find = true;
							else
							{
								find = false;
							}
						}
					}
				}

				if (wantFileName != null)
				{
					string[] want = wantFileName.Split(';');
			
				
					if (wantFileName.Length>1)
					{
						foreach (string  str in want)
						{
							if (!path.Contains(str))
								find = true;
							else
							{
								find = false;
							}
						}
					}
				}
			
				if (find)
					continue;
				
				
				if (tex.height > (int) chooseSize || tex.width > (int) chooseSize)
				{
					pictures.Add(allPath);
				}

			}

			if (file.Extension == ".mat")
			{
				string path = file.DirectoryName;
				string oppositePath = path.Remove(0, path.IndexOf("Assets"));

				string allPath = oppositePath + "\\" + file.Name;
				
				Material mat = AssetDatabase.LoadAssetAtPath<Material>(oppositePath + "\\" + file.Name);
				bool find = false;
				
				if (dontWantFileName != null)
				{
					
					string[] dontWant = dontWantFileName.Split(';');
					if (dontWantFileName.Length>1)
					{
						foreach (string  str in dontWant)
						{
							if (path.Contains(str))
								find = true;
							else
							{
								find = false;
							}
						}
					}
				}

				if (wantFileName != null)
				{
					string[] want = wantFileName.Split(';');
			
				
					if (wantFileName.Length>1)
					{
						foreach (string  str in want)
						{
							if (!path.Contains(str))
								find = true;
							else
							{
								find = false;
							}
						}
					}
				}
			
				if (find)
					continue;
				
				if (mat.shader.name.Contains(wantMaterialName))
				{
					materials.Add(allPath);

				}
			}
		}

		
		
		EditorUtility.ClearProgressBar();
		EditorUtility.DisplayDialog("消息", "遍历完成！请在弹出窗口查看信息！", "OJBK");
		HBErrorShow.Init(pictures,materials);


	}
	
	public static void ClickShow()
	{
		
	}
}
