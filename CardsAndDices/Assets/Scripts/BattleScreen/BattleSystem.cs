using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
/// <summary>
/// 战斗系统
/// </summary>
public class BattleSystem : MonoBehaviour {
    private ToBlack BS_TB;
    public GameObject BattleBG;
    public GameObject[] BattleMonsters=new GameObject[3];//本次的战斗怪物
    public GameObject monster;                     //存储怪物信息的对象
    public GameObject Icon_CanAtk;                  //可以进行攻击的图标
    public GameObject Icon_Atking;                  //正在攻击的图标
    public GameObject Icon_BeAtk;                   //被攻击的图标
    public GameObject Icon_Def;                     //防御中的图标
    //地图的值  0:无物体 1:有障碍物 2：可进行移动  3:已刷过权重 4:有怪物 5.可攻击
    private int[,] BattleMap = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
    //权重数组
    private int[,] Weights = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
    //玩家的战斗数据  0:HitPoint 血量  1: Defense 防御力 2:AttactDamage 物理伤害 3: Critial 暴击率 4
    //          
 //   private int[] PlayerData=new int [4]{10,1,3,10};
  

    //玩家位置
    private int Player_x;                       
    private int Player_y;

    private int ActionNumber;                             //玩家行动力

    private int ThisNumber;                               //本次怪物数量
    public bool CanBattle = false;                         //是否可进行战斗判定
    public bool CanMove = true;                         //可进行移动判定
    private bool isMove = false;                          //正在进行移动
    public bool CanTouch = true;                          //在选项处可进行点击
    public bool CanATK = false;                          //可进行攻击
    public bool HaveATK = false;                           //已进行过攻击

    public bool PlayerTurn = true;                         //当前回合，为真时为玩家回合，为假时为怪物回合
    private bool HaveWeight = false;                      //是否已刷过权重

    private Color Chick=new Color(1f,0.67f,0.74f,1f);     //状态颜色
    private Color Normal = new Color(1f, 1f, 1f, 1f);     //平常颜色
    private Color Disappear = new Color(0, 0, 0, 0);      //消失颜色
 //↓存储地形
 public GameObject BattleLand_BG;
 private GameObject ThisBattleMap;  //本次的地图实例
 public Transform[] BattleLand;
 //↓本次应该出现的怪物
 //public GameObject[] ThisMonsters = new GameObject[4];
 //↓本次应该出现的玩家角色
 public GameObject[] Players = new GameObject[4];
 //↓本次的游戏地图，用于存储应于战斗结束后销毁的对象
 private GameObject[] ThisMap=new GameObject[16];
 //↓本次的战斗角色，用于存储应于战斗结束后销毁的对象
 private GameObject[] ThisPlayers = new GameObject[4];
    //战斗图标
 private GameObject[] BattleIcons = new GameObject[3];
 private GameObject BattleIcons_ATKing;
    private GameObject BattleIcons_BeATK;
    private GameObject Battle_Def;
    //战斗时的怪物位置坐标
    private int [][] MonstersPos=new int [3][];
    //怪物数据 0:HP 1:行动力 2:攻击力 3:防御力 4:暴击
    public int[][] MonsterData_BS = new int[3][];
    public bool[] MonsterActive = new bool[3] { true, true, true };
    private bool IsDef = false;          //是否进行了防御

    //////////////////////////以下是测试数据
    private string[] monstters = { "MON_00000002"};
    //////////////////////////


 private ScriptsManager BS_SM;
 private RoleInfor BS_RI;
 private AllMonsters BS_AllMon;
 private PlayerData BS_PD;
 private CreateMonster BS_CrMon;
 private NewUIManager BS_NUIM;
 private MonstersAI BS_MAI;
 private Booty BS_Booty;
 private MissionInfor BS_MI;
 private MonsterData BS_MD;

 public void L_Start()
 {
     BS_SM = GameObject.Find("Main Camera").GetComponent<ScriptsManager>();
     BS_CrMon = BS_SM.CrMon;
     BS_RI = BS_SM.RI;
     BS_PD = BS_SM.PD;
     BS_MAI = BS_SM.MAI;
     BS_NUIM = BS_SM.NUIM;
     monster = GameObject.Find("monster");
     BS_AllMon = monster.GetComponent<AllMonsters>();
     BS_TB = BS_SM.TB;
     BS_Booty = BS_SM.Bo;
     BS_MI = BS_SM.MI;
     BS_MD = BS_SM.MD;
   
     /////////////////以下是测试数据
 //   CreateBattleScene("Forest", monstters.Length, monstters);
     /////////////////
 }
    /// <summary>
    /// 战斗结束，重置状态，并返回地图
    /// </summary>
 public void ReturnToMap()
 {
     //此处进行了玩家数据变动
     if (CanBattle)
     {
         PlayerData.ActNum = BS_PD.ActionNumber;


         BattleBG.gameObject.SetActive(false);
         Destroy(ThisBattleMap);
         Destroy(ThisPlayers[0]);
         BS_RI.NormalSign.SetActive(true);
         BS_RI.BattleSign.SetActive(false);
         for (int i = 0; i < BattleMonsters.Length; i++)
         {
             if (BattleMonsters[i] != null)
                 Destroy(BattleMonsters[i]);
             BattleMonsters[i] = null;
         }
         for (int i = 0; i < 4; i++)
             for (int j = 0; j < 4; j++)
             {
                 BattleMap[i, j] = 0;
                 Weights[i, j] = 0;
             }
         for (int i = 0; i < MonsterActive.Length; i++)
         {
             MonsterActive[i] = true;
         }
         CanBattle = false;                         //是否可进行战斗判定
         CanMove = true;                         //可进行移动判定
         isMove = false;                          //正在进行移动
         CanTouch = false;                          //在选项处可进行点击
         CanATK = false;                          //可进行攻击
         HaveATK = false;                           //已进行过攻击
         BS_TB.ToWhite();
     }
 }

