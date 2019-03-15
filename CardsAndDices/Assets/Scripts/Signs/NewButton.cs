using UnityEngine;
using System.Collections;
using DG.Tweening;
                                                                                         
//新的按钮实现方式
public class NewButton : MonoBehaviour {
    public GameObject[] Buttons=new GameObject[3];      //三个按钮
    public int ButtonsNumber;                           //本次需要的按钮数目
    private string ScriptsName;                         //调用此脚本的脚本名，用于作出决定

  //  public SpriteRenderer[] NewButRender;              //获取按钮上的 SpriteRenderer（）组件

    public TextMesh[] NewButText;              //按钮上的 TextMesh（）组件

  //  public Sprite ThisImage;                            //显示的图片
  //  public Sprite[] Sprites;                             //按钮的贴图们,顺序与CardEvents()中按钮种类相同         


    public  string[] EventNames;                             //按钮的事件名们,顺序与CardEvents()中按钮种类相同       
    public string thisEvent;                                //触发的事件名

    private string CardName;                             //本次出现的卡牌名
    private int EventTimes;                              //是这张卡牌的第几次事件
    private int[] ButtonKinds = new int[3];              //本次按钮种类
    public GameObject DelayObject;                        //延时物体，用于进行DoTween动作达到延时目的

     
    private SignAbove NewBut_SA;                        //调用SignAbove()
    private CardsEvents NewBut_CE;                      //调用CardsEvents()
    private NewUIManager NewBut_NUIM;
 //   private Booty NewBut_Booty;                         //调用Booty()
    private Roll NewBut_Roll;                             //调用Roll


	// Use this for initialization
	void Start () {
      //  for (int i = 0; i < 3; i++)
      //  {
     //       NewButRender[i] = Buttons[i].GetComponentInChildren<SpriteRenderer>();
     //   }
        NewBut_CE = new CardsEvents();
        NewBut_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();     //实例化
        NewBut_NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
     //   NewBut_Booty = GameObject.Find("BG_Booty").GetComponent<Booty>();
        NewBut_Roll = new Roll();
	}

