using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CreateMap : MonoBehaviour {
 public GameObject[] Way_Map;           //方式卡
    public GameObject[] Supply_Map;     //补给卡
    public GameObject[] Main_Map;       //事件卡
    public GameObject[] This_Map=new GameObject[20];       //此次地图
    public GameObject Forest_MAP;      //得到Forest中存储的地图类卡牌信息
    public int[,] MapArray = new int[5, 5];  //本次地图的二维数组
    private int[,] MapJudge = new int[7, 7];  //地图可创建值域的判断数组
    public int[][] CardArray;           //存储卡牌位置的交错数组
    public int Way_1;                   //方法牌1出现位置
    public int Way_2;                   //方法牌2出现位置
    public int Supply;                  //地图补给点位置
    public int index_Supply;
    public int Limit;                    //地图张数
    public int MapNumber = 0;          //已创建的地图数目
    private int ThisLimit;               //这一次地图的大小 
    private int FirstTime = 1;          //第一次创建地图
    private static int LastWay = 0;       //上一次结束时的方式
    public string ThisName;             //此次传递给SignAbove()和Button()的地图名
    public  int First;                         //记录第一次创建地图
    private int StartPosX;
    private int StartPosY;
    private bool Repeat=false;             //判断本次生成的卡牌是否重复    
    private bool TheLast = false;         //判断本次生成的卡牌是否为终点卡
    private int RepeatX;                 //判断某一地图坐标是否重复
    private int RepeatY;
    //以下是测试数据
    public GameObject TestMonster;
    public GameObject InMonster;
    public TextMesh[][] MonstersText = new TextMesh[3][];
  //  public int [] T = new int [5]{2,3,4,1,5};
  //  public GameObject UIFather;
   // public GameObject PPPPPos;

    private ForestMap fort;             //调用ForestMap()
    private Move CM_Move;               //调用Move()
 //   private UImanager CM_UIm;           //调用UImanager（）
    private SignAbove CM_SA;            //调用SignAbove（）
    private ToBlack CM_TB;       //调用ToBlack()
    private CardsEvents CM_CE;
    private NewUIManager CM_NUIM;
    private ScriptsManager CM_SM;
    private MissionInfor CM_MI;
    private RoleInfor CM_RI;

    void Start()
    {
        CM_SM = GameObject.Find("Main Camera").GetComponent<ScriptsManager>();
        Screen.SetResolution(1080, 1920, false);
         CM_CE = new CardsEvents();
         CM_TB = CM_SM.TB;
         CM_Move = CM_SM.Move;
         CM_RI = CM_SM.RI;
        Forest_MAP = GameObject.FindWithTag("Forest");                
        fort = Forest_MAP.GetComponent<ForestMap>();
        CM_NUIM = CM_SM.NUIM;
        CM_MI = CM_SM.MI;
        CM_TB.ToWhite();
        //以下是测试数据
    //    InMonster = GameObject.Instantiate(TestMonster, TestMonster.transform.position, TestMonster.transform.rotation) as GameObject;
    
  //      if (GameObject.Find(InMonster.name + "/Name").GetComponent<TextMesh>())
    //        Debug.Log("In Name is " + GameObject.Find(InMonster.name + "/Name").GetComponent<TextMesh>().text);
     //   MonstersText[0][0] = GameObject.Find(InMonster.name + "/Name").GetComponent<TextMesh>()as TextMesh;
     ////   MonstersText[0][1] = GameObject.Find(InMonster.name + "/HPPos").GetComponent<TextMesh>() as TextMesh;
    //    MonstersText[0][2] = GameObject.Find(InMonster.name + "/ActNum").GetComponent<TextMesh>() as TextMesh;
    ////    MonstersText[0][3] = GameObject.Find(InMonster.name + "/ATK").GetComponent<TextMesh>() as TextMesh;
    //    MonstersText[0][4] = GameObject.Find(InMonster.name + "/Def").GetComponent<TextMesh>() as TextMesh;

     //   GameObject.Find(InMonster.name + "/HPPos").GetComponent<TextMesh>().text = T[1].ToString();
      //  GameObject.Find(InMonster.name + "/ActNum").GetComponent<TextMesh>().text = T[0].ToString();
     //   GameObject.Find(InMonster.name + "/ATK").GetComponent<TextMesh>().text = T[2].ToString();
     //   GameObject.Find(InMonster.name + "/Def").GetComponent<TextMesh>().text = T[3].ToString();
    //    InMonster.transform.SetParent(UIFather.transform);
     //   InMonster.transform.DOScale(10f, 0);
    //    CM_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
    //    CM_UIm = GameObject.Find("BG").GetComponent<UImanager>();
        InitMap();                                                      
    }
    /// <summary>
    /// 游戏结束重新开始
    /// </summary>
    public void AnotherInitMap()
    {
        Debug.Log("Map ReStart");
        Destroy(This_Map[MapNumber].gameObject);
        InitMap();
      //  CM_SA.CanRotateUp(true);
        CM_NUIM.AnotherStart();
    }
    //↓创建本次的地图
    //  1.得到本次地图的事件卡、方式卡及补给卡
    //  2.创建本次地图的二维数据
    public void InitMap()                                             //在此脚本中的Start()中被调用
    {
     //   Debug.Log("Innnnnnit map");
        for (int x = 0; x < 5; x++)   //为地图的二维数组赋初值
            for (int y = 0; y < 5; y++)
            {
                MapArray[x, y] = 0;
            }
        for (int m = 0; m < 7; m++)  //为地图的判断数组赋初值
            for (int n = 0; n < 7;n++ )
            {
                if (m == 0 || m == 6 || n ==0 || n == 6)
                    MapJudge[m, n] = 1;
                else MapJudge[m, n] = 0;
            }
            MapNumber = 0;
        First = 2;
        if (First > 1) 
            CM_TB.ToWhite();                        //创建地图后，屏幕变白
        if (FirstTime == 1)                                           //第一次创建地图时
        {
            Way_1 = Random.Range(0, 4);                               //随机方法
            FirstTime = 2;
        }
        else
            Way_1 = LastWay;                                          //否则为终点方法

        Way_2 = Random.Range(0, 4);                                   //终点方式为4张中随机一张
        LastWay = Way_2;
        Limit = Random.Range(6, 11);                                  //地图大小为6-10
        ////////////以下为测试数据
        Limit = 6;
        ////////////////
        Supply = Random.Range(1, Limit - 1);                          //补给点为随机点
        ThisLimit = Limit - 1;                                        //此次地图大小
        index_Supply = Random.Range(0, Supply_Map.Length);        //补给方式为补给卡中之一
        CardArray = new int[Limit][];                                //为交错数组赋初值
        int[] Main = GetRandom(Main_Map.Length, 0, Main_Map.Length);  //打乱地图

        for (int a_Way = 0; a_Way < 4; a_Way++)                       //给地图赋初值
        {
            Way_Map[a_Way] = fort.Forest_Way[a_Way];                  //得到该种类地图的所有方式卡
        } 
        for (int a_Supply = 0; a_Supply < 2; a_Supply++)
        {
            Supply_Map[a_Supply] = fort.Forest_Supply[a_Supply];      //得到该种类地图的所有补给卡
        }
        for (int a_Main = 0; a_Main < 14; a_Main++)
        {
            int n = Main[a_Main];
            Main_Map[a_Main] = fort.Forest_Event[n];                  //得到已经打乱了的该种类地图的所有事件
        }

        for (int m = 0; m < Limit; m++)                               //给此次地图赋值
        {
            if (m == 0)
            {
                This_Map[m] = Way_Map[Way_1];                         //这次地图起点为方式卡
            }
            else if (m == Supply)
            {
                This_Map[m] = Supply_Map[index_Supply];               //这次地图的补给卡
            }
            else if (m == Limit - 1)                                  //这次地图的终点卡
            {
                This_Map[m] = Way_Map[Way_2];
                
            }
            else This_Map[m] = Main_Map[m];                           //这次地图的事件
        }


        CreateMapArray();
        CM_RI.GetMap(MapArray);
    //    CM_NUIM.GetMap(MapArray);
     /*   for (int i = 0; i < Limit; i++)
            for (int j = 0; j < 2;j++ )
            {
                Debug.Log("CardArray is " + CardArray[i][j]);
            }*/
            This_Map[MapNumber] = GameObject.Instantiate(This_Map[MapNumber], new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;   //创建起点
       
        CM_Move.GetMap(This_Map,MapNumber,Limit,MapArray,StartPosX,StartPosY);                             //向Move()传递地图信息
        CM_Move.GetThisCardNameAndNumber(This_Map[MapNumber].name,MapNumber);
    //    CM_NUIM.GetNumber(StartPosX, StartPosY);
        CM_RI.GetNumber(StartPosX, StartPosY);      
    }
    //↓得到Move()传递的位置信息进行创建，被Move()中Update()调用,ThisX与ThisY为本次坐标
    public void CreateNext(int Forward,int ThisX,int ThisY)                               
    {
       // Debug.Log("ThisX is " + ThisX + " ThisY is " + ThisY);

        //*******************************
        //此处进行了玩家数据变动
        Move.MoveCount += 1;
        if (Move.MoveCount == 2)
        {
            PlayerData.Sat -= 1;
            Move.MoveCount = 0;
            CM_MI.ChangeInfor();
        }
        if (PlayerData.Sat <= 0)
        {
            int Count = 0 - PlayerData.Sat;
            PlayerData.HP -= Count;
        }
        CM_MI.ChangeInfor();
        //*******************************

        for (int i = 0; i < Limit; i++)
            for (int j = 0; j < 2;j++ )
            {
                if (j==0&&CardArray[i][j] == ThisX&&CardArray[i][j+1]==ThisY)
                {
                 //   CM_NUIM.GetNumber(ThisX, ThisY);
                    CM_RI.GetNumber(ThisX, ThisY);
                    MapNumber = i;
                  //  Debug.Log("i is "+i+"CardArray is " + CardArray[i][j] +"  " +CardArray[i][j+1]+" MapNumber is "+MapNumber+" name is"+This_Map[MapNumber].name.Replace("(Clone)", ""));
                }
          
            }
         /*  for (int i = 0; i < 5; i++)
               for (int j = 0; j < 5; j++)
               { Debug.Log("MapArray[" + i + "," + j + "] is " + MapArray[i, j]); }*/
        if (MapNumber == Limit - 1)
            TheLast = true;
        if (This_Map[MapNumber] == null)
        {
            Repeat = true;
            if (MapNumber == 0)
                This_Map[MapNumber] = Way_Map[Way_1];
            else if (MapNumber == Supply)
                This_Map[MapNumber] = Supply_Map[index_Supply];
            else  if (MapNumber == Limit - 1)
            {
                TheLast = true;
                This_Map[MapNumber] = Way_Map[Way_2];
            }
            else
                This_Map[MapNumber] = Main_Map[MapNumber];
        }
        else 
            Repeat = false;
      //  MapNumber++;                                                  //创建下一张卡
    //    Debug.Log("MapNumber is " + MapNumber);
            This_Map[MapNumber] = GameObject.Instantiate(This_Map[MapNumber], new Vector3(0, 0, 0), Quaternion.Euler(0, 180, 0)) as GameObject;
            ThisName = This_Map[MapNumber].name.Replace("(Clone)", "");                                 //向CardEvents()传递对应的卡牌名
            Debug.Log("ThisName is " + ThisName);
         //   CM_SA.GetName_SA(ThisName);
            if (Forward == 1)
            {
                Tweener Rot = This_Map[MapNumber].transform.DORotate(new Vector3(0, 0, 0), 0.6f);       //进行卡牌出现时的翻转
                Rot.SetEase(Ease.InOutQuad);
                if (TheLast)
                {
                    Rot.OnComplete(delegate() { CM_CE.GetCardNameAndEvent(ThisName, "TheLast", 1); CM_Move.GetThisCardNameAndNumber(This_Map[MapNumber].name, MapNumber); });
                    TheLast = false;
                }
                else if (!Repeat)
                {

                    Rot.OnComplete(delegate() { CM_CE.GetCardNameAndEvent(ThisName, null, 1); CM_Move.GetThisCardNameAndNumber(This_Map[MapNumber].name, MapNumber); });
                }//卡牌创建结束，调用SignAbove()中的CanRotateDown()使上方牌子落下
                else
                {

                    Rot.OnComplete(delegate() { CM_CE.GetCardNameAndEvent(ThisName, "Repeat", 1); CM_Move.GetThisCardNameAndNumber(This_Map[MapNumber].name, MapNumber); });
                }
                }
            if (Forward == 2)
            {
                Tweener Rot = This_Map[MapNumber].transform.DORotate(new Vector3(0, 360, 0), 0.6f);     //进行卡牌出现时的翻转
                Rot.SetEase(Ease.InOutQuad);
                if (TheLast)
                {

                    Rot.OnComplete(delegate() { CM_CE.GetCardNameAndEvent(ThisName, "TheLast", 1); CM_Move.GetThisCardNameAndNumber(This_Map[MapNumber].name, MapNumber); });
                    TheLast = false;
                }
                else if (!Repeat)
                {

                    Rot.OnComplete(delegate() { CM_CE.GetCardNameAndEvent(ThisName, null, 1); CM_Move.GetThisCardNameAndNumber(This_Map[MapNumber].name, MapNumber); });                                  //卡牌创建结束，调用SignAbove()中的CanRotateDown()使上方牌子落下
                }
                    else
                {

                    Rot.OnComplete(delegate() { CM_CE.GetCardNameAndEvent(ThisName, "Repeat", 1); CM_Move.GetThisCardNameAndNumber(This_Map[MapNumber].name, MapNumber); });
          
                }}
            if (Forward == 3)
            {
                Tweener First = This_Map[MapNumber].transform.DORotate(new Vector3(0, 180, 180), 0);    //进行卡牌出现时的翻转
                Tweener Rot = This_Map[MapNumber].transform.DORotate(new Vector3(-180, 180, 180), 0.6f);
                Rot.SetEase(Ease.InOutQuad);
                if (TheLast)
                {

                    Rot.OnComplete(delegate() { CM_CE.GetCardNameAndEvent(ThisName, "TheLast", 1); CM_Move.GetThisCardNameAndNumber(This_Map[MapNumber].name, MapNumber); });
                    TheLast = false;
                }
                else if (!Repeat)
                {

                    Rot.OnComplete(delegate() { CM_CE.GetCardNameAndEvent(ThisName, null, 1); CM_Move.GetThisCardNameAndNumber(This_Map[MapNumber].name, MapNumber); });
                }
                else
                {
                    Rot.OnComplete(delegate() { CM_CE.GetCardNameAndEvent(ThisName, "Repeat", 1); CM_Move.GetThisCardNameAndNumber(This_Map[MapNumber].name, MapNumber); });//卡牌创建结束，调用SignAbove()中的CanRotateDown()使上方牌子落下
                }
                }
            if (Forward == 4)
            {
                Tweener First = This_Map[MapNumber].transform.DORotate(new Vector3(0, 180, 180), 0);     //进行卡牌出现时的翻转
                Tweener Rot = This_Map[MapNumber].transform.DORotate(new Vector3(180, 180, 180), 0.6f);
                Rot.SetEase(Ease.InOutQuad);
                if (TheLast)
                {

                    Rot.OnComplete(delegate() { CM_CE.GetCardNameAndEvent(ThisName, "TheLast", 1); CM_Move.GetThisCardNameAndNumber(This_Map[MapNumber].name, MapNumber); });
                    TheLast = false;
                }
                else if (!Repeat)
                {

                    Rot.OnComplete(delegate() { CM_CE.GetCardNameAndEvent(ThisName, null, 1); CM_Move.GetThisCardNameAndNumber(This_Map[MapNumber].name, MapNumber); });
                }
                else
                {

                    Rot.OnComplete(delegate() { CM_CE.GetCardNameAndEvent(ThisName, "Repeat", 1); CM_Move.GetThisCardNameAndNumber(This_Map[MapNumber].name, MapNumber); });//卡牌创建结束，调用SignAbove()中的CanRotateDown()使上方牌子落下

                }
            }
        
    }
    //↓创建本次地图二维数组值
    private void CreateMapArray()
    {
        int Number=0;                            //本次创建到第几张牌；
      int[] Map = new int[Limit];
        StartPosX = Random.Range(0, 5);     //起始点X坐标
        StartPosY = Random.Range(0, 5);     //起始点Y坐标
        int ThisX=StartPosX;
        int ThisY=StartPosY;
        int Direction = 0;                      //方向，1为上，2为下，3为左，4为右
        MapArray[StartPosX, StartPosY] = 1;
        MapJudge[StartPosX + 1, StartPosY + 1] = 1;
        CardArray[Number] = new int[] { ThisX, ThisY };
        //    Debug.Log("The Array is (" + ThisX + "," + ThisY + ")");
    StartCreate:
          
        if (MapJudge[ThisX+1, ThisY+2] == 1&&MapJudge[ThisX+1, ThisY] == 1&&MapJudge[ThisX, ThisY+1] == 1&&MapJudge[ThisX+2, ThisY+1] == 1) //当本次生成的原点被锁死时
        {
        GetNewStartPos:
            ThisX = Random.Range(0, 5);
            ThisY = Random.Range(0, 5);
            if (MapArray[ThisX, ThisY] == 1)
            {
                goto StartCreate;
            }
            else
                goto GetNewStartPos;
        }
        Direction = Random.Range(1,5);
    
    switch (Direction)
    {
        case 1: ThisY += 1;
            if (ThisY > 4 || MapArray[ThisX, ThisY] == 1)
            {
                ThisY -= 1;
                goto StartCreate;
            }
            else
            {
                MapArray[ThisX, ThisY] = 1;
                MapJudge[ThisX + 1, ThisY + 1] = 1;
           //     Debug.Log("The Array is (" + ThisX + "," + ThisY + ")");
                Number += 1;
                CardArray[Number] = new int[] { ThisX, ThisY };
            }
            break;
        case 2: ThisY -= 1;
            if (ThisY < 0 || MapArray[ThisX, ThisY] == 1)
            {
                ThisY += 1;
                goto StartCreate;
            }
            else
            {
                MapArray[ThisX, ThisY] = 1;
                MapJudge[ThisX + 1, ThisY + 1] = 1;
            //    Debug.Log("The Array is (" + ThisX + "," + ThisY + ")");
                Number += 1;
                CardArray[Number] = new int[] { ThisX, ThisY };
            }
            break;
        case 3: ThisX -= 1;
            if (ThisX < 0 || MapArray[ThisX, ThisY] == 1)
            {
                ThisX += 1;
                goto StartCreate;
            }
            else
            {
                MapArray[ThisX, ThisY] = 1;
                MapJudge[ThisX + 1, ThisY + 1] = 1;
            //    Debug.Log("The Array is (" + ThisX + "," + ThisY + ")");
                Number +=1;
                CardArray[Number] = new int[] { ThisX, ThisY };
            }
        break;
        case 4: ThisX += 1;
        if (ThisX > 4 || MapArray[ThisX, ThisY] == 1)
        {
            ThisX -= 1;
            goto StartCreate;
        }
        else
        {
            MapArray[ThisX, ThisY] = 1;
            MapJudge[ThisX + 1, ThisY + 1] = 1;
        //    Debug.Log("The Array is (" + ThisX + "," + ThisY + ")");
            Number+=1;
            CardArray[Number] = new int[] { ThisX, ThisY };
        }
        break;
        default:
            break;
    }
        if (Number!=Limit-1)
        goto StartCreate;
        for (int i = 0; i < 5; i++)
            for (int j = 0; j < 5;j++ )
            {
                Debug.Log("MapArray["+i+","+j+"] is "+MapArray[i,j]);
            }
    }
    //↓得到某一种类地图的信息并随机打乱，被此脚本中InitMap()调用  
 public int[] GetRandom(int Number, int minNum, int maxNum)         
    {
        int j;
        int[] b = new int[Number];
        int r;
        for (j = 0; j < Number; j++)
        {
            r = Random.Range(0, maxNum);
            int num = 0;
            for (int k = 0; k < j; k++)
            {
                if (b[k] == r)
                {
                    num = num + 1;
                }
            }
            if (num == 0)
            {
                b[j] = r;
            }
            else
            {
                j = j - 1;
            }
        }
        return b;
    }
}
