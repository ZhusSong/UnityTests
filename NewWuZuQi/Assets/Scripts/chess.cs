using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;
public class chess : MonoBehaviour {
    public enum Turn                                    //枚举，定义黑白棋子状态
    { 
        black, 
        white 
    } ;
    Turn chessTurn;                                     //落子顺序
	//↓四个锚点位置，用于计算棋子落点
    public GameObject LeftUp;                           //左下锚点
    public GameObject RightUp;                          //右上锚点
    public GameObject LeftUnder;                        //左下锚点
    public GameObject RightUnder;                       //右上锚点
    //↑
    public GameObject White;                            //白棋子
    public GameObject Black;                            //黑棋子
    public GameObject[] Whites = new GameObject[120];   //白棋子的实例棋子数组
    public GameObject[] Blacks = new GameObject[120];   //黑棋子的实例棋子数组
    private int Whites_i = 0;                           //白棋子的数组下标             
    private int Blacks_i = 0;                           //黑棋子的数组下标    
    private int BlackWinNumber =5;                      //黑棋胜利时进行判定的相连棋子数目
    private int WhiteWinNumber = 5;                     //白棋胜利时进行判定的相连棋子数目
    public Camera Cam;                                  //主摄像机
    private int[] Last_x=new int [225];                 //上一步下的棋的X坐标
    private int[] Last_y=new int [225];                 //上一步下的棋的Y坐标
    private int Lasts=0;                                //一共下了多少步
    //↓锚点在屏幕上的映射位置
    Vector3 LUpPos;
    Vector3 RUppos;
    Vector3 LUnPos;
    Vector3 RUnPos;
    //↑
    Vector3 PointPos;                                //当前点选的位置
    Vector3 ThisPos;                                 //本次棋子应该生成的位置
    float GridWidth =1;                              //棋盘网格宽度
    float GridHeight=1;                              //棋盘网格高度
    float minGridDis;                                //网格宽和高的较小值，用于进行锚点位置出现误差时的补正
    public Vector2[,] ChessPos;                      //Vector2类型数组，存储棋盘上所有可以落子的位置
   public int[,] ChessState;                         //存储棋盘位置上的落子状态，1为白字，-1为黑子
    int winner = 0;                                  //获胜方，1为黑子，-1为白子
   public  bool isPlaying = true;                    //是否处于对弈状态
   private int D_Time = 20;                          //倒计时，被此脚本中DestroyAndCreate()，ChangePlayer()，CreateChess()改变数值
    private UIManager CHess_UIM;                     //调用UIManager()
    //↓进行相应数据的初始化
	void Start () {
        CHess_UIM = GameObject.Find("InforPanel").GetComponent<UIManager>();
        ChessPos = new Vector2[15, 15];                //初始化棋子位置数组
        ChessState =new int[15,15];                    //初始化棋盘值数组
        chessTurn = Turn.black;                        //黑子先行
        //↓获取锚点位置
        LUpPos = LeftUp.transform.position;      
        RUppos = RightUp.transform.position;
        LUnPos = LeftUnder.transform.position;
        RUnPos = RightUnder.transform.position;
        //↑
        //↓计算网格宽度
        GridWidth = (RUppos.x - LUpPos.x) / 14;
        GridHeight = (LUpPos.y - LUnPos.y) / 14;
        minGridDis = GridWidth < GridHeight ? GridWidth : GridHeight;
        //↑
        //↓将可落子点的位置存入数组
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                ChessState[i, j] = 0;
                ChessPos[i, j] = new Vector2(LUnPos.x + GridWidth * i, LUnPos.y + GridHeight * j);
            }
        }
        StopDJS(true, true);
        //↑
	}
	//↓获取输入并进行判断，该函数将在游戏运行的每一帧被调用
	void Update () {
        if (isPlaying && Input.GetMouseButtonDown(0))                                               //检测鼠标输入并确定落子状态
        {
            if (!EventSystem.current.IsPointerOverGameObject())//当点击的物体不为UI时
            {
                PointPos = Cam.ScreenToWorldPoint(Input.mousePosition);//将鼠标点击位置的屏幕坐标转为世界坐标
                for (int i = 0; i < 15; i++)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        if (Dis(PointPos, ChessPos[i, j]) < minGridDis / 2 && ChessState[i, j] == 0)    //找到最接近鼠标点击位置的落子点，如果该位置没有其他棋子则落子
                        {
                            
                            ChessState[i, j] = chessTurn == Turn.black ? 1 : -1;                        //根据下棋顺序确定落子颜色
                            chessTurn = chessTurn == Turn.black ? Turn.black : Turn.white;              //落子成功，更换下棋顺序
                            StopAllCoroutines();//停止倒计时
                          
                            CreateChess(i, j, chessTurn);                                                 //创建棋子
                            if (chessTurn == Turn.black)
                            {
                                CHess_UIM.ChangeWhichText("黑");
                                CHess_UIM.ChangeChessNumberText(1);
                            }
                            else
                            {
                                CHess_UIM.ChangeWhichText("白");
                                CHess_UIM.ChangeChessNumberText(-1);
                            }
                        }
                    }
                }
                int re = result();                                  //调用判断函数，确定是否有获胜方
                if (re == 1)
                {
                    winner = 1;
                    CHess_UIM.Win(winner);
                    isPlaying = false;
                }
                else if (re == -1)
                {
                    winner = -1;
                    CHess_UIM.Win(winner);
                    isPlaying = false;
                }
            }
        }  
	}
    //↓悔棋时的处理函数，被脚本UIManager()中Yes()调用
    public void GiveUp()
    {
        bool flag = true;                   
        if (Blacks[0] == null && Whites[0] == null)    //无棋可悔时
            flag = false;

        if (chessTurn == Turn.white&&flag)             //悔黑棋时，销毁上一次的黑棋，并进行相应的显示重置
        {
            int BlackLast = Blacks_i;
            for (int i = BlackLast; i > 0; i--)
            {
                if (Blacks[i-1] != null)
                {
                    DestroyImmediate(Blacks[i - 1].gameObject);
                    break;
                }
            }
            chessTurn = Turn.black;
            ChessState[Last_x[Lasts-1], Last_y[Lasts-1]] = 0;
            Lasts -= 1;
            CHess_UIM.ChessNumber = Lasts;
           CHess_UIM.ChangeWhichText("黑");
        }
        else if (chessTurn == Turn.black && flag)     //悔白棋时，销毁上一次的白棋，并进行相应的显示重置
        {
            int WhiteLast = Whites_i;
            for (int j = WhiteLast; j > 0; j--)
            {
                if (Whites[j -1] != null)
                {
                    DestroyImmediate(Whites[j - 1].gameObject);
                    break;
                }
            }
            chessTurn = Turn.white;
            ChessState[Last_x[Lasts-1], Last_y[Lasts-1]] = 0;
            Lasts -= 1;
            CHess_UIM.ChessNumber = Lasts;
            CHess_UIM.ChangeWhichText("白");
        }
        D_Time = 20;
        StopDJS(true, true);
    }
    //↓销毁上一次的棋盘并创建新棋盘，被脚本UIManager()中ReStart()调用
    public void DestroyAndCreate()
    {
        for (int m = 0; m < Whites.Length; m++)
        {
            if (Whites[m] != null)
                DestroyImmediate(Whites[m].gameObject);
        }
        for (int n = 0; n < Whites.Length; n++)
        {
            if (Blacks[n] != null)
                DestroyImmediate(Blacks[n].gameObject);
        }
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                ChessState[i, j] = 0;
            }
        }
        isPlaying = true;
        chessTurn = Turn.black;
        Blacks_i = 0;
        Whites_i = 0;
        winner = 0;
        D_Time = 20;
        Lasts = 0;
    }
    //↓计算落子点与棋盘网格点的距离
    float Dis(Vector3 mPos, Vector2 gridPos)
    {
        return Mathf.Sqrt(Mathf.Pow(mPos.x - gridPos.x, 2)+ Mathf.Pow(mPos.y - gridPos.y, 2));
    }
    //↓控制倒计时的开始与关闭
    public void StopDJS(bool Stop,bool Start)
    {
        if (Stop)
        {
            StopAllCoroutines();               //停止所有协程
        }
        if (Start)
        {
            StartCoroutine(DaoJiShi(true));   //开启协程DaoJiShi()
        }
    }
    //↓倒计时实现,被此脚本中StopDJS()、ChangePlayer()、CreateChess()、Start ()调用
    IEnumerator DaoJiShi(bool CanDJS)
    {
        while (CanDJS)                             //当可进行倒计时时，每一秒进行一次迭代
        {
            CHess_UIM.DaoJiShi(D_Time);
            yield return new WaitForSeconds(1);  //等待1s后执行后面的代码块
            D_Time--;
            if (D_Time == 0)
            {
                ChangePlayer();
                CanDJS = false;
            }
        }
    }
    //↑
    //↓倒计时结束后，更改控制权，被此脚本中DaoJiShi()调用
    public void ChangePlayer()
    {
        if (chessTurn == Turn.black)
        {
            CHess_UIM.ChangeWhichText("白");
            chessTurn = Turn.white;
            D_Time = 20;
            StopDJS(true,true);
        }
        else
        {
            CHess_UIM.ChangeWhichText("黑");
            chessTurn = Turn.black;
            D_Time = 20;
            StopDJS(true, true);
        }
    }
    //↑
    //↓创建棋子，被此脚本中Update ()调用
    public void CreateChess(int i,int j,Turn which)
    {
        Last_x[Lasts] = i;
        Last_y[Lasts] = j;
        Lasts += 1;
        CHess_UIM.ChessNumber = Lasts;
        D_Time = 20;
        StopDJS(true, true);
        if (which == Turn.white)
        {
            Whites[Whites_i] = (GameObject)GameObject.Instantiate(White,new Vector3(ChessPos[i,j].x,ChessPos[i,j].y,-0.5f),White.transform.rotation);
            Whites_i += 1;
            chessTurn = Turn.black;
            CHess_UIM.AddChessRoad(i, j, "白");
        }
        else if (which==Turn.black)
        {
            Blacks[Blacks_i] = (GameObject)GameObject.Instantiate(Black, new Vector3(ChessPos[i, j].x, ChessPos[i, j].y, -0.5f), Black.transform.rotation);
            Blacks_i += 1;
            chessTurn = Turn.white;
            CHess_UIM.AddChessRoad(i, j, "黑");
        }
    }
    //↓检测是够获胜的函数，不含禁手检测，被此脚本中Update ()调用
    //  判断算法:对一个点的八方向进行检测，当一个方向不超界时对此方向进行值检测
    public int result()
    {
        int flag = 0;
            for (int i=0;i<15;i++)
                for (int j = 0; j < 15; j++)
                {
                    if (i + BlackWinNumber-1<= 14||i+WhiteWinNumber<=14)   //X轴正向
                    { 
                        if (chessTurn == Turn.white)   //此次将为白棋执子，判断黑棋是否获胜
                            {
                        for (int m = 0; m < BlackWinNumber; m++)
                        {
                           
                            if (ChessState[i+m, j] == 1 )
                            {
                                    if (m == BlackWinNumber - 1)
                                    {
                                        flag = 1;
                                        return flag;
                                    }
                                   
                                }
                            else
                                break;
                            }
                    }
                            else if (chessTurn == Turn.black)   //此次将为黑棋执子，判断白棋是否获胜
                            {
                                for (int m = 0; m < WhiteWinNumber; m++)
                                {

                                    if (ChessState[i + m, j] ==- 1)
                                    {
                                        if (m == WhiteWinNumber - 1)
                                        {
                                            flag =- 1;
                                            return flag;
                                        }

                                    }
                                    else
                                        break;
                                }
                        }
                    }
                    else if (i - (BlackWinNumber - 1) >= 0 || i - (WhiteWinNumber - 1) >= 0)   //X轴负向
                    {
                        if (chessTurn == Turn.white)   //此次将为白棋执子，判断黑棋是否获胜
                        {
                            for (int m = 0; m < BlackWinNumber; m++)
                            {

                                if (ChessState[i - m, j] == 1)
                                {
                                    if (m == BlackWinNumber - 1)
                                    {
                                        flag = 1;
                                        return flag;
                                    }

                                }
                                else
                                    break;
                            }
                        }
                        else if (chessTurn == Turn.black)   //此次将为黑棋执子，判断白棋是否获胜
                        {
                            for (int m = 0; m < WhiteWinNumber; m++)
                            {

                                if (ChessState[i - m, j] == -1)
                                {
                                    if (m == WhiteWinNumber - 1)
                                    {
                                        flag = -1;
                                        return flag;
                                    }

                                }
                                else
                                    break;
                            }
                        }
                    }
                     if (j + WhiteWinNumber - 1 <= 14||j+BlackWinNumber-1<=14)   //Y轴正向
                    {
                        if (chessTurn == Turn.white)   //此次将为白棋执子，判断黑棋是否获胜
                        {
                            for (int m = 0; m < BlackWinNumber; m++)
                            {

                                if (ChessState[i , j+m] == 1)
                                {
                                    if (m == BlackWinNumber - 1)
                                    {
                                        flag = 1;
                                        return flag;
                                    }

                                }
                                else
                                    break;
                            }
                        }
                        else if (chessTurn == Turn.black)   //此次将为黑棋执子，判断白棋是否获胜
                        {
                            for (int m = 0; m < WhiteWinNumber; m++)
                            {

                                if (ChessState[i , j+m] == -1)
                                {
                                    if (m == WhiteWinNumber - 1)
                                    {
                                        flag = -1;
                                        return flag;
                                    }

                                }
                                else
                                    break;
                            }
                        }
                    }
                     if (j - (WhiteWinNumber - 1) >= 0||j-(BlackWinNumber-1)>=0)     //Y轴负向
                    {
                        if (chessTurn == Turn.white)   //此次将为白棋执子，判断黑棋是否获胜
                        {
                            for (int m = 0; m < BlackWinNumber; m++)
                            {

                                if (ChessState[i , j-m] == 1)
                                {
                                    if (m == BlackWinNumber - 1)
                                    {
                                        flag = 1;
                                        return flag;
                                    }

                                }
                                else
                                    break;
                            }
                        }
                        else if (chessTurn == Turn.black)   //此次将为黑棋执子，判断白棋是否获胜
                        {
                            for (int m = 0; m < WhiteWinNumber; m++)
                            {

                                if (ChessState[i , j-m] == -1)
                                {
                                    if (m == WhiteWinNumber - 1)
                                    {
                                        flag = -1;
                                        return flag;
                                    }

                                }
                                else
                                    break;
                            }
                        }
                    }
                     if ((i +WhiteWinNumber-1 <= 14 && j +WhiteWinNumber-1 <= 14)||(i+BlackWinNumber-1<=14&&j+BlackWinNumber-1<=14))  //XY轴正向
                    {
                        if (chessTurn == Turn.white)   //此次将为白棋执子，判断黑棋是否获胜
                        {
                            for (int m = 0; m < BlackWinNumber; m++)
                            {

                                if (ChessState[i + m, j+m] == 1)
                                {
                                    if (m == BlackWinNumber - 1)
                                    {
                                        flag = 1;
                                        return flag;
                                    }

                                }
                                else
                                    break;
                            }
                        }
                        else if (chessTurn == Turn.black)   //此次将为黑棋执子，判断白棋是否获胜
                        {
                            for (int m = 0; m < WhiteWinNumber; m++)
                            {

                                if (ChessState[i + m, j+m] == -1)
                                {
                                    if (m == WhiteWinNumber - 1)
                                    {
                                        flag = -1;
                                        return flag;
                                    }

                                }
                                else
                                    break;
                            }
                        }
                    }
                     if ((i - (WhiteWinNumber - 1) >= 0 && j - (WhiteWinNumber - 1) >= 0) || ((i - (BlackWinNumber - 1) >= 0) && j - (BlackWinNumber - 1) >= 0))    //XY轴负向
                    {
                        if (chessTurn == Turn.white)   //此次将为白棋执子，判断黑棋是否获胜
                        {
                            for (int m = 0; m < BlackWinNumber; m++)
                            {

                                if (ChessState[i -m, j-m] == 1)
                                {
                                    if (m == BlackWinNumber - 1)
                                    {
                                        flag = 1;
                                        return flag;
                                    }

                                }
                                else
                                    break;
                            }
                        }
                        else if (chessTurn == Turn.black)   //此次将为黑棋执子，判断白棋是否获胜
                        {
                            for (int m = 0; m < WhiteWinNumber; m++)
                            {

                                if (ChessState[i - m, j-m] == -1)
                                {
                                    if (m == WhiteWinNumber - 1)
                                    {
                                        flag = -1;
                                        return flag;
                                    }

                                }
                                else
                                    break;
                            }
                        }
                    }
                    if ((i + (WhiteWinNumber - 1) <= 14 && j - (WhiteWinNumber - 1) >= 0)||(i+(BlackWinNumber-1)<=14&&j-(BlackWinNumber-1)>=0))   //X轴正向Y轴负向
                    {
                        if (chessTurn == Turn.white)   //此次将为白棋执子，判断黑棋是否获胜
                        {
                            for (int m = 0; m < BlackWinNumber; m++)
                            {

                                if (ChessState[i + m, j-m] == 1)
                                {
                                    if (m == BlackWinNumber - 1)
                                    {
                                        flag = 1;
                                        return flag;
                                    }

                                }
                                else
                                    break;
                            }
                        }
                        else if (chessTurn == Turn.black)   //此次将为黑棋执子，判断白棋是否获胜
                        {
                            for (int m = 0; m < WhiteWinNumber; m++)
                            {

                                if (ChessState[i + m, j-m] ==- 1)
                                {
                                    if (m == WhiteWinNumber - 1)
                                    {
                                        flag = -1;
                                        return flag;
                                    }

                                }
                                else
                                    break;
                            }
                        }
                    }
                     if ((i - (WhiteWinNumber - 1) >= 0 && j + (WhiteWinNumber - 1) <= 14)||(i-(BlackWinNumber-1)>=0&&(j+(BlackWinNumber-1)<=14)))   //X轴负向Y轴正向
                    {
                        if (chessTurn == Turn.white)   //此次将为白棋执子，判断黑棋是否获胜
                        {
                            for (int m = 0; m < BlackWinNumber; m++)
                            {

                                if (ChessState[i - m, j+m] == 1)
                                {
                                    if (m == BlackWinNumber - 1)
                                    {
                                        flag = 1;
                                        return flag;
                                    }

                                }
                                else
                                    break;
                            }
                        }
                        else if (chessTurn == Turn.black)   //此次将为黑棋执子，判断白棋是否获胜
                        {
                            for (int m = 0; m < WhiteWinNumber; m++)
                            {

                                if (ChessState[i - m, j+m] == -1)
                                {
                                    if (m == WhiteWinNumber - 1)
                                    {
                                        flag = -1;
                                        return flag;
                                    }

                                }
                                else
                                    break;
                            }
                        }
                    }
                }
            return 0;
    }    
}
