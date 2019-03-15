using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

public class UImanager : MonoBehaviour {
    public GameObject[] UIs;          //所有需要的UI组件
    private GameObject ThisUI;          //此次实例化的卡牌UI
    public Button[] Btn;              //Button数组，用于保存得到的ThisUI下的所有Button组件
    private int First = 0;    //只在第一次访问结束后将屏幕变黑
    bool CanOnClock=true;           //判断是否可以添加点击事件
    private int IE_i = 0;
    ToBlack UIM_TB;        
    Move UIM_Move;
    Roll UIM_Roll;
    int Btn_i = 0;
    List<string> BtnsName = new List<string>();      //表，存储Button名
	// Use this for initialization
    void Start()
    {
        UIM_TB = GameObject.Find("ToBlack").GetComponent<ToBlack>();     //得到ToBlack（）脚本组件
       UIM_Move = GameObject.Find("CreateMap").GetComponent<Move>();
       UIM_Roll = GameObject.Find("RollImage").GetComponent<Roll>();

        BtnsName.Add("Roll");          //向表中添加Roll
        BtnsName.Add("Return");       //添加"Return"
        BtnsName.Add("Battle");       //添加"Battle"
        BtnsName.Add("GoFindOut");      //添加"GoFindOut"
        BtnsName.Add("Buy");              //添加"Buy"
        BtnsName.Add("AskForSth");        //添加"AskForSth"
        BtnsName.Add("ThankYou");           //添加"ThankYou"
        BtnsName.Add("Go");                 //添加"Go"
    }

    public void GetNameAndAppear(string Thisname)      //从CreateMap（）得到此次的卡牌名，并创建对应UI
    {
        IE_i = 0;
        CanOnClock = true;
        First = 0;
        int L = UIs.Length;                           //获取UI组长度
        for (int i = 0; i < L; i++)
        {
            Btn_i = 0;
            if (UIs[i].name == Thisname&&CanOnClock)             //为对应的UI时
            {
                ThisUI = UIs[i];        
                ThisUI = (GameObject)Instantiate(UIs[i]);                     //实例化UI
                ThisUI.transform.SetParent(GameObject.Find("BG").transform);     //设定Canvas为父对象
                ThisUI.transform.position = new Vector2(-1000,-1000);            //设置初始位置  (161.5,258)
               Tweener Close = ThisUI.transform.DOScale(0, 0);
               Tweener Move = ThisUI.transform.DOMove(new Vector2(161.5f, 258), 0);
                Tweener Appear = ThisUI.transform.DOScale(1, 0.5f);               //实现从无到有的过程
                Btn = ThisUI.GetComponentsInChildren<Button>();   
                for (int j = 0; j < BtnsName.Count; j++)
                {
                    if (Btn_i == Btn.Length)
                    {
                        i = L - 1;
                        Btn_i = Btn.Length-1;
                        CanOnClock = false;
                        break;
                    }
                           if (Btn[Btn_i].name == BtnsName[j]&&CanOnClock)
                           {
                            //   EventTriggerListener.Get(Btn[Btn_i].gameObject).onClick = OnClock;     //添加委托，将OnClock（）赋值给委托onClick（）
                               j = -1;
                            //   StartCoroutine(ToOnClock());
                               Btn_i++;         //计数下标加一，防止在本次地图探索完后，多次调用ToBlack()函数导致下一次加载出错
                           }
                }
            }
        }
    }  
   public void OnClock(GameObject ThisButton)           //响应事件
    {
        Debug.Log(ThisButton.name);
        for (int i = 0; i < Btn.Length;i++ )
        {
            switch (ThisButton.name)                       //根据传过来的按钮名字决定进行何种响应
            {
                case "Roll":
                    //名为"Roll"时
                    CanRoll();
                    Tweener Close = ThisUI.transform.DOScale(0, 0.5f);
                    Close.OnComplete(delegate() { Destroy(ThisUI); });
                    break;
                case "Battle":                                               //名为"Battle"时
                    CanBattle();
                    Close = ThisUI.transform.DOScale(0, 0.5f);
                    Close.OnComplete(delegate() { Destroy(ThisUI); });
                    break;
                case "Return":                                                //名为"Return"时
                    Close = ThisUI.transform.DOScale(0, 0.5f);
                    Close.OnComplete(delegate() { Destroy(ThisUI); });
                    break;
                case "GoFindOut":                                         //名为"GoFindOut"时
                    CanGoFindOut();
                    Close = ThisUI.transform.DOScale(0, 0.5f);
                    Close.OnComplete(delegate() { Destroy(ThisUI); });
                    break;
                case "ThankYou":                                           //名为"ThankYou"时
                    Supply_MuShi();
                    Close = ThisUI.transform.DOScale(0, 0.5f);
                    Close.OnComplete(delegate() { Destroy(ThisUI); });
                    break;
                case "AskForSth":                                     //名为"AskForSth"时
                    Supply_TuDiShen();
                    Close = ThisUI.transform.DOScale(0, 0.5f);
                    Close.OnComplete(delegate() { Destroy(ThisUI); });
                    break;
                case "Buy":                                            //名为"Buy"时
                    Supply_ShangRen();
                    Close = ThisUI.transform.DOScale(0, 0.5f);
                    Close.OnComplete(delegate() { Destroy(ThisUI); });
                    break;
                case "Go":                                            //名为"Go"时
                    Close = ThisUI.transform.DOScale(0, 0.5f);
                    Close.OnComplete(delegate() { Destroy(ThisUI); });
                   // UIM_Move.SubOne();                                    //本次地图探索完毕，将Move()中i计数减一，防止下一次地图加载出错
                    FirstToBlack();                                       //首次变黑，防止多次调用ToBlack()导致地图加载出错
                    break;
                default:
                    break;
            }
            }
    }            
    void FirstToBlack()
    {
        First++;
        if (First == 1)
        {
            UIM_TB.TOBlack("UImanager");
        }
    }
    void CanRoll()
    {
        return;
    }
    void CanBattle()
    {
        return;
    }
    void CanGoFindOut()
    {
        return;
    }
    void Supply_MuShi()
    {
        return;
    }
    void Supply_TuDiShen()
    {
        return;
    }
    void Supply_ShangRen()
    {
        return;
    }
	// Update is called once per frame
	void Update () {
	
}
}
/*public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
{
    public delegate void VoidDelegate(GameObject go);           //定义委托VoidDelegate（）
    public VoidDelegate onClick;                           //定义委托类型变量
   /* public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;     

    static public EventTriggerListener Get(GameObject go)                  
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null) listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick(gameObject);
    }
  /*  public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(gameObject);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter(gameObject);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit(gameObject);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp(gameObject);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect(gameObject);
    }       */
//}

