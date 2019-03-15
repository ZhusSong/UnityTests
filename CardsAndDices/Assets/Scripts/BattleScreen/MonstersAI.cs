using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using DG.Tweening;
using System.Collections.Generic;
using System;
    //怪物AI
public class MonstersAI : MonoBehaviour {
    //地图的值  0:无物体 1:有障碍物 2：可攻击点  3:已刷过权重  4.不可达 5.假终点 6.某怪物终点
    private int[,] BattleMap = new int[4, 4];  
    private int[][] MonstersPos=new int [3][];
    private int[][] EndPos = new int[3][];     //怪物寻路终点
    private int[,] Mon_Weights = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };

    public GameObject[] Monsters;
    public GameObject Icon_Atking;                  //正在攻击的图标
    public GameObject Icon_BeAtk;                   //被攻击的图标
    public GameObject Icon_Def;                     //防御中的图标
    //怪物数据 0:HP 1:行动力 2:攻击力 3:防御力 4:暴击
    private int[][] MonsterData = new int[3][];
    private int[] MonsterActionNumber = new int[3];
    private bool[] IsOver = new bool[3] { false, false, false };//某怪物是否已完成行动
    private bool[] CanFind = new bool[3] { true, true, true };//某怪物是否可进行移动

    private Color Chick = new Color(1f, 0.67f, 0.74f, 1f);     //状态颜色
    private Color Normal = new Color(1f, 1f, 1f, 1f);     //平常颜色
    private Color Disappear = new Color(0, 0, 0, 0);      //消失颜色
    private int AtkNumber = 0;  //攻击次数，每次只进行一次攻击
    private GameObject BattleIcons_ATKing;
    private GameObject BattleIcons_BeATK;
    private GameObject[] Battle_Def=new GameObject[3];
    private GameObject MAI_Player;
    private bool[] Actives = new bool[3];

    private int Player_x;
    private int Player_y;
    private BattleSystem MAI_BS;
    private ScriptsManager MAI_SM;
    private MissionInfor MAI_MI;
    private Transform[] Land;
   
    private bool[] IsOverWay = new bool[3]{true,true,true};          //怪物X是否可进行移动
    public int Monsters_Count = 0;
	// Use this for initialization
	public void L_Start () {
        MAI_SM = GameObject.Find("Main Camera").GetComponent<ScriptsManager>();
        MAI_BS = MAI_SM.BS;
        MAI_MI = MAI_SM.MI;
     //   MAI_NUIM = MAI_NUIM;
        //////////以下是测试数据
     //   GetMonstersData(0,3,2,1,1,10);
     //   GetMonstersData(1, 2, 1, 1, 1, 10);
        //////////
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
    public void GetMonstersData(int which,int HP,int actNum,int atk,int def,int cri)
    {
        MonsterData[which] = new int[] { HP, actNum, atk, def, cri };
        MonsterActionNumber[which] = MonsterData[which][1];
       // Debug.Log(which+"'s ActionMunber is"+MonsterData[which][1]);
    }
    /// <summary>
    /// 获取战斗地图信息
    /// </summary>
    /// <param name="map"></param>
    public void GetMap(int [,] map)
    {
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                //获取地图信息，将怪物位置置0
                BattleMap[i, j] = map[i, j];
                if (BattleMap[i, j] == 4)
                    BattleMap[i, j] = 0;
            //    Debug.Log("Map[" + i + "," + j + "] is " + BattleMap[i, j]);
            }
    }
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// 获取本次怪物实例，怪物位置，玩家位置
    /// </summary>
    /// <param name="thisMonsters"></param>
    /// <param name="monsterPos"></param>
    /// <param name="player_x"></param>
    /// <param name="player_y"></param>
    public void GetPos(bool[] Active,int [][] MonData,GameObject player,int [,] map,Transform[] land,GameObject[] thisMonsters,int [][] monsterPos,int player_x,int player_y)
    {
        Debug.Log("Monster numbwe is "+thisMonsters.Length);
        for (int i = 0; i < Battle_Def.Length; i++)
        {
            if (Battle_Def[i] != null)
                Destroy(Battle_Def[i]);
        }
        for (int i = 0; i < MonData.Length; i++)
        {
            if (MonData[i] != null)
            {
                MonsterData[i] = MonData[i];
                Actives[i] = Active[i];
                if (!Actives[i])
                {
                    MonsterData[i][1] = 0;
                }
                //  Debug.Log(i + "'s ActionMunber is" + MonsterData[i][1]+" and HP is "+MonsterData[i][0]);
            }
        }
            for (int i = 0; i < IsOver.Length; i++)
            {
                if (Actives[i])
                {
                    IsOver[i] = false;
                    CanFind[i] = true;
                }
                else
                {
                    IsOver[i] = true;
                    CanFind[i] = false;
                }
            }
         
        MAI_Player = player;
        //////////以下是测试数据
      //  GetMonstersData(0, 3, 2, 1, 1, 10, 50);
      //  GetMonstersData(1, 2, 1, 1, 1, 10, 55);
        //////////
        Monsters = thisMonsters;
        Monsters_Count = 0;
        for (int i = 0; i < Monsters.Length; i++)
        {
            if (Monsters[i] != null)
                Monsters_Count += 1;
        }

      
        Land = land;
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                //获取地图信息，将怪物位置置0
                BattleMap[i, j] = map[i, j];
                if (BattleMap[i, j] == 4)
                    BattleMap[i, j] = 0;
                if (Mon_Weights[i, j] == 20 && BattleMap[i, j] != 1)
                    Mon_Weights[i, j] = 0;
            //    Debug.Log("Map[" + i + "," + j + "] is " + BattleMap[i, j]);
            }
        for (int i = 0; i < monsterPos.Length; i++)
        {
            if (monsterPos[i] != null)
            {
                MonstersPos[i] = monsterPos[i];
                //   Debug.Log("Pos is " + MonstersPos[i][0] + "     " + MonstersPos[i][1]);
            }
            Player_x = player_x;
            Player_y = player_y;

        }
      
     //   for (int i = 0; i < Count - 1; i++)
     //   {
       //     EndPos[i] = CanMove(MonstersPos[i][0], MonstersPos[i][1]);
