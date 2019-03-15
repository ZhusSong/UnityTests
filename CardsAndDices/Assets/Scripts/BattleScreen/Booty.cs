using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class Booty : MonoBehaviour {
    public GameObject[] BootysAndButton = new GameObject[4];   //战利品牌子实例数组
    private int WhichBooty=0;                                  //轮到第几个战利品牌子旋转，为3时为按钮
    private int Pivot_Rotate = 0;                              //战利品锚点旋转角度

    public List<string> Forest_Booty_Normal = new List<string>();
    public List<string> Forest_Booty_Rare = new List<string>();
    public List<string> Forest_Booty_SuperRare = new List<string>();

    public List<string> Forest_Booty_Food=new List<string>();
     private SignButton Booty_Button;                               //调用Button()
    private NewButton Booty_NewBut;                            //调用NewButton()
    //↓实例化
	void Start () {
        Booty_Button = GameObject.Find("BG_SA").GetComponent<SignButton>();
        Booty_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();

        Forest_Booty_Normal.Add("WEA_00000004");
        Forest_Booty_Normal.Add("FOO_00000001");
        Forest_Booty_Normal.Add("WEA_00000003");
        Forest_Booty_Normal.Add("WEA_00000002");


        Forest_Booty_Rare.Add("FOO_00000002");
      //  Forest_Booty_Normal.Add("MAT_00000001");
        Forest_Booty_Rare.Add("WEA_00000001");
        Forest_Booty_Rare.Add("PAN_00000001");
        Forest_Booty_Rare.Add("HEL_00000001");
        Forest_Booty_Rare.Add("CHE_00000001");

        Forest_Booty_SuperRare.Add("NEC_00000001");
        Forest_Booty_SuperRare.Add("RIN_00000001");

        Forest_Booty_Food.Add("FOO_00000002");
        Forest_Booty_Food.Add("FOO_00000001");
     
	}
    /// <summary>
    /// 给与战利品
    /// </summary>
    /// <param name="Count">战利品数目</param>
    public string GiveBooty()
    {
        int Odd = Random.Range(0, 101);
        if (Odd <= 60)
        {
            int which = Random.Range(0, Forest_Booty_Normal.Count);
        //    Debug.Log("Booty Count is " + Forest_Booty_Normal.Count+" and which is "+which);
            Debug.Log("Give this booty is " + Forest_Booty_Normal[which]);
            return Forest_Booty_Normal[which];
        }
        if (Odd > 60 && Odd <= 90)
        {
            int which = Random.Range(0, Forest_Booty_Rare.Count);
         //   Debug.Log("Booty Count is " + Forest_Booty_Rare.Count + " and which is " + which);
           Debug.Log("Give this booty is " + Forest_Booty_Rare[which]);
            return Forest_Booty_Rare[which];
        }
        if (Odd > 90)
        {
            int which = Random.Range(0, Forest_Booty_SuperRare.Count);
         //   Debug.Log("Booty Count is " + Forest_Booty_SuperRare.Count + " and which is " + which);
            Debug.Log("Give this booty is " + Forest_Booty_SuperRare[which]);
            return Forest_Booty_SuperRare[which];
        }
        return "1";
    }
    /// <summary>
    /// 给与战利品
    /// </summary>
    /// <param name="Count">战利品数目</param>
    public string GiveBooty(string which,string rare)
    {
        Debug.Log("Give booty");
        string WillGive=null;
   AAA:   if (rare == "ALL")  //全部
        {
            int Odd = Random.Range(0, 101);
            if (Odd <= 60)
            {
                int j = Random.Range(0, Forest_Booty_Normal.Count);
                //    Debug.Log("Booty Count is " + Forest_Booty_Normal.Count+" and which is "+which);
                WillGive = Forest_Booty_Normal[j];
            }
            if (Odd > 60 && Odd <= 90)
            {
                int j = Random.Range(0, Forest_Booty_Rare.Count);
                //   Debug.Log("Booty Count is " + Forest_Booty_Rare.Count + " and which is " + which);
                WillGive = Forest_Booty_Rare[j];
            }
            if (Odd > 90)
            {
                int j = Random.Range(0, Forest_Booty_SuperRare.Count);
                //   Debug.Log("Booty Count is " + Forest_Booty_SuperRare.Count + " and which is " + which);
                WillGive = Forest_Booty_SuperRare[j];
            }
        }
      if (WillGive.Substring(0, 3) == "FOO")
          goto AAA;

    bbb:    if (rare == "R")   //稀有及以上
        {
            int Odd = Random.Range(60, 101);
            if (Odd >= 60 && Odd <= 90)
            {
                int j = Random.Range(0, Forest_Booty_Rare.Count);
                //   Debug.Log("Booty Count is " + Forest_Booty_Rare.Count + " and which is " + which);
                WillGive = Forest_Booty_Rare[j];
            }
            if (Odd > 90)
            {
                int j = Random.Range(0, Forest_Booty_SuperRare.Count);
                //   Debug.Log("Booty Count is " + Forest_Booty_SuperRare.Count + " and which is " + which);
                WillGive = Forest_Booty_SuperRare[j];
            }
        }

        if (WillGive.Substring(0, 3) == "FOO")
            goto bbb;
        if (rare == "SR")  //超级稀有
        {
            int Odd = Random.Range(90, 101);
                int j = Random.Range(0, Forest_Booty_SuperRare.Count);
                //   Debug.Log("Booty Count is " + Forest_Booty_SuperRare.Count + " and which is " + which);
                WillGive = Forest_Booty_SuperRare[j];
        }
        switch (which)
        {
            case "EQU":
                 
                break;
            case "ALL":
                break;
            case "FOO":
                int j = Random.Range(0, 2);
                WillGive = Forest_Booty_Food[j];
                break;
            default :
                break;

        }
        Debug.Log("Give booty  over"+" booty is "+WillGive);
        return WillGive;
    }


    //↓进行战利品牌子的从左至右依次下落
    //  当牌子全部下落完毕后，通知NewButton()进行按钮的翻转，并将计数重置
    //  被Battle()中ToBooty()调用
  /*  public void RotateDown()
    {
            Sequence Booty_Rotate = DOTween.Sequence();       
            Booty_Rotate.SetEase(Ease.OutCubic);//使用Dotween实现旋转=
            Booty_Rotate.Append(BootysAndButton[WhichBooty].transform.DORotate(new Vector3(0, 0, 0), 0.3f));
            Booty_Rotate.Append(BootysAndButton[WhichBooty].transform.DORotate(new Vector3(60, 0, 0), 0.15f));
            Booty_Rotate.Append(BootysAndButton[WhichBooty].transform.DORotate(new Vector3(0, 0, 0), 0.15f));
            Booty_Rotate.Append(BootysAndButton[WhichBooty].transform.DORotate(new Vector3(45, 0, 0), 0.13f));
            Booty_Rotate.Append(BootysAndButton[WhichBooty].transform.DORotate(new Vector3(0, 0, 0), 0.05f));
            Booty_Rotate.Append(BootysAndButton[WhichBooty].transform.DORotate(new Vector3(20, 0, 0), 0.08f));
            Booty_Rotate.Append(BootysAndButton[WhichBooty].transform.DORotate(new Vector3(0, 0, 0), 0.08f));
            if (WhichBooty < 3)
            {
                WhichBooty++;
                Booty_Rotate.OnComplete(RotateDown);
            }
            else  if (WhichBooty == 3)
            {
                Booty_NewBut.ButtonsNumber = 1;
              //  Booty_NewBut.GetButtonsNumber("Booty");
                WhichBooty = 0;
            }
    }
    //↓进行被点击到的牌子的相应事件，which为点击到的是哪个牌子
    //  被此脚本中的Update()调用
    //  
    public void RotateUp(int Which)
    {
        Tweener RotateUP;
        if (Which < 3)
        {
            RotateUP = BootysAndButton[Which].transform.DORotate(new Vector3(90, 0, 0), 0.3f);
            RotateUP.SetEase(Ease.OutQuad);
        }
        if (Which == 3)
        {
            Tweener UP1 =  BootysAndButton[0].transform.DORotate(new Vector3(90, 0, 0), 0.5f);
            UP1.SetEase(Ease.OutQuad);
            Tweener UP2 = BootysAndButton[1].transform.DORotate(new Vector3(90, 0, 0), 0.5f);
            UP2.SetEase(Ease.OutQuad);
            Tweener UP3 = BootysAndButton[2].transform.DORotate(new Vector3(90, 0, 0), 0.5f);
            UP3.SetEase(Ease.OutQuad);
        }
    }
	//↓进行射线检测，根据碰撞到的物体名检测是哪个牌子，并通知RotateUp()将点击的牌子升起
	void Update () {
        Ray ray_Booty = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit_Booty = new RaycastHit();
        if (Input.GetMouseButtonUp(0))
        {
            if (Physics.Raycast(ray_Booty, out hit_Booty))
            {
                for (int i = 0; i < 3; i++)
                {
                //    if (hit_Booty.transform.name == BootysAndButton[i].name.Replace("Pivot", "")) 
                 //       RotateUp(i);
                }
              
            }
        }
	}*/
}
