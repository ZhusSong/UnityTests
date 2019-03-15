using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
//储存卡牌信息，并进行与卡牌有关的事件处理
public class CardsEvents
{
    public enum KindsOfButton            //事件种类的枚举类型
    {
        Return = 0,
        Roll = 1,
        Battle = 2,
        Next = 3,
        Buy = 4,
        Get = 5
    }
    //↓调用类，每当新增卡牌时都需要在此进行声明
    private BaoYu CE_BY;
    private GuaiSheng CE_GS;
    private HouQun CE_HQ;
    private JiLiu CE_JL;
    private JuMang CE_JM;
    private LangQun CE_LQ;
    private LuoNanZhiRen CE_LNZR;
    private ShouXue CE_SX;
    private ShuZhen CE_SZ;
    private SiShi CE_SS;
    private XianJing CE_XJ;
    private XuanYa CE_XY;
    private YiJi CE_YJ;
    private YiShi CE_YS;
    private ZhaoZe CE_ZZ;
    private MuShi CE_MS;
    private ShangRen CE_SR;
    private TuDiShen CE_TDS;
    private MaChe CE_MC;
    private ChuanSongZhen CE_CSZ;
    private ShanDong CE_SD;
    private DuChuan CE_DC;
    private static int First = 0;  //控制每一次游戏进行时，只对卡牌函数实例化一次

    private Move CE_Move;


    public delegate void SendName_EventHander(string cardName, string eventName, int time);  //定义委托，用于传递每一次的卡排名，事件名和调用次数
    public static event SendName_EventHander SendName;
    public delegate void SendDeviationValue_EventHander(int value);  //定义委托，用于对每一个卡牌事件传递掷骰时的补正值
    public static event SendDeviationValue_EventHander SendValue;
    //↓构造函数，进行相关类实例化
    public CardsEvents()
    {
        First++;
        if (First == 1)
        {
            //↓实例化，每次新增卡牌时都需要进行实例化
            CE_BY = new BaoYu();
            CE_GS = new GuaiSheng();
            CE_HQ = new HouQun();
            CE_JL = new JiLiu();
            CE_JM = new JuMang();
            CE_LQ = new LangQun();
            CE_LNZR = new LuoNanZhiRen();
            CE_SX = new ShouXue();
            CE_SZ = new ShuZhen();
            CE_SS = new SiShi();
            CE_XJ = new XianJing();
            CE_XY = new XuanYa();
            CE_YJ = new YiJi();
            CE_YS = new YiShi();
            CE_ZZ = new ZhaoZe();
            CE_MS = new MuShi();
            CE_SR = new ShangRen();
            CE_TDS = new TuDiShen();
            CE_MC = new MaChe();
            CE_SD = new ShanDong();
            CE_DC = new DuChuan();
            CE_CSZ = new ChuanSongZhen();
            CE_Move = GameObject.Find("CreateMap").GetComponent<Move>();
        }
    }
    /// <summary>
    /// 进行广播
    /// </summary>
    /// <param name="cardName"></param>
    /// <param name="eventName"></param>
    /// <param name="time"></param>
    public void GetCardNameAndEvent(string cardName, string eventName, int time)
    {
        // Debug.Log("CE Get Time is " + time);
        if (SendName != null)
        {
            Debug.Log("cardName is " + cardName + " event is " + eventName + " time is " + time);
            //   Debug.Log("This Name Is " + cardName);
            SendName(cardName, eventName, time);
        }
    }
    public void GetDeviationValueAndSend(int value)
    {
        if (SendValue != null)
        {
            SendValue(value);
        }
    }
}
//↓"暴雨"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑
public class BaoYu
{
    private string Name = "BaoYu";                      //卡牌名
    private int ButtonsNumber_1 = 1;                //第一次有1个按钮
    private int ButtonsNumber_2 = 1;                //第二次有1个按钮

    private int ButtonsKind_1_1 = 1;                  //第一次的按钮种类为掷骰
    private int ButtonsKind_2_1 = 0;                  //第二次的按钮种类为返回
    //  private string Text;                              //事件文本
    private Sprite BgPicture;                         //事件贴图
    private Texture2D BgTexture;                      //贴图纹理
    private int Success = 40;                              //成功阈值
    private int GreatSuccess = 90;                         //大成功阈值
    private int Failure = 5;                              //失败阈值
    private string[] Monsters;                        //可能的怪物种类
    private Sprite BigBg;                             //此卡牌的事件背景
    private Sprite EventBg;                           //此卡牌的事件描述
    private int RollCount;                              //普通事件的随机值
    private int DeviationValue = 0;                        //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private Roll CE_Roll;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public BaoYu()
    {
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");

        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_Roll = new Roll();
        CardsEvents.SendName += BaoYu_Event;
        CardsEvents.SendValue += BaoYu_GetValue;
    }
    public void BaoYu_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void BaoYu_Event(string cardName, string eventName, int time)
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            //  Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    EventText = "今天的空气异常的潮湿，你预感到一场暴雨将要来临了，因此你躲到了一棵大树下，祈祷着这场大雨不会给你的冒险带来麻烦。";
                    time++;
                    //  CE_SA.CreateImage(BigBg, EventBg, 0);
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, Name, time);

                }
                if (eventName != null)
                {
                    if (time == 2)
                    {
                        RollCount = CE_Roll.NormalEvent(DeviationValue);
                        if (RollCount >= GreatSuccess)          //大成功
                        {
                            EventText = "  大树帮你挡住了雨，当你准备离开时，发现树下的土壤里多了一件装备，看来是雨水冲开了表面让下面的东西露了出来";
                            Data[0] = "EQU";
                            Rare[0] = "ALL";
                            CE_NUIM.EventInfor(true, Data, 1, Rare, true);
                            //     CE_NUIM.EventShow(); 
                            time++;
                            CE_SA.CreateImage(EventText, 0);
                            CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            //一随机装备
                        }
                        else if (RollCount >= Success && RollCount < GreatSuccess)     //成功
                        {
                            EventText = "  得益于这棵大树，你并没有淋到太多雨，冒险可以继续了";
                            time++;
                            CE_SA.CreateImage(EventText, 0);
                            CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            //
                        }
                        else if (RollCount >= Failure && RollCount < Success)     //失败
                        {
                            EventText = "  这场雨下了很久，你不得不在树下待上一整天";
                            time++;
                            CE_SA.CreateImage(EventText, 0);
                            CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            Data[0] = "SAT";
                            Datas[0] = 1;
                            CE_NUIM.EventInfor(false, Data, Datas, 1, true);
                            //   CE_NUIM.EventShow();
                            //饱食度-1
                        }
                        else if (RollCount < Failure)     //大失败
                        {
                            EventText = "  你在树下瑟瑟发抖地蜷缩着，看着周围的树木不断地被巨大的闪电劈成两半，正当你琢磨着为什么这附近的闪电这么多时，一道闪电向你所在的大树劈了过来";

                            time++;
                            CE_SA.CreateImage(EventText, 0);
                            CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            Data[0] = "HP";
                            Datas[0] = 2;
                            CE_NUIM.EventInfor(false, Data, Datas, 1, true);
                            //    CE_NUIM.EventShow();
                            //HP-2
                        }


                    }
                }
            }
        }
    }
}
//↓"怪声"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑
//   怪物种类为夜灵，骰值为30-40间为1只，20-30为2只，20以下为3只带一精英，10以下为一只王，1为3只带1王
public class GuaiSheng
{
    public string Name = "GuaiSheng";              //卡牌名
    public int ButtonsNumber_1 = 2;                //第一次有2个按钮
    public int ButtonsNumber_2 = 1;                //第二次有1个按钮
    public int ButtonsNumber_3 = 1;                //第三次有1个按钮

    public int ButtonsKind_1_1 = 0;               //第一次的按钮种类一为返回
    public int ButtonsKind_1_2 = 1;               //第一次的按钮种类二为掷骰

    public int ButtonsKind_2_1 = 0;               //第二次的按钮种类一为返回
    public int ButtonsKind_2_2 = 2;               //第二次的按钮种类二为战斗，在掷出失败及大失败时出现

    public int ButtonsKind_3_1 = 0;               //第三次的按钮种类二为返回
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public int Success = 35;                              //成功阈值
    public int GreatSuccess = 90;                         //大成功阈值
    public int Failure = 10;                              //失败阈值
    private int EscapeSuccess = 10;
    public string[] Monsters = { "MON_00000003" };  //可能的怪物们
    public string[] ThisMonsters = new string[3];                     //本次需要被创建的怪物
    public int MonsterNumber = 0;
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int RollCount_Escape;                       //出现战斗情况下的逃跑事件随机值
    private int DeviationValue = 0;                        //进行随机事件时的偏差值 
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private Roll CE_Roll;
    private CreateMonster CE_CrMon;
    private BattleSystem CE_BS;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public GuaiSheng()
    {
        //  BigBg = Resources.Load<Sprite>("Sprite/Bg_Normal");
        //  EventBg = Resources.Load<Sprite>("Sprite/Crards_Bg");

        CE_CrMon = GameObject.Find("BG_Battle").GetComponent<CreateMonster>();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_BS = GameObject.Find("BattleObject").GetComponent<BattleSystem>();
        CE_Roll = new Roll();

        CardsEvents.SendName += GS_Event;
        CardsEvents.SendValue += GS_GetValue;
    }
    public void GS_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void GS_Event(string cardName, string eventName, int time)    //本卡牌的事件处理函数
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            //    Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)   //牌子第一次下落时需要实例的按钮和背景
                {
                    EventText = "晚上，你选了一块干燥的空地点起了营火，但附近总有一些烦人的声音干扰你入眠，你可以选择去看看，或者继续失眠";
                    time++;
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, ButtonsKind_1_2, Name, time);

                }
                if (eventName != null)
                {
                    if (time == 2)          //根据第一次的点击，判断牌子第二次下落时需要实例的按钮和背景
                    {
                        if (eventName == "Return")
                        {
                            //饱食度-1     
                            Data[0] = "SAT";
                            Datas[0] = 1;
                            CE_NUIM.EventInfor(false, Data, Datas, 1, true);
                                CE_NUIM.EventShow(true);
                            time = 5;
                        }
                        else if (eventName == "Roll")
                        {
                            RollCount = CE_Roll.NormalEvent(DeviationValue);
                            if (RollCount >= GreatSuccess)              //大成功
                            {
                                EventText = "你找到了声音的来源:几只小地精正合力搬着一件装备，看到你的出现，它们立刻逃掉了";
                                time++;
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);

                                Data[0] = "EQU";
                                Rare[0] = "ALL";
                                CE_NUIM.EventInfor(true, Data, 1, Rare, true);
                                //     CE_NUIM.EventShow();
                                // 获得一随机装备
                            }
                            else if (RollCount >= Success && RollCount < GreatSuccess)  //成功
                            {
                                EventText = "你在周围走了一圈，没有发现任何奇怪的东西，但庆幸的是，已经没有声音干扰你睡觉了";
                                time++;
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                                //
                            }
                            else if (RollCount >= Failure && RollCount < Success)      //失败                      
                            {
                                EventText = "声音就在不远的前方，你握紧了手中的武器，拨开草丛后，一只滑稽怪出现在你眼前，而它显然也发现了你";
                                time++;
                                EscapeSuccess = 20;
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_2, Name, time);
                                //战斗
                            }
                            else if (RollCount < Failure)     //大失败
                            {
                                EventText = "声音就在不远的前方，你握紧了手中的武器，拨开草丛后，数只滑稽怪出现在你眼前，而它们显然也发现了你";
                                time++;
                                EscapeSuccess = 10;
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_2, Name, time);
                                // 战斗
                            }
                        }
                    }
                    else if (time == 3)         //根据第二次的点击，判断牌子第三次下落时需要实例的按钮和背景
                    {
                        if (eventName == "Return")
                        {
                            time = 5;
                        }
                        else if (eventName == "Battle")
                        {
                            if (RollCount >= Failure && RollCount < Success)
                            {
                                MonsterNumber = 1;
                                ThisMonsters[0] = Monsters[0];
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                EscapeSuccess = 20;
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                            else if (RollCount < Failure)
                            {
                                MonsterNumber = 2;
                                ThisMonsters[0] = Monsters[0];
                                ThisMonsters[1] = Monsters[0];
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                EscapeSuccess = 10;
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                        }
                    }
                    if (eventName == "Escape")      //出现战斗情况时，进行的点击判断
                    {
                        RollCount_Escape = CE_Roll.NormalEvent(DeviationValue);
                        if (RollCount < EscapeSuccess)
                        {
                            CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                        }
                        else
                            CE_CrMon.DestroyCards();
                    }
                    else if (eventName == "ComeOn")
                    {
                        CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                    }
                }
            }
        }
    }
}


