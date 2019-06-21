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
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class HBErrorShow : EditorWindow
{
	private static HBErrorShow errorShow;
	
	public  static List<string> pictures = new List<string>();
	public  static List<string> materials = new List<string>();
	Vector2 scrollPos;
	public static void Init(List<string> _pictures,List<string> _materials)
	{
		errorShow = (HBErrorShow)EditorWindow.GetWindow(typeof(HBErrorShow), false, "MyWindow", false);
		pictures = _pictures;
		materials = _materials;
		errorShow.Show(true);
	}
	
	HBErrorShow ()
	{
		titleContent = new GUIContent("Show Error Info");
	}
	



	void OnGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.Label("错误资源显示", new GUIStyle()
		{
			alignment = TextAnchor.MiddleCenter,
			fontSize = 20,
			normal = new GUIStyleState()
			{
				textColor = Color.red
			}
		});

		GUILayout.Space(15);
	
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		GUILayout.Label("错误图片:", new GUIStyle(){
			alignment = TextAnchor.MiddleLeft,
			fontSize = 20,
			normal = new GUIStyleState()
			{
				textColor = Color.green
			}
		});
		foreach (string str in pictures )
		{
			Texture tex=AssetDatabase.LoadAssetAtPath<Texture>(str);
			tex = EditorGUILayout.ObjectField("本次的错误信息:"+" 路径为:"+str+"",tex,typeof(Texture),true)as Texture;
		}
		
		GUILayout.Label("错误材质:", new GUIStyle(){
			alignment = TextAnchor.MiddleLeft,
			fontSize = 20,
			normal = new GUIStyleState()
			{
				textColor = Color.green
			}
		});
		GUILayout.Space(10);
		foreach (string str in materials )
		{
			Material max=AssetDatabase.LoadAssetAtPath<Material>(str);
			max= EditorGUILayout.ObjectField("本次的错误信息:"+" 路径为:"+str+"",max,typeof(Material),true)as Material;
		}
		
		EditorGUILayout.EndScrollView();
		GUILayout.EndVertical();
		
	}
}