//
       //     if (EndPos[i] != null)

                MonsterMove();
        //    else
         //       Debug.Log("There's no way");
     //   }
    }
    
  
    /// <summary>
    /// 怪物移动，根据整张地图的权重值进行移动
    /// </summary>
    public void MonsterMove()
    {
        ///////判断地图权重
        int Weight_1 = 1;       //赋值权重
        int Weight_2 = 1;         //判断权重
        int Move_x = Player_x;    //判断点x
        int Move_y = Player_y;  //判断点y
        bool Over = false;      //当前权重点是否全部寻找完毕
        bool AllOver = true;      //所有权重点是否已寻找完毕
        bool NeedFind = false;

        //在赋权开始前，先遍历地图，将障碍物的点权重全部赋20
        for (int q = 0; q < 4; q++)
            for (int w = 0; w < 4; w++)
            {
                if (BattleMap[q, w] == 1)
                    Mon_Weights[q, w] = 20;
            }
        //遍历地图，将可攻击点的值置2
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                if (Mathf.Abs(Player_x - i) <= 1 && Mathf.Abs(Player_y - j) <= 1 && BattleMap[i, j] != 1)
                {
                    BattleMap[i, j] = 2;
                    //      Debug.Log("Map["+i+","+j+"] is   "+ BattleMap[i,j]);
                }
            }
        for (int i = 0; i < Monsters_Count; i++)
        {
            if (Monsters[i] != null && BattleMap[MonstersPos[i][0], MonstersPos[i][1]] == 2&&Actives[i])//当某怪物已处于可攻击位置时，将该位置置为不可寻路
            {
                MonsterAction(i);
                CanFind[i] = false;
                //   BattleMap[MonstersPos[i][0], MonstersPos[i][1]] = 2;
                Mon_Weights[MonstersPos[i][0], MonstersPos[i][1]] = 20;
            }
        }
        for (int i = 0; i < Monsters_Count; i++)
        {
            if (Monsters[i] != null && CanFind[i])//存在未在攻击范围内的点，进行寻路
            {
                NeedFind = true;
                break;
            }
        }
        //在赋权开始前，先遍历地图，将不可到达的点权重全部赋20
        for (int q = 0; q < 4; q++)
            for (int w = 0; w < 4; w++)
            {
                if (!AstarIsWay(q, w, Player_x, Player_y))
                    Mon_Weights[q, w] = 20;
            }
        if (NeedFind)
        {
        //AAAA:赋予权重的起点，当所有可赋权重点均有值之后，跳出循环
        AAAA: AllOver = true;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    //当一点处于未赋予权重的状态时，执行赋权过程
                    if (Mon_Weights[i, j] == 0)
                    {
                        AllOver = false;
                        goto DDDD;
                    }
                }
            if (AllOver)
                goto CCCC;
        //DDDD:赋予权重
        DDDD: if ((Move_x - 1 >= 0 && Mon_Weights[Move_x - 1, Move_y] == 0 && (BattleMap[Move_x - 1, Move_y] == 0 || BattleMap[Move_x - 1, Move_y] == 2)) || (Move_x + 1 <= 3 && Mon_Weights[Move_x + 1, Move_y] == 0 && (BattleMap[Move_x + 1, Move_y] == 0 || BattleMap[Move_x + 1, Move_y] == 2)) || (Move_y - 1 >= 0 && Mon_Weights[Move_x, Move_y - 1] == 0 && (BattleMap[Move_x, Move_y - 1] == 0 || BattleMap[Move_x, Move_y - 1] == 2)) || (Move_y + 1 <= 3 && Mon_Weights[Move_x, Move_y + 1] == 0 && (BattleMap[Move_x, Move_y + 1] == 0 || BattleMap[Move_x, Move_y + 1] == 2)))
            {
                if (Move_x - 1 >= 0 && (BattleMap[Move_x - 1, Move_y] == 0 || BattleMap[Move_x - 1, Move_y] == 2) && Mon_Weights[Move_x - 1, Move_y] == 0)
                {
                    Mon_Weights[Move_x - 1, Move_y] = Weight_1;
                }
                else if (Move_x + 1 <= 3 && (BattleMap[Move_x + 1, Move_y] == 0 || BattleMap[Move_x + 1, Move_y] == 2) && Mon_Weights[Move_x + 1, Move_y] == 0)
                {
                    Mon_Weights[Move_x + 1, Move_y] = Weight_1;
                }
                else if (Move_y - 1 >= 0 && (BattleMap[Move_x, Move_y - 1] == 0 || BattleMap[Move_x, Move_y - 1] == 2) && Mon_Weights[Move_x, Move_y - 1] == 0)
                {
                    Mon_Weights[Move_x, Move_y - 1] = Weight_1;
                }
                else if (Move_y + 1 <= 3 && (BattleMap[Move_x, Move_y + 1] == 0 || BattleMap[Move_x, Move_y + 1] == 2) && Mon_Weights[Move_x, Move_y + 1] == 0)
                {
                    Mon_Weights[Move_x, Move_y + 1] = Weight_1;
                }
                goto AAAA;
            }
            else
            {
            //BBBB:当一轮的权重全部赋完后，找寻此轮的某一点作为新一轮权重赋值的起点
            BBBB: for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        if (Mon_Weights[i, j] == Weight_2 && BattleMap[i, j] != 3 && (BattleMap[i, j] == 0 || BattleMap[i, j] == 2))
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
            //置可攻击点
            //遍历地图，将可攻击点的值置2
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (Mathf.Abs(Player_x - i) <= 1 && Mathf.Abs(Player_y - j) <= 1 && BattleMap[i, j] != 1)
                    {
                        BattleMap[i, j] = 2;
                        //      Debug.Log("Map["+i+","+j+"] is   "+ BattleMap[i,j]);
                    }
                    if (Mathf.Abs(Player_x - i) <= 1 && Mathf.Abs(Player_y - j) <= 1 && BattleMap[i, j] != 1)
                    {
                        BattleMap[i, j] = 2;
                        //      Debug.Log("Map["+i+","+j+"] is   "+ BattleMap[i,j]);
                    }
                }

            for (int i = 0; i < Monsters.Length ; i++)
            {
                if (Monsters[i] != null && CanFind[i]&&Actives[i])
                {
                    if (BattleMap[MonstersPos[i][0], MonstersPos[i][1]] != 2)//当怪物不处于可攻击到玩家的范围内时
                    {
                        EndPos[i] = CanMove(i, MonstersPos[i][0], MonstersPos[i][1]);   //计算怪物i的最短路径
                        if (EndPos[i] != null)
                            ShortMove(i, MonstersPos[i][0], MonstersPos[i][1], EndPos[i][0], EndPos[i][1]);
                        else
                        {
                            MonsterData[i][1] = 0;
                            MonsterTurnOver(i);
                        }
                    }
                    else                                                    //怪物处于可攻击到玩家的范围，转至行动
                    {
                        MonsterAction(i);
                    }
                }
            }
            //  for (int i = 0; i < 4; i++)
            //    for (int j = 0; j < 4; j++)
            //    {
            //       Debug.Log("Mon_Weights[" + i + "," + j + "] is " + Mon_Weights[i, j]);
            //遍历地图，将权重小于等于行动力的点赋予"可移动"状态
            //  if (Mon_Weights[i, j] <=3)// ActionNumber)
            //  {
            //坐标与数组下标互换
            //    BattleLand[i + 1 + j * 4].GetComponent<SpriteRenderer>().color = Chick;
            //      BattleMap[i, j] = 2;
            //  }
            // }
        }
    }
    public int[] CanMove(int which,int startX, int startY)
    {
        Debug.Log("this is "+which);
        int[] Count = new int[10];//可达点的权值
        int _i = 0;
        int _I = 0;
        int[][] _Pos = new int[10][];//可达点的坐标
        bool HaveWay = false;          //为真时有路，为假时没路


        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                if (BattleMap[i, j] == 2&&Mon_Weights[i,j]!=20) //&& BattleMap[i, j] != 4)//当一个点在攻击范围且未被占用时
                {
                    if (AstarIsWay(startX, startY, i, j))
                    {
                        //记录可到达终点的权值和坐标
                        Count[_i] = Mathf.Abs(startX - i) + Mathf.Abs(startY - j);
                        _Pos[_i] = new int[] { i, j };
                        _i += 1;
                        //  Debug.Log(_i);
                        HaveWay = true;
                    }
                    else
                        BattleMap[i, j] = 4;
                }
            }
        if (HaveWay)
        {
            for (int i = 0; i < Count.Length; i++)
            {
                if (Count[i + 1] != 0)
                {
                    if (Count[_I] > Count[i + 1])
                    {
                        _I = i + 1;
                    }
                }
                else
                    break;
            }
            BattleMap[_Pos[_I][0], _Pos[_I][1]] = 4;
            return _Pos[_I];
        }
        //当可攻击点少于怪物数目时，将最接近攻击范围的点选为终点
      else 
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (BattleMap[i, j] == 4 && AstarIsWay(startX, startY, i, j))//当一个点在攻击范围且未被占用时
                    {
                        if (i-1>=0&&BattleMap[i-1,j]!=1&&BattleMap[i-1,j]!=4)
                        {
                            BattleMap[i - 1, j] = 5;
                            Count[_i] = Mathf.Abs(startX - i+1) + Mathf.Abs(startY - j);
                            _Pos[_i] = new int[] { i - 1, j };
                            _i += 1;
                        }
                        if (i + 1 <= 3 && BattleMap[i + 1, j] != 1 && BattleMap[i + 1, j] != 4)
                        {
                            BattleMap[i + 1, j] = 5;
                                  Count[_i] = Mathf.Abs(startX - i-1) + Mathf.Abs(startY - j);
                               _Pos[_i] = new int[] { i + 1, j };
                               _i += 1;
                        }
                        if (j - 1 >= 0 && BattleMap[i, j - 1] != 1 && BattleMap[i , j - 1] != 4)
                        {
                            BattleMap[i, j - 1] = 5;
                                Count[_i] = Mathf.Abs(startX - i) + Mathf.Abs(startY - j+1);
                            _Pos[_i] = new int[] { i, j - 1 };
                            _i += 1;
                        }
                        if (j + 1 <= 3 && BattleMap[i, j + 1] != 1 && BattleMap[i, j + 1] != 4)
                        {
                            BattleMap[i, j + 1] = 5;
                               Count[_i] = Mathf.Abs(startX - i) + Mathf.Abs(startY - j-1);
                              _Pos[_i] = new int[] { i, j + 1 };
                              _i += 1;
                        }
                    }
                }
            for (int i = 0; i < Count.Length; i++)
            {
                if (Count[i + 1] != 0)
                {
                    if (Count[_I] > Count[i + 1])
                    {
                        _I = i + 1;
                    }
                }
                else
                    break;
            }
            if (_Pos[_I] != null)
            {
                BattleMap[_Pos[_I][0], _Pos[_I][1]] = 4;

                return _Pos[_I];
            }
            else
                return null;
        }

   //     else
      //      return null;
    }
    public void ShortMove(int which,int PosX, int PosY,int EndX,int EndY)
    {
        //移动开始，置初始位置状态为0
        //   BattleMap[Player_x, Player_y] = 0; ;
        int[][] Pos = new int[20][];  //存储路径点
        int Way = 0;
        int CCCount=0;                //已迭代次数，当次数为3时出现bug，置当前点为终点
    FindWay:
        //当某一点即将移动到可攻击到玩家的位置时
        if ((PosX - 1 == EndX && PosY == EndY) || (PosX + 1 == EndX && PosY == EndY) || (PosX == EndX && PosY - 1 == EndY) || (PosX == EndX && PosY + 1 == EndY))
        {
            if (PosX - 1 == EndX && PosY == EndY)
            {
                PosX = PosX - 1;
                Pos[Way] = new int[] { PosX, PosY };
                Way += 1;
            }
            else if (PosX + 1 == EndX && PosY == EndY)
            {
                PosX = PosX + 1;
                Pos[Way] = new int[] { PosX, PosY };
                Way += 1;
            }
            else if (PosX == EndX && PosY - 1 == EndY)
            {
                PosY = PosY - 1;
                Pos[Way] = new int[] { PosX, PosY };
                Way += 1;
            }
            else if (PosX == EndX && PosY + 1 == EndY)
            {
                PosY = PosY + 1;
                Pos[Way] = new int[] { PosX, PosY };
                Way += 1;
            }
            goto B_Move;
        }
        else
        {
            if (PosX - 1 >= 0 && Mon_Weights[PosX - 1, PosY] < Mon_Weights[PosX, PosY])// && BattleMap[PosX - 1, PosY] != 4)
            {
                PosX = PosX - 1;
                Pos[Way] = new int[] { PosX, PosY };
                Way += 1;
            }
            else if (PosX + 1 <= 3 && Mon_Weights[PosX + 1, PosY] < Mon_Weights[PosX, PosY])// && BattleMap[PosX + 1, PosY] != 4)
            {
                PosX = PosX + 1;
                Pos[Way] = new int[] { PosX, PosY };
                Way += 1;
            }
            else if (PosY - 1 >= 0 && Mon_Weights[PosX, PosY - 1] < Mon_Weights[PosX, PosY])// && BattleMap[PosX, PosY - 1] != 4)
            {
                PosY = PosY - 1;
                Pos[Way] = new int[] { PosX, PosY };
                Way += 1;
            }
            else if (PosY + 1 <= 3 && Mon_Weights[PosX, PosY + 1] < Mon_Weights[PosX, PosY])// && BattleMap[PosX, PosY + 1] != 4)
            {
                PosY = PosY + 1;
                Pos[Way] = new int[] { PosX, PosY };
                Way += 1;
            }
        }
    CCCount += 1;
    if (CCCount - Way > 1)
    {
      
        goto B_Move;
    }
        goto FindWay;
    //    ThisPlayers[0].transform.DOMove(new Vector3(BattleLand[i + 1].transform.position.x, BattleLand[i + 1].transform.position.y, BattleLand[i + 1].transform.position.z - 0.45f), 1f);
    B_Move:
        //开始移动
        StartCoroutine(Movee(which, Way, 0, Pos,MonsterData[which][1]));
    }

    /// <summary>
    /// 进行移动
    /// </summary>
    /// <param name="which">那个怪物</param>
    /// <param name="way">路径点个数</param>
    /// <param name="start">已行动的个数</param>
    /// <param name="poses">路径点</param>
    /// <returns></returns>
    IEnumerator Movee(int which,int way,int start, int[][] poses,int actNum)
    {
        yield return new WaitForSeconds(0.2f);
     //   int i = 0;
        int W = way;
        int Number = 0;
        int[][] Poses = poses;
     //   CanMove = false;           //移动过程中不能进行点击判定
     //   CanTouch = false;
        if (start < way && actNum > 0)    //仍有路径点未移动时
        {
            if (actNum - 1 == 0)//行动力刚好用完时
            {
                if (BattleMap[poses[start][0], poses[start][1]] != 6)
                {
                    Number = Poses[start][0] + 1 + Poses[start][1] * 4;
                    Tweener M = Monsters[which].transform.DOMove(new Vector3(Land[Number].transform.position.x, Land[Number].transform.position.y, Land[Number].transform.position.z - 0.45f), 0.3f);
                    start += 1;
                    actNum -= 1;
                    BattleMap[Poses[start - 1][0], Poses[start - 1][1]] = 6;
                    M.OnComplete(delegate() { StartCoroutine(Movee(which, W, start, poses, actNum)); });
                }
                else
                {
                    MonstersPos[which] = new int[] { Poses[start - 1][0], Poses[start - 1][1] };
                    BattleMap[Poses[start - 1][0], Poses[start - 1][1]] = 6;
                 //   Debug.Log(which + "'s end pos is " + poses[start - 1][0] + "  " + poses[start - 1][1]);
                    MonsterTurnOver(which);
                }
            }
            else
            {
                Number = Poses[start][0] + 1 + Poses[start][1] * 4;
                Tweener M = Monsters[which].transform.DOMove(new Vector3(Land[Number].transform.position.x, Land[Number].transform.position.y, Land[Number].transform.position.z - 0.45f), 0.3f);
                start += 1;
                actNum -= 1;
                M.OnComplete(delegate() { StartCoroutine(Movee(which, W, start, poses, actNum)); });
            }
       //     M.SetEase(Ease.OutCubic);
            //一次移动结束后，延时开始下一次移动
        }
        else      //已移动完时
        {
            try
            {
                MonstersPos[which] = new int[] { poses[start - 1][0], poses[start - 1][1] };
                BattleMap[poses[start - 1][0], poses[start - 1][1]] = 6;
                //   Debug.Log(which + "'s end pos is " + poses[start - 1][0] + "  " + poses[start - 1][1]);
                if (actNum >= 1)
                    MonsterAction(which);
                else
                MonsterTurnOver(which);
            }
            catch (Exception o)
                {
                    MonsterTurnOver(which);
                    Debug.Log("Error is "+o.ToString());
                }
        }
    }
    /// <summary>
    /// 判断是否可达算法
    /// </summary>
    /// <param name="StartX">寻路起点X坐标</param>
    /// <param name="StartY">寻路起点Y坐标</param>
    /// <param name="EndX">寻路终点X坐标</param>
    /// <param name="EndY">寻路终点Y坐标</param>
    /// <returns></returns>
    private bool AstarIsWay(int StartX, int StartY, int EndX, int EndY)
    {
        //   int[,] CloseMap = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };         //关闭列表与战斗地图相等  1为关闭
        int[,] OpenMap = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };          //开启列表 ,1为可判断，0为既没开启也没关闭 2为关闭
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                //  CloseMap[i, j] = BattleMap[i, j];
                if (Mon_Weights[i, j] == 20)
                    OpenMap[i, j] = 2;
            }
   
        int thisX = StartX;                //本次寻路点
        int thisY = StartY;              //本次寻路点
        if (Mon_Weights[thisX, thisY] == 20)   //寻路点为障碍物
            return false;
    // if (BattleMap[thisX, thisY] == 1 || BattleMap[thisX, thisY] == 4)   //寻路点为障碍物
    //   return false;
    //AAAAAA:寻路循环的起点
    AAAAAA:
        //  CloseMap[thisX, thisY] = 1;
        OpenMap[thisX, thisY] = 2;
        int WayLength_Up = 100;           //距终点的四方向路径距离 
        int WayLength_Down = 100;
        int WayLength_Left = 100;
        int WayLength_Right = 100;
        int[] WayLength = new int[4] { 100, 100, 100, 100 };
        int Min = 0;                      //最短路径点
        bool CanFind = false;             //可找到通路
        //四方向判断
        if ((thisX - 1 == EndX && thisY == EndY) || (thisX + 1 == EndX && thisY == EndY) || (thisX == EndX && thisY + 1 == EndY) || (thisX == EndX && thisY - 1 == EndY))//可到达终点
        {
            return true;
        }
        else
        {
            if (thisX - 1 >= 0 && OpenMap[thisX - 1, thisY] != 2)
            {
                OpenMap[thisX - 1, thisY] = 1;
                WayLength_Left = Mathf.Abs(thisX - 1 - EndX) + Mathf.Abs(thisY - EndY);
                WayLength[0] = WayLength_Left;
                CanFind = true;
            }
            if (thisX + 1 <= 3 && OpenMap[thisX + 1, thisY] != 2)
            {
                OpenMap[thisX + 1, thisY] = 1;
                WayLength_Right = Mathf.Abs(thisX + 1 - EndX) + Mathf.Abs(thisY - EndY);
                WayLength[1] = WayLength_Right;
                CanFind = true;
            }
            if (thisY - 1 >= 0 && OpenMap[thisX, thisY - 1] != 2)
            {
                OpenMap[thisX, thisY - 1] = 1;
                WayLength_Down = Mathf.Abs(thisX - EndX) + Mathf.Abs(thisY - 1 - EndY);
                WayLength[2] = WayLength_Down;
                CanFind = true;
            }
            if (thisY + 1 <= 3 && OpenMap[thisX, thisY + 1] != 2)
            {
                OpenMap[thisX, thisY + 1] = 1;
                WayLength_Up = Mathf.Abs(thisX - EndX) + Mathf.Abs(thisY + 1 - EndY);
                WayLength[3] = WayLength_Up;
                CanFind = true;
            }
            Min = WayLength[0];
            for (int i = 0; i < 3; i++)
            {
                if (WayLength[i + 1] < Min)
                    Min = WayLength[i + 1];
            }
            //更换节点
            if (Min == WayLength_Left)
                thisX -= 1;
            else if (Min == WayLength_Right)
                thisX += 1;
            else if (Min == WayLength_Down)
                thisY -= 1;
            else if (Min == WayLength_Up)
                thisY += 1;
        }
        if (CanFind)
            goto AAAAAA;
        else
        {
            //上一次寻路未成功，寻找某个开启点作为新寻路的起点
            bool HavePoint = false;
            for (int l = 0; l < 4; l++)
                for (int p = 0; p < 4; p++)
                {
                    if (OpenMap[l, p] == 1)
                    {
                        thisX = l;
                        thisY = p;
                        HavePoint = true;
                        goto AAAAAA;
                    }
                }
            if (!HavePoint)       //为假时，寻路失败，不存在可到达路径
                return false;
        }
        return false;
    }

    public void MonsterTurnOver(int which)
    {
        Debug.Log("over this is " + which);
       // Debug.Log("Monster "+which+"is over and "+which +"'s pos is "+MonstersPos[which][0]+"  "+MonstersPos[which][1]);
        IsOver[which]=true;
        bool AllOver=true;
        for (int i = 0; i < Monsters_Count; i++)
        {
            if (!IsOver[i])
            {
                AllOver = false;
                break;
            }
        }
        if (AllOver)
            {
                AtkNumber = 0;
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                        Mon_Weights[i, j] = 0;
                MAI_BS.Monster_RemoveMap(MonstersPos);//重置地图
                MAI_BS.ChangeTurn("Player");
            }
      
    }
    /// <summary>
    /// 怪物做出行动
    /// </summary>
    public void MonsterAction(int which)
    {
      //  Debug.Log("Monster" + which + "has action");
     //   Monster_Def(which);
        if (MonsterData[which][0] > 1)
        {
            Monster_ATK(which);
        }
        else
        {
            Monster_Def(which);
        }
     //   Debug.Log("Monster" + which + "has action");
    }
    private void Monster_ATK(int which)
    {
        MonsterActionNumber[which] -= 1;
    
        Monster_ATKing(true,Player_x,Player_y);

        //**************************************
        //此处进行了玩家数据变动
        if (MonsterData[which][2]-PlayerData.Def>0)
            PlayerData.HP -= (MonsterData[which][2] - PlayerData.Def);
        MAI_MI.ChangeInfor();
        //**************************************

        if (MonsterActionNumber[which] >= 1)
            Monster_Def(which);
        else
            MonsterTurnOver(which);
      //  MAI_BS.ATKing(false, true, Player_x, Player_y);
    }
    private void Monster_Def(int which)
    {
        MonsterActionNumber[which] = 0;
        MonsterTurnOver(which);
        Battle_Def[which] = GameObject.Instantiate(Icon_Def, new Vector3(Monsters[which].transform.position.x, Monsters[which].transform.position.y, Monsters[which].transform.position.z - 0.2f), Icon_Def.transform.rotation) as GameObject;
        
    }

    public void Monster_ATKing(bool ATK, int PosX, int PosY)
    {
        if (AtkNumber == 0)
        {
   
            BattleIcons_ATKing = (GameObject)GameObject.Instantiate(Icon_Atking, new Vector3(Land[PosX + 1 + PosY * 4].transform.position.x, Land[PosX + 1 + PosY * 4].transform.position.y,Land[PosX + 1 + PosY * 4].transform.position.z - 0.47f), Icon_Atking.transform.rotation);

            BattleIcons_BeATK = (GameObject)GameObject.Instantiate(Icon_BeAtk, new Vector3(Land[PosX + 1 + PosY * 4].transform.position.x, Land[PosX + 1 + PosY * 4].transform.position.y, Land[PosX + 1 + PosY * 4].transform.position.z - 0.47f), Icon_BeAtk.transform.rotation);
            BattleIcons_BeATK.transform.SetParent(MAI_Player.transform);

            BattleIcons_ATKing.GetComponent<SpriteRenderer>().DOColor(Disappear,0.3f);
            Sequence Battle_BeAtk = DOTween.Sequence();
            Battle_BeAtk.Append(MAI_Player.transform.DOMove(new Vector3(MAI_Player.transform.position.x - 0.1f, MAI_Player.transform.position.y - 0.1f, MAI_Player.transform.position.z), 0.08f));
            Battle_BeAtk.Append(MAI_Player.transform.DOMove(new Vector3(MAI_Player.transform.position.x, MAI_Player.transform.position.y, MAI_Player.transform.position.z), 0.08f));
            Battle_BeAtk.Append(MAI_Player.transform.DOMove(new Vector3(MAI_Player.transform.position.x - 0.1f, MAI_Player.transform.position.y - 0.1f, MAI_Player.transform.position.z), 0.08f));
            Battle_BeAtk.Append(MAI_Player.transform.DOMove(new Vector3(MAI_Player.transform.position.x, MAI_Player.transform.position.y, MAI_Player.transform.position.z), 0.08f));

            Battle_BeAtk.OnComplete(delegate() { Destroy(BattleIcons_BeATK); Destroy(BattleIcons_ATKing); });
                       
            AtkNumber += 1;
        }
     }
  
}