 /// <summary>
 /// 战斗结束，重置状态，并返回地图
 /// </summary>
 public void ReturnToMap(string a)
 {
     //此处进行了玩家数据变动
     if (CanBattle)
     {
         PlayerData.ActNum = BS_PD.ActionNumber;


         BattleBG.gameObject.SetActive(false);
         Destroy(ThisBattleMap);
         Destroy(ThisPlayers[0]);
         BS_RI.NormalSign.SetActive(true);
         BS_RI.BattleSign.SetActive(false);
         for (int i = 0; i < BattleMonsters.Length; i++)
         {
             if (BattleMonsters[i] != null)
                 Destroy(BattleMonsters[i]);
             BattleMonsters[i] = null;
         }
         for (int i = 0; i < 4; i++)
             for (int j = 0; j < 4; j++)
             {
                 BattleMap[i, j] = 0;
                 Weights[i, j] = 0;
             }
         for (int i = 0; i < MonsterActive.Length; i++)
         {
             MonsterActive[i] = true;
         }
         CanBattle = false;                         //是否可进行战斗判定
         CanMove = true;                         //可进行移动判定
         isMove = false;                          //正在进行移动
         CanTouch = false;                          //在选项处可进行点击
         CanATK = false;                          //可进行攻击
         HaveATK = false;                           //已进行过攻击
       
     }
 }
 /// <summary>
 /// 得到怪物数据
 /// </summary>
 /// <param name="which">下标</param>
 /// <param name="HP">血量</param>
 /// <param name="ActNum">行动力</param>
 /// <param name="Atk">攻击力</param>
 /// <param name="Def">防御力</param>
 /// <param name="Cri">暴击率</param>
 /// <param name="Acc">命中率</param>
 public void GetMonstersData(int which, int HP, int actNum, int atk, int def, int cri)
 {
     MonsterData_BS[which] = new int[] { HP, actNum, atk, def, cri };
     // Debug.Log(which+"'s ActionMunber is"+MonsterData[which][1]);
 }
    /// <summary>
 ///进行战斗地图的创建，被CardEvents()中相应的卡牌类调用
 /// 首先创建战斗背景，然后确定玩家和怪物的位置，最后在地图的相应为位置创建玩家和怪物
 /// 玩家控制的角色始终被创建于第一排，怪物始终被创建于第四排
    /// </summary>
    /// <param name="number"></param>
    /// <param name="monsters"></param>
 public void CreateBattleScene(string Scene,int number,string[] monsters)
 {
 //    PlayerData.HP -= 1;
   //  Debug.Log("Player hp is "+PlayerData.HP);
     //赋予行动力
   Debug.Log("number is "+number);
     /////以下是测试数据
     for (int i = 0; i < number; i++)
     {
         int[] datas = BS_MD.GiveMonsterData(monsters[i]);
         GetMonstersData(i,datas[1],datas[2],datas[3],datas[4],datas[5]);
     }


     CanTouch = true;
    // Debug.Log("Number is "+number);
  //   for (int i = 0; i < monsters.Length;i++ )
     //  Debug.Log("Monster is " + monsters[i]);
     ActionNumber = BS_PD.ActionNumber;
     ////////////////
     ThisNumber = number;
    
  //   Debug.Log("The Battle Is Begin! and ThisNumber is "+ThisNumber);
     BS_TB.TOBlack("BattleSystem");

     //**************************************
     //此处进行了玩家数据变动
     PlayerData.Sat -= 1;
     BS_MI.ChangeInfor();
     //**************************************
         StartCoroutine(DelayAndCreate(ThisNumber, monsters));
 
 }
    /// <summary>
    /// 创建障碍牌
    /// </summary>
    /// <param name="number">障碍数目</param>
 private void CreateObatacle(int number)
 {
     int First;
     int Second;
     int Third;


     ////////////以下是测试数据
   // number = 3;
     ////////////


     if (number == 1)
     {
         First =UnityEngine.Random.Range(5, 13);
         BattleLand[First].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BattleMap/Forest/Forest_Obstacle_Tree");
      //   Debug.Log(First + "     " + ((First - 5 >= 4) ? (First - 9) : (First - 5)));
         BattleMap[((First - 5 >= 4) ? (First - 9) : (First - 5)), ((First - 9 >= 0) ? 2 : 1)] = 1;
     }
     else if (number == 2)
     {
         First = UnityEngine.Random.Range(5, 13);
     Pos2: Second = UnityEngine.Random.Range(5, 13);
         if (Second == First)
             goto Pos2;
   
         BattleLand[First].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BattleMap/Forest/Forest_Obstacle_Tree");
         BattleLand[Second].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BattleMap/Forest/Forest_Obstacle_Tree");
      //   Debug.Log(First + "     " + ((First - 5 >= 4) ? (First - 9) : (First - 5)));
         BattleMap[((First - 5 >= 4) ? (First - 9) : (First - 5)), ((First - 9 >= 0) ? 2 : 1)] = 1;
         BattleMap[((Second - 5 >= 4) ? (Second - 9) : (Second - 5)), ((Second - 9 >= 0) ? 2 : 1)] = 1;

     }
     else if (number == 3)
     {
         First = UnityEngine.Random.Range(5, 13);
     Pos2: Second = UnityEngine.Random.Range(5, 13);
         if (Second == First)
             goto Pos2;
     Pos3: Third = UnityEngine.Random.Range(5, 13);
         if (Third == First || Third == Second)
             goto Pos3;

         /////////以下是测试数据
   //  First = 9;
   //  Second = 10;
   //  Third = 11;
         /////////



         BattleLand[First].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BattleMap/Forest/Forest_Obstacle_Tree");
         BattleLand[Second].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BattleMap/Forest/Forest_Obstacle_Tree");
         BattleLand[Third].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BattleMap/Forest/Forest_Obstacle_Tree");
   //      Debug.Log(First + "     " + ((First - 5 >= 4) ? (First - 9) : (First - 5)));
        BattleMap[((First - 5 >= 4) ? (First - 9) : (First - 5)), ((First - 9 >= 0) ? 2 : 1)] = 1;
         BattleMap[((Second - 5 >= 4) ? (Second - 9) : (Second - 5)), ((Second - 9 >= 0) ? 2 : 1)] = 1;
        BattleMap[((Third - 5 >= 4) ? (Third - 9) : (Third - 5)), ((Third - 9 >= 0) ? 2 : 1)] = 1;
     }
 }
  /// <summary>
  /// 进行延时，并创建战斗场景
  /// </summary>
  /// <param name="number"></param>
  /// <param name="monsters"></param>
  /// <returns></returns>
 IEnumerator DelayAndCreate(int number, string[] monsters)
 {
     yield return new WaitForSeconds(2f);                  //进行延时
     Debug.Log("Number is "+number);
     BS_CrMon.DestroyCards();
     BS_RI.NormalSign.SetActive(false);
     BS_RI.BattleSign.SetActive(true); 
     BattleBG.gameObject.SetActive(true);
     CanBattle = true;

     int PlayerPos = 0;
     int MonsterPos1 = 0;
     int MonsterPos2 = 0;
     int MonsterPos3 = 0;


     ThisBattleMap = GameObject.Instantiate(BattleLand_BG);
     BattleLand = ThisBattleMap.GetComponentsInChildren<Transform>();
     //障碍物生成数量及位置
     int Obstacle_Number = UnityEngine.Random.Range(1, 4);
     CreateObatacle(Obstacle_Number);



     PlayerPos = UnityEngine.Random.Range(1, 5);                             //随机角色生成位置


  
     //////以下是测试数据
    // PlayerPos = 3;
     //////
     Player_x = PlayerPos - 1;
     Player_y = 0;
     BattleMap[Player_x, Player_y] = 1;

     //↓根据怪物数量随机怪物生成位置并实例化
     if (number == 1)
     {
         MonsterPos1 = UnityEngine.Random.Range(1, 5);
         BattleMap[MonsterPos1-1,3]=4;
         MonstersPos[0] = new int[] { MonsterPos1 - 1, 3 };
     }
     else if (number == 2)
     {
         MonsterPos1 = UnityEngine.Random.Range(1, 5);
     Pos2: MonsterPos2 = UnityEngine.Random.Range(1, 5);
         if (MonsterPos2 == MonsterPos1)
             goto Pos2;


         ///////以下是测试数据
         MonsterPos1 = 1;
       MonsterPos2 =3;
         ///////
         BattleMap[MonsterPos1 - 1, 3] = 4;
         BattleMap[MonsterPos2 - 1, 3] = 4;

         MonstersPos[0] =new int[] {MonsterPos1 - 1,3} ;
         MonstersPos[1] = new int[] { MonsterPos2 - 1, 3 };
     }
     else if (number == 3)
     {
         MonsterPos1 = UnityEngine.Random.Range(1, 5);
     Pos2: MonsterPos2 = UnityEngine.Random.Range(1, 5);
         if (MonsterPos2 == MonsterPos1)
             goto Pos2;
     Pos3: MonsterPos3 = UnityEngine.Random.Range(1, 5);
         if (MonsterPos3 == MonsterPos1 || MonsterPos3 == MonsterPos2)
             goto Pos3;
         BattleMap[MonsterPos1 - 1, 3] = 4;
         BattleMap[MonsterPos2 - 1, 3] = 4;
         BattleMap[MonsterPos3 - 1, 3] = 4;

         MonstersPos[0] = new int[] { MonsterPos1 - 1, 3 };
         MonstersPos[1] = new int[] { MonsterPos2 - 1, 3 };
         MonstersPos[2] = new int[] { MonsterPos3- 1, 3 };
     }

     int Num = 0;
             for (int j = 0; j < monsters.Length; j++)
                 for (int k = 0; k < BS_AllMon.Monsters.Length; k++)
                 {
                     if (BS_AllMon.Monsters[k] != null && monsters[j] == BS_AllMon.Monsters[k].name)
                     {
                         BattleMonsters[j] = BS_AllMon.Monsters[k];
                         if (j == 0)//实例化怪物1
                         {
                             BattleMonsters[j] = GameObject.Instantiate(BattleMonsters[j], new Vector3(BattleLand[12 + MonsterPos1].transform.position.x, BattleLand[12 + MonsterPos1].transform.position.y, BattleLand[12 + MonsterPos1].transform.position.z - 0.45f), BattleMonsters[j].transform.rotation) as GameObject;
                             BattleMonsters[j].GetComponent<BoxCollider>().enabled = false;
                             //怪物数据 0:HP 1:行动力 2:攻击力 3:防御力 4:暴击
                             BattleMonsters[j].transform.Find("HPPos").GetComponent<TextMesh>().text = MonsterData_BS[j][0].ToString();
                             BattleMonsters[j].transform.Find("ActNum").GetComponent<TextMesh>().text = MonsterData_BS[j][1].ToString();
                             BattleMonsters[j].transform.Find("ATK").GetComponent<TextMesh>().text = MonsterData_BS[j][2].ToString();
                             BattleMonsters[j].transform.Find("Def").GetComponent<TextMesh>().text = MonsterData_BS[j][3].ToString();
                         }
                         else if (j == 1)//实例化怪物2
                         {
                             BattleMonsters[j] = GameObject.Instantiate(BattleMonsters[j], new Vector3(BattleLand[12 + MonsterPos2].transform.position.x, BattleLand[12 + MonsterPos2].transform.position.y, BattleLand[12 + MonsterPos2].transform.position.z - 0.45f), BattleMonsters[j].transform.rotation) as GameObject;
                             BattleMonsters[j].GetComponent<BoxCollider>().enabled = false;
                             //怪物数据 0:HP 1:行动力 2:攻击力 3:防御力 4:暴击
                             BattleMonsters[j].transform.Find("HPPos").GetComponent<TextMesh>().text = MonsterData_BS[j][0].ToString();
                             BattleMonsters[j].transform.Find("ActNum").GetComponent<TextMesh>().text = MonsterData_BS[j][1].ToString();
                             BattleMonsters[j].transform.Find("ATK").GetComponent<TextMesh>().text = MonsterData_BS[j][2].ToString();
                             BattleMonsters[j].transform.Find("Def").GetComponent<TextMesh>().text = MonsterData_BS[j][3].ToString();
                         }
                         else if (j == 2)//实例化怪物3
                         {
                             BattleMonsters[j] = GameObject.Instantiate(BattleMonsters[j], new Vector3(BattleLand[12 + MonsterPos3].transform.position.x, BattleLand[12 + MonsterPos3].transform.position.y, BattleLand[12 + MonsterPos3].transform.position.z - 0.45f), BattleMonsters[j].transform.rotation) as GameObject;
                             BattleMonsters[j].GetComponent<BoxCollider>().enabled = false;
                             //怪物数据 0:HP 1:行动力 2:攻击力 3:防御力 4:暴击
                             BattleMonsters[j].transform.Find("HPPos").GetComponent<TextMesh>().text = MonsterData_BS[j][0].ToString();
                             BattleMonsters[j].transform.Find("ActNum").GetComponent<TextMesh>().text = MonsterData_BS[j][1].ToString();
                             BattleMonsters[j].transform.Find("ATK").GetComponent<TextMesh>().text = MonsterData_BS[j][2].ToString();
                             BattleMonsters[j].transform.Find("Def").GetComponent<TextMesh>().text = MonsterData_BS[j][3].ToString();
                         }
                             CanBattle = true;
                         break;
                     }
                 }
   ThisPlayers[0]= GameObject.Instantiate(Players[0], new Vector3(BattleLand[0 + PlayerPos ].transform.position.x, BattleLand[0 + PlayerPos ].transform.position.y, BattleLand[0 + PlayerPos].transform.position.z - 0.45f), Players[0].transform.rotation)as GameObject;
 //  BS_MAI.GetMap(BattleMap);
     BS_TB.ToWhite();
 }


