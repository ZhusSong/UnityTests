using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class SignButton : MonoBehaviour{
    enum ButtonState                        //定义按钮状态
    { 
        unselected = 0,                     //未被点击
        PointInter=1,                       //光标进入范围
        selected = 2                        //点击
    };
    public GameObject[] Buttons;            //是哪个按钮
    public Texture2D imageUnselected;       //未点击时的按钮贴图
    public Texture2D imageSelected;         //点击时的按钮贴图
    public Texture2D imageInter;            //鼠标进入按钮范围时的按钮贴图
    private ButtonState This_state;         //按钮状态
    public Rect[] ImageRect;                //点击判定范围矩阵
    public SpriteRenderer[] ButtonRenderer; //获取按钮上的 SpriteRenderer（）组件
    public Sprite ThisImage;                //显示的图片
    Vector2 realPos;                        //鼠标点击时的GUI坐标 
    private bool Selected = false;          //未点击，防止点击贴图和悬停贴图冲突
    private int WhichButton = 0;            //进行判定的是哪个按钮,为0时为左至右第一个，为1时第二个，为2时第三个，为4时都不进行
    private int ButtonNumber=0;             //按钮个数
    private bool CanCreateButton = true;    //是否可创建按钮，控制Update()防止创建额外按钮，为真时可进行创建
    private int WhichEvent=0;
    private string ScriptsName;
   
    private SignAbove Button_SA;            //调用SignAbove()
    private Battle Button_Battle;            //调用Button_battle()
    private Booty Button_Booty;
    void Start()
    {
        for (int i = 0; i < Buttons.Length; i++)                                              //得到按钮的组件
        {
            ButtonRenderer[i] = Buttons[i].GetComponentInChildren<SpriteRenderer>();
        }
        Button_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();                     //得到SignAbovbe()组件
        Button_Battle = GameObject.Find("BG_Battle").GetComponent<Battle>();
        Button_Booty = GameObject.Find("BG_Booty").GetComponent<Booty>();
    }
	// Use this for initialization
    public void CanCreateRectOfJudge()      //可以进行判定矩阵创建,被SignAbove()中CanRotateDown()调用
    {
        if (imageUnselected != null&&CanCreateButton)        //生成判定范围矩阵并绘制贴图,按钮数为3时
       // {
            ImageRect[0] = new Rect(Camera.main.WorldToScreenPoint(Buttons[0].transform.position).x, Screen.height - Camera.main.WorldToScreenPoint(Buttons[0].transform.position).y, imageUnselected.width * 0.065f, imageUnselected.height * 0.04f);
            ImageRect[1] = new Rect(Camera.main.WorldToScreenPoint(Buttons[1].transform.position).x, Screen.height - Camera.main.WorldToScreenPoint(Buttons[1].transform.position).y, imageUnselected.width * 0.065f, imageUnselected.height * 0.04f);
            ImageRect[2] = new Rect(Camera.main.WorldToScreenPoint(Buttons[2].transform.position).x, Screen.height - Camera.main.WorldToScreenPoint(Buttons[2].transform.position).y, imageUnselected.width * 0.065f, imageUnselected.height * 0.04f);
           
      //  }
       // else if (ButtonNumber == 2)                          //按钮数为2时
      //  {
            ImageRect[3] = new Rect(Camera.main.WorldToScreenPoint(Buttons[3].transform.position).x, Screen.height - Camera.main.WorldToScreenPoint(Buttons[0].transform.position).y, imageUnselected.width * 0.065f, imageUnselected.height * 0.04f);
            ImageRect[4] = new Rect(Camera.main.WorldToScreenPoint(Buttons[4].transform.position).x, Screen.height - Camera.main.WorldToScreenPoint(Buttons[1].transform.position).y, imageUnselected.width * 0.065f, imageUnselected.height * 0.04f);

       // }
       // else if (ButtonNumber == 1)                          //按钮数为1时
       // {
            ImageRect[5] = new Rect(Camera.main.WorldToScreenPoint(Buttons[5].transform.position).x, Screen.height - Camera.main.WorldToScreenPoint(Buttons[0].transform.position).y, imageUnselected.width * 0.065f, imageUnselected.height * 0.04f);

       // }
        Selected = false;                                   
        CanCreateButton = true;
    }
    public void GetName_Button(string name)
    {
        Debug.Log("Button's name is: " + name);
    }
    public void Which_Event(int i)
    {
        WhichEvent = i;
    }
    public void IfCanCreate(bool CanOrNot)      //是否可创建按钮，用于被其他脚本调用控制CanCreateButton变量
    {
      //  CanCreateButton = CanOrNot;
    }
    public void GetButtonNumber(int i,string name)                                                        //从SignAbove()得到需要的按钮数量并创建，被SignAbove()中GetName_SA()调用
    {
        ScriptsName = name;
        ButtonNumber = i;                
        if (ButtonNumber == 1)                                                                //当按钮数为1时，创建按钮
        {
            ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[5].x, ImageRect[5].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
            ButtonRenderer[5].sprite = ThisImage;
            CanCreateButton = true;
        }
        else if (ButtonNumber == 2)                                                           //当按钮数为2时，创建按钮
        {
            ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[3].x, ImageRect[3].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
            ButtonRenderer[3].sprite = ThisImage;
            ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[4].x, ImageRect[4].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
            ButtonRenderer[4].sprite = ThisImage;
            CanCreateButton = true;
        }
        else if (ButtonNumber == 3)                                                           //当按钮数为3时，创建按钮
        {
            ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[0].x, ImageRect[0].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
            ButtonRenderer[0].sprite = ThisImage;
            ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[1].x, ImageRect[1].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
            ButtonRenderer[1].sprite = ThisImage;
            ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[2].x, ImageRect[2].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
            ButtonRenderer[2].sprite = ThisImage;
            CanCreateButton = true;
        }
    }
	
	void Update () 
    {
        realPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);  //计算鼠标实际位置    
        if (CanCreateButton)                                                                  //能创建按钮时
        {
            if (ButtonNumber == 3)                                                            //按钮数为3
            {
                if (ImageRect[0].Contains(realPos) && !Selected)                              //当鼠标进入第1个按钮的判定矩阵时
                {
                    This_state = ButtonState.PointInter;                                      //将This_state赋值为进入判定范围
                    WhichButton = 0;                                                          //第一个按钮
                    ChangeButtonImage(This_state, WhichButton);                               //向ChangeButtonImage()传递参数，创建对应贴图
                }
                else if (ImageRect[1].Contains(realPos) && !Selected)                         //当鼠标进入第2个按钮的判定矩阵时
                {
                    This_state = ButtonState.PointInter;                                      
                    WhichButton = 1;                                                          //第二个按钮
                    ChangeButtonImage(This_state, WhichButton);                               
                }
                else if (ImageRect[2].Contains(realPos) && !Selected)                         //当鼠标进入第3个按钮的判定矩阵时
                {
                    This_state = ButtonState.PointInter;
                    WhichButton = 2;                                                          //第三个按钮
                    ChangeButtonImage(This_state, WhichButton);
                }
                else
                {
                    if (!Selected)                                                            //光标既未点击、未抬起，也未进入判定范围时重绘制未点击贴图                       
                    {
                        This_state = ButtonState.unselected;                                  //将This_state赋值为未点击
                        ChangeButtonImage(This_state, 4);                                     
                    }
                }
            }
           else if (ButtonNumber == 2)                                                        //按钮数为2时
            {
                if (ImageRect[3].Contains(realPos) && !Selected)                              //当鼠标进入第一个按钮的判定矩阵时
                {
                    This_state = ButtonState.PointInter;                                       
                    WhichButton = 0;                                                          //第一个按钮
                    ChangeButtonImage(This_state, WhichButton); 
                }
                else if (ImageRect[4].Contains(realPos) && !Selected)                         //当鼠标进入第二个按钮的判定矩阵时
                {
                    This_state = ButtonState.PointInter;
                    WhichButton = 1;                                                          //第二个按钮
                    ChangeButtonImage(This_state, WhichButton);
                }
                else
                {
                    if (!Selected)                                                            //光标既未点击、未抬起，也未进入判定范围时重绘制未点击贴图                      
                    {
                        This_state = ButtonState.unselected;
                        ChangeButtonImage(This_state, 4);
                    }
                }
            }
            else if (ButtonNumber == 1)                                                       //按钮数为1时
            {
                if (ImageRect[5].Contains(realPos) && !Selected)                              //当鼠标进入第一个按钮的判定矩阵时
                {
                    This_state = ButtonState.PointInter;
                    WhichButton = 0;                                                          //第一个按钮 
                    ChangeButtonImage(This_state, WhichButton);                               
                }
                else
                {
                    if (!Selected)                                                            //重绘制未点击贴图                       
                    {
                        This_state = ButtonState.unselected;
                        ChangeButtonImage(This_state, 4);
                    }
                }
            }
            if (Input.GetMouseButtonDown(0))                                                  //鼠标点击时
            {
                Selected = true;                                                              //已点击         
                if (ButtonNumber == 3)                                                        //按钮数为3时
                {
                    if (ImageRect[0].Contains(realPos))                                       //当鼠标点击位置在第一个按钮的判定范围内时
                    {
                        This_state = ButtonState.selected;                                    //传递点击事件
                        WhichButton = 0;
                        ChangeButtonImage(This_state, WhichButton);
                    }
                    if (ImageRect[1].Contains(realPos))                                       //当鼠标点击位置在第二个按钮的判定范围内时
                    {
                        This_state = ButtonState.selected;                                    //传递点击事件
                        WhichButton = 1;
                        ChangeButtonImage(This_state, WhichButton);
                    }
                    if (ImageRect[2].Contains(realPos))                                       //当鼠标点击位置在第三个按钮的判定范围内时
                    {
                        This_state = ButtonState.selected;                                    //传递点击事件
                        WhichButton = 2;
                        ChangeButtonImage(This_state, WhichButton);
                    }
                }
                else if (ButtonNumber == 2)                                                   //按钮数为2时
                {
                    if (ImageRect[3].Contains(realPos))                                       //当鼠标点击位置在第一个按钮的判定范围内时
                    {
                        This_state = ButtonState.selected;                                    //传递点击事件
                        WhichButton = 0;
                        ChangeButtonImage(This_state, WhichButton);
                    }
                    if (ImageRect[4].Contains(realPos))                                       //当鼠标点击位置在第二个按钮的判定范围内时
                    {
                        This_state = ButtonState.selected;                                    //传递点击事件
                        WhichButton = 1;
                        ChangeButtonImage(This_state, WhichButton);
                    }
                } 
                else if (ButtonNumber == 1)                                                   //按钮数为1时
                {
                    if (ImageRect[5].Contains(realPos))                                       //当鼠标点击位置在第一个按钮的判定范围内时
                    {
                        This_state = ButtonState.selected;                                    //传递点击事件
                        WhichButton = 0;
                        ChangeButtonImage(This_state, WhichButton);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))                                               //鼠标抬起时
            {
                if (ButtonNumber == 3)                                                        //按钮数为3时
                {
                    if (ImageRect[0].Contains(realPos))                                       //当鼠标抬起位置位于判定矩阵，即做出决定时
                    {
                        This_state = ButtonState.unselected;                                  //↓
                        WhichButton = 0;                                                      //传递点击事件
                        ChangeButtonImage(This_state, WhichButton); 
                        if (true&&ScriptsName=="SA")
                        {
                            Button_SA.CanRotateUp();                                         //通知SignAbove()中CanRotateUp()，1为战斗场景，使牌子抬起
                        }
                        else if (ScriptsName == "Booty")
                        {
                         //   Button_Booty.RotateUp(3);
                            Button_SA.CanRotateUp();  
                        }
                    }                                                                          //↑
                    else                                                                      //鼠标位置在判定矩阵外，不作出决定     
                    {
                        This_state = ButtonState.unselected;                                  //↓
                        WhichButton = 0;                                                      //传递点击事件
                        ChangeButtonImage(This_state, WhichButton);                           //↑
                        Selected = false;                                                     //置点击判断变量为假
                    }
                    if (ImageRect[1].Contains(realPos))                                       //↓同上，仅进行判定的按钮不同
                    {                                                                          
                        This_state = ButtonState.unselected;                                   
                        WhichButton = 1;
                        ChangeButtonImage(This_state, WhichButton);
                        if (true && ScriptsName == "SA")
                        {
                            Button_SA.CanRotateUp();                                         //通知SignAbove()中CanRotateUp()，1为战斗场景，使牌子抬起
                        }
                        else if (ScriptsName == "Booty")
                        {
                       //     Button_Booty.RotateUp(3);
                            Button_SA.CanRotateUp();  
                        }
                    }
                    else                                                                       
                    {
                        This_state = ButtonState.unselected;
                        WhichButton = 1;
                        ChangeButtonImage(This_state, WhichButton);
                        Selected = false;                                                     //↑
                    }
                    if (ImageRect[2].Contains(realPos))                                       //↓同上，仅进行判定的按钮不同
                    {                                                     
                        This_state = ButtonState.unselected;
                        WhichButton = 2;
                        ChangeButtonImage(This_state, WhichButton);
                        if (true && ScriptsName == "SA")
                        {
                            Button_SA.CanRotateUp();                                         //通知SignAbove()中CanRotateUp()，1为战斗场景，使牌子抬起
                        }
                        else if (ScriptsName == "Booty")
                        {
                       //     Button_Booty.RotateUp(3);
                            Button_SA.CanRotateUp();  
                        }
                    }
                    else
                    {
                        This_state = ButtonState.unselected;
                        WhichButton = 2;
                        ChangeButtonImage(This_state, WhichButton);
                        Selected = false;                                                     //↑
                    }
                } 
                if (ButtonNumber == 2)                                                        //当按钮数为2时
                {
                    if (ImageRect[3].Contains(realPos))                                       //↓同上，仅进行判定的按钮不同
                    {
                        This_state = ButtonState.unselected;
                        WhichButton = 0;
                        ChangeButtonImage(This_state, WhichButton);
                        if (true && ScriptsName == "SA")
                        {
                            Button_SA.CanRotateUp();                                         //通知SignAbove()中CanRotateUp()，1为战斗场景，使牌子抬起
                        }
                        else if (ScriptsName == "Booty")
                        {
                      //      Button_Booty.RotateUp(3);
                            Button_SA.CanRotateUp();  
                        }                          
                    }
                    else                                                                                  
                    {
                        This_state = ButtonState.unselected;
                        WhichButton = 0;
                        ChangeButtonImage(This_state, WhichButton);
                        Selected = false;                                                     //↑
                    }
                    if (ImageRect[4].Contains(realPos))                                       //↓同上，仅进行判定的按钮不同
                    {
                        This_state = ButtonState.unselected;
                        WhichButton = 1;
                        ChangeButtonImage(This_state, WhichButton);
                        if (true && ScriptsName == "SA")
                        {
                            Button_SA.CanRotateUp();                                         //通知SignAbove()中CanRotateUp()，1为战斗场景，使牌子抬起
                        }
                        else if (ScriptsName == "Booty")
                        {
                       //     Button_Booty.RotateUp(3);
                            Button_SA.CanRotateUp();  
                        }
                    }
                    else
                    {
                        This_state = ButtonState.unselected;
                        WhichButton = 1;
                        ChangeButtonImage(This_state, WhichButton);
                        Selected = false;                                                     //↑
                    }
                }

                if (ButtonNumber == 1)                                                        //当按钮数为1时
                {
                    if (ImageRect[5].Contains(realPos))                                       //↓同上，仅进行判定的按钮不同
                    {
                        This_state = ButtonState.unselected;
                        WhichButton = 0;
                        ChangeButtonImage(This_state, WhichButton);
                        if (true && ScriptsName == "SA")
                        {
                            Button_SA.CanRotateUp();                                         //通知SignAbove()中CanRotateUp()，1为战斗场景，使牌子抬起
                        }
                        else if (ScriptsName == "Booty")
                        {
                     //       Button_Booty.RotateUp(3);
                            Button_SA.CanRotateUp();  
                        }
                    }
                    else                                                                                  
                    {
                        This_state = ButtonState.unselected;
                        WhichButton = 0;
                        ChangeButtonImage(This_state, WhichButton);
                        Selected = false;                                                      //↑
                    } 

                }
            }
        }
	}
    public void DestroyButton()                                                          //销毁上一次的按钮，被SignAbove()中CanRotateUp()调用
    {
        CanCreateButton = false;                                                   //不可创建按钮
        for (int i = 0; i <6; i++)
        {
            ButtonRenderer[i].sprite=null;                                       //置所有按钮贴图为空
        }
    }
    void ChangeButtonImage(ButtonState i, int j)                                              //根据参数绘制贴图，被此脚本中Update()调用，i为按钮状态，j为第几个按钮
    {  
        if (i == ButtonState.unselected && j == 4&&ButtonNumber==3)                           //按钮数为3且鼠标未进入任一按钮时，创建未点击贴图
        {
            ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[0].x, ImageRect[0].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
            ButtonRenderer[0].sprite = ThisImage;
            ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[1].x, ImageRect[1].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
            ButtonRenderer[1].sprite = ThisImage;
            ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[2].x, ImageRect[2].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
            ButtonRenderer[2].sprite = ThisImage;
        }
        else if (i == ButtonState.unselected && j == 4 && ButtonNumber == 2)                   //按钮数为2且鼠标未进入任一按钮时，创建未点击贴图
        {
            ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[0].x, ImageRect[0].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
            ButtonRenderer[3].sprite = ThisImage;
            ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[1].x, ImageRect[1].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
            ButtonRenderer[4].sprite = ThisImage;
        }
        else if (i == ButtonState.unselected && j == 4 && ButtonNumber == 1)                   //按钮数为1且鼠标未进入任一按钮时，创建未点击贴图
        {
            ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[0].x, ImageRect[0].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
            ButtonRenderer[5].sprite = ThisImage;
        }
        if (ButtonNumber == 3)                                                                 //按钮数为3时
        {
            if (i == ButtonState.unselected && j == 0)                                         //第一个按钮未被点击时，创建未点击贴图
            {
                ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[0].x, ImageRect[0].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[0].sprite = ThisImage;
            }
            else if (i == ButtonState.selected && j == 0)                                      //第一个按钮被点击时，创建点击贴图
            {
                ThisImage = Sprite.Create(imageSelected, new Rect(ImageRect[0].x, ImageRect[0].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[0].sprite = ThisImage;
            }
            else if (i == ButtonState.PointInter && j == 0)                                    //光标进入第一个按钮范围是，创建进入贴图
            {
                ThisImage = Sprite.Create(imageInter, new Rect(ImageRect[0].x, ImageRect[0].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[0].sprite = ThisImage;
            }
            if (i == ButtonState.unselected && j == 1)                                         //第二个按钮未被点击时，创建未点击贴图
            {
                ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[1].x, ImageRect[1].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[1].sprite = ThisImage;
            }
            else if (i == ButtonState.selected && j == 1)                                      //第二个按钮被点击时，创建点击贴图
            {
                ThisImage = Sprite.Create(imageSelected, new Rect(ImageRect[1].x, ImageRect[1].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[1].sprite = ThisImage;
            }
            else if (i == ButtonState.PointInter && j == 1)                                    //光标进入第二个按钮范围是，创建进入贴图
            {
                ThisImage = Sprite.Create(imageInter, new Rect(ImageRect[1].x, ImageRect[1].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[1].sprite = ThisImage;
            }
            if (i == ButtonState.unselected && j == 2)                                         //第三个按钮未被点击时，创建未点击贴图
            {
                ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[2].x, ImageRect[2].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[2].sprite = ThisImage;
            }
            else if (i == ButtonState.selected && j == 2)                                      //第三个按钮被点击时，创建点击贴图
            {
                ThisImage = Sprite.Create(imageSelected, new Rect(ImageRect[2].x, ImageRect[2].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[2].sprite = ThisImage;
            }
            else if (i == ButtonState.PointInter && j == 2)                                    //光标进入第三个按钮范围是，创建进入贴图
            {
                ThisImage = Sprite.Create(imageInter, new Rect(ImageRect[2].x, ImageRect[2].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[2].sprite = ThisImage;
            }
        }
        else if (ButtonNumber == 2)                                                            //按钮数为2时
        {
            if (i == ButtonState.unselected && j == 0)                                         //第一个按钮未被点击时，创建未点击贴图
            {
                ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[3].x, ImageRect[3].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[3].sprite = ThisImage;
            }
            else if (i == ButtonState.selected && j == 0)                                      //第一个按钮被点击时，创建点击贴图
            {
                ThisImage = Sprite.Create(imageSelected, new Rect(ImageRect[3].x, ImageRect[3].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[3].sprite = ThisImage;
            }
            else if (i == ButtonState.PointInter && j == 0)                                    //光标进入第一个按钮范围是，创建进入贴图
            {
                ThisImage = Sprite.Create(imageInter, new Rect(ImageRect[3].x, ImageRect[3].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[3].sprite = ThisImage;
            }
            if (i == ButtonState.unselected && j == 1)                                         //第二个按钮未被点击时，创建未点击贴图
            {
                ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[4].x, ImageRect[4].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[4].sprite = ThisImage;
            }
            else if (i == ButtonState.selected && j == 1)                                      //第二个按钮被点击时，创建点击贴图
            {
                ThisImage = Sprite.Create(imageSelected, new Rect(ImageRect[4].x, ImageRect[4].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[4].sprite = ThisImage;
            }
            else if (i == ButtonState.PointInter && j == 1)                                    //光标进入第二个按钮范围是，创建进入贴图
            {
                ThisImage = Sprite.Create(imageInter, new Rect(ImageRect[4].x, ImageRect[4].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[4].sprite = ThisImage;
            }
        }
        else if (ButtonNumber == 1)                                                            //按钮数为1时
        {
            if (i == ButtonState.unselected && j == 0)                                         //第一个按钮未被点击时，创建未点击贴图
            {
                ThisImage = Sprite.Create(imageUnselected, new Rect(ImageRect[5].x, ImageRect[5].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[5].sprite = ThisImage;
            }
            else if (i == ButtonState.selected && j == 0)                                      //第一个按钮被点击时，创建点击贴图
            {
                ThisImage = Sprite.Create(imageSelected, new Rect(ImageRect[5].x, ImageRect[5].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[5].sprite = ThisImage;
            }
            else if (i == ButtonState.PointInter && j == 0)                                    //光标进入第一个按钮范围是，创建进入贴图
            {
                ThisImage = Sprite.Create(imageInter, new Rect(ImageRect[5].x, ImageRect[5].y, imageUnselected.width * 0.02f, imageUnselected.height * 0.02f), new Vector2(0.5f, 0.5f));
                ButtonRenderer[5].sprite = ThisImage;
            }
        }
    }   
}
