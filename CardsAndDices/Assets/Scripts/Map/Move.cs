using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Move : MonoBehaviour {
    public GameObject[] ThisTimeMap=new GameObject[20];             //这一次的游戏总地图      
    private GameObject _trans;                   // 目标物体的空间变换组件  

    public int i;                               //可变的地图数目值，用于表示此次控制的是第几张地图，当i<0时通知CreateMap()进行下一张地图加载
    public bool CanMove=true;                    //控制是否可拖拽，当鼠标离开、地图已探索完时不可拖拽，当鼠标点击时、离开运动结束可拖拽
    public int DontMove_Move = 0;                //由其他函数控制，强制使卡牌不可拖动，为1时不可动
    private bool CanDo=true;                     //一个动作结束前，不可触发其他动作
    public bool HaveIt;                         //判断鼠标是否点击到卡牌
    private Vector3 ScreenPosition;              //点击到的物体的屏幕坐标值      
    private Vector3 offset;                      //点击到的物体的坐标与鼠标点击点的屏幕坐标的偏差值，消除点击位置的向量对移动造成的不平滑影响。
    private Vector3 X = new Vector3(0,0,0);      //X轴向量
    private Vector3 Y = new Vector3(0, 0, 0);    //Y轴向量
    private Vector3 StartPos = new Vector3();    //鼠标点击时的初始位置（世界坐标）
    Vector3 currentScreenSpace = new Vector3();  //鼠标位置的世界坐标 
    private bool Xzhou=true;                     //锁定初始运动方向为X轴
    private bool Yzhou = true;                   //锁定初始运动方向为Y轴
    Vector3 currentPosition = new Vector3();     //鼠标位置的屏幕坐标与偏差值之和，
    private int Right = 0;                       //此次卡牌向东飞出
    private int Up = 0;                          //——北
    private int Down = 0;                        //——南
    private int Left = 0;                        //——西
    public int limit;                           //固定的地图数目值，用于销毁此次地图
    private int[,] MapArray_Move = new int[5, 5];
    private int StartX;
    private int StartY;
    public string ThisName;           //本次创建出的卡牌名
    public int ThisNumber;            //本此卡牌在数组中的位置

    public static int MoveCount = 0;        //移动步数，为2时减少一点饱食度
    CreateMap Move_CM;                           //调用CreateMap（）脚本 
//    ToBlack Move_TB;                             //调用ToBlack（）脚本
    public NewUIManager Move_NUIM;
    private ScriptsManager Move_SM;
    private RayManager Move_RM;

	void Start () {
        Move_SM = GameObject.Find("Main Camera").GetComponent<ScriptsManager>();
        Move_CM = GameObject.Find("CreateMap").GetComponent<CreateMap>();                        //得到CreateMap（）脚本组件
    //    Move_TB = GameObject.Find("ToBlack").GetComponent<ToBlack>();     //得到ToBlack（）脚本组件
        Move_NUIM = Move_SM.NUIM;
        Move_RM = Move_SM.RM;
	}
    //↓得到地图信息
    //  被CreateMap（）中InitMap()调用
	public void GetMap(GameObject[] map,int n,int l,int[,] m,int x,int y)                            
    {
        limit = l;
        
            ThisTimeMap = map;
                i = n;
        for (int q = 0; q < 5; q++)
            for (int j = 0; j < 5; j++)
                MapArray_Move[q, j] = m[q, j];
        StartX = x;
        StartY = y;
    }
    //↓得到卡牌名字和数字
    //  被CreateMap中InitMap()调用
    public void GetThisCardNameAndNumber(string name,int number)
    {
        Debug.Log("Move Name is "+ name+"Number is "+number);
        ThisName = name;
    //    if (Move_CM.ThisName!=null)
   //     ThisName = Move_CM.ThisName;
        ThisNumber = number;
    }
    //↓进行射线检测，以及一切与卡牌拖动有关运算的处理
	void Update () {
      /*  if (i == limit)                                                   
        {
            CanMove = false;
            Right = 0;                  
            Left = 0;
            Up = 0;
            Down = 0;
            CanDo = true;
            Xzhou = true;
            Yzhou = true;
            i++;
            DestroyAndCreate();                                          
        }*/
        if (Input.GetMouseButtonDown(0))                                   //鼠标点击时
        {
            StartPos = Input.mousePosition;
            CanMove = true;
             Ray ray_Move = Camera.main.ScreenPointToRay(Input.mousePosition);   //发出射线，判断是否点击到了卡牌
              RaycastHit hit_Move=new RaycastHit();
              if (Physics.Raycast(ray_Move, out hit_Move))              
                {
                //    Debug.Log("thisName is "+ThisName);
                    if (hit_Move.transform.name == ThisName)
                    {
                        HaveIt = true;
                    }
                    else HaveIt = false;
                }
                else HaveIt = false;
                if (DontMove_Move == 1)
                    HaveIt = false;
                if (HaveIt)
                {
                    ScreenPosition = Camera.main.WorldToScreenPoint(ThisTimeMap[ThisNumber].transform.position);
                    offset = ThisTimeMap[ThisNumber].transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPosition.z));
                }
        }
        if (Input.GetMouseButtonUp(0) && HaveIt)                                    //鼠标抬起时
        {
            CanMove = false;                                                        //鼠标抬起时不进行拖拽判断
            if (Input.mousePosition.x - StartPos.x >= 120 && CanDo&&Xzhou)           //X正方向飞出
            {
                Right = 1;
                if (StartX + 1 > 4 || MapArray_Move[StartX + 1, StartY] != 1)
                {
                    Debug.Log("NoWay 's StartX is " + StartX + " startY is " + StartY);
                    Tweener Return = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(0, 0, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                    Return.SetEase(Ease.InOutQuad);
                    Move_NUIM.NoWayOut();
                }
                else
                {
                    StartX += 1;
                    Move_CM.CreateNext(Right,StartX,StartY);                                          //飞出时创建下一张卡
                    CanDo = false;
                    Tweener GoOut = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(30, 0, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                    GoOut.SetEase(Ease.OutQuad);
               //     i = i + 1;
                //    if (i == limit) Move_TB.TOBlack("Move");                                      //最后一张卡飞出时，屏幕变黑
                    GoOut.OnComplete(delegate() { CanDo = true; DestroyImmediate(ThisTimeMap[ThisNumber].gameObject); });
                    Right = 0;
                    Yzhou = true;
                }
            }
            else if (Input.mousePosition.x - StartPos.x <= -120 && CanDo && Xzhou)  //X负方向飞出
                {
                    Left = 2;
                    if (StartX - 1 < 0 || MapArray_Move[StartX - 1, StartY] != 1)
                    {
                       Debug.Log("NoWay 's StartX is " + StartX + " startY is " + StartY);
                        Tweener Return = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(0, 0, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                        Return.SetEase(Ease.InOutQuad);
                        Move_NUIM.NoWayOut();
                    }
                    else
                    {
                        StartX -=1;
                        Move_CM.CreateNext(Left, StartX, StartY);                                      //飞出时创建下一张卡
                        CanDo = false;
                        Tweener GoOut = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(-30, 0, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                        GoOut.SetEase(Ease.OutQuad);
                     //   i = i + 1;
                     //   if (i == limit) Move_TB.TOBlack("Move");                                  //最后一张卡飞出时，屏幕变黑
                        GoOut.OnComplete(delegate() { CanDo = true; DestroyImmediate(ThisTimeMap[ThisNumber].gameObject); });
                        Left = 0;
                        Yzhou = true;
                    }
                }
            else if (Input.mousePosition.y - StartPos.y >= 120 && CanDo && Yzhou)    //Y正方向飞出
            {
                    Up = 3;
                    if (StartY + 1 > 4 || MapArray_Move[StartX, StartY + 1] != 1)
                    {
                        Debug.Log("NoWay 's StartX is " + StartX + " startY is " + StartY);
                        Tweener Return = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(0, 0, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                        Return.SetEase(Ease.InOutQuad);
                        Move_NUIM.NoWayOut();
                    }
                    else
                    {
                        StartY += 1;
                        Move_CM.CreateNext(Up, StartX, StartY);                                        //飞出时创建下一张卡
                        CanDo = false;
                        Tweener GoOut = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(0, 15, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                        GoOut.SetEase(Ease.OutQuad);
                    //    i = i + 1;
                    //    if (i == limit) Move_TB.TOBlack("Move");                                  //最后一张卡飞出时，屏幕变黑
                        GoOut.OnComplete(delegate() { CanDo = true; DestroyImmediate(ThisTimeMap[ThisNumber].gameObject); });
                        Up = 0;
                        Xzhou = true;
                    }
                }
            else if (Input.mousePosition.y - StartPos.y <= -120 && CanDo && Yzhou)   //Y负方向飞出
            {
                   
                    Down = 4;
                    if (StartY - 1 < 0 || MapArray_Move[StartX, StartY - 1] != 1)
                    {
                        Debug.Log("NoWay 's StartX is " + StartX + " startY is " + StartY);
                        Tweener Return = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(0, 0, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                        Return.SetEase(Ease.InOutQuad);
                        Move_NUIM.NoWayOut();
                    }
                    else
                    {
                        StartY -= 1;
                        Move_CM.CreateNext(Down, StartX, StartY);                                      //飞出时创建下一张卡
                        CanDo = false;
                        Tweener GoOut = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(0, -15, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                        GoOut.SetEase(Ease.OutQuad);
                    //    i = i + 1;
                   //     if (i == limit) Move_TB.TOBlack("Move");                                  //最后一张卡飞出时，屏幕变黑
                        GoOut.OnComplete(delegate() { CanDo = true; DestroyImmediate(ThisTimeMap[ThisNumber].gameObject); });
                        Down = 0;
                        Xzhou = true;
                    }
                }

            if (Input.mousePosition.x - StartPos.x < 120 && Input.mousePosition.x - StartPos.x >= 0 && CanDo && Xzhou)   //拖拽距离不足X正方向返回
            {
                CanDo = false;
                Tweener Return = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(0, 0, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                    Return.SetEase(Ease.InOutQuad);
                    Return.OnComplete(delegate()   {  CanDo = true;    });
                    Yzhou = true;
                    HaveIt = false;
            }
            else if (Input.mousePosition.x - StartPos.x > -120 && Input.mousePosition.x - StartPos.x <= 0 && CanDo && Xzhou)  //拖拽距离不足X负方向返回
            {
                CanDo = false;
                Tweener Return = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(0, 0, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                    Return.SetEase(Ease.InOutQuad);
                    Return.OnComplete(delegate()   {  CanDo = true;      });
                    Yzhou = true;
                    HaveIt = false;
            }
            else if (Input.mousePosition.y - StartPos.y < 120 && Input.mousePosition.y - StartPos.y >= 0 && CanDo && Yzhou)   //拖拽距离不足Y正方向返回
            {
                CanDo = false;
                Tweener Return = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(0, 0, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                Return.SetEase(Ease.InOutQuad);
                Return.OnComplete(delegate()   {  CanDo = true;    });
                Xzhou = true;
                HaveIt = false;
            }
            else if (Input.mousePosition.y - StartPos.y > -120 && Input.mousePosition.y - StartPos.y <= 0 && CanDo && Yzhou)    //拖拽距离不足Y负方向返回
            {
                CanDo = false;
                Tweener Return = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(0, 0, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                Return.SetEase(Ease.InOutQuad);
                Return.OnComplete(delegate() {  CanDo = true;});
                Xzhou = true;
                HaveIt = false;
            }
        }
        if (CanMove && HaveIt )              //实现拖拽
        {
            currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPosition.z );
            currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace)+offset;
            if (Input.mousePosition.x - StartPos.x > Input.mousePosition.y - StartPos.y && Input.mousePosition.x - StartPos.x > 0&&Xzhou)
            {
                Yzhou = false;
                X.z = ThisTimeMap[ThisNumber].transform.position.z;
                X.x = currentPosition.x;
                ThisTimeMap[ThisNumber].transform.position = X;
                if (Input.mousePosition.x - StartPos.x > 400)
                {
                    CanDo = false;
                    HaveIt = false;
                    Tweener Return_2 = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(0, 0, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                    Return_2.OnComplete(delegate() {  CanDo = true;  });
                    Yzhou = true;

                }
            }
            if (Input.mousePosition.x - StartPos.x < Input.mousePosition.y - StartPos.y && Input.mousePosition.x - StartPos.x < 0 && Xzhou)
            {
                Yzhou = false;
                X.z = ThisTimeMap[ThisNumber].transform.position.z;
                X.x = currentPosition.x;
                ThisTimeMap[ThisNumber].transform.position = X;
                if (Input.mousePosition.x - StartPos.x < -400)
                {
                    CanDo = false;
                    HaveIt = false;
                    Tweener Return_2 = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(0, 0, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                    Return_2.OnComplete(delegate()  {  CanDo = true;       });
                    Yzhou = true;
                }
            }
            if (Input.mousePosition.y - StartPos.y > Input.mousePosition.x - StartPos.x && Input.mousePosition.y - StartPos.y > 0&&Yzhou)
            {
                Xzhou = false;
                Y.z = ThisTimeMap[ThisNumber].transform.position.z;
                Y.y = currentPosition.y;
                ThisTimeMap[ThisNumber].transform.position = Y;
                if (Input.mousePosition.y - StartPos.y > 400)
                {
                    CanDo = false;
                    HaveIt = false;
                    Tweener Return_2 = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(0, 0, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                    Return_2.OnComplete(delegate()  {   CanDo = true;   });
                    Xzhou = true;
                }
            }
            if (Input.mousePosition.y - StartPos.y < Input.mousePosition.x - StartPos.x && Input.mousePosition.y - StartPos.y < 0 && Yzhou)
            {
                Xzhou = false;

                Y.z = ThisTimeMap[ThisNumber].transform.position.z;
               Y.y= currentPosition.y;
               ThisTimeMap[ThisNumber].transform.position = Y;
                if (Input.mousePosition.y - StartPos.y < -400)
                {
                    CanDo = false;
                    HaveIt = false;
                    Tweener Return_2 = ThisTimeMap[ThisNumber].transform.DOMove(new Vector3(0, 0, ThisTimeMap[ThisNumber].transform.position.z), 0.5f);
                    Return_2.OnComplete(delegate()    { CanDo = true;    });
                    Xzhou = true;
                }
            }
        }
	}
  
 //   void OnGUI()
  //  {
  //  }
    //↓销毁当前地图，被ToBlack中TOBlack()调用
    public void DestroyAndCreate()                                   
    {
        Debug.Log("DESTROY!!!!!!!!");
         Destroy(ThisTimeMap[limit-1].gameObject);
    }
}
