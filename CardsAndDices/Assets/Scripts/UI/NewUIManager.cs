using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class NewUIManager : MonoBehaviour {
    public GameObject ThereIsNoWay;   //控制出现没路时的UI
    public GameObject MapUI;          //控制出现地图的UI
    public GameObject Maps;          //地图卡牌UI
    public GameObject MapPivot;      //本次生成的地图
    public GameObject[] ButtonsOfUnderSigns = new GameObject[4];//储存下方牌子展示需要的UI，按从左至右排列
    public GameObject[] Map;         //存储本次应生成的地图
    public GameObject TipsOfUnderButtons;//下方牌子的显示提示信息
   // public GameObject Component_Show;    //图片展示组件
    public GameObject WarningText;       //警告字幕
    public GameObject BattleOver;         //战斗结束UI
    public GameObject[] Bootys;               //战利品格子
    public GameObject Failure;            //战斗失败
    public GameObject EquipOperation;             //装备操作按钮
    public GameObject GameOverImage;         //游戏结束界面
    public GameObject GameOverBut;          //游戏结束按钮

    public GameObject EventInfors;               //事件的信息展示
    public GameObject Text01;                   //标题
    public GameObject GetOrLose;                //得到或失去
    public GameObject[] Booty;                   //物品
    public GameObject[] Data;                    //数据

    public GameObject OverTurn;          //回合结束按钮
    //角色信息  0:HP  1:攻击力  2:防御力  3:行动力 4.饱食度
 //   public GameObject[] Infors=new GameObject[5];
   // public Button OverTurn_But;
  

    private Move NUIM_Move;     //调用Move（）脚本
    private ScriptsManager NUIM_SM;
    private BattleSystem NUIM_BS;
    private PlayerData NUIM_PD;
    private ToBlack NUIM_TB;
    private NewShow NUIM_NS;
    private MissionInfor NUIM_MI;
    private RoleInfor NUIM_RI;
    private Booty NUIM_BOO;


    private int[,] MapArray_NUIM=new int [5,5];  //存储地图数据的数组
    private bool CanShowMap = true;  //是否可显示地图
    private int Player_PosX;              //玩家所在位置的X坐标
    private int Player_PosY;               //玩家所在位置的Y坐标
    private int Player_MapColor = 1;//玩家所在的地图位置
    private bool CanCreate = false;//控制地图是否可以生成
    private bool CanChangeSign = true;//控制是否可旋转下方牌子，防止出现点击过快动画不完全的状况
    private bool CanShow = false;
    /// <summary>
    /// 获取相关实例，并进行UI界面的初始缩小化
    /// </summary>
	public void L_Start () {
        NUIM_SM = GameObject.Find("Main Camera").GetComponent<ScriptsManager>();
        NUIM_Move = NUIM_SM.Move;//获取Move()脚本
        NUIM_BS = NUIM_SM.BS;
        NUIM_PD = NUIM_SM.PD;
        NUIM_TB = NUIM_SM.TB;
        NUIM_NS = NUIM_SM.NS;
        NUIM_MI = NUIM_SM.MI;
        NUIM_BOO = NUIM_SM.Bo;
        NUIM_RI = NUIM_SM.RI;

        GameOverImage.transform.DOScale(0, 0);
        WarningText.transform.DOScale(0, 0);
        OverTurn.transform.DOScale(0,0);
        BattleOver.transform.DOScale(0, 0);
        ThereIsNoWay.transform.DOScale(0, 0);
        EventInfors.transform.DOScale(0, 0);
        
       EventTriggerListener.Get(OverTurn.GetComponent<Button>().gameObject).onClick = MyOnClick;   //获取点击事件
       EventTriggerListener.Get(BattleOver.GetComponentInChildren<Button>().gameObject).onClick = MyOnClick;   //获取点击事件
       EventTriggerListener.Get(EquipOperation.GetComponentInChildren<Button>().gameObject).onClick = MyOnClick;
       EventTriggerListener.Get(GameOverBut.GetComponentInChildren<Button>().gameObject).onClick = MyOnClick;   
      //  OverTurn.GetComponent<Button>().onClick.AddListener(MyAAAOnClick);
       Debug.Log("Fuck!");
        for (int i = 0; i < 4; i++)          //获取控制底部牌子展示信息的按钮的点击组件
        {
        //  EventTriggerListener.Get(ButtonsOfUnderSigns[i].gameObject).onClick = ChangeButttonText;
         // EventTriggerListener.Get(ButtonsOfUnderSigns[i].gameObject).onEnter = TipsOpen;
        }
        Tweener MapClose = MapUI.transform.DOScale(0, 0);             //隐藏地图

   
      // Tweener TipsClose= TipsOfUnderButtons.transform.DOScale(0,0);
	}
    //↓读取玩家的按键并进行地图的出现与消失
   /* void Update()
    {
        if (Input.GetKeyDown("space") && CanShowMap)
        {
            CanShowMap = false;
            Tweener MapOpen = MapUI.transform.DOScale(1, 0.3f);
            //  StartCoroutine(MapOpen1());
            // MapOpen.OnComplete(OpenMap);
            ChangeMapColor();                //地图出现时，进行重复地图的颜色变化
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            //   DestroyMap();
            Tweener MapClose = MapUI.transform.DOScale(0, 0.3f);
            MapClose.OnComplete(delegate() { CanShowMap = true; });
        }

    }*/
    /// <summary>
    /// UI初始化
    /// </summary>
    /// 
    public void AnotherStart()
    {
        Debug.Log("UI ReStart");
        GameOverImage.transform.DOScale(0, 0);
        WarningText.transform.DOScale(0, 0);
        OverTurn.transform.DOScale(0, 0);
        BattleOver.transform.DOScale(0, 0);
        ThereIsNoWay.transform.DOScale(0, 0);
        EventInfors.transform.DOScale(0, 0);
        EventInfor(false, null,null, 0, false);
        EventInfor(false, null, 0, null, false);
    }
    public void MyOnClick(GameObject But)
    {
        //控制回合结束按钮的点击事件
        if (But == OverTurn.GetComponent<Button>().gameObject) 
        {
            if (NUIM_BS.PlayerTurn)
                NUIM_BS.ChangeTurn("Monster");
            else
                NUIM_BS.ChangeTurn("Player");
        }
        //控制战斗结束按钮的点击事件
        if (But == BattleOver.GetComponentInChildren<Button>().gameObject)
        {
            Debug.Log("OtmdJBK");
          //  NUIM_BS.ReturnToMap();
            NUIM_TB.TOBlack("BS");
            BattleOver.transform.DOScale(0, 0.3f);
        }
        if (But == EquipOperation.GetComponent<Button>().gameObject)
        {
            switch (EquipOperation.GetComponentInChildren<Text>().text)
            {
                case "卸下":
                    int number_01 = NUIM_NS.ThisImage - 2;
                    string serialNumber_01 = NUIM_NS.AllCards[number_01].name;
                  //  Debug.Log("SerialNumber is "+serialNumber);
                    NUIM_PD.RemoveEquip(serialNumber_01);
                    break;
                case "吃掉":
                    int number_02 = NUIM_NS.ThisImage - 2;
                    string serialNumber_02 = NUIM_NS.AllCards[number_02].name;
               //     Debug.Log("吃掉了" + NUIM_NS.AllCards[number_02].name);
               //     GameObject thisFood = NUIM_NS.AllCards[number_02];

                    NUIM_PD.EatFood(serialNumber_02);
                    break;
                case "使用":
                    break;
                case "装备":
                    int number_03 = NUIM_NS.ThisImage - 2;
                    string serialNumber_03 = NUIM_NS.AllCards[number_03].name;
                    NUIM_PD.PutIntoEquip(serialNumber_03);
                    break;
                default:
                    break;
            }
            NUIM_NS.CloseThis();
        }
        if (But == GameOverBut.GetComponentInChildren<Button>().gameObject)
        {
            Debug.Log("重新开始");
            //  NUIM_BS.ReturnToMap();
            NUIM_TB.TOBlack("GameOver");
            GameOverImage.transform.DOScale(0, 0.3f);
           // BattleOver.transform.DOScale(0, 0.3f);
        }
    }
    void Update()
    {

    }
    public void GameOver()
    {
        GameOverImage.transform.DOScale(1, 0.3f);
    }
    /// <summary>
    /// 打开事件信息组件
    /// </summary>
    public void EventShow()
    {
        Debug.Log("Show AND CanShow is "+CanShow);
        NUIM_RI.CanTouch = false;
        if (CanShow)
        {
          
            EventInfors.transform.DOScale(1, 0.3f);
        }
        else
            NUIM_RI.CanTouch = true;
    }

    public void EventShow(bool show)
    {
      //  Debug.Log("Show AND CanShow is " + CanShow);
        CanShow = show;
        NUIM_RI.CanTouch = false;
        if (CanShow)
        {

            EventInfors.transform.DOScale(1, 0.3f);
        }
        else
            NUIM_RI.CanTouch = true;
    }
    IEnumerator EShow()
    {
        yield return new WaitForSeconds(0.5f);
        EventInfors.transform.DOScale(1, 0.3f);
    }
    /// <summary>
    /// 关闭事件信息组件
    /// </summary>
    public void EventClose()
    {
        CanShow = false;
        Tweener Close = EventInfors.transform.DOScale(0, 0.3f);
        Close.OnComplete(NUIM_RI.ChangeTouch);
    }
    /// <summary>
    /// 事件信息展示函数  属性
    /// </summary>
    /// <param name="get">得到或失去 真:得到 假:失去</param>
    /// <param name="datas">数据类型</param>
    /// <param name="counts">数据大小</param>
    /// <param name="number">数据个数</param>
    public void EventInfor(bool get, string[] datas, int[] counts, int number, bool canShow)
    {
        
        CanShow = canShow;
        Debug.Log("Event infor has OK and CanShow is " + CanShow);
        for (int i = 0; i < 3; i++)
        {
            Booty[i].gameObject.SetActive(false);
            if (i < number)
            Data[i].gameObject.SetActive(true);
            else
                Data[i].gameObject.SetActive(false);

        }
            if (get)
            {
                Text01.GetComponent<Text>().text = "恭喜你";
                GetOrLose.GetComponent<Text>().text = "你的";
                for (int i = 0; i < number; i++)
                {
                    switch (datas[i])
                    {
                        case "HP":
                            PlayerData.HP += counts[i];
                            Data[i].GetComponent<Text>().text = "生命值+" + counts[i].ToString();
                            break;
                        case "SAT":
                            PlayerData.Sat += counts[i];
                            Data[i].GetComponent<Text>().text = "饱食度+" + counts[i].ToString();
                            break;
                        case "DEF":
                            PlayerData.Def += counts[i];
                            Data[i].GetComponent<Text>().text = "防御力+" + counts[i].ToString();
                            break;
                        case "ATK":
                            PlayerData.Atk += counts[i];
                            Data[i].GetComponent<Text>().text = "攻击力+" + counts[i].ToString();
                            break;
                        case "ActNum":
                            PlayerData.ActNum += counts[i];
                            Data[i].GetComponent<Text>().text = "行动力+" + counts[i].ToString();
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                Text01.GetComponent<Text>().text = "很遗憾";
                GetOrLose.GetComponent<Text>().text = "你的";
                for (int i = 0; i < number; i++)
                {
                    switch (datas[i])
                    {
                        case "HP":
                            PlayerData.HP -= counts[i];
                            Data[i].GetComponent<Text>().text = "生命值-" + counts[i].ToString();
                            break;
                        case "SAT":
                            PlayerData.Sat -= counts[i];
                            Data[i].GetComponent<Text>().text = "饱食度-" + counts[i].ToString();
                            break;
                        case "DEF":
                            PlayerData.Def -= counts[i];
                            Data[i].GetComponent<Text>().text = "防御力-" + counts[i].ToString();
                            break;
                        case "ATK":
                            PlayerData.Atk -= counts[i];
                            Data[i].GetComponent<Text>().text = "攻击力-" + counts[i].ToString();
                            break;
                        case "ActNum":
                            PlayerData.ActNum -= counts[i];
                            Data[i].GetComponent<Text>().text = "行动力-" + counts[i].ToString();
                            break;
                        default:
                            break;
                    }
                }
            }
        NUIM_MI.ChangeInfor();
    }
    /// <summary>
    /// 事件信息展示 物品
    /// </summary>
    /// <param name="get">得到或失去 真:得到 假:失去</param>
    /// <param name="which">类型</param>
    /// <param name="number">数目</param>
    /// <param name="rare">稀有度</param>
    public void EventInfor(bool get, string[] which, int number, string[] rare, bool canShow)
    {
   
        CanShow = canShow;
        Debug.Log("Event equ has OK and CanShow is " + CanShow);
        string[] Equips = new string[3];
        string thisBooty;
        for (int i = 0; i < 3; i++)
        {
         
            Data[i].gameObject.SetActive(false);
            if (i<number)
                Booty[i].gameObject.SetActive(true);
            else
                Booty[i].gameObject.SetActive(false);
        }
        if (get)
        {
            Text01.GetComponent<Text>().text = "恭喜你";
            GetOrLose.GetComponent<Text>().text = "你获得了";
            for (int i = 0; i < number; i++)
            {
                thisBooty= NUIM_BOO.GiveBooty(which[i], rare[i]);
                Debug.Log("this booty is "+thisBooty);
                Booty[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/SmallIcons/" + thisBooty);
                Equips[i] = thisBooty;
                NUIM_PD.PutIntoBag(Equips[i]);
            }
            
        }
        else
        {
            Text01.GetComponent<Text>().text = "很遗憾";
            GetOrLose.GetComponent<Text>().text = "你失去了";
                for (int i = 0; i < number; i++)
            {
               string Which= NUIM_PD.RandomLoseBag();
               Booty[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/SmallIcons/" + Which);
            }
           }
      }
    
    /// <summary>
    /// 控制角色信息
    /// </summary>
    /// <param name="which">哪一个属性</param>
    /// <param name="addOrSub">增加或减少</param>
    /// <param name="count">数目</param>
    public void RoleInfors(string which,int count)
    {

    }
   

    //↓没路提示部分
    //  当玩家所想要行进的方向没有路时，出现相关提示
    public void NoWayOut()
    {
        Tweener NoWayOut = ThereIsNoWay.transform.DOScale(1, 0.3f);
        NUIM_Move.DontMove_Move = 1;
        StartCoroutine(SlowClose());
    }
    //↓“这儿没路了”延时方法
    IEnumerator SlowClose()
    {
        yield return new WaitForSeconds(1f);
        Tweener NoWayClose = ThereIsNoWay.transform.DOScale(0, 0.3f);
        NoWayClose.OnComplete(delegate() { NUIM_Move.DontMove_Move = 0; });
    }

    //↓RoleInfor所需的UI
    public void UI_RI()
    {

    }
    
    // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
    // ↓牌子显示信息的提示方法
    public void TipsOpen(GameObject button)
    {

    }
   
    // ↓牌子显示信息的提示方法
    // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
}
//  **************************************************************
//↓ 地图展示部分


/// <summary>
/// 点击显示地图后，出现本次生成的地图
/// 被ToBlack中TOBlack()、CreateMap中InitMap()调用
/// </summary>
/// <param name="thisMap"></param>
/*   public void GetMap(int [,] thisMap)       
   {
       CanCreate = false;             
       Debug.Log("GetMap is the First");
       if (thisMap != null)                     //若参数不为空，即当此函数被CreateMap()调用时
       {
           CanCreate = true;
           for (int i = 0; i < 5; i++)
               for (int j = 0; j < 5; j++)
               {
                   MapArray_NUIM[i, j] = thisMap[i, j];     //为本脚本使用的地图数组赋值
               } 
       }
       
       DestroyAndCreateMap();                         //销毁上次的地图并重新创建
          Tweener NoWayClose = ThereIsNoWay.transform.DOScale(0, 0);
        //  Tweener MapClose = MapUI.transform.DOScale(0, 0);
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
       MapArray_NUIM[Repeat_x, Repeat_y] = 3;      //将重复的地图坐标值置为3
       Player_PosX = Repeat_x;
       Player_PosY = Repeat_y;
    //   OpenMap();
   }
   /// <summary>
   /// 向展示信息组件传递图片信息
   /// </summary>
   /// <param name="sprites"></param>
   public void ToShow(List<Sprite> sprites)
   {
   //    Component_Show.GetComponent<ShowAndMove>().GetSprites(sprites);
   }
   //↓进行地图创建
   //  被此脚本中DestroyAndCreateMap()调用
   public void OpenMap()   
   {
   //    CardPos = ThisMap.GetComponent<RectTransform>().anchoredPosition;
       Tweener MapClose = MapUI.transform.DOScale(0, 0);  //关闭地图UI
       int m = 0 ;
       for (int i = 0; i < 5; i++)   //进行地图的实例化并显示
           for (int j = 0; j < 5; j++)
           {
               if (MapArray_NUIM[i, j] == 1)  
               {
                   Map[m] = Instantiate(Maps, new Vector2(MapPivot.transform.position.x + i * 0.8f, MapPivot.transform.position.y + j * 0.8f), Maps.transform.rotation) as GameObject;
            //       Debug.Log("Card's AnchoredPos is " + CardPos);
                //   Debug.Log("Card's World Pos is " + MapPivot.transform.position);
                   Map[m].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Cards1");
                   Map[m].transform.SetParent(MapUI.transform);
                //   Debug.Log("PositionX is " + (CardPos.x + i * 31) + " PositionY is " + (CardPos.y + j * 31));
                   m++;
               }
               else if (MapArray_NUIM[i, j] ==3)
               {
                //   Debug.Log("PositionX is " + (ThisMap.transform.position.x + i * 31)+" PositionY is " + (ThisMap.transform.position.y + j * 31));

                   Map[m] = Instantiate(Maps, new Vector2(MapPivot.transform.position.x + i * 0.8f, MapPivot.transform.position.y + j * 0.8f), Maps.transform.rotation) as GameObject;
                   Map[m].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Cards3");
                   Map[m].transform.SetParent(MapUI.transform);  //  CardPos = map.GetComponent<RectTransform>().anchoredPosition;
                 //  Debug.Log("Map[" + m + "]'s position is " + Map[m].transform.position);
                 //  Debug.Log("Card's AnchoredPos is " + CardPos);
                //   Debug.Log("Positionis " + map.transform.position);
                   m++;
               }
           }
    //   Tweener MapOpen = MapUI.transform.DOScale(1, 2f);
   }

   //↓地图出现后，改变重复地图的颜色
   //  被此脚本中Update()调用
   public void ChangeMapColor()   
   {
       int[] Number = new int[Map.Length];
       int n = -1;
       for (int i=0;i<5;i++)
           for (int j = 0; j < 5; j++)
           {
               if (MapArray_NUIM[i, j] == 1 || MapArray_NUIM[i, j] == 3)
               {
                   n++;
                   Number[n] = n;
               }
               if (i == Player_PosX && j == Player_PosY)
                   ChangeColor();
               else if (MapArray_NUIM[i, j] == 3)
               {
                   Map[Number[n]].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Cards3");
               }
             
           }
   }
   //↓销毁上一次的地图，并在销毁结束后创建新地图
   //  被此脚本中GetMap()调用
   public void DestroyAndCreateMap()
   {
    //   Debug.Log("Dddddd");
       for (int i = 0; i < 25; i++)
       {
           if (Map[i] != null)
               DestroyImmediate(Map[i]);
       }
       if (CanCreate)
       {
           Tweener Open = MapUI.transform.DOScale(1, 0);
           Open.OnComplete(OpenMap);
       }
   }
   //↓将角色所在地图进行颜色交替变化
   //  被此脚本中ChangeMapColor()调用
   void ChangeColor()
   {
       //   Debug.Log("Time is " + Time.time);
       int n = -1;
       int[] Number = new int[Map.Length];
       for (int i = 0; i < 5; i++)
           for (int j = 0; j < 5; j++)
           {
               if (MapArray_NUIM[i, j] == 1 || MapArray_NUIM[i, j] == 3)
               {
                   n++;
                   Number[n] = n;
                   if (i == Player_PosX && j == Player_PosY && !CanShowMap)
                   {
                       if (Player_MapColor == 1)
                       {
                           Map[Number[n]].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Cards1");
                           Player_MapColor = 2;
                       }
                       else if (Player_MapColor == 2)
                       {
                           Map[Number[n]].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Cards3");
                           Player_MapColor = 1;
                       }
                       break;
                   }
               }

           }
       if (!CanShowMap)
           Invoke("ChangeColor", 0.5f);
   }
   //↑ 地图部分结束
   //  ***************************************************************
 * */