//↓"猴群"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑
//   怪物种类为猴子，骰值为35-50间为1只，25-35为2只，15以下为3只带一精英，5以下为一只王，1为3只带1王
public class HouQun
{
    public string Name = "HouQun";                  //卡牌名
    public int ButtonsNumber_1 = 2;                 //第一次有2个按钮 
    public int ButtonsNumber_2 = 1;                 //第二次有1个按钮
    public int ButtonsNumber_3 = 1;                 //第三次有1个按钮

    public int ButtonsKind_1_1 = 0;                 //第一次的按钮种类一为返回
    public int ButtonsKind_1_2 = 1;                 //第一次的按钮种类二为掷骰

    public int ButtonsKind_2_1 = 0;                 //第二次的按钮种类二为返回
    public int ButtonsKind_2_2 = 2;                 //第二次的按钮种类二为战斗，在掷出失败及大失败时出现

    public int ButtonsKind_3_1 = 0;                  //第三次的按钮种类一为返回
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public int Success = 50;                              //成功阈值
    public int GreatSuccess = 85;                         //大成功阈值
    public int Failure = 15;                              //失败阈值
    public int GreatFailure = 0;                         //大失败阈值
    private int EscapeSuccess;
    public string[] Monsters = { "MON_00000001", "MON_00000003" };  //可能的怪物们
    public string[] ThisMonsters = new string[3];                     //本次需要被创建的怪物
    public int MonsterNumber = 0;
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int RollCount_Escape;                       //出现战斗情况下的逃跑事件随机值
    private int DeviationValue = 0;                        //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private Roll CE_Roll;
    private CreateMonster CE_CrMon;
    private BattleSystem CE_BS;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public HouQun()
    {
        //    BigBg = Resources.Load<Sprite>("Sprite/Bg_Normal");
        //    EventBg = Resources.Load<Sprite>("Sprite/Crards_Bg");

        CE_BS = GameObject.Find("BattleObject").GetComponent<BattleSystem>();
        CE_CrMon = GameObject.Find("BG_Battle").GetComponent<CreateMonster>();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CE_Roll = new Roll();
        CardsEvents.SendName += HQ_Event;
        CardsEvents.SendValue += HQ_GetValue;
    }
    public void HQ_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void HQ_Event(string cardName, string eventName, int time)
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            //  Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    EventText = "你如往常一样在森林中走着，突然从树杈上出现了一只毛茸茸的手伸向你的背包，你转头看去，发现树上有很多调皮的猴子，你拔出武器向它们挥舞着，希望能够吓跑它们，或者不甘心地认裁";
                    time++;
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, ButtonsKind_1_2, Name, time);
                //    CE_NUIM.EventInfor(false, null, NULL, null, true);

                }
                if (eventName != null)
                {
                    if (time == 2)
                    {
                        if (eventName == "Return")
                        {
                            //随机失去一件背包物品
                            CE_NUIM.EventInfor(false, null, 1, null, true);
                          //   CE_NUIM.EventShow();
                             CE_NUIM.EventShow(true);
                            time = 5;
                        }
                        else if (eventName == "Roll")
                        {
                            time++;
                            RollCount = CE_Roll.NormalEvent(DeviationValue);
                            if (RollCount >= GreatSuccess)   //大成功
                            {
                                EventText = "  它们被吓跑了，除了你的，还留下了另一个可怜人的东西";


                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                                //获得随机一物品
                                Data[0] = "ALL";
                                Rare[0] = "ALL";
                                CE_NUIM.EventInfor(true, Data, 1, Rare, true);
                                //    CE_NUIM.EventShow();
                            }
                            else if (RollCount >= Success && RollCount < GreatSuccess)  //成功
                            {
                                EventText = "  它们被吓跑了，你的失窃物从树上掉了下来";
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }
                            else if (RollCount >= Failure && RollCount < Success)    //失败
                            {
                                EventText = "  你成功的激怒了它们，看来一场战斗不可避免";
                                // 一怪物
                                RollCount_Escape = 40;
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_2, Name, time);
                            }
                            else if (RollCount < Failure)           //大失败
                            {
                                EventText = "  你成功的激怒了它们，看来一场战斗不可避免";
                                // 2怪物
                                RollCount_Escape = 20;
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_2, Name, time);
                            }

                        }
                    }
                    else if (time == 3)
                    {
                        if (eventName == "Return")
                        {
                            time = 5;
                        }
                        else if (eventName == "Battle")
                        {
                            if (RollCount >= Failure && RollCount < Success)
                            {
                                MonsterNumber = 1;
                                ThisMonsters[0] = Monsters[0];
                                CE_SA.CreateImage(BigBg, null, MonsterNumber);
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                            else if (RollCount < Failure)
                            {
                                MonsterNumber = 2;
                                ThisMonsters[0] = Monsters[0];
                                ThisMonsters[1] = Monsters[0];

                                CE_SA.CreateImage(BigBg, null, MonsterNumber);
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }

                        }
                    }
                    if (eventName == "Escape")   //出现战斗情况时，进行的点击判断
                    {
                        RollCount_Escape = CE_Roll.NormalEvent(DeviationValue);
                        if (RollCount < EscapeSuccess)
                        {
                            CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                        }

                        else
                            CE_CrMon.DestroyCards();
                    }
                    else if (eventName == "ComeOn")
                    {
                        CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                    }

                }
            }
        }
    }
}
//↓"急流"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑
public class JiLiu
{
    public string Name = "JiLiu";                      //卡牌名
    public int ButtonsNumber_1 = 2;                 //第一次有2个按钮 
    public int ButtonsNumber_2 = 1;                 //第二次有1个按钮

    public int ButtonsKind_1_1 = 0;                 //第一次的按钮种类一为返回
    public int ButtonsKind_1_2 = 1;                 //第一次的按钮种类二为掷骰

    public int ButtonsKind_2_1 = 0;                  //第二次的按钮种类一为返回
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public int Success = 40;                              //成功阈值
    public int GreatSuccess = 90;                         //大成功阈值
    public int Failure = 5;                              //失败阈值

    public string[] Monsters;                        //可能的怪物种类
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int DeviationValue = 0;                        //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private Roll CE_Roll;
    private SignAbove CE_SA;
    private NewButton CE_NewBut;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public JiLiu()
    {
        //  BigBg = Resources.Load<Sprite>("Bg_Normal") ;
        //   EventBg = Resources.Load<Sprite>("Crards_Bg");

        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_Roll = new Roll();
        CardsEvents.SendName += JL_Event;
        CardsEvents.SendValue += JL_GetValue;
    }
    public void JL_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void JL_Event(string cardName, string eventName, int time)
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            //   Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    EventText = "  一条很急的河流挡住了你的去路，你可以冒险趟过它，或者找一条路绕过它";
                    time++;
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, ButtonsKind_1_2, Name, time);
                }
                if (eventName != null)
                {
                    if (time == 2)
                    {
                        if (eventName == "Roll")
                        {
                            time++;
                            RollCount = CE_Roll.NormalEvent(DeviationValue);
                            if (RollCount >= GreatSuccess)   //大成功
                            {
                                EventText = "  你成功地趟过了河，还顺手拿了一件从上游飘下来的装备";
                                //获得随机一装备   
                                Data[0] = "EQU";
                                Rare[0] = "ALL";
                                CE_NUIM.EventInfor(true, Data, 1, Rare, true);
                                //   CE_NUIM.EventShow();

                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }
                            else if (RollCount >= Success && RollCount < GreatSuccess)  //成功
                            {
                                EventText = "  你成功的的趟过了河";
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }
                            else if (RollCount >= Failure && RollCount < Success)    //失败
                            {
                                EventText = "  在趟河时，你脚滑摔了一跤，庆幸的是你被冲到了一块大石头上，但你的背包显然没那么幸运";

                                // 随机失去一背包物品
                                CE_NUIM.EventInfor(false, null, 1, null, true);
                                //   CE_NUIM.EventShow();

                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }
                            else if (RollCount < Failure)           //大失败
                            {
                                EventText = "  在趟河时，你脚滑摔了一跤，然后就被无情地冲走了";
                                // HP-2 饱食度-2
                                Data[0] = "HP";
                                Data[1] = "SAT";
                                Datas[0] = 2;
                                Datas[1] = 2;

                                CE_NUIM.EventInfor(false, Data, Datas, 2, true);
                                //   CE_NUIM.EventShow();

                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }


                        }
                    }
                }
            }
        }
    }
}
//↓"巨蟒"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑
//   怪物种类为蛇，为一精英，大失败为一王
public class JuMang
{
    public string Name = "JuMang";                      //卡牌名
    public int ButtonsNumber_1 = 3;                 //第一次有3个按钮 
    public int ButtonsNumber_2 = 1;                 //第二次有1个按钮
    public int ButtonsNumber_3 = 1;                 //第三次有1个按钮，仅在第二次出现战斗时进入第三次