    //↓得到需要的按钮属性，并进行相应的赋值，num为按钮数目，kind为按钮种类，name为卡牌名字，time为第几次事件
    //  被CardEvents()中对应卡牌调用
    //  只有一个按钮时
    public void GetButtonsProperty(int num,int kind,string name,int time)            
    {
        ButtonKinds[1] = kind;
        ButtonsNumber = num;
        CardName = name;
        EventTimes = time;
        thisEvent = EventNames[kind];
        NewButText[1].text = thisEvent;
        
    }
    //  有两个按钮时
    public void GetButtonsProperty(int num, int kind1,int kind2, string name, int time)
    {
        ButtonKinds[0] = kind1;
        ButtonKinds[2] = kind2;
        ButtonsNumber = num;
        CardName = name;
        EventTimes = time;
        thisEvent = EventNames[kind1];
        NewButText[0].text = thisEvent;
            thisEvent = EventNames[kind2];
            NewButText[2].text = thisEvent;
    }
    //↓有三个按钮时
    public void GetButtonsProperty(int num, int kind1, int kind2,int kind3, string name, int time)
    {
        ButtonKinds[0] = kind1;
        ButtonKinds[1] = kind2;
        ButtonKinds[2] = kind3;
        ButtonsNumber = num;
        CardName = name;
        EventTimes = time;
        thisEvent = EventNames[kind1];
        NewButText[0].text = thisEvent;
        thisEvent = EventNames[kind2];
        NewButText[1].text = thisEvent;
        thisEvent = EventNames[kind3];
        NewButText[2].text = thisEvent;
    }
    //↓控制按钮旋转
    //  被SignAbove()中CanRotateDown()调用
    public void CanSpinButton()
    {
      //  Debug.Log("Spin");
        if (ButtonsNumber == 1)
        {
            Tweener Mid_Rotate = Buttons[1].transform.DORotate(new Vector3(0, 180, 0), 0.3f);
        }
        else if (ButtonsNumber == 2)
        {
            Tweener Left_Rotate = Buttons[0].transform.DORotate(new Vector3(0, 180, 0), 0.3f);
            Tweener Right_Rotate = Buttons[2].transform.DORotate(new Vector3(0, 180, 0), 0.3f);
        }
        else if (ButtonsNumber == 3)
        {
           
            Tweener Mid_Rotate = Buttons[1].transform.DORotate(new Vector3(0, 180, 0), 0.3f);
            Tweener Left_Rotate = Buttons[0].transform.DORotate(new Vector3(0, 180, 0), 0.3f);
            Tweener Right_Rotate = Buttons[2].transform.DORotate(new Vector3(0, 180, 0), 0.3f);
        }
    }
	//↓发出射线，并根据射线检测的物体名，进行点击判定
    //  根据按钮数与点击物体名进行判定，并在延时结束后将按钮的对应名字传递给CardEvents()进行下一步事件传递
    void Update()
    {
        Ray ray_Booty = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit_Button = new RaycastHit();
        if (Input.GetMouseButtonUp(0))
        {
            if (Physics.Raycast(ray_Booty, out hit_Button))
            {
              //  Debug.Log("Button's name is "+hit_Button.transform.name);
                if (ButtonsNumber == 1 && hit_Button.transform.name == Buttons[1].name)
                {
                    CanRotate(1);
                    NewBut_NUIM.EventShow();
                    Tweener Delay = DelayObject.transform.DOMove(new Vector3(233,0,0),1f);  //延时1s
                    Delay.OnComplete(delegate() { NewBut_CE.GetCardNameAndEvent(CardName, NewButText[1].text, EventTimes); });
                 //   Debug.Log("Button kind is " + Buttons[1].GetComponentInChildren<SpriteRenderer>().sprite.name + " Time is " + EventTimes);
                }
                else if (ButtonsNumber == 2 && hit_Button.transform.name == Buttons[0].name )
                {
                    CanRotate(2);
                    NewBut_NUIM.EventShow();
                    Tweener Delay = DelayObject.transform.DOMove(new Vector3(233, 0, 0), 1f);
                    Delay.OnComplete(delegate() { NewBut_CE.GetCardNameAndEvent(CardName, NewButText[0].text, EventTimes); });
               //     Debug.Log("Button kind is " + Buttons[0].GetComponentInChildren<SpriteRenderer>().sprite.name + " Time is " + EventTimes);
                }
                else if (ButtonsNumber == 2 &&  hit_Button.transform.name == Buttons[2].name)
                {
                    CanRotate(2);
                    NewBut_NUIM.EventShow();
                    Tweener Delay = DelayObject.transform.DOMove(new Vector3(233, 0, 0),1f);
                    Delay.OnComplete(delegate() { NewBut_CE.GetCardNameAndEvent(CardName, NewButText[2].text, EventTimes); });
                 //   Debug.Log("Button kind is " + Buttons[2].GetComponentInChildren<SpriteRenderer>().sprite.name + " Time is " + EventTimes);
                }
                else if (ButtonsNumber == 3 && hit_Button.transform.name == Buttons[0].name )
                {
                    CanRotate(3);
                    NewBut_NUIM.EventShow();
                    Tweener Delay = DelayObject.transform.DOMove(new Vector3(233, 0, 0), 1f);
                    Delay.OnComplete(delegate() { NewBut_CE.GetCardNameAndEvent(CardName, NewButText[0].text, EventTimes); });
                  //  Debug.Log("Button kind is " + Buttons[0].GetComponentInChildren<SpriteRenderer>().sprite.name + " Time is " + EventTimes);
                }
                else if (ButtonsNumber == 3 &&  hit_Button.transform.name == Buttons[1].name )
                {
                    CanRotate(3);
                    NewBut_NUIM.EventShow();
                    Tweener Delay = DelayObject.transform.DOMove(new Vector3(233, 0, 0), 1f);
                    Delay.OnComplete(delegate() { NewBut_CE.GetCardNameAndEvent(CardName, NewButText[1].text, EventTimes); });
                  //  Debug.Log("Button kind is " + Buttons[1].GetComponentInChildren<SpriteRenderer>().sprite.name + " Time is " + EventTimes);
                }
                else if (ButtonsNumber == 3 &&  hit_Button.transform.name == Buttons[2].name)
                {
                    CanRotate(3);
                    NewBut_NUIM.EventShow();
                    Tweener Delay = DelayObject.transform.DOMove(new Vector3(233, 0, 0), 1f);
                    Delay.OnComplete(delegate() { NewBut_CE.GetCardNameAndEvent(CardName, NewButText[2].text, EventTimes); });
                 //   Debug.Log("Button kind is " + Buttons[2].GetComponentInChildren<SpriteRenderer>().sprite.name + " Time is " + EventTimes);
                }
            }
        }
    }
    //↓进行点击后的相应翻转，whichButton用于判定点击事件
    //  被此脚本中Update()调用
    //  根据ScriptsName获得的脚本名，于翻转完成后进行相应的事件信息传递，具体信息规则写于SignAbove()与Booty()中
    public void CanRotate(int whichButton)
    {
        if (whichButton == 1)
        {
            Tweener Rotate = Buttons[1].transform.DORotate(new Vector3(0, 0, 0), 0.4f);
                Rotate.OnComplete( NewBut_SA.CanRotateUp);
          
              //  Rotate.OnComplete(delegate() { NewBut_SA.CanRotateUp(2); NewBut_Booty.RotateUp(3); });
        }
        else if (whichButton == 2)
        {
            Tweener Rotate1 = Buttons[0].transform.DORotate(new Vector3(0, 0, 0), 0.4f);
            Tweener Rotate2 = Buttons[2].transform.DORotate(new Vector3(0, 0, 0), 0.4f);
                Rotate2.OnComplete( NewBut_SA.CanRotateUp);
            //    Rotate2.OnComplete(delegate() { NewBut_SA.CanRotateUp(2); NewBut_Booty.RotateUp(3); });
            
        }
        else if (whichButton == 3)
        {
            Tweener Rotate1 = Buttons[0].transform.DORotate(new Vector3(0, 0, 0), 0.4f);
            Tweener Rotate2 = Buttons[1].transform.DORotate(new Vector3(0, 0, 0), 0.4f);
            Tweener Rotate3 = Buttons[2].transform.DORotate(new Vector3(0, 0, 0), 0.4f);
            Rotate3.OnComplete(NewBut_SA.CanRotateUp);
            //  Rotate3.OnComplete(delegate() { NewBut_SA.CanRotateUp(2); NewBut_Booty.RotateUp(3); });

        }
        else if (whichButton ==4)
        {
            Tweener Rotate1 = Buttons[0].transform.DORotate(new Vector3(0, 0, 0), 0f);
            Tweener Rotate2 = Buttons[1].transform.DORotate(new Vector3(0, 0, 0), 0f);
            Tweener Rotate3 = Buttons[2].transform.DORotate(new Vector3(0, 0, 0), 0f);

            NewBut_SA.CanRotateUp(true);
        }
        else
            NewBut_SA.CanRotateUp(true);
    }
}
