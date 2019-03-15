using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour{
    public GameObject MainPanel;                //主界面
    public GameObject WhichText;                //显示当前执子方的txt组件
    public GameObject WhiteNumberText;          //显示白棋步数的txt组件
    public GameObject BlackNumberText;          //显示黑棋步数的txt组件
    public GameObject ChessRoads;               //显示棋路的txt组件
    public GameObject CountDownText;            //显示倒计时的txt组件
    public GameObject SettingButton;            //显示设置菜单的按钮组件
    public GameObject SettingPanel;             //设置菜单
    public GameObject CloseImage;               //关闭设置菜单的按钮
    public GameObject RestartImage;             //重新开始的按钮
    public GameObject WithDrawImage;            //悔棋的按钮`
    public GameObject WinRestartImage;          //胜利菜单的重新开始按钮
    public GameObject NoChessImage;             //没棋可悔时的图片
    public GameObject Now_Time;                 //当前时间
    public Sprite[] WinPictures=new Sprite[2];  //存储两张不同胜利情况下的图片
    public Image WinImage;                      //胜利界面
    public string[] ChessRoad=new string[100];  //存储棋路的数组
    int ChessRoad_i=0;                          //棋路数组的下标
    private GameObject IsOrNo;                  //显示是否确定悔棋的菜单
    private Text CloseText;                     //关闭设置菜单按钮的文本
    private Text RestartText;                   //重新开始按钮的文本
    private Text WithDrawText;                  //悔棋按钮的文本
    private Text WinRestartText;                //胜利菜单下的重新开始文本
    public int White = 0;                       //白棋步数
    public int Black = 0;                       //黑棋步数
    public int ChessNumber = 0;                 //当前共下了多少步
    private chess UIM_chess;                    //调用chess（）脚本
    private string WhichChess;                  //当前为哪个棋子
    System.DateTime NowTime ;                   //调用系统时间
    private int Time_Clocktime=0;               //点击的次数
    private bool CanShow = true;                //系统时间是否可显示
    Color color2;                               //文本颜色
	//↓初始化相应变量
	void Start () {
        IsOrNo = GameObject.Find("IsOrNo");
        UIM_chess = GameObject.Find("Plane").GetComponent<chess>();
        CloseText = GameObject.Find("CloseText").GetComponent<Text>();
        RestartText = GameObject.Find("RestartText").GetComponent<Text>();
        WithDrawText = GameObject.Find("WithDrawText").GetComponent<Text>();
        WinRestartText = GameObject.Find("WRText").GetComponent<Text>();
        //↓关闭UI界面
        IsOrNo.transform.DOScale(0, 0);  
        SettingPanel.transform.DOScale(0, 0);
        NoChessImage.transform.DOScale(0, 0);
        WinImage.transform.DOScale(0, 0);
        //↑
        EventTriggerListener.Get(CloseImage.gameObject).onEnter = GetEnterGameObject;
        EventTriggerListener.Get(RestartImage.gameObject).onEnter = GetEnterGameObject;
        EventTriggerListener.Get(WithDrawImage.gameObject).onEnter = GetEnterGameObject;
        EventTriggerListener.Get(WinRestartImage.gameObject).onEnter = GetEnterGameObject;
        EventTriggerListener.Get(CloseImage.gameObject).onExit = GetExitGameObject;
        EventTriggerListener.Get(RestartImage.gameObject).onExit = GetExitGameObject;
        EventTriggerListener.Get(WithDrawImage.gameObject).onExit = GetExitGameObject;
        EventTriggerListener.Get(WinRestartImage.gameObject).onExit = GetExitGameObject;
	}
    //↓更新系统时间，每一帧被调用
    void Update()
    {
        NowTime = System.DateTime.Now;
        if(CanShow)
        Now_Time.GetComponent<Text>().text = "时间:" + NowTime;//更新系统时间
    }
    //↓点击关闭显示系统时间或出现系统时间
    public void CloseTime()
    {
        Time_Clocktime += 1;
        if (Time_Clocktime % 2 == 1)
        {
            Now_Time.GetComponent<Text>().text = null;
            CanShow = false;

        }
        else
        {
            CanShow = true;
        }
    }
    //↓胜利时的处理函数，被脚本chess()中Update()调用
    public void Win(int which)
    {
        UIM_chess.isPlaying = false;
        UIM_chess.StopDJS(true, false);
        if (which == 1)
        {
            WinImage.sprite = WinPictures[0];
            WinImage.transform.DOScale(1, 0.2f); //用0.2s显示UI界面
        }
        else if (which == -1)
        {
            WinImage.sprite = WinPictures[1];
            WinImage.transform.DOScale(1, 0.2f); //用0.2s显示UI界面
        }
    }
    //↓改变执子方显示，被脚本chess()中Update()、GiveUp()、ChangePlayer()；脚本UIManager()中ReStart()调用
    public void ChangeWhichText(string which)
    {
        WhichChess = which;
        WhichText.GetComponent<Text>().text = which + "棋执子";
    }
    //↓改变倒计时显示、，被脚本chess()中Start()、StopDJS(）、ChangePlayer()、 CreateChess()调用
    public void DaoJiShi(int time)
    {
        CountDownText.GetComponent<Text>().text = "倒计时:\n" + time;
    }
    //↓改变棋子步数显示，被脚本chess()中Update ()；UIManager()中Yes()调用
    public void ChangeChessNumberText(int i)
    {
        if (i == 1)
        {
            Black += 1;
            BlackNumberText.GetComponent<Text>().text = null;
            BlackNumberText.GetComponent<Text>().text +=Black;
        }
        else if (i == -1)
        {
            White += 1;
            WhiteNumberText.GetComponent<Text>().text = null;
            WhiteNumberText.GetComponent<Text>().text += White;
        }
    }
    //↓改变棋子路数显示，被脚本chess()中CreateChess()调用
    public void AddChessRoad(int x,int y,string which)
    {
        ChessRoad[ChessRoad_i] = which + ":" + x + "," + y + "\n";
        ChessRoads.GetComponent<Text>().text += ChessRoad[ChessRoad_i];
        ChessRoad_i+=1;
    }
    //↓开启设置界面,在SettingButton按钮组件的OnClick()响应函数中被调用(引擎中界面拖动，无需编写脚本)
    public void OpenSettingPanel()
    {
        UIM_chess.isPlaying = false;
        UIM_chess.StopDJS(true, false);
        SettingPanel.transform.DOScale(1, 0.3f);          //用0.3s显示UI界面
    }
    //↓选择重新开始时的响应事件，在RestartImage按钮组件的OnClick()响应函数中被调用(引擎中界面拖动，无需编写脚本)
    public void ReStart()
    {
        UIM_chess.DestroyAndCreate();                     //销毁棋盘
        ChangeWhichText("黑");                            //重置执子方
        BlackNumberText.GetComponent<Text>().text = "0";  //重置黑棋步数显示
        WhiteNumberText.GetComponent<Text>().text = "0";  //重置白棋步数显示
        ChessRoads.GetComponent<Text>().text = null;      //重置棋路
        for (int i = 0; i < ChessRoad.Length; i++)        //重置棋路数组  
        {
            if (ChessRoad[i] != null)
                ChessRoad[i] = null;
        }
        ChessRoad_i = 0;                                  //重置棋路数组下标                
        Black = 0;                                        //重置黑棋步数
        White = 0;                                        //重置白棋步数
        CloseSettingPanel();                              //关闭设置菜单
    }
    //↓关闭设置界面，在CloseImage按钮组件的OnClick()响应函数中被调用(引擎中界面拖动，无需编写脚本)，被脚本UIManager中ReStart()被调用
    public void CloseSettingPanel()
    {
        UIM_chess.isPlaying = true;
        SettingPanel.transform.DOScale(0, 0.3f);       //用0.3s关闭UI界面
        WinImage.transform.DOScale(0, 0.3f);           //用0.3s关闭UI界面
        UIM_chess.StopDJS(false, true);
    }

    //↓选择悔棋时的响应事件，在WithDrawImage按钮组件的OnClick()响应函数中被调用(引擎中界面拖动，无需编写脚本)
    public void WithDraw()
    {
        IsOrNo.transform.DOScale(1, 0.3f);            //用0.3s显示UI界面
    }
    //↓确定悔棋，在引擎Hierarchy界面下的Yes按钮组件的OnClick()响应函数中被调用(引擎中界面拖动，无需编写脚本)
    public void Yes()
    {
        if (ChessNumber == 0)
            NoChessImage.transform.DOScale(1, 0.3f); //用0.3s显示UI界面
        if (ChessNumber != 0)
        {
            UIM_chess.GiveUp();
            IsOrNo.transform.DOScale(0, 0.3f);       //用0.3s关闭UI界面
            SettingPanel.transform.DOScale(0, 0.3f); //用0.3s关闭UI界面
            UIM_chess.isPlaying = true;
            ChessRoad_i -= 1;
            ChessRoad[ChessRoad_i] = null;
            ChessRoads.GetComponent<Text>().text = null;
            for (int i = 0; i < ChessRoad.Length; i++)
            {
                ChessRoads.GetComponent<Text>().text += ChessRoad[i];
                if (ChessRoad[i] == null)
                    break;
            }
            if (WhichChess == "白")
            {
                Black -= 2;
                ChangeChessNumberText(1);
            }
            else if (WhichChess == "黑")
            {
                White -= 2;
                ChangeChessNumberText(-1);

            }
        }
    }
    //↓不悔棋，在引擎Hierarchy界面下的No按钮组件的OnClick()响应函数中被调用(引擎中界面拖动，无需编写脚本)
    public void No()
    {
        IsOrNo.transform.DOScale(0, 0.3f);         //用0.3s关闭UI界面
    }
    //↓没棋可悔时的返回，在引擎Hierarchy界面下的Return按钮组件的OnClick()响应函数中被调用(引擎中界面拖动，无需编写脚本)
    public void Return()
    {
        NoChessImage.transform.DOScale(0, 0.3f);  //用0.3s关闭UI界面
    }
    //↓设置鼠标进入选项按钮时的响应事件
    private void GetEnterGameObject(GameObject GO)
    {

        if (GO == CloseImage)
        {
            color2 = CloseText.color;            //改变字体颜色
            color2.a=1;
            CloseText.DOColor(color2, 0.5f);
        }
       if (GO == RestartImage)
        {
            color2 = RestartText.color;          //改变字体颜色
            color2.a = 1;
            RestartText.DOColor(color2, 0.5f);
        }
        if (GO == WithDrawImage)
        {
            color2 = WithDrawText.color;        //改变字体颜色
            color2.a = 1;
            WithDrawText.DOColor(color2, 0.5f);
        }
        if (GO == WinRestartImage)
        {
            color2 = WinRestartText.color;      //改变字体颜色
            color2.a = 1;
            WinRestartText.DOColor(color2, 0.5f);
        }
    }
    //↓鼠标离开选项界面时的响应事件
    private void GetExitGameObject(GameObject GO)
    {
        if (GO == CloseImage)
        {
            color2 = CloseText.color;          //改变字体颜色
            color2.a = 0;
            CloseText.DOColor(color2, 0.5f);
        }
       if (GO == RestartImage)
        {
            color2 = RestartText.color;        //改变字体颜色
            color2.a = 0;
            RestartText.DOColor(color2, 0.5f);
        }
         if (GO == WithDrawImage)
        {
            color2 = WithDrawText.color;       //改变字体颜色
            color2.a = 0;
            WithDrawText.DOColor(color2, 0.5f);
        }
         if (GO == WinRestartImage)
         {
             color2 = WinRestartText.color;    //改变字体颜色
             color2.a = 0;
             WinRestartText.DOColor(color2, 0.5f);
         }
    }
}
