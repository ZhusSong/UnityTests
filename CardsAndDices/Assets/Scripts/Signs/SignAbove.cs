using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System;
using TMPro;

public class SignAbove : MonoBehaviour {
    GameObject Sign;                             //牌子
   // public Sprite[] BG_Sign;                     //贴图集,BG_Sign[0]为普通场景贴图，[1]为战斗场景贴图
 
    public Sprite BigBg;                        //需要显示的贴图
    public Sprite EventBg;
    public GameObject MonsterCardsNode;
    private SpriteRenderer BigBg_Renderer;                   //SpriteRenderer组件
    private SpriteRenderer EventBg_Renderer;
    

   // public Texture2D Tex;                        //纹理

    private SignButton SA_Button;                    //调用Button()脚本
    private Move SA_Move;                        //调用Move()脚本
    private Battle SA_Battle;
    private CreateMonster SA_CreMon;
    private NewButton SA_NewBut;
    private int BgNumber = 0;                    //这次使用的背景图片，0为普通场景，1为战斗场景
    private int DelayTime = 0;
    public TextMeshPro Infors;                      //文本信息
  
   /* public enum CardsNameAndButtonNumber         //枚举，用于存放卡牌名字,以及对应的第一次下落时的按钮数量
    {
        BaoYu=1,
        GuaiSheng=2,
        HouQun=2,
        JiLiu=2,
        JuMang=2,
        LangQun=1,
        LuoNanZhiRen=2,
        ShouXue=2,
        ShuZhen=1,
        SiShi=2,
        XianJing=1,
        XuanYa=1,
        YiJi=2,
        YiShi=2,
        ZhaoZe=2,
        ChuanSongZhen=1,
        DuChuan=1,
        MaChe=1,
        ShanDong=1,
        MuShi=2,
        ShangRen=2,
        TuDiShen=2
    }      */
    //↓进行相应组件的实例化
	void Start () {
        Sign = this.gameObject;
        BigBg_Renderer = GameObject.Find("BG_SA").GetComponent<SpriteRenderer>();
        EventBg_Renderer = GameObject.Find("Cards_Bg").GetComponent<SpriteRenderer>();                    
  //      Bgs = GameObject.Find("BG_SA").GetComponent<SpriteRenderer>();                                            
      //  SA_Button = GameObject.Find("BG_SA").GetComponent<Button>();
        SA_Move = GameObject.Find("CreateMap").GetComponent<Move>();
        SA_CreMon = GameObject.Find("BG_Battle").GetComponent<CreateMonster>();
        SA_Battle = GameObject.Find("BG_Battle").GetComponent<Battle>();
        SA_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();

	}
    //↓创建这次的背景并控制主牌子下落,并根据传递过来的贴图创建背景（参数Which判断应该创建的是哪个背景，0为普通场景，1为战斗场景）
    // 被CreateMap()中CreateNext()调用 
    // 调用此脚本中 CanRotateDown()，并传递相应事件参数
    public void CreateImage(Sprite BBg,Sprite EveBg,int number)                                                                                   
    {
    //    BigBg = BBg;
        DelayTime = number;
     //   Infors.text = "完全OJBK";
            CanRotateDown();
    }