    public int ButtonsKind_1_1 = 0;                 //第一次的按钮种类一为返回
    public int ButtonsKind_1_2 = 1;                 //第一次的按钮种类二为掷骰
    public int ButtonsKind_1_3 = 2;                 //第一次的按钮种类三为战斗

    public int ButtonsKind_2_1 = 0;                  //第二次的按钮种类一为返回
    public int ButtonsKind_2_2 = 2;                 //第二次的按钮种类二为战斗，在掷出大失败时出现

    public int ButtonsKind_3_1 = 0;                  //第三次的按钮种类一为返回
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public int Success = 55;                              //成功阈值
    public int GreatSuccess = 95;                         //大成功阈值
    public int Failure = 10;                              //失败阈值
    public int GreatFailure = 15;                         //大失败阈值
    private int EscapeSuccess = 40;
    public string[] Monsters = { "MON_00000001", "MON_00000002" };  //可能的怪物们
    public string[] ThisMonsters = new string[3];                     //本次需要被创建的怪物
    public int MonsterNumber = 0;
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int RollCount_Escape;                       //出现战斗情况下的逃跑事件随机值
    private int DeviationValue = 0;                       //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private Roll CE_Roll;
    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private CreateMonster CE_CrMon;
    private BattleSystem CE_BS;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public JuMang()
    {
        //  BigBg = Resources.Load<Sprite>("Bg_Normal") ;
        //  EventBg = Resources.Load<Sprite>("Crards_Bg") ;

        CE_CrMon = GameObject.Find("BG_Battle").GetComponent<CreateMonster>();
        CE_Roll = new Roll();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CE_BS = GameObject.Find("BattleObject").GetComponent<BattleSystem>();
        CardsEvents.SendName += JM_Event;
        CardsEvents.SendValue += JM_GetValue;
    }
    public void JM_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void JM_Event(string cardName, string eventName, int time)
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            //     Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    EventText = "  你拨开面前繁密的树叶，感觉手触碰到了什么滑溜溜的东西，小心地拨开全部树叶后，才发现那是一条巨大的蛇，它现在正在堆满了尸骨的窝里睡觉，你可以离开，或者试试运气，或直接用武器叫醒它";
                    time++;
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, ButtonsKind_1_2, ButtonsKind_1_3, Name, time);

                }
                if (eventName != null)
                {
                    if (time == 2)
                    {
                        if (eventName == "Return")
                        {
                            time = 5;
                        }
                        else if (eventName == "Roll")
                        {
                            time++;
                            RollCount = CE_Roll.NormalEvent(DeviationValue);
                            if (RollCount >= GreatSuccess) //大成功
                            {
                                EventText = "  你提起胆子，走到了它旁边向窝里摸去，整个过程没有发出一点声音";

                                Data[0] = "EQU";
                                Data[1] = "EQU";
                                Rare[0] = "ALL";
                                Rare[1] = "ALL";
                                CE_NUIM.EventInfor(true, Data, 2, Rare, true);
                                //   CE_NUIM.EventShow();
                                //随机二装备

                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }
                            else if (RollCount >= Success && RollCount < GreatSuccess)  //成功
                            {
                                EventText = "  你提起胆子，走到了它旁边向窝里摸去，整个过程没有发出一点声音";

                                Data[0] = "EQU";
                                Rare[0] = "ALL";
                                CE_NUIM.EventInfor(true, Data, 1, Rare, true);
                                //    CE_NUIM.EventShow();
                                //随机一装备
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }
                            else if (RollCount >= Failure && RollCount < Success)    //失败
                            {
                                EventText = "  你提起胆子，走到了它旁边，可正当你准备伸手时，你和一对满溢着怒火的蛇眼对视了";
                                // 一怪物
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_2, Name, time);
                            }
                            else if (RollCount < Failure)           //大失败
                            {
                                EventText = "  你提起胆子，走到了它旁边，可正当你准备伸手时，你和一对满溢着怒火的蛇眼对视了";
                                // 二怪物
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_2, Name, time);
                            }

                        }
                        else if (eventName == "Battle")
                        {
                            MonsterNumber = 1;
                            ThisMonsters[0] = Monsters[1];
                            CE_SA.CreateImage(BigBg, null, MonsterNumber);
                            CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                            CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                        }

                    }
                    else if (time == 3)
                    {
                        if (eventName == "Return")
                        {
                            time = 5;
                        }
                        else if (eventName == "Battle")
                        {

                            if (RollCount >= Failure && RollCount < Success)
                            {
                                MonsterNumber = 1;
                                ThisMonsters[0] = Monsters[1];
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                EscapeSuccess = 25;
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                            else if (RollCount < Failure)
                            {
                                MonsterNumber = 2;
                                ThisMonsters[0] = Monsters[1];
                                ThisMonsters[1] = Monsters[0];
                                EscapeSuccess = 8;
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                        }
                    }
                    if (eventName == "Escape")       //出现战斗情况时，进行的点击判断
                    {
                        RollCount_Escape = CE_Roll.NormalEvent(DeviationValue);
                        if (RollCount < EscapeSuccess)
                        {
                            CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                        }
                        else
                            CE_CrMon.DestroyCards();
                    }
                    else if (eventName == "ComeOn")
                    {
                        CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                    }

                }
            }
        }
    }
}
//↓"狼群"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑
//    常规战斗为2只，失败为3只，大失败为3只带一精英，小于5为三只带1王
public class LangQun
{
    public string Name = "LangQun";                      //卡牌名
    public int ButtonsNumber_1 = 2;                 //第一次有2个按钮 
    public int ButtonsNumber_2 = 1;                 //第二次有1个按钮
    public int ButtonsNumber_3 = 1;                 //第三次有1个按钮，仅在第二次出现战斗时进入第三次

    public int ButtonsKind_1_1 = 1;                 //第一次的按钮种类一为掷骰
    public int ButtonsKind_1_2 = 2;                 //第一次的按钮种类二为战斗

    public int ButtonsKind_2_1 = 0;                  //第二次的按钮种类一为返回，仅在掷出大成功时出现
    public int ButtonsKind_2_2 = 2;                 //第二次的按钮种类二为战斗

    public int ButtonsKind_3_1 = 0;                  //第三次的按钮种类一为返回
    public string Text;                              //事件文本
    public Texture2D BgTexture;                      //贴图纹理
    public Sprite BgPicture;                         //事件背景
    public int Success = 65;                              //成功阈值
    public int GreatSuccess;                         //大成功阈值
    public int Failure = 15;                              //失败阈值
    public int GreatFailure = 20;                         //大失败阈值
    private int EscapeSuccess = 40;
    public string[] Monsters = { "MON_00000002" };  //可能的怪物们
    public string[] ThisMonsters = new string[3];                     //本次需要被创建的怪物
    public int MonsterNumber = 0;
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int RollCount_Escape;                       //出现战斗情况下的逃跑事件随机值
    private int DeviationValue = 0;                      //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private Roll CE_Roll;
    private CreateMonster CE_CrMon;
    private BattleSystem CE_BS;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public LangQun()
    {
        //   BigBg = Resources.Load<Sprite>("Bg_Normal") ;
        //    EventBg = Resources.Load<Sprite>("Crards_Bg");

        CE_CrMon = GameObject.Find("BG_Battle").GetComponent<CreateMonster>();
        CE_Roll = new Roll();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CE_BS = GameObject.Find("BattleObject").GetComponent<BattleSystem>();
        CardsEvents.SendName += LQ_Event;
        CardsEvents.SendValue += LQ_GetValue;
    }
    public void LQ_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void LQ_Event(string cardName, string eventName, int time)
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            //     Debug.Log("This Time is " + time + " and EventName is " + eventName);
            EventText = "  晚上，你在火边享用着晚餐，突然听到了数声狼嚎，这时你才感觉到周围那些不善的视线，你可以祈祷它们并不是很饿，或者直接抄起武器";
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    time++;
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, ButtonsKind_1_2, Name, time);

                }
                if (eventName != null)
                {
                    if (time == 2)
                    {
                        if (eventName == "Roll")
                        {
                            time++;
                            RollCount = CE_Roll.NormalEvent(DeviationValue);
                            if (RollCount >= Success)
                            {
                                EventText = "  过了一会儿，你听到了脚步声渐渐远去的声音，喘了一口气后，你坐了下来继续吃着晚餐";
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }
                            else if (RollCount < Success)
                            {
                                EventText = "  很倒霉，它们看起来正为了晚餐发愁";
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_2, Name, time);
                            }


                        }
                        if (eventName == "Battle")
                        {
                            MonsterNumber = 1;
                            ThisMonsters[0] = Monsters[0];
                            EscapeSuccess = 20;
                            EventText = "";
                            CE_SA.CreateImage(EventText, MonsterNumber);
                            CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                            CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                        }
                    }
                    else if (time == 3)
                    {
                        if (eventName == "Return")
                        {
                            time = 5;
                        }
                        else if (eventName == "Battle")
                        {

                            if (RollCount >= Failure && RollCount < Success)
                            {
                                MonsterNumber = 1;
                                ThisMonsters[0] = Monsters[0];
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                EscapeSuccess = 15;
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                            else if (RollCount < Failure)
                            {
                                MonsterNumber = 2;
                                ThisMonsters[0] = Monsters[0];
                                ThisMonsters[1] = Monsters[0];
                                EscapeSuccess = 5;
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                        }
                    }
                    if (eventName == "Escape")      //出现战斗情况时，进行的点击判断
                    {
                        RollCount_Escape = CE_Roll.NormalEvent(DeviationValue);
                        if (RollCount_Escape < EscapeSuccess)
                        {
                            Debug.Log("This Roll Value is " + RollCount_Escape);
                            CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                        }
                        else
                            CE_CrMon.DestroyCards();
                    }
                    else if (eventName == "ComeOn")
                    {
                        CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                    }

                }
            }
        }
    }
}
//↓"落难之人"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑
//    怪物种类未定。
public class LuoNanZhiRen
{
    public string Name = "LuoNanZhiRen";                      //卡牌名
    public int ButtonsNumber_1 = 2;                 //第一次有2个按钮 
    public int ButtonsNumber_2 = 1;                 //第二次有1个按钮
    public int ButtonsNumber_3 = 1;                 //第三次有1个按钮，仅在第二次出现战斗时进入第三次

    public int ButtonsKind_1_1 = 0;                 //第一次的按钮种类一为返回
    public int ButtonsKind_1_2 = 1;                 //第一次的按钮种类二为掷骰