 //↓BattleLand[,]下标与地图二维数组坐标互换
 //[(i - 3 > 0 ? (i - 7 > 0 ? (i - 11 > 0 ? i - 12 : i - 8) : i - 4) : i), (i - 3 > 0 ? (i - 7 > 0 ? (i - 11 > 0 ? 3 : 2) : 1) : 0)]

    /// <summary>
    /// 进行点击检测，当怪物全被消灭时销毁战斗地图   
    /// </summary>
	void Update () {
        Ray Ray_Battle = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit = new RaycastHit();
        if (Input.GetMouseButtonDown(0)&&CanBattle)
        {
              if (Physics.Raycast(Ray_Battle, out Hit))
               {
                  Debug.Log("Hit is "+Hit.transform.name);
                   for (int i = 0; i < 16; i++)
                   {     //可进行战斗
                       if (BattleMap[(i - 3 > 0 ? (i - 7 > 0 ? (i - 11 > 0 ? i - 12 : i - 8) : i - 4) : i), (i - 3 > 0 ? (i - 7 > 0 ? (i - 11 > 0 ? 3 : 2) : 1) : 0)] == 5 && CanATK && Hit.transform.name == BattleLand[i + 1].name)
                       {
                           ATKing(true,true, (i - 3 > 0 ? (i - 7 > 0 ? (i - 11 > 0 ? i - 12 : i - 8) : i - 4) : i), (i - 3 > 0 ? (i - 7 > 0 ? (i - 11 > 0 ? 3 : 2) : 1) : 0));
                       }
                       //点击地图上颜色改变的某点，即决定移动时
                       if (CanMove && Hit.transform.name == BattleLand[i + 1].name && !isMove && BattleMap[(i - 3 > 0 ? (i - 7 > 0 ? (i - 11 > 0 ? i - 12 : i - 8) : i - 4) : i), (i - 3 > 0 ? (i - 7 > 0 ? (i - 11 > 0 ? 3 : 2) : 1) : 0)] == 2)
                       {
                           Battle_Player_Move(false);
                               isMove = true;
                           ShortestMove((i - 3 > 0 ? (i - 7 > 0 ? (i - 11 > 0 ? i - 12 : i - 8) : i - 4) : i),(i - 3 > 0 ? (i - 7 > 0 ? (i - 11 > 0 ? 3 : 2) : 1) : 0));
                       }
                    
                   }
               }
        }
        if (PlayerData.ActNum == 0 && CanTouch && CanBattle)
        {
            BS_NUIM.OverTurn.transform.DOScale(1, 0.3f);
        }
	}
    /// <summary>
    /// 重置行动力，改变当前回合主角
    /// </summary>
    public void ChangeTurn(string which)
    {
       
        switch (which)
        {
            case "Player":
                //此处进行了玩家数据变动
                PlayerData.ActNum = BS_PD.ActionNumber;
                if (IsDef)
                PlayerData.Def -= (int)BS_PD.AttactDamage / 3;
                IsDef = false;
                BS_MI.ChangeInfor();
               HaveATK = false;
                PlayerTurn = true;
                Battle_Player_Defence(false);
                CanTouch = true;
                HaveWeight = false;
        //        Debug.Log("this is " + which);
                ActionNumber = BS_PD.ActionNumber;
                PlayerData.ActNum = BS_PD.ActionNumber;
                BS_NUIM.OverTurn.transform.DOScale(0, 0.3f);
                break;
            case "Monster":
          //      Debug.Log("this is " + which);
                PlayerTurn = false;
            //      Debug.Log("Player is  "+Player_x+"  "+Player_y);
                  CanTouch = false;
                  BS_NUIM.OverTurn.transform.DOScale(0, 0.3f);
                BS_MAI.GetPos( MonsterActive ,MonsterData_BS,ThisPlayers[0],BattleMap,BattleLand,BattleMonsters,MonstersPos, Player_x, Player_y);   //向AI传递坐标
             //     ActionNumber = BS_PD.ActionNumber;
          
             //     CanTouch = true;
            //    MonsterTurn = true;
              
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 销毁战斗地图
    /// </summary>
    public void DestroyBattleMap()
{
    BS_TB.TOBlack("BattleSystem");
    StartCoroutine(DelayAndReturnToMap());
}
    IEnumerator DelayAndReturnToMap()
    {
        yield return new WaitForSeconds(2f);
        BattleBG.gameObject.SetActive(false);
        BS_TB.ToWhite();
    }
    /// <summary>
    /// 关闭警告延时
    /// </summary>
    /// <returns></returns>
    IEnumerator Close()
    {
        yield return new WaitForSeconds(1.5f);
        Tweener Close = BS_NUIM.WarningText.transform.DOScale(0, 0.3f);
        Close.OnComplete(delegate() { CanTouch = true; }); 
    }
    /// <summary>
    /// 当点击移动时，给整张地图刷权重
    /// </summary>
    /// <param name="mmmove">参数，为真时进行正常地图赋权过程，为假时改变地图颜色为正常</param>
    public void Battle_Player_Move(bool mmmove)
    {
        CanMove = true;
        if (!mmmove)
        {
            for (int k = 0; k < 4; k++)
                for (int j = 0; j < 4; j++)
                {
                    if (Weights[k, j] <= 3)
                        BattleLand[k + 1 + j * 4].GetComponent<SpriteRenderer>().color = Normal;
                }
         //   CanMove = false;
        }
        else
        {
          if (HaveWeight)
          {    
              for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        //遍历地图，将权重小于等于行动力的点赋予"可移动"状态
                        if (Weights[i, j] <= PlayerData.ActNum)
                        {
                            //坐标与数组下标互换
                            BattleLand[i + 1 + j * 4].GetComponent<SpriteRenderer>().color = Chick;
                            BattleMap[i, j] = 2;
                        }
                    }
          }
          if (!HaveWeight)
            {
                if (PlayerData.ActNum <= 0)
                {
                    //警告部分
                    CanMove = false;  //警告过程中不能进行点击判定
                    BS_NUIM.WarningText.GetComponent<Text>().text = "没行动力了！";
                    BS_NUIM.WarningText.transform.DOScale(1, 0.3f);
                    StartCoroutine(Close());
                }
                ///////判断地图权重
                int Weight_1 = 1;       //赋值权重
                int Weight_2 = 1;         //判断权重
                int Move_x = Player_x;    //判断点x
                int Move_y = Player_y;  //判断点y
                bool Over = false;      //当前权重点是否全部寻找完毕
                bool AllOver = true;      //所有权重点是否已寻找完毕

                //在赋权开始前，先遍历地图，将不可到达的点权重全部赋20
                for (int q = 0; q < 4; q++)
                    for (int w = 0; w < 4; w++)
                    {
                        if (!AstarFindWay(q, w, Player_x, Player_y))
                            Weights[q, w] = 20;
                    }
            //AAAA:赋予权重的起点，当所有可赋权重点均有值之后，跳出循环
            AAAA: AllOver = true;
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        //当一点处于未赋予权重的状态时，执行赋权过程
                        if (Weights[i, j] == 0)
                        {
                            AllOver = false;
                            goto DDDD;
                        }
                    }
                if (AllOver)
                    goto CCCC;
            //DDDD:赋予权重
            DDDD: if ((Move_x - 1 >= 0 && Weights[Move_x - 1, Move_y] == 0 && BattleMap[Move_x - 1, Move_y] == 0) || (Move_x + 1 <= 3 && Weights[Move_x + 1, Move_y] == 0 && BattleMap[Move_x + 1, Move_y] == 0) || (Move_y - 1 >= 0 && Weights[Move_x, Move_y - 1] == 0 && BattleMap[Move_x, Move_y - 1] == 0) || (Move_y + 1 <= 3 && Weights[Move_x, Move_y + 1] == 0 && BattleMap[Move_x, Move_y + 1] == 0))
                {
                    if (Move_x - 1 >= 0 && BattleMap[Move_x - 1, Move_y] == 0 && Weights[Move_x - 1, Move_y] == 0)
                    {
                        Weights[Move_x - 1, Move_y] = Weight_1;
                    }
                    else if (Move_x + 1 <= 3 && BattleMap[Move_x + 1, Move_y] == 0 && Weights[Move_x + 1, Move_y] == 0)
                    {
                        Weights[Move_x + 1, Move_y] = Weight_1;
                    }
                    else if (Move_y - 1 >= 0 && BattleMap[Move_x, Move_y - 1] == 0 && Weights[Move_x, Move_y - 1] == 0)
                    {
                        Weights[Move_x, Move_y - 1] = Weight_1;
                    }
                    else if (Move_y + 1 <= 3 && BattleMap[Move_x, Move_y + 1] == 0 && Weights[Move_x, Move_y + 1] == 0)
                    {
                        Weights[Move_x, Move_y + 1] = Weight_1;
                    }
                    goto AAAA;
                }
                else
                {
                //BBBB:当一轮的权重全部赋完后，找寻此轮的某一点作为新一轮权重赋值的起点
                BBBB: for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                        {
                            if (BattleMap[i, j] == 1 || BattleMap[i, j] == 4)
                            {
                                Weights[i, j] = 20;
                            }
                            if (Weights[i, j] == Weight_2 && BattleMap[i, j] != 3 && BattleMap[i, j] == 0)
                            {
                                Over = false;
                                BattleMap[i, j] = 3;
                                Move_x = i;
                                Move_y = j;
                                Weight_1 += 1;
                                if (Weight_1 - Weight_2 > 1)
                                    Weight_1 -= 1;
                                goto AAAA;
                            }
                            else
                                Over = true;
                        }
                    if (Over)
                    {
                        Weight_2 += 1;
                        goto BBBB;
                    }
                }
            //CCCC:跳出权重循环
        CCCC:
            HaveWeight = true;
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        //遍历地图，将权重小于等于行动力的点赋予"可移动"状态
                        if (Weights[i, j] <= PlayerData.ActNum)
                        {
                            //坐标与数组下标互换
                            BattleLand[i + 1 + j * 4].GetComponent<SpriteRenderer>().color = Chick;
                            BattleMap[i, j] = 2;
                        }
                    }
            }
        }
    }
    /// <summary>
    /// A星寻路算法
    /// </summary>
    /// <param name="StartX">寻路起点X坐标</param>
    /// <param name="StartY">寻路起点Y坐标</param>
    /// <param name="EndX">寻路终点X坐标</param>
    /// <param name="EndY">寻路终点Y坐标</param>
    /// <returns></returns>
    private bool AstarFindWay(int StartX,int StartY,int EndX,int EndY)
    {
        int[,] CloseMap = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };         //关闭列表与战斗地图相等  1为关闭
        int[,] OpenMap = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };          //开启列表 ,1为可判断，0为既没开启也没关闭 2为关闭
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                CloseMap[i, j] = BattleMap[i, j];
            }
        int thisX=StartX;                //本次寻路点
        int thisY = StartY;              //本次寻路点
        if (BattleMap[thisX,thisY]==1||BattleMap[thisX,thisY]==4)   //寻路点为障碍物
            return false; 
        //Debug.Log("UI");
        //AAAAAA:寻路循环的起点
        AAAAAA:
        CloseMap[thisX, thisY] = 1;
        OpenMap[thisX, thisY] = 2;
        int WayLength_Up = 100;           //距终点的四方向路径距离 
        int WayLength_Down= 100;
        int WayLength_Left = 100;
        int WayLength_Right = 100;
        int[] WayLength = new int[4]{100,100,100,100};
        int Min = 0;                      //最短路径点
        bool CanFind = false;             //可找到通路
        //四方向判断
        if ((thisX - 1 == EndX && thisY == EndY)||(thisX+1==EndX&&thisY==EndY)||(thisX==EndX&&thisY+1==EndY)||(thisX==EndX&&thisY-1==EndY))//可到达终点
        {
            return true;
        }
        else
        {
            if (thisX - 1 >= 0 && CloseMap[thisX - 1, thisY] != 1 && CloseMap[thisX - 1, thisY] != 4)
            {
                OpenMap[thisX - 1, thisY] = 1;
                WayLength_Left = Mathf.Abs(thisX - 1 - EndX) + Mathf.Abs(thisY  - EndY);
                WayLength[0] = WayLength_Left;
                CanFind = true;
            }
            if (thisX + 1 <= 3 && CloseMap[thisX + 1, thisY] != 1 && CloseMap[thisX + 1, thisY] != 4)
            {
                OpenMap[thisX + 1, thisY] = 1;
                WayLength_Right = Mathf.Abs(thisX + 1 - EndX) + Mathf.Abs(thisY - EndY);
                WayLength[1] = WayLength_Right;
                CanFind = true;
            }
            if (thisY - 1 >= 0 && CloseMap[thisX, thisY - 1] != 1 && CloseMap[thisX, thisY - 1] != 4)
            {
                OpenMap[thisX, thisY - 1] = 1;
                WayLength_Down = Mathf.Abs(thisX - EndX) + Mathf.Abs(thisY - 1 - EndY);
                WayLength[2] = WayLength_Down;
                CanFind = true;
            }
            if (thisY + 1 <= 3 && CloseMap[thisX, thisY + 1] != 1 && CloseMap[thisX, thisY + 1] != 4)
            {
                OpenMap[thisX, thisY + 1] = 1;
                WayLength_Up = Mathf.Abs(thisX - EndX) + Mathf.Abs(thisY + 1 - EndY);
                WayLength[3] = WayLength_Up;
                CanFind = true;
            }
            Min=WayLength[0];
            for (int i = 0; i < 3; i++)
            {
                if (WayLength[i + 1] < Min)
                    Min = WayLength[i + 1];
            }
                //更换节点
                if (Min == WayLength_Left)
                {
                    thisX -= 1;
                }
                else if (Min == WayLength_Right)
                {
                    thisX += 1;
                }
                else if (Min == WayLength_Down)
                {
                    thisY -= 1;
                }
                else if (Min == WayLength_Up)
                {
                    thisY += 1;
                }
        }
        if (CanFind)
            goto AAAAAA;
        else
        {
            //上一次寻路未成功，寻找某个开启点作为新寻路的起点
            bool HavePoint = false;
            for(int l=0;l<4;l++)
                for (int p=0;p<4;p++)
                {
                    if (OpenMap[l,p]==1)
                    {
                        thisX=l;
                        thisY=p;
                        HavePoint = true;
                        goto AAAAAA;
                    }
                }
            if (!HavePoint)       //为假时，寻路失败，不存在可到达路径
                return false;
        }
        return false;
    }
    /// <summary>
    /// 根据目的地坐标找出最短路径，并做出最短移动，路径权重相同时，按照计算时的路径进行移动
    /// </summary>
    private void ShortestMove(int PosX,int PosY)
    {
        Debug.Log("X is "+PosX+" Y is "+PosY);
        //移动开始，置玩家初始位置状态为0
        BattleMap[Player_x, Player_y] = 0; ;
        int[][] Pos=new int [10][];  //存储路径点
        Pos[0]=new int[]{PosX,PosY};
        int Way = 0;
    FindWay:
        //当某一点即将移动到玩家位置时
        if ((PosX - 1 == Player_x && PosY == Player_y) || (PosX +1 == Player_x && PosY == Player_y) || (PosX  == Player_x && PosY-1 == Player_y) || (PosX  == Player_x && PosY +1== Player_y))
        {
            goto B_Move;
        }
        else
        {
            if (PosX - 1 >= 0 && Weights[PosX  - 1, PosY] < Weights[PosX, PosY])
            {
                PosX = PosX-1;
                Way += 1;
                Pos[Way] = new int[] { PosX, PosY };
            }
            else if (PosX + 1 <= 3 && Weights[PosX + 1, PosY] < Weights[PosX,  PosY])
            {
                PosX = PosX+ 1;
                Way += 1;
                Pos[Way] = new int[] { PosX, PosY };
            }
            else if ( PosY- 1 >= 0 && Weights[PosX, PosY - 1] < Weights[PosX,  PosY])
            {
                PosY = PosY-1;
                Way += 1;
                Pos[Way] = new int[] { PosX, PosY };
            }
            else if (PosY + 1 <= 3 && Weights[PosX, PosY + 1] < Weights[PosX, PosY])
            {
                PosY = PosY+1;
                Way += 1;
                Pos[Way] = new int[] { PosX, PosY };
            }
        }
    if (PosX != Player_x && PosY != Player_y)
            goto FindWay;
            //    ThisPlayers[0].transform.DOMove(new Vector3(BattleLand[i + 1].transform.position.x, BattleLand[i + 1].transform.position.y, BattleLand[i + 1].transform.position.z - 0.45f), 1f);
    B_Move:
        //开始移动
        if (CanMove)
        StartCoroutine(Moveeeeeeee(Way,Pos));
    }
    //
    IEnumerator Moveeeeeeee(int way,int [][] poses)
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("way is "+way);
        int i = way;
        int Number = 0;
        int[][] Poses = poses;
        CanMove = false;           //移动过程中不能进行点击判定
        CanTouch = false;
        if (i >= 0)    //仍有路径点未移动时
        {
            
            Number = Poses[i][0] + 1 + Poses[i][1] * 4;
            Tweener M = ThisPlayers[0].transform.DOMove(new Vector3(BattleLand[Number].transform.position.x, BattleLand[Number].transform.position.y, BattleLand[Number].transform.position.z - 0.45f), 0.2f);
            i -= 1;
            M.SetEase(Ease.OutCubic);
            //一次移动结束后，延时开始下一次移动
            M.OnComplete(delegate() { StartCoroutine(Moveeeeeeee(i, poses)); });
        }
        else          //移动结束时
        {
            isMove = false;//移动结束
            //此处进行了玩家数据变动
            PlayerData.ActNum -= Weights[poses[0][0], poses[0][1]];
            BS_MI.ChangeInfor();
           
        }
        //移动结束，重置地图权重值
        if (!isMove)
        {
            //重置玩家坐标
            CanMove = true;
            CanTouch = true;
            Player_x = Poses[0][0];
            Player_y = Poses[0][1];
            BattleMap[Player_x,Player_y]=1;
           
            //重置权重值
            for (int k=0;k<4;k++)
                for (int j=0;j<4;j++)
                {
                    if (BattleMap[k, j] != 1 && BattleMap[k, j] != 4 && BattleMap[k, j] != 5) //地图上无物体时，权重为0
                    {
                        Weights[k, j] = 0;
                        BattleMap[k, j] = 0;
                    }
                    else
                        Weights[k, j] = 20;   //地图上有物体时，权重为20
                }
        }
    }
    public void Monster_RemoveMap(int [][] newPos)
    {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (BattleMap[i, j] == 1) //地图上有物体时，权重为20
                    {
                        Weights[i, j] = 20;
                    }
                    else if (BattleMap[i, j] == 4)  //旧怪物位置，置0
                    {
                        Weights[i, j] = 0; 
                        BattleMap[i, j] = 0;
                    } 
                }
      for (int m = 0; m < ThisNumber;m++)
          for (int i = 0; i < 4; i++)
              for (int j = 0; j < 4; j++)
              {
                  if (i == newPos[m][0] && j == newPos[m][1]&&MonsterActive[m])
                  {
                      MonstersPos[m] = newPos[m];
                  //    Debug.Log("Monster "+m+"'s pos is "+MonstersPos[m][0]+"  "+MonstersPos[m][1]);
                      BattleMap[i, j] = 4;
                      Weights[i, j] = 20;
                  }
              }
      /*     for (int i = 0; i < 4; i++)
               for (int j = 0; j < 4; j++)
               {
                   Debug.Log("BattleMap["+i+","+j+"] is "+BattleMap[i,j]+"  and Weights["+i+","+j+"] is "+Weights[i,j]);
               }*/
    }
    /// <summary>
    /// 重置权重值
    /// </summary>
    private void RemoveWeights()
    {
    }
    /// <summary>
    /// 控制玩家攻击
    /// </summary>
    public void Battle_Player_Attack()
    {
        CanMove = false;
           for (int i = 0; i < 4; i++)
               for (int j = 0; j < 4; j++)
               {
                 //  Debug.Log("BattleMap["+i+","+j+"] is "+BattleMap[i,j]+"  and Weights["+i+","+j+"] is "+Weights[i,j]);
               }
        if (HaveATK)
        {
            BS_NUIM.WarningText.GetComponent<Text>().text = "本回合已进行过攻击！";
            BS_NUIM.WarningText.transform.DOScale(1, 0.3f);
            StartCoroutine(Close());
        }
        else
        {
            CanATK = false;
            bool HaveMonster = false;
            int Icon_Count = 0;
            if (PlayerData.ActNum <= 0)
            {
                //警告部分
                BS_NUIM.WarningText.GetComponent<Text>().text = "没行动力了！";
                BS_NUIM.WarningText.transform.DOScale(1, 0.3f);
                CanTouch = false;
                StartCoroutine(Close());
            }
            else
            {
                //对玩家所处位置的八个方向判断，若有怪物则将怪物卡颜色变红，若没有弹出警告："攻击范围内没有怪物"
                if (Player_x + 1 <= 3 && (BattleMap[Player_x + 1, Player_y] == 4 || BattleMap[Player_x + 1, Player_y] == 5))
                {
                    BattleIcons[Icon_Count] = (GameObject)GameObject.Instantiate(Icon_CanAtk, new Vector3(BattleLand[Player_x + 2 + Player_y * 4].transform.position.x, BattleLand[Player_x + 2 + Player_y * 4].transform.position.y, BattleLand[Player_x + 2 + Player_y * 4].transform.position.z - 0.47f), Icon_CanAtk.transform.rotation);
                    // BattleIcons[Icon_Count].transform.SetParent(BattleMonsters[Icon_Count].transform);
                    for (int i = 0; i < MonstersPos.Length; i++)
                    {
                        if (MonstersPos[i] != null && Player_x + 1 == MonstersPos[i][0] && Player_y == MonstersPos[i][1])
                        {
                            BattleIcons[Icon_Count].transform.SetParent(BattleMonsters[i].transform);
                        }
                    }
                    Icon_Count += 1;

                    HaveMonster = true;
                    BattleMap[Player_x + 1, Player_y] = 5;//置此点为可攻击状态
                }
                if (Player_x - 1 >= 0 && (BattleMap[Player_x - 1, Player_y] == 4 || BattleMap[Player_x - 1, Player_y] == 5))
                {
                    BattleIcons[Icon_Count] = (GameObject)GameObject.Instantiate(Icon_CanAtk, new Vector3(BattleLand[Player_x + Player_y * 4].transform.position.x, BattleLand[Player_x + Player_y * 4].transform.position.y, BattleLand[Player_x + Player_y * 4].transform.position.z - 0.47f), Icon_CanAtk.transform.rotation);
                    for (int i = 0; i < MonstersPos.Length; i++)
                    {
                        if (MonstersPos[i] != null && Player_x - 1 == MonstersPos[i][0] && Player_y == MonstersPos[i][1])
                        {
                            BattleIcons[Icon_Count].transform.SetParent(BattleMonsters[i].transform);
                        }
                    }
                    Icon_Count += 1;
                    HaveMonster = true;
                    BattleMap[Player_x - 1, Player_y] = 5;//置此点为可攻击状态
                }
                if (Player_y + 1 <= 3 && (BattleMap[Player_x, Player_y + 1] == 4 || BattleMap[Player_x, Player_y + 1] == 5))
                {
                    BattleIcons[Icon_Count] = (GameObject)GameObject.Instantiate(Icon_CanAtk, new Vector3(BattleLand[Player_x + 1 + (Player_y + 1) * 4].transform.position.x, BattleLand[Player_x + 1 + (Player_y + 1) * 4].transform.position.y, BattleLand[Player_x + 1 + (Player_y + 1) * 4].transform.position.z - 0.47f), Icon_CanAtk.transform.rotation);
                    for (int i = 0; i < MonstersPos.Length; i++)
                    {
                        if (MonstersPos[i] != null && Player_x == MonstersPos[i][0] && Player_y + 1 == MonstersPos[i][1])
                        {
                            BattleIcons[Icon_Count].transform.SetParent(BattleMonsters[i].transform);
                        }
                    }
                    Icon_Count += 1;
                    HaveMonster = true;
                    BattleMap[Player_x, Player_y + 1] = 5;//置此点为可攻击状态
                }
                if (Player_y - 1 >= 0 && (BattleMap[Player_x, Player_y - 1] == 4 || BattleMap[Player_x, Player_y - 1] == 5))
                {
                    BattleIcons[Icon_Count] = (GameObject)GameObject.Instantiate(Icon_CanAtk, new Vector3(BattleLand[Player_x + 1 + (Player_y - 1) * 4].transform.position.x, BattleLand[Player_x + 1 + (Player_y - 1) * 4].transform.position.y, BattleLand[Player_x + 1 + (Player_y - 1) * 4].transform.position.z - 0.47f), Icon_CanAtk.transform.rotation);
                    for (int i = 0; i < MonstersPos.Length; i++)
                    {
                        if (MonstersPos[i] != null && Player_x == MonstersPos[i][0] && Player_y - 1 == MonstersPos[i][1])
                        {
                            BattleIcons[Icon_Count].transform.SetParent(BattleMonsters[i].transform);
                        }
                    }
                    Icon_Count += 1;
                    HaveMonster = true;
                    BattleMap[Player_x, Player_y - 1] = 5;//置此点为可攻击状态
                }
                if (Player_x + 1 <= 3 && Player_y + 1 <= 3 && (BattleMap[Player_x + 1, Player_y] != 1|| BattleMap[Player_x, Player_y + 1] != 1) && (BattleMap[Player_x + 1, Player_y + 1] == 4 || BattleMap[Player_x + 1, Player_y + 1] == 5))
                {
                    BattleIcons[Icon_Count] = (GameObject)GameObject.Instantiate(Icon_CanAtk, new Vector3(BattleLand[Player_x + 2 + (Player_y + 1) * 4].transform.position.x, BattleLand[Player_x + 2 + (Player_y + 1) * 4].transform.position.y, BattleLand[Player_x + 2 + (Player_y + 1) * 4].transform.position.z - 0.47f), Icon_CanAtk.transform.rotation);
                    for (int i = 0; i < MonstersPos.Length; i++)
                    {
                        if (MonstersPos[i] != null && Player_x + 1 == MonstersPos[i][0] && Player_y + 1 == MonstersPos[i][1])
                        {
                            BattleIcons[Icon_Count].transform.SetParent(BattleMonsters[i].transform);
                        }
                    }
                    Icon_Count += 1;
                    HaveMonster = true;
                    BattleMap[Player_x + 1, Player_y + 1] = 5;//置此点为可攻击状态
                }
                if (Player_x + 1 <= 3 && Player_y - 1 >= 0 && (BattleMap[Player_x + 1, Player_y] != 1 || BattleMap[Player_x, Player_y - 1] != 1) && (BattleMap[Player_x + 1, Player_y - 1] == 4 || BattleMap[Player_x + 1, Player_y - 1] == 5))
                {
                    BattleIcons[Icon_Count] = (GameObject)GameObject.Instantiate(Icon_CanAtk, new Vector3(BattleLand[Player_x + 2 + (Player_y - 1) * 4].transform.position.x, BattleLand[Player_x + 2 + (Player_y - 1) * 4].transform.position.y, BattleLand[Player_x + 2 + (Player_y - 1) * 4].transform.position.z - 0.47f), Icon_CanAtk.transform.rotation);
                    for (int i = 0; i < MonstersPos.Length; i++)
                    {
                        if (MonstersPos[i] != null && Player_x + 1 == MonstersPos[i][0] && Player_y - 1 == MonstersPos[i][1])
                        {
                            BattleIcons[Icon_Count].transform.SetParent(BattleMonsters[i].transform);
                        }
                    }
                    Icon_Count += 1;
                    HaveMonster = true;
                    BattleMap[Player_x + 1, Player_y - 1] = 5;//置此点为可攻击状态
                }
                if (Player_x - 1 >= 0 && Player_y + 1 <= 3 && (BattleMap[Player_x - 1, Player_y] != 1 || BattleMap[Player_x, Player_y + 1] != 1) && (BattleMap[Player_x - 1, Player_y + 1] == 4 || BattleMap[Player_x - 1, Player_y + 1] == 5))
                {
                    BattleIcons[Icon_Count] = (GameObject)GameObject.Instantiate(Icon_CanAtk, new Vector3(BattleLand[Player_x + (Player_y + 1) * 4].transform.position.x, BattleLand[Player_x + (Player_y + 1) * 4].transform.position.y, BattleLand[Player_x + (Player_y + 1) * 4].transform.position.z - 0.47f), Icon_CanAtk.transform.rotation);
                    for (int i = 0; i < MonstersPos.Length; i++)
                    {
                        if (MonstersPos[i] != null && Player_x - 1 == MonstersPos[i][0] && Player_y + 1 == MonstersPos[i][1])
                        {
                            BattleIcons[Icon_Count].transform.SetParent(BattleMonsters[i].transform);
                        }
                    }
                    Icon_Count += 1;
                    HaveMonster = true;
                    BattleMap[Player_x - 1, Player_y + 1] = 5;//置此点为可攻击状态
                }
                if (Player_x - 1 >= 0 && Player_y - 1 >= 0 && (BattleMap[Player_x - 1, Player_y] != 1||BattleMap[Player_x, Player_y - 1] != 1) && (BattleMap[Player_x - 1, Player_y - 1] == 4 || BattleMap[Player_x - 1, Player_y - 1] == 5))
                {
                    BattleIcons[Icon_Count] = (GameObject)GameObject.Instantiate(Icon_CanAtk, new Vector3(BattleLand[Player_x + (Player_y - 1) * 4].transform.position.x, BattleLand[Player_x + (Player_y - 1) * 4].transform.position.y, BattleLand[Player_x + (Player_y - 1) * 4].transform.position.z - 0.47f), Icon_CanAtk.transform.rotation);
                    for (int i = 0; i < MonstersPos.Length; i++)
                    {
                        if (MonstersPos[i] != null && Player_x - 1 == MonstersPos[i][0] && Player_y - 1 == MonstersPos[i][1])
                        {
                            BattleIcons[Icon_Count].transform.SetParent(BattleMonsters[i].transform);
                        }
                    }
                    Icon_Count += 1;
                    HaveMonster = true;
                    BattleMap[Player_x - 1, Player_y - 1] = 5;//置此点为可攻击状态
                }
                if (!HaveMonster)
                {
                    //警告部分
                    BS_NUIM.WarningText.GetComponent<Text>().text = "范围内没有怪物！";
                    BS_NUIM.WarningText.transform.DOScale(1, 0.3f);
                    StartCoroutine(Close());
                }
                else
                    CanATK = true;
            }
        }
    }
    /// <summary>
    /// 正在进行攻击,每次攻击结束后行动力减一
    /// </summary>
    /// <param name="ATK">判断参数，为真时演示攻击动画，为假时销毁战斗图标</param>
    /// <param name="PosX">需要进行动画的物体的X坐标</param>
    /// <param name="PosY">需要进行动画的物体的Y坐标</param>
    public void ATKing(bool player,bool ATK,int PosX,int PosY)
    {
        if (ATK)
        {
        //    Debug.Log("PosX is "+PosX+"   PosY is "+PosY+"BattleMap["+PosX+","+PosY+"] is "+BattleMap[PosX,PosY]);
          //置可攻击怪物的状态为不可攻击
                for (int p = 0; p < 4; p++)
                    for (int m = 0; m < 4; m++)
                    {
                        if (BattleMap[p, m] == 5)
                            BattleMap[p, m] = 4;
                    }
                for (int i = 0; i < MonstersPos.Length; i++)
                {
                    CanATK = false;
                    if (MonstersPos[i] != null && PosX == MonstersPos[i][0] && PosY == MonstersPos[i][1]&&MonsterActive[i])
                    {
                        MonsterData_BS[i][0] -= PlayerData.Atk;
                        BattleMonsters[i].transform.Find("HPPos").GetComponent<TextMesh>().text = MonsterData_BS[i][0].ToString();
                      //  Debug.Log("monster" + i + "'s hp is" + MonsterData_BS[i][0]);
                        for (int j = 0; j < BattleIcons.Length; j++)
                        {
                            if (BattleIcons[j] != null)
                                Destroy(BattleIcons[j]);
                            else
                                break;
                        }
                        BattleIcons_ATKing = (GameObject)GameObject.Instantiate(Icon_Atking, new Vector3(BattleLand[PosX + 1 + PosY * 4].transform.position.x, BattleLand[PosX + 1 + PosY * 4].transform.position.y, BattleLand[PosX + 1 + PosY * 4].transform.position.z - 0.47f), Icon_Atking.transform.rotation);

                        BattleIcons_BeATK = (GameObject)GameObject.Instantiate(Icon_BeAtk, new Vector3(BattleLand[PosX + 1 + PosY * 4].transform.position.x, BattleLand[PosX + 1 + PosY * 4].transform.position.y, BattleLand[PosX + 1 + PosY * 4].transform.position.z - 0.47f), Icon_BeAtk.transform.rotation);
                        BattleIcons_BeATK.transform.SetParent(BattleMonsters[i].transform);

                        BattleIcons_ATKing.GetComponent<SpriteRenderer>().DOColor(Disappear, 0.2f);
                        Sequence Battle_BeAtk = DOTween.Sequence();
                        Battle_BeAtk.Append(BattleMonsters[i].transform.DOMove(new Vector3(BattleMonsters[i].transform.position.x - 0.1f, BattleMonsters[i].transform.position.y - 0.1f, BattleMonsters[i].transform.position.z), 0.05f));
                        Battle_BeAtk.Append(BattleMonsters[i].transform.DOMove(new Vector3(BattleMonsters[i].transform.position.x, BattleMonsters[i].transform.position.y, BattleMonsters[i].transform.position.z), 0.05f));
                        Battle_BeAtk.Append(BattleMonsters[i].transform.DOMove(new Vector3(BattleMonsters[i].transform.position.x - 0.1f, BattleMonsters[i].transform.position.y - 0.1f, BattleMonsters[i].transform.position.z), 0.05f));
                        Battle_BeAtk.Append(BattleMonsters[i].transform.DOMove(new Vector3(BattleMonsters[i].transform.position.x, BattleMonsters[i].transform.position.y, BattleMonsters[i].transform.position.z), 0.05f));

                        Battle_BeAtk.OnComplete(delegate() { Destroy(BattleIcons_BeATK); Destroy(BattleIcons_ATKing); PlayerData.ActNum -= 1; });
                        if (MonsterData_BS[i][0] <= 0)
                        {
                            int thiss=i;
                            BattleMap[MonstersPos[i][0], MonstersPos[i][1]] = 0;
                            Weights[MonstersPos[i][0], MonstersPos[i][1]] = 0;
                            MonsterActive[i] = false;
                            TextMesh[] TM = BattleMonsters[i].GetComponentsInChildren<TextMesh>();
                            foreach (TextMesh t in TM)
                            {
                                t.fontSize = 0;
                            }
                       
                            BattleMonsters[i].GetComponentInChildren<TextMesh>().fontSize = 0;
                           Tweener Dis= BattleMonsters[i].GetComponent<SpriteRenderer>().DOColor(Disappear,0.5f);

                           
                        //   Dis.OnComplete(delegate() { Debug.Log(thiss + "will be destroy"); Destroy(BattleMonsters[thiss]); });
                        }

                        //判断战斗是否结束
                        int OverCount=0;
                        for (int o = 0; o < ThisNumber; o++)
                        {
                            if (!MonsterActive[o])
                                OverCount += 1;
                        }
                        if (OverCount == ThisNumber)//怪物已被消灭，战斗结束
                        {
                            CanTouch = false;
                           // Debug.Log("Battle is Over");
                           // Debug.Log("THIS NUM IS "+ThisNumber);
                            BS_NUIM.BattleOver.transform.DOScale(1, 0.3f);
                            int BootyNumber = UnityEngine.Random.Range(1, ThisNumber);
                            for (int u = 0; u < BootyNumber; u++)
                            {
                                BS_NUIM.Bootys[u].GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/SmallIcons/" + BS_Booty.GiveBooty());
                                Debug.Log("Booty name is"+BS_NUIM.Bootys[u].GetComponent<Image>().sprite.name);
                                BS_PD.PutIntoBag(BS_NUIM.Bootys[u].GetComponent<Image>().sprite.name);
                            }
                        }
                            HaveATK = true;
                    }
                }       
        }
        else
        {
            for (int i = 0; i < BattleIcons.Length; i++)
            {
                if (BattleIcons[i] != null)
                    Destroy(BattleIcons[i]);
                else
                    break;
            }
        }
    }
    //攻击时的伤害检定
    public void ATKCount(string name)
    {
        if (name == "Player")
        {
        }
         if (name=="Monster1")
        {
        }
          if (name=="Monster2")
        {
        }
    }
    /// <summary>
    /// 控制玩家防御
    /// </summary>
    /// <param name="PosX">需要防御的物体的X坐标</param>
    /// <param name="PosY">需要防御的物体的Y坐标</param>
    /// <param name="DEF">判断参数，为真时进行正常防御，为假时解除防御状态</param>
    public void Battle_Player_Defence(bool DEF)
    {
        CanMove = false;
        if (DEF)   //进入防御状态
        {
            if (PlayerData.ActNum <= 0)
            {
                //警告部分
                BS_NUIM.WarningText.GetComponent<Text>().text = "没行动力了！";
                BS_NUIM.WarningText.transform.DOScale(1, 0.3f);
                CanTouch = false;
                StartCoroutine(Close());
            }
            else
            {
            //    Debug.Log("Defence");

                //**************************************
                //此处进行了玩家数据变动
                PlayerData.Def += (int)BS_PD.AttactDamage/3;
                IsDef = true;
                PlayerData.ActNum = 0;
                BS_MI.ChangeInfor();
                //**************************************
                Battle_Def = GameObject.Instantiate(Icon_Def, new Vector3(ThisPlayers[0].transform.position.x, ThisPlayers[0].transform.position.y, ThisPlayers[0].transform.position.z - 0.2f), Icon_Def.transform.rotation) as GameObject;
            
            }
        }
        else     //解除防御状态
        {
            CanTouch = true;
            Destroy(Battle_Def);
        }
    }
    /// <summary>
    /// 控制玩家释放技能
    /// </summary>
    public void Battle_Player_Skill()
    {   if (ActionNumber <= 0)
        {
            //警告部分  
        CanTouch = false;
            BS_NUIM.WarningText.GetComponent<Text>().text = "没行动力了！";
            BS_NUIM.WarningText.transform.DOScale(1, 0.3f);
         
            StartCoroutine(Close());
        }
        else
        {
            Debug.Log("Skill");
        }
     }
    /// <summary>
    /// 给与玩家战利品
    /// </summary>
    public void GiveBooty()
    {
    }
}
