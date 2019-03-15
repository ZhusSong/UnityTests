using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class ShowAndMove : MonoBehaviour {
    public List<Sprite> AllImages = new List<Sprite>();
    public GameObject[] Items=new GameObject[7];     //图片显示组件
    public GameObject Left;
    public GameObject Right;
    public GameObject Use;
    public GameObject[] Node = new GameObject[7];   //移动节点
    public Sprite[] Tests = new Sprite[10];
  //  public  Sprite[] Sprites;
    private int ThisImage = 2;            //当前显示的图片在序列中的位数
    private int WhichImage_Left = 0;           //当前需要更换图片的Item
    private int WhichImage_Right = 6;           //当前需要更换图片的Item
    private float MoveTime = 0.6f;         //每一次移动的时间
    private bool CanMove_Left = true;          //是否可向左移动
    private bool CanMove_Right = true;          //是否可向右移动
    public Camera cam;

    public int[] Node_Items = new int[7] { 0, 1, 2, 3, 4, 5, 6 };//Items的节点下标
	// Use this for initialization
    Color a = new Color(1, 1, 1, 1);
    Color b = new Color(1, 1, 1, 0.85f);
    Color c = new Color(1, 1, 1, 0.7f);
    Color d = new Color(1, 1, 1, 0f);

    private RoleInfor SAM_RI;
    private ScriptsManager SAM_SM;
    private RayManager SAM_RM;
  
    /// <summary>
    /// 得到卡牌实例
    /// </summary>
    public void GetCards()
    {
    }
    /// <summary>
    /// 得到需要展示的图片
    /// </summary>
    /// <param name="sprites"></param>
    public void GetSprites(List<Sprite> sprites)
    {
        //进行数据及定位的初始化
        ThisImage = 2;            //当前显示的图片在序列中的位数
        WhichImage_Left = 0;           //当前需要更换图片的Item
        WhichImage_Right = 6;           //当前需要更换图片的Item
        AllImages = null;
        Node_Items = new int[7] { 0, 1, 2, 3, 4, 5, 6 };
        for (int i = 0; i < 7; i++)
        {
            Items[i].GetComponent<Image>().sprite = null;
            Items[i].GetComponent<RectTransform>().anchoredPosition = Node[i].GetComponent<RectTransform>().anchoredPosition;
        }
        CanMove_Left = true;          //是否可向左移动
        CanMove_Right = true;          //是否可向右移动
        this.transform.DOScale(1, 0.3f);
                 ///////////////////////
                 AllImages = sprites;
            if (AllImages.Count > 0)
            {
            AllImages.Add(null);
            AllImages.Add(null);
            Debug.Log("Count is "+AllImages.Count);
            Left.transform.SetAsLastSibling();
            Right.transform.SetAsLastSibling();
            Use.transform.SetAsLastSibling();
            //↓左1的图片
            Items[0].transform.SetSiblingIndex(this.transform.childCount - 6);
            Items[0].transform.DOScale(0.7f, 0);
            Items[0].GetComponent<Image>().color = d;
            //↓左2的图片
            Items[1].transform.SetSiblingIndex(this.transform.childCount - 5);
            Items[1].transform.DOScale(0.8f, 0);
            Items[1].GetComponent<Image>().color = d;
            //↓左3的图片
            Items[2].transform.SetSiblingIndex(this.transform.childCount - 4);
            Items[2].transform.DOScale(0.9f, 0);
            Items[2].GetComponent<Image>().color = d;
            switch (AllImages.Count)
            {
                case 3:   //↓中间的图片
                    Items[3].GetComponent<Image>().sprite = AllImages[ThisImage - 2];
                    Items[3].transform.SetSiblingIndex(this.transform.childCount - 3);
                    Items[3].transform.DOScale(1f, 0);
                    Items[3].GetComponent<Image>().color = a;
                    //↓右1的图片
                    Items[4].transform.SetSiblingIndex(this.transform.childCount - 4);
                    Items[4].transform.DOScale(0.9f, 0);
                    Items[4].GetComponent<Image>().color = d;
                    //↓右2的图片
                    Items[5].transform.DOScale(0.8f, 0);
                    Items[5].GetComponent<Image>().color = d;
                    //↓右3的图片
                    Items[6].transform.SetSiblingIndex(this.transform.childCount - 6);
                    Items[6].transform.DOScale(0.7f, 0);
                    Items[6].GetComponent<Image>().color = d;
                    break;
                case 4: //↓中间的图片
                    Items[3].GetComponent<Image>().sprite = AllImages[ThisImage - 2];
                    Items[3].transform.SetSiblingIndex(this.transform.childCount - 3);
                    Items[3].transform.DOScale(1f, 0);
                    Items[3].GetComponent<Image>().color = a;
                    //↓右1的图片
                    Items[4].GetComponent<Image>().sprite = AllImages[ThisImage - 1];
                    Items[4].transform.SetSiblingIndex(this.transform.childCount - 4);
                    Items[4].transform.DOScale(0.9f, 0);
                    Items[4].GetComponent<Image>().color = b;
                    //↓右2的图片
                    Items[5].transform.SetSiblingIndex(this.transform.childCount - 5);
                    Items[5].transform.DOScale(0.8f, 0);
                    Items[5].GetComponent<Image>().color = d;
                    //↓右3的图片
                    Items[6].transform.SetSiblingIndex(this.transform.childCount - 6);
                    Items[6].transform.DOScale(0.7f, 0);
                    Items[6].GetComponent<Image>().color = d;
                    break;
                case 5://↓中间的图片
                    Items[3].GetComponent<Image>().sprite = AllImages[ThisImage - 2];
                    Items[3].transform.SetSiblingIndex(this.transform.childCount - 3);
                    Items[3].transform.DOScale(1f, 0);
                    Items[3].GetComponent<Image>().color = a;
                    //↓右1的图片
                    Items[4].GetComponent<Image>().sprite = AllImages[ThisImage - 1];
                    Items[4].transform.SetSiblingIndex(this.transform.childCount - 4);
                    Items[4].transform.DOScale(0.9f, 0);
                    Items[4].GetComponent<Image>().color = b;
                    //↓右2的图片
                    Items[5].transform.SetSiblingIndex(this.transform.childCount - 5);
                    Items[5].GetComponent<Image>().sprite = AllImages[ThisImage];
                    Items[5].transform.DOScale(0.8f, 0);
                    Items[5].GetComponent<Image>().color = c;
                    //↓右3的图片
                    Items[6].transform.SetSiblingIndex(this.transform.childCount - 6);
                    Items[6].transform.DOScale(0.7f, 0);
                    Items[6].GetComponent<Image>().color = d;
                    break;
                default:
                    //↓中间的图片
                    Items[3].GetComponent<Image>().sprite = AllImages[ThisImage - 2];
                    Items[3].transform.SetSiblingIndex(this.transform.childCount - 3);
                    Items[3].transform.DOScale(1f, 0);
                    Items[3].GetComponent<Image>().color = a;
                    //↓右1的图片
                    Items[4].GetComponent<Image>().sprite = AllImages[ThisImage - 1];
                    Items[4].transform.SetSiblingIndex(this.transform.childCount - 4);
                    Items[4].transform.DOScale(0.9f, 0);
                    Items[4].GetComponent<Image>().color = b;
                    //↓右2的图片
                    Items[5].transform.SetSiblingIndex(this.transform.childCount - 5);
                    Items[5].GetComponent<Image>().sprite = AllImages[ThisImage];
                    Items[5].transform.DOScale(0.8f, 0);
                    Items[5].GetComponent<Image>().color = c;
                    //↓右3的图片
                    Items[6].transform.SetSiblingIndex(this.transform.childCount - 6);
                    Items[6].transform.DOScale(0.7f, 0);
                    Items[6].GetComponent<Image>().sprite = AllImages[ThisImage + 1];
                    Items[6].GetComponent<Image>().color = d;
                    break;
            }
        }
          
    }

    /// <summary>
    /// 初始化显示
    /// </summary>
	public void L_Start () {
        SAM_SM = GameObject.Find("Main Camera").GetComponent<ScriptsManager>();
        SAM_RI = SAM_SM.RI;
        SAM_RM = SAM_SM.RM;
        this.transform.DOScale(0, 0);
	}
   /// <summary>
   /// 延时方法
   /// </summary>
   /// <returns></returns>
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(MoveTime+0.05f);
        CanMove_Left = CanMove_Right = true;
    }
    public void CloseThis()
   {
        this.transform.DOScale(0, 0.1f);
        SAM_RI.CanTouch = true;
    
   
    }
    /// <summary>
    /// 向左移动的按钮
    /// </summary>
    public void ToLeft()
    {
        if (ThisImage < AllImages.Count - 1&&CanMove_Left)
            ThisImage += 1;
        else
            CanMove_Left = false;
        if (CanMove_Left)
        {
            CanMove_Left =CanMove_Right= false;
            StartCoroutine(Wait());
            for (int i = 0; i < 7; i++)
            {
                    Node_Items[i] = Node_Items[i] - 1 < 0 ? 6 : (Node_Items[i] - 1);
                    Items[i].transform.DOMove(Node[Node_Items[i]].transform.position, MoveTime);
                    switch (Node_Items[i] - 3)
                    {
                        case -3:
                            Items[i].transform.DOScale(0.7f, MoveTime);
                            if (ThisImage > 4 && Items[i].GetComponent<Image>().sprite!=null)
                                Items[i].GetComponent<Image>().DOColor(d, MoveTime);
                            Items[i].transform.SetSiblingIndex(this.transform.childCount - 6);
                            break;
                        case -2:
                            Items[i].transform.DOScale(0.8f, MoveTime);
                            if (ThisImage > 4 && Items[i].GetComponent<Image>().sprite != null)
                                Items[i].GetComponent<Image>().DOColor(c, MoveTime);
                            Items[i].transform.SetSiblingIndex(this.transform.childCount - 5);
                            break;
                        case -1:
                            Items[i].transform.DOScale(0.9f, MoveTime);
                            Items[i].GetComponent<Image>().DOColor(b, MoveTime);
                            Items[i].transform.SetSiblingIndex(this.transform.childCount - 4);
                            break;
                        case 0:
                            Items[i].transform.DOScale(1f, MoveTime);
                            Items[i].GetComponent<Image>().DOColor(a, MoveTime);
                            Items[i].transform.SetSiblingIndex(this.transform.childCount - 3);
                            break;
                        case 1:
                            Items[i].transform.DOScale(0.9f, MoveTime);
                            if (ThisImage < AllImages.Count && Items[i].GetComponent<Image>().sprite != null)
                                Items[i].GetComponent<Image>().DOColor(c, MoveTime);
                            else
                                Items[i].GetComponent<Image>().color = d;
                            Items[i].transform.SetSiblingIndex(this.transform.childCount - 4);
                            break;
                        case 2:
                            Items[i].transform.DOScale(0.8f, MoveTime);
                            if (ThisImage < AllImages.Count && Items[i].GetComponent<Image>().sprite != null)
                                Items[i].GetComponent<Image>().DOColor(c, MoveTime);
                            else
                                Items[i].GetComponent<Image>().color = d;
                            Items[i].transform.SetSiblingIndex(this.transform.childCount - 5);
                            break;
                        case 3:
                                Items[i].GetComponent<Image>().color = d;
                                Items[i].transform.DOScale(0.7f, MoveTime);
                                if (ThisImage + 1 < AllImages.Count)
                                    Items[i].GetComponent<Image>().sprite = AllImages[ThisImage+1];
                            Items[i].transform.SetSiblingIndex(this.transform.childCount - 6);
                            break;
                        default:
                            break;
                    }
            }
            WhichImage_Left += 1;
            WhichImage_Right += 1;
            if (WhichImage_Left > 6)
                WhichImage_Left = 0;
            if (WhichImage_Right >6)
                WhichImage_Right = 0;
        }
    }

    /// <summary>
    /// 向右的按钮
    /// </summary>
    public void ToRight()
    {
        if (ThisImage - 2 > 0&&CanMove_Right)
            ThisImage -= 1;
        else
            CanMove_Right = false;
        if (CanMove_Right)
        {
            CanMove_Left = CanMove_Right = false;
            StartCoroutine(Wait());
            for (int i = 0; i < 7; i++)
            {
                Node_Items[i] = Node_Items[i] + 1 > 6 ? 0 : (Node_Items[i] + 1);
                Items[i].transform.DOMove(Node[Node_Items[i]].transform.position, MoveTime);
                switch (Node_Items[i] - 3)
                {
                    case 3:
                        Items[i].transform.DOScale(0.7f, MoveTime);
                            Items[i].GetComponent<Image>().DOColor(d, MoveTime);
                        Items[i].transform.SetSiblingIndex(this.transform.childCount - 6);
                        break;
                    case 2:
                        Items[i].transform.DOScale(0.8f, MoveTime);
                        if (AllImages[ThisImage]!=null)
                            Items[i].GetComponent<Image>().DOColor(c, MoveTime);
                        Items[i].transform.SetSiblingIndex(this.transform.childCount - 5);
                        break;
                    case 1:
                        Items[i].transform.DOScale(0.9f, MoveTime);
                        Items[i].GetComponent<Image>().DOColor(b, MoveTime);
                        Items[i].transform.SetSiblingIndex(this.transform.childCount - 4);
                        break;
                    case 0:
                        Items[i].transform.DOScale(1f, MoveTime);
                        Items[i].GetComponent<Image>().DOColor(a, MoveTime);
                        Items[i].transform.SetSiblingIndex(this.transform.childCount - 3);
                        break;
                    case -1:
                        Items[i].transform.DOScale(0.9f, MoveTime);
                        if (Items[i].GetComponent<Image>().sprite != null)
                            Items[i].GetComponent<Image>().DOColor(c, MoveTime);
                        Items[i].transform.SetSiblingIndex(this.transform.childCount - 4);
                        break;
                    case -2:
                        Items[i].transform.DOScale(0.8f, MoveTime);
                        if (Items[i].GetComponent<Image>().sprite!=null)
                            Items[i].GetComponent<Image>().DOColor(c, MoveTime);
                        Items[i].transform.SetSiblingIndex(this.transform.childCount - 5);
                        break;
                    case -3:
                            Items[i].GetComponent<Image>().color = d;
                            Items[i].transform.DOScale(0.7f, MoveTime);
                            if (ThisImage - 4> 0)
                                Items[i].GetComponent<Image>().sprite = AllImages[ThisImage - 5];
                            else
                                Items[i].GetComponent<Image>().sprite = null;
                        Items[i].transform.SetSiblingIndex(this.transform.childCount - 6);
                        break;
                    default:
                        break;
                }
            }
            WhichImage_Right -= 1;
            WhichImage_Left -= 1;
            if (WhichImage_Left < 0)
                WhichImage_Left = 6;
            if (WhichImage_Right < 0)
                WhichImage_Right = 6;

        }
    }
}