    public int ButtonsKind_2_1 = 0;                  //第二次的按钮种类一为返回
    public int ButtonsKind_2_2 = 2;                 //第二次的按钮种类二为战斗，仅在掷出大失败时出现

    public int ButtonsKind_3_1 = 0;                  //第三次的按钮种类一为返回
    public string Text;                              //事件文本
    public Texture2D BgTexture;                      //贴图纹理
    public Sprite BgPicture;                         //事件背景
    public int Success = 35;                              //成功阈值
    public int GreatSuccess = 95;                         //大成功阈值
    public int Failure = 10;                              //失败阈值
    public int GreatFailure = 10;                         //大失败阈值
    private int EscapeSuccess = 40;
    public string[] Monsters = { "MON_00000001", "MON_00000003" };  //可能的怪物们
    public string[] ThisMonsters = new string[3];                     //本次需要被创建的怪物
    public int MonsterNumber = 0;
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int RollCount_Escape;                       //出现战斗情况下的逃跑事件随机值
    private int DeviationValue = 0;                       //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private Roll CE_Roll;
    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private CreateMonster CE_CrMon;
    private BattleSystem CE_BS;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public LuoNanZhiRen()
    {
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");

        CE_CrMon = GameObject.Find("BG_Battle").GetComponent<CreateMonster>();
        CE_Roll = new Roll();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_BS = GameObject.Find("BattleObject").GetComponent<BattleSystem>();
        CardsEvents.SendName += LNZR_Event;
        CardsEvents.SendValue += LNZR_GetValue;
    }
    public void LNZR_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void LNZR_Event(string cardName, string eventName, int time)
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            //    Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    EventText = "  你听到不远处有人的呼救声，这可能是个陷阱，或真的是有人遇到了麻烦";
                    time++;
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, ButtonsKind_1_2, Name, time);

                }
                if (eventName != null)
                {
                    if (time == 2)
                    {
                        if (eventName == "Return")
                        {
                            time = 4;
                        }
                        else if (eventName == "Roll")
                        {
                            time++;
                            RollCount = CE_Roll.NormalEvent(DeviationValue);

                            if (RollCount >= GreatSuccess) //大成功
                            {
                                EventText = "  你果然发现了一位陷入危险的商人，帮他赶跑了怪物后，他十分感激地送了你一些东西";

                                Data[0] = "FOO";
                                Data[1] = "FOO";
                                Data[2] = "EQU";
                                Rare[0] = "ALL";
                                Rare[1] = "ALL";
                                Rare[2] = "ALL";
                                CE_NUIM.EventInfor(true, Data, 3, Rare, true);
                                //    CE_NUIM.EventShow();
                                //随机二装备
                                //随机两个食物+一装备
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }
                            else if (RollCount >= Success && RollCount < GreatSuccess)  //成功
                            {
                                EventText = "  你果然发现了一位陷入危险的冒险者，帮他赶跑了怪物后，他十分感激地送了你一些东西";
                                Data[0] = "FOO";
                                Rare[0] = "ALL";
                                CE_NUIM.EventInfor(true, Data, 1, Rare, true);
                                //     CE_NUIM.EventShow();
                                //随机一个食物
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }
                            else if (RollCount < Success)    //失败
                            {
                                EventText = "  你找到了声音的来源:一伙地精，其中一个正模仿人的呼救声，显然他的伙伴看向你的目光并没有任何善意";
                                // 
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_2, Name, time);
                            }
                        }
                    }
                    else if (time == 3)
                    {
                        if (eventName == "Return")
                        {
                            time = 4;
                        }
                        else if (eventName == "Battle")
                        {

                            if (RollCount >= Failure && RollCount < Success)
                            {
                                MonsterNumber = 1;
                                ThisMonsters[0] = Monsters[0];
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                EscapeSuccess = 25;
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                            else if (RollCount < Failure)
                            {
                                MonsterNumber = 2;
                                ThisMonsters[0] = Monsters[0];
                                ThisMonsters[1] = Monsters[1];
                                EscapeSuccess = 15;
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                        }
                    }
                    if (eventName == "Escape")      //出现战斗情况时，进行的点击判断
                    {
                        RollCount_Escape = CE_Roll.NormalEvent(DeviationValue);
                        if (RollCount < EscapeSuccess)
                        {
                            CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                        }
                        else
                            CE_CrMon.DestroyCards();
                    }
                    else if (eventName == "ComeOn")
                    {
                        CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                    }
                }
            }
        }
    }
}
//↓"兽穴"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑
//    怪物种类未定
public class ShouXue
{
    public string Name = "ShouXue";                      //卡牌名
    public int ButtonsNumber_1 = 2;                 //第一次有2个按钮 
    public int ButtonsNumber_2 = 1;                 //第二次有1个按钮
    public int ButtonsNumber_3 = 1;                 //第三次有1个按钮，仅在第二次出现战斗时进入第三次

    public int ButtonsKind_1_1 = 0;                 //第一次的按钮种类一为返回
    public int ButtonsKind_1_2 = 1;                 //第一次的按钮种类二为掷骰

    public int ButtonsKind_2_1 = 0;                  //第二次的按钮种类一为返回
    public int ButtonsKind_2_2 = 2;                 //第二次的按钮种类二为战斗，在掷出失败或大失败时出现

    public int ButtonsKind_3_1 = 0;                  //第三次的按钮种类一为返回
    public string Text;                              //事件文本
    public Texture2D BgTexture;                      //贴图纹理
    public Sprite BgPicture;                         //事件背景
    public int Success = 50;                              //成功阈值
    public int GreatSuccess = 95;                         //大成功阈值
    public int Failure = 10;                              //失败阈值
    public int GreatFailure = 20;                         //大失败阈值
    private int EscapeSuccess = 40;
    public string[] Monsters = { "MON_00000001", "MON_00000002" };  //可能的怪物们
    public string[] ThisMonsters = new string[3];                     //本次需要被创建的怪物
    public int MonsterNumber = 0;
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int RollCount_Escape;                       //出现战斗情况下的逃跑事件随机值
    private int DeviationValue = 0;                      //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private Roll CE_Roll;
    private CreateMonster CE_CrMon;
    private BattleSystem CE_BS;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public ShouXue()
    {
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");

        CE_CrMon = GameObject.Find("BG_Battle").GetComponent<CreateMonster>();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CE_BS = GameObject.Find("BattleObject").GetComponent<BattleSystem>();
        CE_Roll = new Roll();
        CardsEvents.SendName += SX_Event;
        CardsEvents.SendValue += SX_GetValue;
    }
    public void SX_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void SX_Event(string cardName, string eventName, int time)
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            //   Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    EventText = "  走着走着，你突然闻到一股恶臭，顺着味道走过去，你发现了一只猛兽，它刚刚巢穴去觅食，也许它的巢里有一些宝贝，但前提是你不会被发现";
                    time++;
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, ButtonsKind_1_2, Name, time);

                }
                if (eventName != null)
                {
                    if (time == 2)
                    {
                        if (eventName == "Return")
                        {
                            time = 4;
                        }
                        else if (eventName == "Roll")
                        {
                            time++;
                            RollCount = CE_Roll.NormalEvent(DeviationValue);

                            if (RollCount >= GreatSuccess) //大成功
                            {
                                EventText = "  巢里堆满了战利品，你拿了几件，在主人回来之前离开了";

                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);

                                Data[0] = "EQU";
                                Data[1] = "EQU";
                                Rare[0] = "ALL";
                                Rare[1] = "ALL";
                                CE_NUIM.EventInfor(true, Data, 2, Rare, true);
                                //    CE_NUIM.EventShow();
                                // 随机二装备
                            }
                            else if (RollCount >= Success && RollCount < GreatSuccess)  //成功
                            {
                                EventText = "  巢里堆满了战利品，你拿了几件，在主人回来之前离开了";

                                Data[0] = "EQU";
                                Rare[0] = "ALL";
                                CE_NUIM.EventInfor(true, Data, 1, Rare, true);
                                //    CE_NUIM.EventShow();
                                // 随机一装备
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }
                            else if (RollCount < Success)    //失败
                            {
                                EventText = "  你搜刮了一番，正准备离开时，闻到了身后传来的一股恶臭";
                                // 
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_2, Name, time);
                            }
                        }
                    }
                    else if (time == 3)
                    {
                        if (eventName == "Return")
                        {
                            time = 4;
                        }
                        else if (eventName == "Battle")
                        {

                            if (RollCount >= Failure && RollCount < Success)
                            {
                                MonsterNumber = 1;
                                ThisMonsters[0] = Monsters[0];
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                EscapeSuccess = 15;
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                            else if (RollCount < Failure)
                            {
                                MonsterNumber = 2;
                                ThisMonsters[0] = Monsters[0];
                                ThisMonsters[1] = Monsters[1];
                                EscapeSuccess = 5;
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                        }
                    }
                    if (eventName == "Escape")      //出现战斗情况时，进行的点击判断
                    {
                        RollCount_Escape = CE_Roll.NormalEvent(DeviationValue);
                        if (RollCount < EscapeSuccess)
                        {
                            CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                        }
                        else
                            CE_CrMon.DestroyCards();
                    }
                    else if (eventName == "ComeOn")
                    {
                        CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                    }
                }
            }
        }
    }
}
//↓"树阵"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑
public class ShuZhen
{
    public string Name = "ShuZhen";                      //卡牌名
    public int ButtonsNumber_1 = 1;                 //第一次有2个按钮 
    public int ButtonsNumber_2 = 1;                 //第二次有1个按钮
    public int ButtonsNumber_3 = 1;                 //第三次有1个按钮
    public int ButtonsNumber_4 = 1;                 //第三次有1个按钮

    public int ButtonsKind_1_1 = 1;                 //第一次的按钮种类一为掷骰

    public int ButtonsKind_2_1 = 1;                  //第二次的按钮种类一为掷骰
    public int ButtonsKind_2_2 = 0;                  //第二次的按钮种类二为返回，仅在第一次掷出大成功时出现

    public int ButtonsKind_3_1 = 1;                  //第三次的按钮种类一为掷骰
    public int ButtonsKind_3_2 = 0;                  //第三次的按钮种类二为返回，仅在第二次掷出成功时出现

