using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class MissionInfor : MonoBehaviour {
    GameObject Mission;                   //脚本挂载的物体
    public GameObject InforCode;
    public GameObject[] Infors=new GameObject[5];
    private PlayerData MI_PD;
    private ScriptsManager MI_SM;
    public NewUIManager MI_NUIM;
    public int NNNN = 0;
  /*  private bool HaveIt=false;                  //判断是否点击到物体
    private bool CanRotate=false;               //判断是否可进行旋转
    private float Rot_Speed = 1.0f;              //牌子旋转速度
    private float Distance = 0;                 //鼠标拖动距离与起始位置差值
    private Vector3 ScreenPosition;              //点击到的物体的屏幕坐标值      
    private Vector3 offset;                      //点击到的物体的坐标与鼠标点击点的屏幕坐标的偏差值，消除点击位置的向量对移动造成的不平滑影响。
    private Vector3 StartPos = new Vector3();//鼠标点击的起始位置
    private Ray Ray_MI;
    private RaycastHit Hit_MI;*/
	// Update is called once per frame
   public  void L_Start()
    {
         
        MI_SM = GameObject.Find("Main Camera").GetComponent<ScriptsManager>();
        MI_PD = MI_SM.PD;
        MI_NUIM = MI_SM.NUIM;
      
        Mission = this.gameObject;

      /*  Infors[0].GetComponent<TextMesh>().text += MI_PD.GivePlayerData("HP");
        Infors[1].GetComponent<TextMesh>().text += MI_PD.GivePlayerData("Atk");
        Infors[2].GetComponent<TextMesh>().text += MI_PD.GivePlayerData("Def");
        Infors[3].GetComponent<TextMesh>().text += MI_PD.GivePlayerData("ActNum");
        Infors[4].GetComponent<TextMesh>().text += MI_PD.GivePlayerData("Satiety");*/
        ChangeInfor();
        RotateDown();
    }
   /// <summary>
   /// 改变角色信息
   /// </summary>
   /// <param name="which">哪一个字段</param>
   public void ChangeInfor()
   {
       Debug.Log("HP is "+PlayerData.HP);
       NNNN += 1;
      
     //  else
    //   {
           Infors[0].GetComponent<TextMesh>().text = null;
           Infors[0].GetComponent<TextMesh>().text = "生命:" + PlayerData.HP;

           Infors[2].GetComponent<TextMesh>().text = null;
           Infors[2].GetComponent<TextMesh>().text = "防御:" + PlayerData.Def;

           Infors[1].GetComponent<TextMesh>().text = null;
           Infors[1].GetComponent<TextMesh>().text = "攻击力:" + PlayerData.Atk;

           Infors[3].GetComponent<TextMesh>().text = null;
           Infors[3].GetComponent<TextMesh>().text = "行动力:" + PlayerData.ActNum;

           Infors[4].GetComponent<TextMesh>().text = null;
           Infors[4].GetComponent<TextMesh>().text = "饱食度:" + PlayerData.Sat;
  //     }
           if (NNNN >= 3 && PlayerData.HP <= 0)
               MI_NUIM.GameOver();
   }
    public void ChangeInformation_MI()
    {
        Tweener RotUp = Mission.transform.DORotate(new Vector3(0,270,0), 0.6f);
        RotUp.SetEase(Ease.OutCubic);
        RotUp.OnComplete(delegate() { Invoke("RotateDown", 0.2f); });


    }
    public void RotateDown()
    {
         Sequence Mission_Rotate = DOTween.Sequence(); 
        Mission_Rotate.Append(Mission.transform.DORotate(new Vector3(0, 0, 0), 0.4f));
        Mission_Rotate.Append(Mission.transform.DORotate(new Vector3(0, 45, 0), 0.15f));
        Mission_Rotate.Append(Mission.transform.DORotate(new Vector3(0, 0, 0), 0.15f));
        Mission_Rotate.Append(Mission.transform.DORotate(new Vector3(0, 20, 0), 0.1f));
        Mission_Rotate.Append(Mission.transform.DORotate(new Vector3(0, 0, 0), 0.1f));
        Mission_Rotate.SetEase(Ease.OutCubic);

    
    }
    /*void OnGUI()
    {
        GUI.Label(new Rect(20, 80, 100, 100), "Distance is " + Distance);
    }*/
}

