using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
public class RoleInfor : MonoBehaviour {
    GameObject Role;                      //脚本挂载的物体
    public enum Scenes            //信息栏种类的枚举类型
    {
        Equip = 1,         //穿戴状态
    //    Skills = 2,        //系统设置
        MysticalCondition = 3,//神秘属性状态
        Bag = 0                //背包状态
    }
    private bool HaveIt=false;                  //判断是否点击到物体
    private bool CanRotate=true;               //判断是否可进行旋转
    private float Rot_Speed = 1.0f;              //牌子旋转速度
    public string NowScene="Equip";                     //目前的角色信息栏
    public bool CanTouch = true;              //是否可进行点击

    public GameObject[] Maps = new GameObject[25];
    public GameObject MapComponent;
    public GameObject MapButton;
    private int[,] RIMapArray=new int[5,5];         //地图值
    private int Player_PosX;              //玩家所在位置的X坐标
    private int Player_PosY;               //玩家所在位置的Y坐标
    private int Player_MapColor = 1;//玩家所在的地图位置
    private bool CanCreate = false;//控制地图是否可以生成
 

    public Sprite[] Equip = new Sprite[9];            //穿戴信息
   // public Sprite[] Skills = new Sprite[9];           //技能信息
    public Sprite[] MysticalCondition = new Sprite[9];//神秘状态信息
    public Sprite[] Bag = new Sprite[9];              //背包信息
    public GameObject[] RI_Icons = new GameObject[9];

    public GameObject ToLeft;                         //左侧按钮
    public GameObject ToRight;                        //右侧按钮
    public GameObject NormalSign;                     //平时的信息栏
    public GameObject BattleSign;                     //战斗时的信息栏
    public GameObject NewShow;                        //信息展示组件

  //  private NewUIManager RI_NUIM;
    private PlayerData RI_PD;
    private BattleSystem RI_BS;
    private ScriptsManager RI_SM;
    private NewShow RI_NS;
    private CreateMap RI_CM;
    
    private Scenes thisScene = Scenes.Equip;


	// Use this for initialization
    public void L_Start()
    {
        RI_SM = GameObject.Find("Main Camera").GetComponent<ScriptsManager>();
       // RI_RM = RI_SM.RM;
    //    RI_NUIM = RI_SM.NUIM;
        RI_PD = RI_SM.PD;
        RI_BS = RI_SM.BS;
        RI_NS = RI_SM.NS;
        RI_CM = RI_SM.CM;
        Role = this.gameObject;
        RotateDown();
        ToLeft.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToBag");
        ToRight.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToBag");
       MapComponent.transform.DOScale(0, 0);
        MapButton.transform.DOScale(0,0);
        Equip = RI_PD.PlayerEquip;
        for (int i = 0; i < 25; i++)
            Maps[i].transform.DOScale(0, 0);
        for (int i = 0; i < 9; i++)
        {
            if (Equip[i] != null&&i!=4)
                RI_Icons[i].GetComponent<SpriteRenderer>().sprite = Equip[i];
            else if (i!=4)
                RI_Icons[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/NewIcon/NUL_00000001");
        }
    }
    /// <summary>
    /// 重新开始
    /// </summary>
    public void ReStart()
    {
        Equip = RI_PD.PlayerEquip;
    }
    //****************地图部分*************************************************
    /// <summary>
    /// 得到地图
    /// </summary>
    /// <param name="thisMap"></param>
    public void GetMap(int[,] thisMap)
    {
        CanCreate = false;
        Debug.Log("GetMap is the First");
        if (thisMap != null)                     //若参数不为空，即当此函数被CreateMap()调用时
        {
            CanCreate = true;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    RIMapArray[i, j] = thisMap[i, j];     //为本脚本使用的地图数组赋值
                }
        }

   //     DestroyAndCreateMap();                         //销毁上次的地图并重新创建
    }