    public int ButtonsKind_4_1 = 0;                  //第四次的按钮种类一为返回，在第三次掷出成功，或失败时出现
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public int Success = 55;                              //成功阈值
    public int GreatSuccess = 80;                         //大成功阈值
    public int Failure;                              //失败阈值
    public int GreatFailure;                         //大失败阈值
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int DeviationValue = 0;                       //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private Roll CE_Roll;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public ShuZhen()
    {
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");
        CE_Roll = new Roll();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CardsEvents.SendName += SZ_Event;
        CardsEvents.SendValue += SZ_GetValue;
    }
    public void SZ_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void SZ_Event(string cardName, string eventName, int time)
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            //   Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    EventText = "  今天早上你一如既往地在丛林中走着，可是附近的环境总是让你觉得非常熟悉，于是你用小刀在一棵树上刻了个记号";
                    time++;
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, Name, time);

                }
                if (eventName != null)
                {
                    if (time == 2)
                    {
                        if (eventName == "Roll")
                        {
                            time++;
                            EventText = "  拨开眼前的树叶走出灌木丛后，你看到了不久前的那个记号，于是你换了一条路走";

                            RollCount = CE_Roll.NormalEvent(DeviationValue);
                            if (RollCount >= GreatSuccess)
                            {
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_2, Name, time);
                            }
                            else
                            {
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }

                        }
                    }
                    else if (time == 3)
                    {
                        if (eventName == "Return")
                        {
                            time = 5;
                        }
                        else if (eventName == "Roll")
                        {
                            time++;
                            EventText = "  很显然，这个记号就是你最开始刻下的，你已经忘了自己究竟走过了哪几条路，于是索性闭着眼睛选了一条";
                            RollCount = CE_Roll.NormalEvent(DeviationValue);
                            if (RollCount >= Success)
                            {
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_3, ButtonsKind_3_2, Name, time);
                            }
                            else
                            {
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_3, ButtonsKind_3_1, Name, time);
                            }

                        }
                    }
                    else if (time == 4)
                    {
                        if (eventName == "Return")
                        {
                            time = 4;
                        }
                        else if (eventName == "Roll")
                        {
                            time++;
                            EventText = "  太阳已经下山，你筋疲力尽地躺在了那棵做了记号的树下，甚至连举起双臂的力气都没了";

                            Data[0] = "SAT";
                            Datas[0] = 2;
                            CE_NUIM.EventInfor(false, Data, Datas, 1, true);
                            //  CE_NUIM.Invoke("EventShow", 1f);
                            //  饱食度-2;
                            RollCount = CE_Roll.NormalEvent(DeviationValue);
                            CE_SA.CreateImage(EventText, 0);
                            CE_NewBut.GetButtonsProperty(ButtonsNumber_4, ButtonsKind_4_1, Name, time);

                        }
                    }
                }
            }
        }
    }
}
//↓"死尸"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑
public class SiShi
{
    public string Name = "SiShi";                      //卡牌名
    public int ButtonsNumber_1 = 2;                 //第一次有2个按钮 
    public int ButtonsNumber_2 = 1;                 //第二次有1个按钮
    public int ButtonsNumber_3 = 1;                 //第三次有1个按钮，仅在第二次出现战斗时进入第三次

    public int ButtonsKind_1_1 = 0;                 //第一次的按钮种类一为返回
    public int ButtonsKind_1_2 = 1;                 //第一次的按钮种类二为掷骰

    public int ButtonsKind_2_1 = 0;                  //第二次的按钮种类一为返回
    public int ButtonsKind_2_2 = 2;                 //第二次的按钮种类二为战斗，在掷出失败或大失败时出现

    public int ButtonsKind_3_1 = 0;                  //第三次的按钮种类一为返回
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public int Success = 45;                              //成功阈值
    public int GreatSuccess = 95;                         //大成功阈值
    public int Failure = 10;                              //失败阈值
    public int GreatFailure = 10;                         //大失败阈值
    private int EscapeSuccess = 40;
    public string[] Monsters = { "MON_00000001", "MON_00000002" };  //可能的怪物们
    public string[] ThisMonsters = new string[3];                     //本次需要被创建的怪物
    public int MonsterNumber = 0;
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int RollCount_Escape;                       //出现战斗情况下的逃跑事件随机值
    private int DeviationValue = 0;                        //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private Roll CE_Roll;
    private CreateMonster CE_CrMon;
    private BattleSystem CE_BS;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public SiShi()
    {
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");

        CE_CrMon = GameObject.Find("BG_Battle").GetComponent<CreateMonster>();
        CE_Roll = new Roll();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CE_BS = GameObject.Find("BattleObject").GetComponent<BattleSystem>();
        CardsEvents.SendName += SS_Event;
        CardsEvents.SendValue += SS_GetValue;
    }
    public void SS_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void SS_Event(string cardName, string eventName, int time)
    {
        if (cardName == Name)
        {
            if (cardName == "Over")
            {
                time = 0;
            }
            //  Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    EventText = "  你发现了一具刚死去不久的尸体，看起来像是被什么东西啃死的，这很恶心，但也许他身上有什么你可以用到的东西";
                    time++;
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, ButtonsKind_1_2, Name, time);

                }
                if (eventName != null)
                {
                    if (time == 2)
                    {
                        if (eventName == "Return")
                        {
                            time = 4;
                        }
                        else if (eventName == "Roll")
                        {
                            time++;
                            RollCount = CE_Roll.NormalEvent(DeviationValue);


                            if (RollCount >= GreatSuccess) //大成功
                            {
                                EventText = "  你埋葬了他，顺便从他的身上和背包里拿了些东西，”留在这里反正都会坏掉，还是跟着我走吧。”你这样安慰着自己";


                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                                Data[0] = "FOO";
                                Data[1] = "FOO";
                                Data[2] = "EQU";
                                Rare[0] = "ALL";
                                Rare[1] = "ALL";
                                Rare[2] = "ALL";
                                CE_NUIM.EventInfor(true, Data, 3, Rare, true);
                                //   CE_NUIM.EventShow();

                                //随机2食物 1装备
                            }
                            else if (RollCount >= Success && RollCount < GreatSuccess)  //成功
                            {
                                EventText = "  你埋葬了他，但他身上已经没有什么可用的了，你只好搜了一遍他的背包";


                                //随机1食物
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);

                                Data[0] = "FOO";
                                Rare[0] = "ALL";
                                CE_NUIM.EventInfor(true, Data, 1, Rare, true);
                                //    CE_NUIM.EventShow();
                            }
                            else if (RollCount >= Failure && RollCount < Success)    //失败
                            {
                                EventText = "  你正准备埋葬他时，感觉到身后射来了一道不善的目光，转过头，一只怪物正紧盯着你";
                                // 一怪物
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_2, Name, time);
                            }
                            else if (RollCount < Failure)           //大失败
                            {
                                EventText = "  你正准备埋葬他时，感觉到身后射来了数道不善的目光，转过头，一只怪物正紧盯着你";
                                // 二怪物
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_2, Name, time);
                            }
                        }
                    }
                    else if (time == 3)
                    {
                        if (eventName == "Return")
                        {
                            time = 4;
                        }
                        else if (eventName == "Battle")
                        {
                            if (RollCount >= Failure && RollCount < Success)
                            {
                                MonsterNumber = 1;
                                ThisMonsters[0] = Monsters[0];
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                EscapeSuccess = 25;
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                            else if (RollCount < Failure)
                            {
                                MonsterNumber = 2;
                                ThisMonsters[0] = Monsters[0];
                                ThisMonsters[1] = Monsters[1];
                                EscapeSuccess = 10;
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                        }
                    }
                    if (eventName == "Escape")      //出现战斗情况时，进行的点击判断
                    {
                        RollCount_Escape = CE_Roll.NormalEvent(DeviationValue);
                        if (RollCount < EscapeSuccess)
                        {
                            CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                        }
                        else
                            CE_CrMon.DestroyCards();
                    }
                    else if (eventName == "ComeOn")
                    {
                        CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                    }
                }
            }
        }
    }
}
//↓"陷阱"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑
public class XianJing
{
    public string Name = "XianJing";                      //卡牌名
    public int ButtonsNumber_1 = 1;                 //第一次有1个按钮 
    public int ButtonsNumber_2 = 1;                 //第二次有1个按钮

    public int ButtonsKind_1_1 = 1;                 //第一次的按钮种类一为掷骰

    public int ButtonsKind_2_1 = 0;                  //第二次的按钮种类一为返回
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public int Success = 50;                              //成功阈值
    public int GreatSuccess = 90;                         //大成功阈值
    public int Failure = 10;                              //失败阈值
    public int GreatFailure;                         //大失败阈值
    private Sprite BigBg;
    private Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int DeviationValue = 0;                      //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private Roll CE_Roll;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public XianJing()
    {
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");
        CE_Roll = new Roll();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CardsEvents.SendName += XJ_Event;
        CardsEvents.SendValue += XJ_GetValue;
    }
    public void XJ_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void XJ_Event(string cardName, string eventName, int time)
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            //    Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    EventText = "  你听说有一些怪物会在地上挖出捕猎食物的陷阱，但当时你只把那当成一个笑话，很可惜，现在你快要变成食物了";
                    time++;
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, Name, time);

                }
                if (eventName != null)
                {
                    if (time == 2)
                    {
                        if (eventName == "Roll")
                        {
                            time++;
                            RollCount = CE_Roll.NormalEvent(DeviationValue);

                            if (RollCount >= GreatSuccess) //大成功
                            {
                                EventText = " 你掉了下去，刚好错开了从地面伸出的木刺，揉了揉屁股后，你看见一只刚死去不久的小鹿";

                                Data[0] = "FOO";
                                Data[1] = "FOO";
                                Rare[0] = "ALL";
                                Rare[1] = "ALL";
                                CE_NUIM.EventInfor(true, Data, 2, Rare, true);
                                //    CE_NUIM.EventShow();

                                //随机2食物
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }
                            else if (RollCount >= Success && RollCount < GreatSuccess)  //成功
                            {
                                EventText = "  你掉了下去，刚好错开了从地面伸出的木刺，庆幸的是，这个陷阱并不算很深，你可以很轻松地爬出去";

                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }
                            else if (RollCount >= Failure && RollCount < Success)    //失败
                            {
                                EventText = "  你掉了下去，刚好错开了从地面伸出的木刺，但怎么从这里出去仍旧是个问题";

                                Data[0] = "SAT";
                                Datas[0] = 1;
                                CE_NUIM.EventInfor(false, Data, Datas, 1, true);
                                //     CE_NUIM.EventShow();
                                //  HP-1;
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }
                            else if (RollCount < Failure)           //大失败
                            {
                                EventText = "  你掉了下去，尽管已经在空中尽力扭着身子，但还是有一根木刺刺中了你，你双眼一黑，晕了过去";
                                //   HP-3;
                                Data[0] = "HP";
                                Datas[0] = 3;
                                CE_NUIM.EventInfor(false, Data, Datas, 1, true);
                                //    CE_NUIM.EventShow();

                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                            }
                        }
                    }
                }
            }
        }
    }
}
//↓"悬崖"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑
public class XuanYa
{
    public string Name = "XuanYa";                      //卡牌名
    public int ButtonsNumber_1 = 1;                 //第一次有1个按钮 
    public int ButtonsNumber_2 = 1;                 //第二次有1个按钮