   //新函数
    public void CreateImage(string infors, int number)
    {
        //    BigBg = BBg;
        DelayTime = number;
        if (infors.Length <= 30)
            Infors.fontSize =6f;
        else if (infors.Length <= 50)
            Infors.fontSize = 5.5f;
        else if (infors.Length <= 70)
            Infors.fontSize = 5f;
       else 
                Infors.fontSize = 4.5f;
        Infors.text = infors;
        CanRotateDown();
    }
    //↓得到此次卡牌名字，被CreateMap()中CreateNext()调用
    //遍历CardsNameAndButtonNumber，找出名字相同的枚举变量并将按钮数目赋值给NewButton中变量 ButtonsNumber
  /*  public void GetName_SA(string name)                                                                          
    {
        foreach (string item in Enum.GetNames(typeof(CardsNameAndButtonNumber)))                                 
        {
            if (item.ToString() == name)
            {
                SA_NewBut.ButtonsNumber=(int)Enum.Parse(typeof(CardsNameAndButtonNumber), item.ToString());  
            }
        }
    }*/
    public void CanRotateDown()                                                                   //控制牌子落下，参数WhichEvent决定为何事件，0为无事件，被此脚本中CreateImage()调用 
    {
        Tweener RotateDown_MCN;
        RotateDown_MCN = MonsterCardsNode.transform.DORotate(new Vector3(0, 0, 0), 0.1f);
            Sequence Sign_Rotate = DOTween.Sequence();                                                               //使用Dotween实现旋转
            Sign_Rotate.Append(Sign.transform.DORotate(new Vector3(0, 0, 0), 0.35f));
            Sign_Rotate.Append(Sign.transform.DORotate(new Vector3(60, 0, 0), 0.18f));
            Sign_Rotate.Append(Sign.transform.DORotate(new Vector3(0, 0, 0), 0.18f));
            Sign_Rotate.Append(Sign.transform.DORotate(new Vector3(45, 0, 0), 0.11f));
            Sign_Rotate.Append(Sign.transform.DORotate(new Vector3(0, 0, 0), 0.09f));
            Sign_Rotate.Append(Sign.transform.DORotate(new Vector3(20, 0, 0), 0.06f));
            Sign_Rotate.Append(Sign.transform.DORotate(new Vector3(0, 0, 0), 0.06f));
            Sign_Rotate.SetEase(Ease.OutCubic);                                                                  //DoTween运动方式
         //   if (WhichEvent == 0)
         //   {
            Sign_Rotate.OnComplete(LetButtonRotate);                    
         //   }
        //    else if (WhichEvent == 1)
        //    {
        //        Sign_Rotate.OnComplete(delegate() { SA_CreMon.CanCreateMonster(3); });                           //旋转结束，事件为1，调用CreateMonster()中 CanCreateMonster()并传递怪物数量
         //   }
            SA_Move.DontMove_Move = 1;                                                                           //改变Move()中DontMove_Move为1，使地图卡牌不可拖动
    }
    //↓旋转结束
    private void RotateOver()
    {
        SA_Button.DestroyButton();
    }
    //↓控制牌子抬起
    //  被NewButton中CanRotate()调用
    public void CanRotateUp()
    {
        Infors.text = "";
        Tweener RotateUp;                                                                                        //使用Dotween实现旋转
        Tweener RotateUp_MCN;
        RotateUp=Sign.transform.DORotate(new Vector3(90, 0, 0), 0.5f);                    
        RotateUp.SetEase(Ease.OutQuad);                                                                          //Dotween运动方式
        RotateUp_MCN = MonsterCardsNode.transform.DORotate(new Vector3(90, 0, 0), 0.5f);
        RotateUp_MCN.SetEase(Ease.OutQuad);    
      //  if (WhichEvent == 1)            
      //  {
       //     Debug.Log("Will Battle");
     //       RotateUp.OnComplete(delegate() { CreateImage(); });                                                 //旋转结束，通知Button()进行按钮销毁并进行下一张事件的背景创建
            SA_Move.DontMove_Move = 0;                                                                               //改变Move()中DontMove_Move变量
       //     SA_Button.IfCanCreate(false);
      //  }
      //  else if (WhichEvent == 2)
      //  {
            RotateUp.OnComplete(delegate() { SA_Move.DontMove_Move = 0;  });   
                                                                                        //改变Move()中DontMove_Move变量
       // }
    }

    //↓控制牌子抬起
    //  被NewButton中CanRotate()调用
    public void CanRotateUp(bool isa)
    {
        Infors.text = "";
        Tweener RotateUp;                                                                                        //使用Dotween实现旋转
        Tweener RotateUp_MCN;
        RotateUp = Sign.transform.DORotate(new Vector3(90, 0, 0), 0f);
        RotateUp.SetEase(Ease.OutQuad);                                                                          //Dotween运动方式
        RotateUp_MCN = MonsterCardsNode.transform.DORotate(new Vector3(90, 0, 0), 0f);
        RotateUp_MCN.SetEase(Ease.OutQuad);
        //  if (WhichEvent == 1)            
        //  {
        //     Debug.Log("Will Battle");
        //       RotateUp.OnComplete(delegate() { CreateImage(); });                                                 //旋转结束，通知Button()进行按钮销毁并进行下一张事件的背景创建
        SA_Move.DontMove_Move = 0;                                                                               //改变Move()中DontMove_Move变量
        //     SA_Button.IfCanCreate(false);
        //  }
        //  else if (WhichEvent == 2)
        //  {
        RotateUp.OnComplete(delegate() { SA_Move.DontMove_Move = 0; });
        //改变Move()中DontMove_Move变量
        // }
    }
    //↓根据是否有怪物进行必要的延时，被此脚本中CanRotateDown()调用
    public void LetButtonRotate()
    {
        StartCoroutine(WillDelay());
    }
    private IEnumerator WillDelay()
    {
   //     Debug.Log("Delay is Begin");
        if (DelayTime == 0)
        {
            yield return null;
            SA_NewBut.CanSpinButton();
        }
        if (DelayTime == 1)
        {
            yield return new WaitForSeconds(0.8f);
            SA_NewBut.CanSpinButton();
        }
        if (DelayTime == 2)
        {
            yield return new WaitForSeconds(1.3f);
            SA_NewBut.CanSpinButton();
        }
        if (DelayTime == 3)
        {
            yield return new WaitForSeconds(1.8f);
            SA_NewBut.CanSpinButton();
        }
    }
}
