using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class HanoiTower : MonoBehaviour {

	public GameObject Plate;                                            //盘子预设体
public GameObject[] Pillar=new GameObject[3];                           //柱子预设体
	
	public  List<GameObject> StartPlates=new List<GameObject>();        //起始柱子上的盘子列表
	public  List<GameObject> TransitionPlates=new List<GameObject>();   //过渡柱子上的盘子列表
	public  List<GameObject> EndPlates=new List<GameObject>();          //结束柱子上的盘子列表
	
	public List<GameObject> AllPlates=new List<GameObject>();           //总的盘子列表
	
	public int ObjCount;                                                //输入的步数，最大限制为7

	private int moveCount = 0;                                          //步数

	private Color[] plateColors=new Color[7]{Color.red, Color.yellow, Color.blue, Color.green, Color.cyan, Color.black, Color.magenta};
	// Use this for initialization
	void Start ()
	{
		if (ObjCount > 7)
			ObjCount = 7;
		//创建盘子
		for (int i = ObjCount; i > 0; i--) 
		{
			GameObject plates = Instantiate(Plate,
				new Vector3(Pillar[0].transform.position.x, 
					Pillar[0].transform.position.y-0.65f+0.15f*i,
					Pillar[0].transform.position.z),
				new Quaternion(0, 0, 0, 0));
			plates.transform.localScale=new Vector3(1-0.1f*i,0.1f,1-0.1f*i);
			plates.name = "palte_" + (ObjCount-i);
			plates.GetComponent<MeshRenderer>().materials[0].color = plateColors[ObjCount-i];

			AllPlates.Add(plates);
			StartPlates.Add(plates);

		}

		StartCoroutine(StartHanioMove(ObjCount,StartPlates,TransitionPlates,EndPlates,"start","transition","end"));
		
	}

	IEnumerator StartHanioMove(int n,List<GameObject> StartPos,List<GameObject>  TransitionPos,List<GameObject> EndPos,string startName,string transitionName,string endName)
	{
		if (n==0)
		{
			yield break;
		}
		else
		{
			yield return  StartHanioMove(n-1,StartPos,EndPos,TransitionPos,startName,endName,transitionName);
			yield return Move(n,StartPos,EndPos,startName,endName);
			yield return StartHanioMove(n-1,TransitionPos,StartPos,EndPos,transitionName,startName,endName);
		}
	}

	
	IEnumerator Move(int n,List<GameObject> StartPos,List<GameObject> EndPos,string startName,string endName)
	{
		yield return new WaitForSeconds(0.5f);
		EndPos.Add(AllPlates[n-1]);
			
		StartPos.Remove(AllPlates[n-1]);
		Debug.Log( string.Format("第{0}步，将{1}盘子从{2}柱移到{3}柱,{4}柱上的盘子数为{5}",moveCount++,AllPlates[n-1].name,startName,endName,endName,EndPos.Count));
				switch (endName)
				{
					case "end":
						AllPlates[n-1].transform.position=new Vector3(
							Pillar[2].transform.position.x,
							Pillar[2].transform.position.y-0.65f+0.15f*EndPlates.Count,
							Pillar[2].transform.position.z
							);
						break; 
					case "start":
						AllPlates[n-1].transform.position=new Vector3(
							Pillar[0].transform.position.x,
							Pillar[0].transform.position.y-0.65f+0.15f*StartPlates.Count,
							Pillar[0].transform.position.z
							);
						break;
					case  "transition":
						AllPlates[n-1].transform.position=new Vector3(
							Pillar[1].transform.position.x,
							Pillar[1].transform.position.y-0.65f+0.15f*TransitionPlates.Count,
							Pillar[1].transform.position.z
							);
						break;
					
				}
			
				
	}

	IEnumerator wait()
	{
		yield return new WaitForSeconds(1f);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