    public int ButtonsKind_1_1 = 1;                 //第一次的按钮种类一为掷骰

    public int ButtonsKind_2_1 = 0;                  //第二次的按钮种类一为返回
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public int Success = 50;                              //成功阈值
    public int GreatSuccess = 90;                         //大成功阈值
    public int Failure = 10;                              //失败阈值
    public int GreatFailure;                         //大失败阈值
    public string[] Monsters;                        //可能的怪物种类
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int DeviationValue = 0;                       //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private Roll CE_Roll;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public XuanYa()
    {
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");
        CE_Roll = new Roll();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CardsEvents.SendName += XY_Event;

        CardsEvents.SendValue += XY_GetValue;
    }
    public void XY_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void XY_Event(string cardName, string eventName, int time)
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            //    Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    EventText = "  每个老冒险者都曾说过”注意脚下”，但你显然没听进去，不然也不会像现在一样吊在一处悬崖边上";
                    time++;
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, Name, time);

                }
                if (eventName != null)
                {
                    if (time == 2)
                    {
                        time++;

                        RollCount = CE_Roll.NormalEvent(DeviationValue);

                        if (RollCount >= GreatSuccess) //大成功
                        {
                            EventText = "  最终，你撑不下去了，在你下落的途中，一只翼龙刚好出现在你身下，你紧紧地抱住了它落到了地面，但你对它的感谢却是将它做成了一顿美餐";
                            Data[0] = "FOO";
                            Data[1] = "FOO";
                            Rare[0] = "ALL";
                            Rare[1] = "ALL";
                            CE_NUIM.EventInfor(true, Data, 2, Rare, true);
                            //   CE_NUIM.EventShow();
                            //随机2食物
                            CE_SA.CreateImage(EventText, 0);
                            CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                        }
                        else if (RollCount >= Success && RollCount < GreatSuccess)  //成功
                        {
                            EventText = "  “最后一搏！”你这么想着，脚下一用力向上跳去，双手在空中胡乱挥舞着，刚好抓住了一根树藤，慢慢地爬了上去";

                            CE_SA.CreateImage(EventText, 0);
                            CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                        }
                        else if (RollCount >= Failure && RollCount < Success)    //失败
                        {
                            EventText = "  “最后一搏！”你这么想着，脚下一用力向上跳去，但距离上面实在太远，你还是掉了下去。不过在你下落的途中有不少树木，你很幸运地没怎么受伤";
                            //  HP-1;

                            Data[0] = "HP";
                            Datas[0] = 1;
                            CE_NUIM.EventInfor(false, Data, Datas, 1, true);
                            //   CE_NUIM.EventShow();
                            //  HP-1;
                            CE_SA.CreateImage(EventText, 0);
                            CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                        }
                        else if (RollCount < Failure)           //大失败
                        {
                            EventText = "  “最后一搏”你这么想着，脚下一用力向上跳去，但距离上面实在太远，你还是掉了下去。尽管在下落的途中有一些树木，你还是受了不少伤";
                            //     HP-3;

                            Data[0] = "HP";
                            Datas[0] = 3;
                            CE_NUIM.EventInfor(false, Data, Datas, 1, true);
                            CE_NUIM.EventShow();

                            CE_SA.CreateImage(EventText, 0);
                            CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                        }
                    }
                }
            }
        }
    }
}
//↓"遗迹"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑
public class YiJi
{
    public string Name = "YiJi";                      //卡牌名
    public int ButtonsNumber_1 = 2;                 //第一次有2个按钮 
    public int ButtonsNumber_2 = 1;                 //第二次有1个按钮
    public int ButtonsNumber_3 = 1;                 //第三次有1个按钮，仅在第二次出现战斗时进入第三次

    public int ButtonsKind_1_1 = 0;                 //第一次的按钮种类一为返回
    public int ButtonsKind_1_2 = 1;                 //第一次的按钮种类二为掷骰

    public int ButtonsKind_2_1 = 0;                  //第二次的按钮种类一为返回
    public int ButtonsKind_2_2 = 2;                 //第二次的按钮种类二为战斗，仅在第一次掷出大失败时出现

    public int ButtonsKind_3_1 = 0;                  //第三次的按钮种类一为返回
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public int Success = 50;                              //成功阈值
    public int GreatSuccess = 95;                         //大成功阈值
    public int Failure = 15;                              //失败阈值
    public int GreatFailure = 15;                         //大失败阈值
    private int EscapeSuccess = 40;
    public string[] Monsters = { "MON_00000001", "MON_00000003" };  //可能的怪物们
    public string[] ThisMonsters = new string[3];                     //本次需要被创建的怪物
    public int MonsterNumber = 0;
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int RollCount_Escape;                       //出现战斗情况下的逃跑事件随机值
    private int DeviationValue = 0;                      //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述
    private bool CanShow = false;

    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private Roll CE_Roll;
    private CreateMonster CE_CrMon;
    private BattleSystem CE_BS;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public YiJi()
    {
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");

        CE_CrMon = GameObject.Find("BG_Battle").GetComponent<CreateMonster>();
        CE_Roll = new Roll();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CE_BS = GameObject.Find("BattleObject").GetComponent<BattleSystem>();
        CardsEvents.SendName += YJ_Event;
        CardsEvents.SendValue += YJ_GetValue;
    }
    public void YJ_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void YJ_Event(string cardName, string eventName, int time)
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            //   Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    EventText = "  你正走在一处被树木掩盖住的古代遗迹中，周围的石雕上的符号你一个都看不懂，再往里走可能会有危险，但对应的也可能会有一些宝贝";
                    time++;
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, ButtonsKind_1_2, Name, time);

                }
                if (eventName != null)
                {
                    if (time == 2)
                    {
                        if (eventName == "Return")
                        {
                            time = 4;
                        }
                        else if (eventName == "Roll")
                        {
                            time++;
                            RollCount = CE_Roll.NormalEvent(DeviationValue);
                            RollCount = 99;

                            if (RollCount >= GreatSuccess) //大成功
                            {
                                EventText = "  在一堆石头中，你看到了什么东西在闪着亮光，刨开石头后，你发现了一些古人遗留的装备";


                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                                Data[0] = "EQU";
                                Data[1] = "EQU";
                                Rare[0] = "ALL";
                                Rare[1] = "ALL";
                                CE_NUIM.EventInfor(true, Data, 2, Rare, true);
                                //   CE_NUIM.EventShow();
                                //随机一装备+ R以上装备*1
                            }
                            else if (RollCount >= Success && RollCount < GreatSuccess)  //成功
                            {
                                EventText = "  古人在离开这里时已经带走了属于他们的东西，你不甘心地继续寻找着，最终还是找到了一些有用的东西";


                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                                Data[0] = "EQU";
                                Rare[0] = "ALL";
                                CE_NUIM.EventInfor(true, Data, 1, Rare, true);
                                //    CE_NUIM.EventShow();
                                // 随机一装备
                            }
                            else if (RollCount >= Failure && RollCount < Success)    //失败
                            {
                                EventText = "  古人在离开这里时已经带走了属于他们的东西，你不甘心地继续寻找着，但显然只是徒劳";

                                //  饱食度-1；
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                                Data[0] = "SAT";
                                Datas[0] = 1;
                                CE_NUIM.EventInfor(false, Data, Datas, 1, true);
                                //       CE_NUIM.EventShow();
                            }
                            else if (RollCount < Failure)           //大失败
                            {
                                EventText = "  这里不仅没有什么宝贝，反而成了怪物们的住处，你的坏运气和贪心让你不可避免地进入了一场战斗";
                                //    进入战斗 2怪
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_2, Name, time);
                            }
                        }
                    }
                    else if (time == 3)
                    {
                        if (eventName == "Return")
                        {
                            time = 4;
                        }
                        else if (eventName == "Battle")
                        {
                            if (RollCount >= Failure && RollCount < Success)
                            {
                                MonsterNumber = 1;
                                ThisMonsters[0] = Monsters[1];
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                EscapeSuccess = 25;
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                            else if (RollCount < Failure)
                            {
                                MonsterNumber = 2;
                                ThisMonsters[0] = Monsters[0];
                                ThisMonsters[1] = Monsters[1];
                                EscapeSuccess = 10;
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                        }
                    }
                    if (eventName == "Escape")      //出现战斗情况时，进行的点击判断
                    {
                        RollCount_Escape = CE_Roll.NormalEvent(DeviationValue);
                        if (RollCount_Escape < EscapeSuccess)
                        {
                            CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                        }
                        else
                            CE_CrMon.DestroyCards();
                    }
                    else if (eventName == "ComeOn")
                    {
                        CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                    }
                }
            }
        }
    }
}
//↓"仪式"的所有信息
//   ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑
//   怪物种类为信徒，第一次战斗时为4怪1精英3普通，
public class YiShi
{
    public string Name = "YiShi";                      //卡牌名
    public int ButtonsNumber_1 = 2;                 //第一次有2个按钮 
    public int ButtonsNumber_2 = 1;                 //第二次有1个按钮
    public int ButtonsNumber_3 = 2;                 //第三次有3个按钮
    public int ButtonsNumber_4_1 = 3;               //第四次第一种情况有3个按钮，在第三次选择继续时出现
    public int ButtonsNumber_4_2 = 2;               //第四次第二种情况有2个按钮，在第三次出现战斗时发现
    public int ButtonsNumber_5_1 = 1;                 //第五次第一种情况有1个按钮
    public int ButtonsNumber_5_2 = 2;                 //第五次第二种情况有2个按钮
    public int ButtonsNumber_6 = 1;                 //第六次有1个按钮，在第五次选择战斗时出现

    public int ButtonsKind_1_1 = 0;                 //第一次的按钮种类一为返回
    public int ButtonsKind_1_2 = 3;                 //第一次的按钮种类二为存在下一事件

    public int ButtonsKind_2_1 = 1;                  //第二次的按钮种类一为掷骰  

    public int ButtonsKind_3_1 = 0;                  //第三次的按钮种类一为返回，在掷出非大失败时出现
    public int ButtonsKind_3_2 = 3;                  //第三次的按钮种类二为存在下一事件，在掷出非大失败时出现
    public int ButtonsKind_3_3 = 2;                  //第三次的按钮种类三为战斗，在掷出大失败时出现

    public int ButtonsKind_4_1 = 0;                  //第四次的按钮种类一为返回，在第三次出现战斗时出现
    public int ButtonsKind_4_2 = 1;                  //第四次的按钮种类二为存在下一事件
    public int ButtonsKind_4_3 = 2;                  //第四次的按钮种类三为战斗

