using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIMove : MonoBehaviour,IDragHandler,IEndDragHandler,IPointerDownHandler,IPointerUpHandler,IPointerExitHandler,IPointerEnterHandler{
    public static RectTransform canvas;                    //得到canvas的ugui坐标
    private RectTransform imgRect;                         //得到图片的ugui坐标
    Vector2 offset = new Vector3();                        //用来得到鼠标和图片的差值
    Vector3 imgReduceScale = new Vector3(0.8f, 0.8f, 1);   //设置图片缩放
    Vector3 imgNormalScale = new Vector3(1, 1, 1);         //正常大小
    Vector2 uguiPos = new Vector2();                       //用来接收转换后的拖动坐标
    Vector2 X = new Vector2();                             //X轴向量
    Vector2 Y = new Vector2();                             //Y轴向量
    Vector2 EndX = new Vector2();                          //飞出点X
    Vector2 EndY = new Vector2();                          //飞出点Y
   public Vector2 StartPos;                                //起始位置
    Vector2 TouchPos = new Vector2();
    private Image ThisImage;
    void Start()
    {
        ThisImage = this.gameObject.GetComponent<Image>();
        imgRect = GetComponent<RectTransform>();
        StartPos =new Vector2( ThisImage.transform.position.x,ThisImage.transform.position.y);
    }
    public void OnPointerDown(PointerEventData eventData)  //当鼠标按下时调用 接口对应  IPointerDownHandler
    {
        Vector2 mouseDown = eventData.position;            //记录鼠标按下时的屏幕坐标
        Vector2 mouseUguiPos = new Vector2();              //定义一个接收返回的ugui坐标
        bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, mouseDown, eventData.enterEventCamera, out mouseUguiPos);
        if (isRect)                                        //如果在
        {
          offset = imgRect.anchoredPosition - mouseUguiPos;//计算图片中心和鼠标点的差值
        }
    }
    public void OnDrag(PointerEventData eventData)         //当鼠标拖动时调用   对应接口 IDragHandler 
    {
        Vector2 mouseDrag = eventData.position;            //当鼠标拖动时的屏幕坐标
        bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, mouseDrag, eventData.enterEventCamera, out uguiPos);
        if (isRect)
        {
            
            if (uguiPos.x > uguiPos.y && uguiPos.x >= 0)   //当拖动方向在X正半轴上时
            {
                X.x = uguiPos.x;
                imgRect.anchoredPosition =  X;
                if (X.x >= 150)
                {
                    Tweener Return = ThisImage.rectTransform.DOMove(StartPos, 0.5f);
                    Return.SetEase(Ease.InOutQuad);
                }
            }
            if (uguiPos.y > uguiPos.x && uguiPos.y>= 0)    //当拖动方向在Y正半轴上时
            {
                Y.y = uguiPos.y;
                if (Y.y >= 150)
                {
                    Tweener Return = ThisImage.rectTransform.DOMove(StartPos, 0.5f);
                    Return.SetEase(Ease.InOutQuad);
                }
                imgRect.anchoredPosition =  Y;
            }
            if (uguiPos.x < uguiPos.y && uguiPos.x<= 0)    //当拖动方向在X负半轴上时
            {
                X.x = uguiPos.x;
                imgRect.anchoredPosition =  X;
                if (X.x <= -150)
                {
                    Tweener Return = ThisImage.rectTransform.DOMove(StartPos, 0.5f);
                    Return.SetEase(Ease.InOutQuad);
                }
            }
            if (uguiPos.y< uguiPos.x && uguiPos.y <= 0)    //当拖动方向在Y负半轴上时
            {
              Y.y= uguiPos.y;
              imgRect.anchoredPosition =  Y;
              if (Y.y <= -150)
              {
                  Tweener Return = ThisImage.rectTransform.DOMove(StartPos, 0.5f);
                  Return.SetEase(Ease.InOutQuad);
              }
            } 
           
        }
    }

    //当鼠标抬起时调用  对应接口  IPointerUpHandler
    public void OnPointerUp(PointerEventData eventData)
    {
        offset = Vector2.zero;
    }

    //当鼠标结束拖动时调用   对应接口  IEndDragHandler
    public void OnEndDrag(PointerEventData eventData)
    {
     
        offset = Vector2.zero;
        if (X.x < 60 && X.x >0)           //当拖拽距离不足时返回
        {
            Tweener Return = ThisImage.rectTransform.DOMove(StartPos, 0.5f);
            Return.SetEase(Ease.InOutQuad);
        }
        else if (Y.y < 60 && Y.y > 0)
        {
            Tweener Return = ThisImage.rectTransform.DOMove(StartPos, 0.5f);
            Return.SetEase(Ease.InOutQuad);
        }
        else if (X.x > -60 && X.x < 0)
        {
            Tweener Return = ThisImage.rectTransform.DOMove(StartPos, 0.5f);
            Return.SetEase(Ease.InOutQuad);
        }
        else if (Y.y >- 60 && Y.y < 0)
        {
            Tweener Return = ThisImage.rectTransform.DOMove(StartPos, 0.5f);
            Return.SetEase(Ease.InOutQuad);
        }

        if (X.x >= 60 && X.x < 100)                 //当拖拽距离达到一定值时飞出
        {
            EndX.x = StartPos.x + 300;
            EndX.y = 263.5f;
            Tweener GoOut = ThisImage.rectTransform.DOMove(EndX, 0.4f);
                GoOut.SetEase(Ease.OutQuad);
                GoOut.OnComplete( delegate() {  Destroy(this.gameObject); });    
        }
        else if (Y.y>= 60 && Y.y < 100)
        {
            EndY.y = StartPos.y + 500;
            EndY.x = 164.5f;
            Tweener GoOut = ThisImage.rectTransform.DOMove(EndY, 0.4f);
            GoOut.SetEase(Ease.OutQuad);
            GoOut.OnComplete(delegate() { Destroy(this.gameObject); });    
        }
        else if (X.x <= -60 && X.x > -100)
        {
            EndX.x = StartPos.x - 500;
            EndX.y = 263.5f;
            Tweener GoOut = ThisImage.rectTransform.DOMove(EndX, 0.4f);
            GoOut.SetEase(Ease.OutQuad);
            GoOut.OnComplete(delegate() { Destroy(this.gameObject); });    
        }
        else if (Y.y <= -60 && Y.y > -100)
        {
            EndY.y = StartPos.y - 500;
            EndY.x = 164.5f;
            Tweener GoOut = ThisImage.rectTransform.DOMove(EndY, 0.4f);
            GoOut.SetEase(Ease.OutQuad);
            GoOut.OnComplete(delegate() { Destroy(this.gameObject); });    
        }
    }
    public void GetCanves()
    {
      
    }
    //当鼠标进入图片时调用   对应接口   IPointerEnterHandler
    public void OnPointerEnter(PointerEventData eventData)
    {
       // imgRect.localScale = imgReduceScale;   //缩小图片
    }

    //当鼠标退出图片时调用   对应接口   IPointerExitHandler
    public void OnPointerExit(PointerEventData eventData)
    {
     //   imgRect.localScale = imgNormalScale;   //回复图片
    }
    void OnGUI()
    {
     
      
    }
    public void UpDate()
    {
   
    }
}