    /// <summary>
    ///  得到玩家去过的地图的坐标
    ///  被CreateMap中InitMap()、CreateNext调用
    /// </summary>
    /// <param name="Repeat_x"></param>
    /// <param name="Repeat_y"></param>
    public void GetNumber(int Repeat_x, int Repeat_y)
    {
        //  Debug.Log("RepeatX is " + Repeat_x + "RepeatY is " + Repeat_y);
        RIMapArray[Repeat_x, Repeat_y] = 3;      //将重复的地图坐标值置为3
        Player_PosX = Repeat_x;
        Player_PosY = Repeat_y;
        //   OpenMap();
    }
    /// <summary>
    /// 展示地图
    /// </summary>
    public void ShowMap()
    {
        CanTouch = false;
            for (int i=0;i<5;i++)
                for (int j = 0; j < 5; j++)
                {
                    if (RIMapArray[i,j]!=3)
                    RIMapArray[i, j] = RI_CM.MapArray[i, j];
                }
            for (int i = 0; i < 25; i++)
            {
                Maps[i].transform.DOScaleX(0.9f, 0.2f);
                Maps[i].transform.DOScaleY(0.75f, 0.2f);
            }
       for (int i=0;i<5;i++)
           for (int j = 0; j < 5; j++)
           {
               if (RIMapArray[i, j] == 1)
               {
                   Maps[i + j * 5].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Map/Land");
             
               }
               else if (RIMapArray[i, j] == 3)
               {
                   Maps[i + j * 5].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Map/LandOK");
               }
               else if(RIMapArray[i,j]==0)
                   Maps[i + j * 5].GetComponent<SpriteRenderer>().sprite = null;
             if (i == Player_PosX && j == Player_PosY)
             {
                 Maps[i + j * 5].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Map/LandPlayer");
             }
           }
       MapButton.transform.DOScale(1, 0.3f);
       MapComponent.transform.DOScaleX(2, 0.3f);
       MapComponent.transform.DOScaleY(1.5F, 0.3f);
    }
    /// <summary>
    /// 关闭并初始化地图
    /// </summary>
    public void CloseMap()
    {
        for (int i = 0; i < 25; i++)
        {
            Maps[i].GetComponent<SpriteRenderer>().sprite = null;
            }
        MapButton.transform.DOScale(0, 0.3f);
    //    MapComponent.transform.DOScale(0, 0.3f);
        Tweener Close = MapComponent.transform.DOScale(0, 0.3f);
        Close.OnComplete(ChangeTouch);
    }
    public void ChangeTouch()
    {
        CanTouch = true;
    }
    //****************地图部分结束*************************************************
    //↓进行点击检测
	void Update () {
        if (Input.GetMouseButtonUp(0)&&CanTouch)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);   //发出射线，判断是否点击到了卡牌
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray,out hit))
            {
                if (hit.transform == ToLeft.transform)
                {
                    RotateUp();
                 ////   thisScene -= 1;
                //    if (thisScene < 0)
                 //       thisScene = Scenes.MysticalCondition;
                    if (thisScene == Scenes.Equip)
                        thisScene = Scenes.Bag;
                    else thisScene = Scenes.Equip;
                    switch (thisScene)
                    {
                        case Scenes.Equip:
                            ToLeft.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToBag");
                            ToRight.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToBag");
                            Equip = RI_PD.PlayerEquip;
                            for (int i = 0; i < 9; i++)
                            {
                                if (i!=4)
                                {
                                if (Equip[i].name.Length>3)
                                RI_Icons[i].GetComponent<SpriteRenderer>().sprite = Equip[i];
                                else
                                    RI_Icons[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/NewIcon/NUL_00000001");
                            
                                }
                            }
                                break;
                        case Scenes.Bag:
                                ToLeft.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToEquips");
                            ToRight.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToEquips");
                           Bag = RI_PD.PlayerBag;
                           for (int i = 0; i < 9; i++)
                           {
                               RI_Icons[i].GetComponent<SpriteRenderer>().sprite = Bag[i];
                           }
                            break;
               /*         case Scenes.MysticalCondition: 
                             ToLeft.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToSkills");
                            ToRight.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToBag");
                            MysticalCondition = RI_PD.PlayerMysCon;
                            for (int i = 0; i < 9; i++)
                            {
                                RI_Icons[i].GetComponent<SpriteRenderer>().sprite = MysticalCondition[i];
                            }
                            break;
                   /*     case Scenes.Skills: 
                             ToLeft.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToEquips");
                            ToRight.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToMysCon");
                            Skills = RI_PD.PlayerSkills;
                            for (int i = 0; i < 9; i++)
                            {
                                RI_Icons[i].GetComponent<SpriteRenderer>().sprite = Skills[i];
                            }
                            break;*/
                        default:
                            break;
                    }
                }
                else if (hit.transform == ToRight.transform)
                {
                    RotateUp();
                 //   thisScene += 1;
                 //   if ((int)thisScene > 3)
                   //     thisScene = Scenes.Bag;
                    if (thisScene == Scenes.Equip)
                        thisScene = Scenes.Bag;
                    else thisScene = Scenes.Equip;
                    switch (thisScene)
                    {
                        case Scenes.Equip:
                            ToLeft.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToBag");
                            ToRight.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToBag");
                            Equip = RI_PD.PlayerEquip;
                            for (int i = 0; i < 9; i++)
                            {
                                if (i != 4)
                                    RI_Icons[i].GetComponent<SpriteRenderer>().sprite = Equip[i];
                                else
                                    RI_Icons[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Map/Map");
                            }
                            break;
                        case Scenes.Bag:
                            ToLeft.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToEquips");
                            ToRight.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToEquips");
                            Bag = RI_PD.PlayerBag;
                            for (int i = 0; i < 9; i++)
                            {
                                if (i != 4)
                                RI_Icons[i].GetComponent<SpriteRenderer>().sprite = Bag[i];
                                  else
                                    RI_Icons[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Map/Map");
                            }
                            break;
                  /*      case Scenes.MysticalCondition:
                            ToLeft.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToSkills");
                            ToRight.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToBag");
                            MysticalCondition = RI_PD.PlayerMysCon;
                            for (int i = 0; i < 9; i++)
                            {
                                RI_Icons[i].GetComponent<SpriteRenderer>().sprite = MysticalCondition[i];
                            }
                            break;
                    /*   case Scenes.Skills:
                            ToLeft.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToEquips");
                            ToRight.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/ToMysCon");
                            Skills = RI_PD.PlayerSkills;
                            for (int i = 0; i < 9; i++)
                            {
                                RI_Icons[i].GetComponent<SpriteRenderer>().sprite = Skills[i];
                            }
                            break;*/
                        default:
                            break;
                    }
                }
                else if (hit.transform.tag=="RI_Icons")
                {
                    switch (thisScene)
                    {
                        case Scenes.Equip:
                            EquipIcon(hit.transform.name);
                            break;
                        case Scenes.Bag:
                            BagIcon(hit.transform.name);
                            break;
                    /*    case Scenes.MysticalCondition:
                            MysticalConditionIcon(hit.transform.name);
                            break;
                     /*   case Scenes.Skills:
                            SkillsIcon(hit.transform.name);
                            break;*/
                        default:
                            break;
                    }
                }
                else if (hit.transform.tag == "RI_Battle" && RI_BS.CanTouch)
                {
                  BattleIcon(hit.transform.name);
                }
            }
        }
	}
    private void BattleIcon(string iconName)
    {
        List<Sprite> Icons = new List<Sprite>();
        List<string> Icons_string = new List<string>();
        RI_BS.Battle_Player_Defence(false);
        RI_BS.ATKing(false,false, 0, 0);          //进行任何其他点击时，销毁战斗图标
        RI_BS.Battle_Player_Move(false);    //进行任何一次点击时，重置地图颜色
        RI_BS.CanATK = false;               //置可攻击状态为假
        switch (iconName)
        {
            case "BS_01":
              //  RI_BS.ATKing(false, 0, 0);          //进行任何其他点击时，销毁战斗图标
             //   RI_BS.Battle_Player_Move(false);    //进行任何一次点击时，重置地图颜色
             //   RI_BS.CanATK = false;               //置可攻击状态为假
               

                    Icons_string = RI_PD.GiveSerialNumber("PlayerBag", "FOO", null);
                if (Icons_string.Count > 0)
                {
                  //  for (int i = 0; i < Icons_string.Count; i++)
                  //      Icons.Add(Resources.Load<Sprite>("Icons/NewIcon/" + Icons_string[i]));
                    NewShow.GetComponent<NewShow>().GetCardsName(Icons_string, "EAT");
                //    RI_NUIM.Component_Show.transform.DOScale(1, 0.3f);
                    CanTouch = false;
                }
                break;
            case "BS_02":
              //  RI_BS.ATKing(false, 0, 0);              //进行任何其他点击时，销毁战斗图标
               //     RI_BS.Battle_Player_Move(false);    //进行任何一次点击时，重置地图颜色
                 //   RI_BS.CanATK = false;               //置可攻击状态为假

                RI_BS.Battle_Player_Move(true);
                break;
            case "BS_03":
             //   RI_BS.ATKing(false, 0, 0);              //进行任何其他点击时，销毁战斗图标
                 //   RI_BS.Battle_Player_Move(false);    //进行任何一次点击时，重置地图颜色
                   // RI_BS.CanATK = false;               //置可攻击状态为假

                RI_BS.Battle_Player_Attack();
                break;
            case "BS_04":
              //  RI_BS.ATKing(false, 0, 0);              //进行任何其他点击时，销毁战斗图标
              //      RI_BS.Battle_Player_Move(false);    //进行任何一次点击时，重置地图颜色
               //     RI_BS.CanATK = false;               //置可攻击状态为假

                RI_BS.Battle_Player_Defence(true);
                break;
          //  case "BS_05":
            //    RI_BS.ATKing(false, 0, 0);              //进行任何其他点击时，销毁战斗图标
              //      RI_BS.Battle_Player_Move(false);    //进行任何一次点击时，重置地图颜色
                //    RI_BS.CanATK = false;               //置可攻击状态为假
//
            //    RI_BS.Battle_Player_Skill();
             //   break;
            default:
                break;
        }
    }
    //↓根据点击到的图标,进行装备界面的响应
    //  02 12 22 01 11 21 00 10 20
  //   RI_Icons = new GameObject[9];
    private void EquipIcon(string thisIcon)
    {
        switch (thisIcon)
        {
            case "RI02":
            //    Debug.Log(RI_Icons[0].GetComponent<SpriteRenderer>().sprite.name.Substring(0, 3));
                if (RI_Icons[0].GetComponent<SpriteRenderer>().sprite.name.Substring(0, 3) != "NUL")
                {
                    RI_NS.GetCardsName(RI_Icons[0].GetComponent<SpriteRenderer>().sprite.name,"XIE");
                }
                break;
            case "RI12": if (RI_Icons[1].GetComponent<SpriteRenderer>().sprite.name.Substring(0, 3) != "NUL")
                {
                    RI_NS.GetCardsName(RI_Icons[1].GetComponent<SpriteRenderer>().sprite.name, "XIE");
                }
                break;
            case "RI22": if (RI_Icons[2].GetComponent<SpriteRenderer>().sprite.name.Substring(0, 3) != "NUL")
                {
                    RI_NS.GetCardsName(RI_Icons[2].GetComponent<SpriteRenderer>().sprite.name, "XIE");
                }
                break;
            case "RI01": if (RI_Icons[3].GetComponent<SpriteRenderer>().sprite.name.Substring(0, 3) != "NUL")
                {
                    RI_NS.GetCardsName(RI_Icons[3].GetComponent<SpriteRenderer>().sprite.name, "XIE");
                }
                break;
            case "RI11":
                ShowMap();
                break;
            case "RI21": if (RI_Icons[5].GetComponent<SpriteRenderer>().sprite.name.Substring(0, 3) != "NUL")
                {
                    RI_NS.GetCardsName(RI_Icons[5].GetComponent<SpriteRenderer>().sprite.name, "XIE");
                }
                break;
            case "RI00": if (RI_Icons[6].GetComponent<SpriteRenderer>().sprite.name.Substring(0, 3) != "NUL")
                {
                    RI_NS.GetCardsName(RI_Icons[6].GetComponent<SpriteRenderer>().sprite.name, "XIE");
                }
                break;
            case "RI10": if (RI_Icons[7].GetComponent<SpriteRenderer>().sprite.name.Substring(0, 3) != "NUL")
                {
                    RI_NS.GetCardsName(RI_Icons[7].GetComponent<SpriteRenderer>().sprite.name, "XIE");
                }
                break;
            case "RI20": if (RI_Icons[8].GetComponent<SpriteRenderer>().sprite.name.Substring(0, 3) != "NUL")
                {
                    RI_NS.GetCardsName(RI_Icons[8].GetComponent<SpriteRenderer>().sprite.name, "XIE");
                }
                break;
            default:
                break;
        }
    }
    //↓根据点击到的图标，进行背包界面的响应
    private void BagIcon(string thisIcon)
    {
        List<string> Icons_string = new List<string>();
        List<Sprite> Icons = new List<Sprite>();
        Sprite A;
        switch (thisIcon)
        {
            case "RI02":
                foreach (string i in RI_PD.PlayerBagData)
                {
                    if (i.Substring(0,3)=="FOO")
                       Icons_string.Add(i);
                  }
                if (Icons_string.Count>0 )

                {
                    for (int i = 0; i < Icons_string.Count; i++)
                        Debug.Log("Icon is "+Icons_string[i]);
                    NewShow.GetComponent<NewShow>().GetCardsName(Icons_string,"EAT");
                    //   RI_NUIM.Component_Show.transform.DOScale(1, 0.3f);
                    CanTouch = false;
                }
                else
                {
                }
                   
                break;
            case "RI12":
                foreach (string i in RI_PD.PlayerBagData)
                {
                    if (i.Substring(0, 3) == "FOO")
                        Icons_string.Add(i);
                }
            //    Icons_string = RI_PD.GiveSerialNumber("PlayerBag", "CON", null);
                if (Icons_string.Count > 0)
                {
                //    for (int i = 0; i < Icons_string.Count; i++)
                 //       Icons.Add(Resources.Load<Sprite>("Icons/NewIcon/" + Icons_string[i]));
                    NewShow.GetComponent<NewShow>().GetCardsName(Icons_string, "EAT");
                //    RI_NUIM.Component_Show.transform.DOScale(1, 0.3f);
                    CanTouch = false;
                }
                else
                {
                }
                break;
            case "RI22": 
                foreach (string i in RI_PD.PlayerBagData)
                {
                    if (i.Substring(0, 3) == "FOO")
                        Icons_string.Add(i);
                }
            //    Icons_string = RI_PD.GiveSerialNumber("PlayerBag", "MAT", null);
                if (Icons_string.Count > 0)
                {
                //    for (int i = 0; i < Icons_string.Count; i++)
                  //      Icons.Add( Resources.Load<Sprite>("Icons/NewIcon/" + Icons_string[i]));
                    NewShow.GetComponent<NewShow>().GetCardsName(Icons_string, "EAT");
                //    RI_NUIM.Component_Show.transform.DOScale(1, 0.3f);
                    CanTouch = false;
                }
                else
                {
                }
                break;
            case "RI01":
                foreach (string i in RI_PD.PlayerBagData)
                {
                    if (i.Substring(0, 3) == "WEA")
                        Icons_string.Add(i);
                }
             //   Icons_string = RI_PD.GiveSerialNumber("PlayerBag", "WEA", null);
                if (Icons_string.Count > 0)
                {
                 //   for (int i = 0; i < Icons_string.Count; i++)
                   //     Icons.Add( Resources.Load<Sprite>("Icons/NewIcon/" + Icons_string[i]));
                    NewShow.GetComponent<NewShow>().GetCardsName(Icons_string,"EQU");
               //     RI_NUIM.Component_Show.transform.DOScale(1, 0.3f);
                    CanTouch = false;
                }
                else
                {
                }
                break;
            case "RI11": ShowMap();
                break;
            case "RI21":
                foreach (string i in RI_PD.PlayerBagData)
                {
                    if (i.Substring(0, 3) == "NEC" || i.Substring(0, 3) == "RIN")
                        Icons_string.Add(i);
                }
            //    Icons_string = RI_PD.GiveSerialNumber("PlayerBag", "NEC", null);
             //   Icons_string.AddRange(RI_PD.GiveSerialNumber("PlayerBag", "RIN", null));
                if (Icons_string.Count > 0)
                {
                 //   for (int i = 0; i < Icons_string.Count; i++)
                   //     Icons.Add(Resources.Load<Sprite>("Icons/NewIcon/" + Icons_string[i]));
                    NewShow.GetComponent<NewShow>().GetCardsName(Icons_string, "EQU");
               //     RI_NUIM.Component_Show.transform.DOScale(1, 0.3f);
                    CanTouch = false;
                }
                else
                {
                }
                break;
            case "RI00":
                foreach (string i in RI_PD.PlayerBagData)
                {
                    if (i.Substring(0, 3) == "HEL")
                        Icons_string.Add(i);
                }
             //   Icons_string = RI_PD.GiveSerialNumber("PlayerBag", "HEL", null);
                if (Icons_string.Count > 0)
                {
                 //   for (int i = 0; i < Icons_string.Count; i++)
                    //    Icons.Add(Resources.Load<Sprite>("Icons/NewIcon/" + Icons_string[i]));
                    NewShow.GetComponent<NewShow>().GetCardsName(Icons_string, "EQU");
              //      RI_NUIM.Component_Show.transform.DOScale(1, 0.3f);
                    CanTouch = false;
                }
                else
                {
                }
                break;
            case "RI10":
                foreach (string i in RI_PD.PlayerBagData)
                {
                    if (i.Substring(0, 3) == "CHE")
                        Icons_string.Add(i);
                }
            //    Icons_string = RI_PD.GiveSerialNumber("PlayerBag", "CHE", null);
                if (Icons_string.Count > 0)
                {
                 //   for (int i = 0; i < Icons_string.Count; i++)
                    //    Icons.Add(Resources.Load<Sprite>("Icons/NewIcon/" + Icons_string[i]));
                    NewShow.GetComponent<NewShow>().GetCardsName(Icons_string, "EQU");
                   // RI_NUIM.Component_Show.transform.DOScale(1, 0.3f);
                    CanTouch = false;
                }
                else
                {
                }
                break;
            case "RI20":
                foreach (string i in RI_PD.PlayerBagData)
                {
                    if (i.Substring(0, 3) == "PAN")
                        Icons_string.Add(i);
                }
            //    Icons_string = RI_PD.GiveSerialNumber("PlayerBag", "PAN", null);
                if (Icons_string.Count > 0)
                {
                   // for (int i = 0; i < Icons_string.Count; i++)
                     //   Icons.Add(Resources.Load<Sprite>("Icons/NewIcon/" + Icons_string[i]));
                    NewShow.GetComponent<NewShow>().GetCardsName(Icons_string, "EQU");
               //     RI_NUIM.Component_Show.transform.DOScale(1, 0.3f);
                    CanTouch = false;
                }
                else
                {
                }
                break;
            default:
                break;
        }
    }
  
    //↓根据点击到的图标，进行神秘状态界面的响应
  /*  private void MysticalConditionIcon(string thisIcon)
    {
        switch (thisIcon)
        {
            case "RI02":
                break;
            case "RI12":
                break;
            case "RI22":
                break;
            case "RI01":
                break;
            case "RI11":
                break;
            case "RI21":
                break;
            case "RI00":
                break;
            case "RI10":
                break;
            case "RI20":
                break;
            default:
                break;
        }
    }
    //↓根据点击到的图标，进行技能界面的响应
    private void SkillsIcon(string thisIcon)
    {
        switch (thisIcon)
        {
            case "RI02":
                break;
            case "RI12":
                break;
            case "RI22":
                break;
            case "RI01":
                break;
            case "RI11":
                break;
            case "RI21":
                break;
            case "RI00":
                break;
            case "RI10":
                break;
            case "RI20":
                break;
            default:
                break;
        }
    }*/

    /// <summary>
    /// 控制牌子抬起,运动结束后调用RotateDown使牌子落下
    /// </summary>
    public void RotateUp()
    {
        CanTouch = false;
        if (CanRotate)
        {
            Tweener RotUp = Role.transform.DORotate(new Vector3(0, 90, 0), 0.2f);
            RotUp.SetEase(Ease.OutCubic);
            RotUp.OnComplete(delegate() { Invoke("RotateDown", 0.2f); CanTouch = true; });
        }
    }

    /// <summary>
    /// 控制牌子落下
    /// </summary>
    public void RotateDown()
    {
        CanRotate = false;
        CanTouch = false;
        Sequence Role_Rotate = DOTween.Sequence(); 
        Role_Rotate.Append(Role.transform.DORotate(new Vector3(0, 0, 0), 0.4f));
        Role_Rotate.Append(Role.transform.DORotate(new Vector3(0, 45, 0), 0.15f));
        Role_Rotate.Append(Role.transform.DORotate(new Vector3(0, 0, 0), 0.15f));
        Role_Rotate.Append(Role.transform.DORotate(new Vector3(0, 20, 0), 0.1f));
        Role_Rotate.Append(Role.transform.DORotate(new Vector3(0, 0, 0), 0.1f));
        Role_Rotate.SetEase(Ease.OutCubic);
        Role_Rotate.OnComplete(delegate() { CanRotate = true; CanTouch = true; });

      
    }
  
}