    public int ButtonsKind_5_1 = 0;                  //第五次的按钮种类一为返回
    public int ButtonsKind_5_2 = 3;                  //第五次的按钮种类二为存在下一事件
    public int ButtonsKind_5_3 = 2;                  //第五次的按钮种类三为战斗,在第四次选择下一事件时出现


    public int ButtonsKind_6_1 = 0;                  //第六次的按钮种类一为返回,在第五次选择战斗时出现
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public int Success = 41;                              //成功阈值
    public int GreatSuccess;                         //大成功阈值
    public int Failure = 40;                              //失败阈值
    public int GreatFailure = 15;                         //大失败阈值
    private int EscapeSuccess = 40;
    public string[] Monsters;                        //可能的怪物种类
    public int MonsterNumber = 0;
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount_Escape;                       //出现战斗情况下的逃跑事件随机值
    private int RollCount;                              //普通事件的随机值
    private int DeviationValue = 0;                        //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图

    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private Roll CE_Roll;
    private CreateMonster CE_CrMon;
    private BattleSystem CE_BS;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public YiShi()
    {
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");

        CE_CrMon = GameObject.Find("BG_Battle").GetComponent<CreateMonster>();
        CE_Roll = new Roll();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CE_BS = GameObject.Find("BattleObject").GetComponent<BattleSystem>();
        CardsEvents.SendName += YS_Event;
        CardsEvents.SendValue += YS_GetValue;
    }
    public void YS_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void YS_Event(string cardName, string eventName, int time)
    {

        if (cardName == Name)
        {
            //   Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    time++;
                    //   CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, ButtonsKind_1_2, Name, time);

                }
                if (eventName != null)
                {
                    if (time == 2)
                    {
                        if (eventName == "Next")
                        {
                            time++;
                            //     CE_SA.CreateImage(EventText, 0);
                            CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);

                        }
                    }
                    else if (time == 3)
                    {
                        if (eventName == "Roll")
                        {
                            time++;
                            RollCount = CE_Roll.NormalEvent(DeviationValue);
                            if (RollCount > GreatFailure)
                            {
                                //       CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_3, ButtonsKind_3_1, ButtonsKind_3_2, Name, time);
                            }
                            else if (RollCount <= GreatFailure)
                            {
                                CE_SA.CreateImage(BigBg, EventBg, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_3, ButtonsKind_3_1, ButtonsKind_3_3, Name, time);
                            }
                        }
                    }
                    else if (time == 4)
                    {
                        if (eventName == "Next")
                        {
                            time++;
                            CE_SA.CreateImage(BigBg, EventBg, 0);
                            CE_NewBut.GetButtonsProperty(ButtonsNumber_4_1, ButtonsKind_4_1, ButtonsKind_4_2, ButtonsKind_4_3, Name, time);
                        }
                        else if (eventName == "Battle")
                        {
                            CE_SA.CreateImage(BigBg, EventBg, 0);
                            CE_NewBut.GetButtonsProperty(ButtonsNumber_4_2, ButtonsKind_4_1, ButtonsKind_4_3, Name, time);
                        }
                    }
                    else if (time == 5)
                    {
                        if (eventName == "Battle" && RollCount > GreatFailure)
                        {

                        }
                        else if (eventName == "Roll")
                        {

                        }
                    }
                    if (eventName == "Escape")      //出现战斗情况时，进行的点击判断
                    {
                        RollCount_Escape = CE_Roll.NormalEvent(DeviationValue);
                        if (RollCount <= RollCount_Escape)
                        {
                            CE_BS.CreateBattleScene("Forest", MonsterNumber, null);
                        }
                        else
                            CE_CrMon.DestroyCards();
                    }
                    else if (eventName == "ComeOn")
                    {
                        CE_BS.CreateBattleScene("Forest", MonsterNumber, null);
                    }
                }
            }
        }
    }
}
//↓"沼泽"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑
public class ZhaoZe
{
    public string Name = "ZhaoZe";                      //卡牌名
    public int ButtonsNumber_1 = 1;                 //第一次有2个按钮 
    public int ButtonsNumber_2 = 1;                 //第二次有1个按钮
    public int ButtonsNumber_3 = 1;                 //第三次有1个按钮，在第二次出现战斗时出现

    public int ButtonsKind_1_1 = 1;                 //第一次的按钮种类一为掷骰

    public int ButtonsKind_2_1 = 0;                  //第二次的按钮种类一为返回
    public int ButtonsKind_2_2 = 2;                 //第二次的按钮种类二为战斗,在第一次掷出大失败时出现

    public int ButtonsKind_3_1 = 0;                  //第三次的按钮种类一为返回
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public int Success = 60;                              //成功阈值
    public int GreatSuccess = 95;                         //大成功阈值
    public int Failure = 15;                              //失败阈值
    public int GreatFailure = 90;                         //大失败阈值
    private int EscapeSuccess = 40;             //出现战斗情况下的逃跑事件随机值
    public string[] Monsters = { "MON_00000002", "MON_00000003" };  //可能的怪物们
    public string[] ThisMonsters = new string[3];                     //本次需要被创建的怪物
    public int MonsterNumber = 0;
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int RollCount_Escape;                       //出现战斗情况下的逃跑事件随机值
    private int DeviationValue = 0;                      //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private Roll CE_Roll;
    private CreateMonster CE_CrMon;
    private BattleSystem CE_BS;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public ZhaoZe()
    {
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");

        CE_Roll = new Roll();
        CE_CrMon = GameObject.Find("BG_Battle").GetComponent<CreateMonster>();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CE_BS = GameObject.Find("BattleObject").GetComponent<BattleSystem>();
        CardsEvents.SendName += ZZ_Event;
        CardsEvents.SendValue += ZZ_GetValue;
    }
    public void ZZ_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void ZZ_Event(string cardName, string eventName, int time)
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    EventText = "  天知道为什么树林中会有这么大一片危险的沼泽，你握着武器小心翼翼地迈着脚步，除了要小心脚下，你还要尽力躲开路线上的怪物";
                    time++;
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, Name, time);

                }
                if (eventName != null)
                {
                    if (time == 2)
                    {
                        if (eventName == "Roll")
                        {
                            time++;
                            RollCount = CE_Roll.NormalEvent(DeviationValue);


                            if (RollCount >= GreatSuccess) //大成功
                            {
                                EventText = "  离岸边很近了，你大步流星地淌过淤泥，突然脚碰到了一件硬物，弯腰捡起后，发现那是一件装备";


                                //随机一装备+ R以上装备*1
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                                Data[0] = "EQU";
                                Data[1] = "EQU";
                                Rare[0] = "ALL";
                                Rare[1] = "R";
                                CE_NUIM.EventInfor(true, Data, 2, Rare, true);
                                //   CE_NUIM.EventShow();
                            }
                            else if (RollCount >= Success && RollCount < GreatSuccess)  //成功
                            {
                                EventText = "  你有惊无险地淌过了这一片沼泽，身体和精神的疲惫让你直接躺在了岸边";

                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                             //   Data[0] = "EQU";
                            //    Rare[0] = "ALL";
                           //     CE_NUIM.EventInfor(true, Data, 1, Rare, true);
                                //   CE_NUIM.EventShow();
                                // 随机一装备
                            }
                            else if (RollCount >= Failure && RollCount < Success)    //失败
                            {
                                EventText = "  很不幸，沼泽中的怪物发现了你，现在你不仅要注意脚下，还要注意眼前";
                                //  一怪物
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_2, Name, time);
                            }
                            else if (RollCount < Failure)           //大失败
                            {
                                EventText = "  很不幸，沼泽中的怪物发现了你，现在你不仅要注意脚下，还要注意眼前";
                                //     二怪物
                                CE_SA.CreateImage(EventText, 0);
                                CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_2, Name, time);
                            }
                        }
                    }
                    else if (time == 3)
                    {
                        if (eventName == "Return")
                        {
                            time = 4;
                        }
                        else if (eventName == "Battle")
                        {
                            if (RollCount >= Failure && RollCount < Success)
                            {
                                MonsterNumber = 1;
                                ThisMonsters[0] = Monsters[0];
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                EscapeSuccess = 20;
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                            else if (RollCount < Failure)
                            {
                                MonsterNumber = 2;
                                ThisMonsters[0] = Monsters[0];
                                ThisMonsters[1] = Monsters[1];
                                EscapeSuccess = 10;
                                EventText = "";
                                CE_SA.CreateImage(EventText, MonsterNumber);
                                CE_NewBut.GetButtonsProperty(2, 6, 7, Name, time);
                                CE_CrMon.CanCreateMonster(MonsterNumber, ThisMonsters);
                            }
                        }
                    }
                    if (eventName == "Escape")      //出现战斗情况时，进行的点击判断
                    {
                        RollCount_Escape = CE_Roll.NormalEvent(DeviationValue);
                        if (RollCount < EscapeSuccess)
                        {

                            CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                        }
                        else
                            CE_CrMon.DestroyCards();
                    }
                    else if (eventName == "ComeOn")
                    {
                        CE_BS.CreateBattleScene("Forest", MonsterNumber, ThisMonsters);
                    }
                }
            }
        }
    }
}
//↓"牧师"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得
public class MuShi
{
    public string Name = "MuShi";                      //卡牌名
    public int ButtonsNumber_1 = 1;                 //第一次有1个按钮 
    public int ButtonsNumber_2 = 1;                 //第二次有1个按钮 

    public int ButtonsKind_1_1 = 5;                 //第一次的按钮种类一为获得
    public int ButtonsKind_2_1 = 0;                 //第二次的按钮种类一为返回
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public int Success;                              //成功阈值
    public int GreatSuccess;                         //大成功阈值
    public int Failure;                              //失败阈值
    public int GreatFailure;                         //大失败阈值
    public string[] Monsters;                        //可能的怪物种类
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int DeviationValue = 0;                       //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private Roll CE_Roll;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public MuShi()
    {
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");
        CE_Roll = new Roll();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CardsEvents.SendName += MS_Event;

    }
    public void MS_Event(string cardName, string eventName, int time)
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            //   Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    EventText = "  你碰到了一位正在旅行的老牧师，给他指了路后，他很感激地送了你一些食物";

                    Debug.Log("This is " + Name);
                    time++;
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, Name, time);
                    Data[0] = "FOO";
                    Data[1] = "FOO";
                    Rare[0] = "ALL";
                    Rare[1] = "ALL";
                    CE_NUIM.EventInfor(true, Data, 2, Rare, true);
                    //    CE_NUIM.EventShow();
                    //      随机2食物
                }
                if (eventName != null)
                {
                    if (time == 3)
                    {
                        time++;
                        CE_SA.CreateImage(EventText, 0);
                        CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                    }
                }
            }
        }
    }
}
//↓"商人"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得
public class ShangRen
{
    public string Name = "ShangRen";                      //卡牌名
    public int ButtonsNumber_1 = 3;                 //第一次有3个按钮 
    public int ButtonsNumber_n = 2;                 //第n次有1个按钮，当玩家选择返回后结束商人事件

    public int ButtonsKind_1_1 = 0;                 //第一次的按钮种类一为返回
    public int ButtonsKind_1_2 = 5;                 //第一次的按钮种类二为获得
    public int ButtonsKind_1_3 = 4;                 //第一次的按钮种类二为购买

    public int ButtonsKind_n_1 = 0;                  //第n次的按钮种类一为返回
    public int ButtonsKind_n_2 = 4;                 //第n次的按钮种类二为购买
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public int Success;                              //成功阈值
    public int GreatSuccess;                         //大成功阈值
    public int Failure;                              //失败阈值
    public int GreatFailure;                         //大失败阈值
    public string[] Monsters;                        //可能的怪物种类
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int DeviationValue = 0;                       //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private Roll CE_Roll;
    private SignAbove CE_SA;
    private NewButton CE_NewBut;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public ShangRen()
    {
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");
        CE_Roll = new Roll();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CardsEvents.SendName += SR_Event;

        CardsEvents.SendValue += SR_GetValue;
    }
    public void SR_GetValue(int value)    //得到补正值
    {
        DeviationValue = value;
    }
    public void SR_Event(string cardName, string eventName, int time)
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            //   Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    //   EventText = "  你救了一只生活在树林中的妖精，它很感激地带你来到了它的家中，饱餐一顿后又送了一件它曾在林中捡到的装备";

                    time++;
                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, ButtonsKind_1_2, ButtonsKind_1_3, Name, time);
                }
                if (eventName != null)
                {
                    if (time >= 2)
                    {
                        if (eventName != "Return")
                        {
                            time++;
                            CE_SA.CreateImage(EventText, 0);
                            CE_NewBut.GetButtonsProperty(ButtonsNumber_n, ButtonsKind_n_1, ButtonsKind_n_2, Name, time);
                        }
                        else
                        {
                            time = 0;
                        }
                    }
                }
            }
        }
    }
}
//↓"土地神"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑,8为进入下一张地图
public class TuDiShen
{
    public string Name = "TuDiShen";                      //卡牌名
    public int ButtonsNumber_1 = 1;                 //第一次有1个按钮 
    public int ButtonsNumber_2 = 1;                 //第二次有1个按钮 

    public int ButtonsKind_1_1 = 5;                 //第一次的按钮种类一为获得
    public int ButtonsKind_2_1 = 0;                 //第二次的按钮种类一为返回
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public int Success;                              //成功阈值
    public int GreatSuccess;                         //大成功阈值
    public int Failure;                              //失败阈值
    public int GreatFailure;                         //大失败阈值
    public string[] Monsters;                        //可能的怪物种类
    public Sprite BigBg;
    public Sprite EventBg;
    private int RollCount;                              //普通事件的随机值
    private int DeviationValue = 0;                      //进行随机事件时的偏差值
    private bool IsReturn = false;                     //本次点击了返回，不进行次数相加直接返回地图
    private string EventText;                             //事件描述

    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private Roll CE_Roll;

    private NewUIManager CE_NUIM;
    private string[] Data = new string[3];
    private int[] Datas = new int[3];
    private string[] Rare = new string[3];
    public TuDiShen()
    {
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");
        CE_Roll = new Roll();
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CardsEvents.SendName += TDS_Event;

    }
    public void TDS_Event(string cardName, string eventName, int time)
    {
        if (cardName == "Over")
        {
            time = 0;
        }
        if (cardName == Name)
        {
            //    Debug.Log("This Time is " + time + " and EventName is " + eventName);
            if (eventName != "Repeat")
            {
                if (time == 1)
                {
                    Debug.Log("This is " + Name);
                    time++;
                    EventText = "  你救了一只生活在树林中的妖精，它很感激地带你来到了它的家中，饱餐一顿后又送了一件它曾在林中捡到的装备";


                    CE_SA.CreateImage(EventText, 0);
                    CE_NewBut.GetButtonsProperty(ButtonsNumber_1, ButtonsKind_1_1, Name, time);
                    Data[0] = "FOO";
                    Data[1] = "EQU";
                    Rare[0] = "ALL";
                    Rare[1] = "ALL";
                    CE_NUIM.EventInfor(true, Data, 2, Rare, true);
                    //   CE_NUIM.EventShow();
                }
                if (eventName != null)
                {
                    if (time == 3)
                    {
                        time++;
                        CE_SA.CreateImage(EventText, 0);
                        CE_NewBut.GetButtonsProperty(ButtonsNumber_2, ButtonsKind_2_1, Name, time);
                    }
                }
            }
        }
    }
}
//↓"马车"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑,8为进入下一张地图
public class MaChe
{
    private string Name = "MaChe";
    private int ButtonsNumber = 2;                 //有2个按钮 

    private int ButtonsKind_1 = 0;                 //按钮种类一为返回
    private int ButtonsKind_2 = 8;                 //按钮种类二为进入下一张地图

    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private ToBlack CE_TB;
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public Sprite BigBg;
    public Sprite EventBg;
    private string EventText;                             //事件描述
    public MaChe()
    {
        CE_TB = GameObject.Find("ToBlack").GetComponent<ToBlack>();
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CardsEvents.SendName += MC_Event;
    }
    public void MC_Event(string cardName, string eventName, int time)
    {
        if (cardName == Name)
        {
            if (eventName == "TheLast")
            {
                EventText = "  你碰到了一辆坐着马车的人，他会带你离开这里，你也可以选择在这里再待一段时间";
                CE_SA.CreateImage(EventText, 0);
                Debug.Log("event is " + eventName);
                CE_NewBut.GetButtonsProperty(ButtonsNumber, ButtonsKind_1, ButtonsKind_2, Name, time);
            }
            if (eventName == "Go")
            {
                CE_TB.TOBlack("CE");
            }
        }
    }
}
//↓"传送阵"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑,8为进入下一张地图
public class ChuanSongZhen
{
    private string Name = "ChuanSongZhen";
    private int ButtonsNumber = 2;                 //有2个按钮 

    private int ButtonsKind_1 = 0;                 //按钮种类一为获得
    private int ButtonsKind_2 = 8;                 //按钮种类二为获得
    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private ToBlack CE_TB;
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public Sprite BigBg;
    public Sprite EventBg;
    private string EventText;                             //事件描述
    public ChuanSongZhen()
    {
        CE_TB = GameObject.Find("ToBlack").GetComponent<ToBlack>();
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CardsEvents.SendName += CSZ_Event;
    }
    public void CSZ_Event(string cardName, string eventName, int time)
    {
        if (cardName == Name)
        {
            Debug.Log("event is " + eventName);
            if (eventName == "TheLast")
            {
                EventText = "  你发现了一个古老的传送阵，通过它你可以离开这里";
                CE_SA.CreateImage(EventText, 0);
                CE_NewBut.GetButtonsProperty(ButtonsNumber, ButtonsKind_1, ButtonsKind_2, Name, time);
            }
            if (eventName == "Go")
            {
                CE_TB.TOBlack("CE");
            }
            //  // if (eventName == "Return")
            //   {
            //       CE_TB.TOBlack("CE");
            //   }
        }
    }
}
//↓"渡船"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑,8为进入下一张地图
public class DuChuan
{
    private string Name = "DuChuan";
    private int ButtonsNumber = 2;                 //有2个按钮 

    private int ButtonsKind_1 = 0;                 //按钮种类一为获得
    private int ButtonsKind_2 = 8;                 //按钮种类二为获得
    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private ToBlack CE_TB;
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public Sprite BigBg;
    public Sprite EventBg;
    private string EventText;                             //事件描述
    public DuChuan()
    {
        CE_TB = GameObject.Find("ToBlack").GetComponent<ToBlack>();
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CardsEvents.SendName += DC_Event;
    }
    public void DC_Event(string cardName, string eventName, int time)
    {
        if (cardName == Name)
        {
            Debug.Log("event is " + eventName);
            if (eventName == "TheLast")
            {
                EventText = "  你经过了一条河流，岸边靠着一只小船，也许它可以载着你离开这里";
                CE_SA.CreateImage(EventText, 0);
                CE_NewBut.GetButtonsProperty(ButtonsNumber, ButtonsKind_1, ButtonsKind_2, Name, time);
            }
            if (eventName == "Go")
            {
                CE_TB.TOBlack("CE");
            }
            //    if (eventName == "Return")
            //     {
            //        CE_TB.TOBlack("CE");
            //    }
        }
    }
}
//↓"山洞"的所有信息
//    ButtonsNumber_n   第n次的按钮数目
//    ButtonsKind_m_n   第m次的按钮为n种类， n：0为返回，1为掷骰，2为战斗，3为存在下一事件，4为购买，5为获得，6为开始战斗，7为逃跑,8为进入下一张地图
public class ShanDong
{
    private string Name = "ShanDong";
    private int ButtonsNumber = 2;                 //有2个按钮 

    private int ButtonsKind_1 = 0;                 //按钮种类一为获得
    private int ButtonsKind_2 = 8;                 //按钮种类二为获得
    private SignAbove CE_SA;
    private NewButton CE_NewBut;
    private ToBlack CE_TB;
    public string Text;                              //事件文本
    public Sprite BgPicture;                         //事件背景
    public Texture2D BgTexture;                      //贴图纹理
    public Sprite BigBg;
    public Sprite EventBg;
    private string EventText;                             //事件描述
    public ShanDong()
    {
        CE_TB = GameObject.Find("ToBlack").GetComponent<ToBlack>();
        BigBg = Resources.Load<Sprite>("Bg_Normal");
        EventBg = Resources.Load<Sprite>("Crards_Bg");
        CE_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE_NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        CardsEvents.SendName += SD_Event;
    }
    public void SD_Event(string cardName, string eventName, int time)
    {
        if (cardName == Name)
        {
            Debug.Log("event is " + eventName);
            if (eventName == "TheLast")
            {
                EventText = "  你面前是一个黑乎乎的山洞，天知道它会通向哪里";
                CE_SA.CreateImage(EventText, 0);
                CE_NewBut.GetButtonsProperty(ButtonsNumber, ButtonsKind_1, ButtonsKind_2, Name, time);
            }
            if (eventName == "Go")
            {
                CE_TB.TOBlack("CE");
            }
            //     if (eventName == "Return")
            //     {
            //         CE_TB.TOBlack("CE");
            //    }
        }
    }
}
