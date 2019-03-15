using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 卡牌展示与点击检测
/// </summary>
public class NewShow : MonoBehaviour {
    public List<Sprite> AllImages = new List<Sprite>();
    public List<GameObject> AllCards = new List<GameObject>();
    public GameObject[] Items = new GameObject[7];     //图片显示组件
    public GameObject Left;
    public GameObject Right;
    public GameObject Use;
    public GameObject Close;
    public GameObject[] Node = new GameObject[7];   //移动节点
 
    public List<GameObject> Tests = new List<GameObject>();
    //  public  Sprite[] Sprites;
    public int ThisImage = 2;            //当前显示的图片在序列中的位数
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

    private RoleInfor NS_RI;
    private ScriptsManager NS_SM;
    private RayManager NS_RM;
    private PlayerData NS_PD;

    /// <summary>
    /// 初始化显示
    /// </summary>
    public void L_Start()
    {
        NS_SM = GameObject.Find("Main Camera").GetComponent<ScriptsManager>();
        NS_RI = NS_SM.RI;
        NS_RM = NS_SM.RM;
        NS_PD = NS_SM.PD;
        this.transform.DOScale(0, 0);
        Left.transform.DOScale(0, 0);
        Right.transform.DOScale(0, 0);
        Use.transform.DOScale(0, 0);
        Close.transform.DOScale(0, 0);
    }
    /// <summary>
    /// 得到卡牌实例
    /// </summary>
    public void GetCardsName(List<string> names,string which)
    {
        for (int i = 0; i < names.Count; i++)
        {
         //   Debug.Log("names[" + i + "] is " + names[i]);
            AllCards.Add(Resources.Load<GameObject>("Prefab/Equips/" + names[i]));
        } 
        switch (which)
        {
            case "EAT":
                Use.GetComponentInChildren<Text>().text = "吃掉";
                break;
            case "USE":
                Use.GetComponentInChildren<Text>().text = "使用";
                break;
            case "EQU":
                Use.GetComponentInChildren<Text>().text = "装备";
                break;
            case "XIE":
                Use.GetComponentInChildren<Text>().text = "卸下";

                break;
            default:
                break;
        }
        this.transform.DOScale(1, 0.2f);
        Left.transform.DOScale(1, 0.2f);
        Right.transform.DOScale(1, 0.2f);
        Use.transform.DOScale(1, 0.2f);
        Close.transform.DOScale(1, 0.2f);
        GetSprites(AllCards);
    }
    //只有一个参数的重载
    public void GetCardsName(string name, string which)
    {
        switch (which)
        {
            case "EAT":
                Use.GetComponentInChildren<Text>().text = "吃掉";
                break;
            case "USE":
                Use.GetComponentInChildren<Text>().text = "使用";
                break;
            case "EQU":
                Use.GetComponentInChildren<Text>().text = "装备";
                break;
            case "XIE":
                Use.GetComponentInChildren<Text>().text = "卸下";

                break;
            default :
                break;
        }
            AllCards.Add(Resources.Load<GameObject>("Prefab/Equips/" + name));
        this.transform.DOScale(1, 0.2f);
        Left.transform.DOScale(1, 0.2f);
        Right.transform.DOScale(1, 0.2f);
        Use.transform.DOScale(1, 0.2f);
        Close.transform.DOScale(1, 0.2f);
        GetSprites(AllCards);
    }
    public void CloseThis()
    {
        for (int i = 0; i < Items.Length; i++)
            if (Items[i] != null)
            {
                Destroy(Items[i].gameObject);
                Items[i] = new GameObject();
            }
        AllCards.Clear();
            this.transform.DOScale(0, 0.2f);
        Left.transform.DOScale(0, 0.2f);
        Right.transform.DOScale(0, 0.2f);
        Use.transform.DOScale(0, 0.2f);
        Close.transform.DOScale(0, 0.2f);
        NS_RI.CanTouch = true;
    }
 
    /// <summary>
    /// 得到需要展示的GameObject
    /// </summary>
    /// <param name="sprites"></param>
    public void GetSprites(List<GameObject> Cards)
    {
        //进行数据及定位的初始化
        ThisImage = 2;            //当前显示的图片在序列中的位数
        WhichImage_Left = 0;           //当前需要更换图片的Item
        WhichImage_Right = 6;           //当前需要更换图片的Item
        AllImages = null;
        Node_Items = new int[7] { 0, 1, 2, 3, 4, 5, 6 };
        for (int i = 0; i < 7; i++)
      {
          // Items[i] = new GameObject();
          Items[i].transform.position = Node[i].transform.position;
      }
        CanMove_Left = true;          //是否可向左移动
        CanMove_Right = true;          //是否可向右移动
        this.transform.DOScale(1, 0.3f);
        ///////////////////////
            AllCards = Cards;

            if (AllCards.Count > 0)
        {
            AllCards.Add(null);
            AllCards.Add(null);
           Debug.Log("Count is " + AllCards.Count);

            Left.transform.SetAsLastSibling();
            Right.transform.SetAsLastSibling();
            Use.transform.SetAsLastSibling();

            //↓左1的GameObject
         //   Items[0].transform.SetSiblingIndex(this.transform.childCount - 6);
            Items[0].transform.DOScale(0.3f, 0);
         //   Items[0].GetComponent<SpriteRenderer>().color = d;
            //↓左2的GameObject
       //     Items[1].transform.SetSiblingIndex(this.transform.childCount - 5);
            Items[1].transform.DOScale(0.4f, 0);
       //     Items[1].GetComponent<SpriteRenderer>().color = d;
            //↓左3的GameObject
        //    Items[2].transform.SetSiblingIndex(this.transform.childCount - 4);
            Items[2].transform.DOScale(0.5f, 0);
       //     Items[2].GetComponent<SpriteRenderer>().color = d;
            switch (AllCards.Count)
            {
                case 3:   //↓中间的GameObject
                    Items[3] = GameObject.Instantiate(AllCards[ThisImage - 2], Node[3].transform.position, AllCards[ThisImage - 2].transform.rotation) as GameObject;
                  //  Items[3].transform.SetParent(this.transform);

                    NS_PD.EquipInfors(Items[3]);


                    Items[3].transform.DOScale(0.6f, 0);
                 //   Items[3].GetComponent<SpriteRenderer>().color = a;
                        // Items[3].GetComponentsInChildren<TextMesh>().color = a;
                        //↓右1的GameObject
            
                    Items[4].transform.DOScale(0.5f, 0);
                //    Items[4].GetComponent<SpriteRenderer>().color = d;
                    //↓右2的GameObject
                    Items[5].transform.DOScale(0.4f, 0);
               //     Items[5].GetComponent<SpriteRenderer>().color = d;
                    //↓右3的GameObject
           
                    Items[6].transform.DOScale(0.3f, 0);
                 //   Items[6].GetComponent<SpriteRenderer>().color = d;
                    break;
                case 4: //↓中间的GameObject
                    Items[3] = GameObject.Instantiate(AllCards[ThisImage - 2], Node[3].transform.position, AllCards[ThisImage - 2].transform.rotation) as GameObject;
               //    Items[3].transform.SetParent(this.transform);
                    NS_PD.EquipInfors(Items[3]);
                    Items[3].transform.DOScale(0.6f, 0);
                //    Items[3].GetComponent<SpriteRenderer>().color = a;
                    //↓右1的GameObject
                    Items[4] = GameObject.Instantiate(AllCards[ThisImage - 1], Node[4].transform.position, AllCards[ThisImage - 1].transform.rotation) as GameObject;
                    NS_PD.EquipInfors(Items[4]);
                //    Items[4].transform.SetParent(this.transform);
                    Items[4].transform.DOScale(0.5f, 0);
                //    Items[4].GetComponent<SpriteRenderer>().color = b;
                    //↓右2的GameObject
            
                    Items[5].transform.DOScale(0.4f, 0);
                //    Items[5].GetComponent<SpriteRenderer>().color = d;
                    //↓右3的GameObject
              
                    Items[6].transform.DOScale(0.3f, 0);
                 //   Items[6].GetComponent<SpriteRenderer>().color = d;
                    break;
                case 5://↓中间的GameObject
                    Items[3] = GameObject.Instantiate(AllCards[ThisImage - 2], Node[3].transform.position, AllCards[ThisImage - 2].transform.rotation) as GameObject;
                    NS_PD.EquipInfors(Items[3]);
                 //   Items[3].transform.SetParent(this.transform);
                    Items[3].transform.DOScale(0.6f, 0);
               //     Items[3].GetComponent<SpriteRenderer>().color = a;
                    //↓右1的GameObject
                    Items[4] = GameObject.Instantiate(AllCards[ThisImage - 1], Node[4].transform.position, AllCards[ThisImage - 1].transform.rotation) as GameObject;
                    NS_PD.EquipInfors(Items[4]);
              //     Items[4].transform.SetParent(this.transform); 
                    Items[4].transform.DOScale(0.5f, 0);
                //    Items[4].GetComponent<SpriteRenderer>().color = b;
                    //↓右2的GameObject
                    Items[5] = GameObject.Instantiate(AllCards[ThisImage], Node[5].transform.position, AllCards[ThisImage].transform.rotation) as GameObject;
                    Items[5].transform.DOScale(0.4f, 0);
                    NS_PD.EquipInfors(Items[5]);
            //       Items[5].transform.SetParent(this.transform);
                //    Items[5].GetComponent<SpriteRenderer>().color = c;
                    //↓右3的GameObject
                    Items[6].transform.DOScale(0.3f, 0);
                //    Items[6].GetComponent<SpriteRenderer>().color = d;
                    break;
                default:
                    //↓中间的GameObject
                    Items[3] = GameObject.Instantiate(AllCards[ThisImage - 2], Node[3].transform.position, AllCards[ThisImage - 2].transform.rotation) as GameObject;
                    NS_PD.EquipInfors(Items[3]);
                //    Items[3].transform.SetParent(this.transform);
                    Items[3].transform.DOScale(0.6f, 0);
               //     Items[3].GetComponent<SpriteRenderer>().color = a;
                    //↓右1的GameObject
                    Items[4] = GameObject.Instantiate(AllCards[ThisImage - 1], Node[4].transform.position, AllCards[ThisImage - 1].transform.rotation) as GameObject;
                    NS_PD.EquipInfors(Items[4]);
              //     Items[4].transform   NS_PD.EquipInfors(Items[3].name);.SetParent(this.transform);
                    Items[4].transform.DOScale(0.5f, 0);
                //    Items[4].GetComponent<SpriteRenderer>().color = b;
                    //↓右2的GameObject
           
                    Items[5] = GameObject.Instantiate(AllCards[ThisImage], Node[5].transform.position, AllCards[ThisImage].transform.rotation) as GameObject;
                    Items[5].transform.DOScale(0.4f, 0);
                    NS_PD.EquipInfors(Items[5]);
               //    Items[5].transform.SetParent(this.transform);
                //    Items[5].GetComponent<SpriteRenderer>().color = c;
                    //↓右3的GameObject
          
                    Items[6] = GameObject.Instantiate(AllCards[ThisImage + 1], Node[6].transform.position, AllCards[ThisImage + 1].transform.rotation) as GameObject;
                    NS_PD.EquipInfors(Items[6]);
                    Items[6].transform.DOScale(0.3f, 0);
            //       Items[6].transform.SetParent(this.transform);
                  
                 //   Items[6].GetComponent<SpriteRenderer>().color = d;
                    break;
            }
        }

    }

    
    /// <summary>
    /// 延时方法
    /// </summary>
    /// <returns></returns>
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(MoveTime + 0.05f);
        CanMove_Left = CanMove_Right = true;
    }
    /// <summary>
    /// 向左移动的按钮
    /// </summary>
    public void ToLeft()
    {
     //   Debug.Log("ThisImage is "+ThisImage);
        if (ThisImage < AllCards.Count - 1 && CanMove_Left)
            ThisImage += 1;
        else
            CanMove_Left = false;
        if (CanMove_Left)
        {
            CanMove_Left = CanMove_Right = false;
            StartCoroutine(Wait());
            for (int i = 0; i < 7; i++)
            {
                Node_Items[i] = Node_Items[i] - 1 < 0 ? 6 : (Node_Items[i] - 1);
                Items[i].transform.DOMove(Node[Node_Items[i]].transform.position, MoveTime);
                switch (Node_Items[i] - 3)
                {
                    case -3:
                     //   Items[i] = null;
                        Items[i].transform.DOScale(0.3f, MoveTime);
                      //  if (ThisImage > 4 && Items[i]!= null)
                       //     Items[i].transform.DOScale(0.3f, 0);
                   //     Items[i].transform.SetSiblingIndex(this.transform.childCount - 6);
                        break;
                    case -2:
                        Items[i].transform.DOScale(0.4f, MoveTime);
                   //   if (ThisImage > 4 && Items[i] != null)
                       //     Items[i].GetComponent<Image>().DOColor(c, MoveTime);
                  //      Items[i].transform.SetSiblingIndex(this.transform.childCount - 5);
                        break;
                    case -1:
                        Items[i].transform.DOScale(0.5f, MoveTime);
                   //     Items[i].GetComponent<Image>().DOColor(b, MoveTime);
                   //     Items[i].transform.SetSiblingIndex(this.transform.childCount - 4);
                        break;
                    case 0:
                        Items[i].transform.DOScale(0.6f, MoveTime);
                     //   Items[i].GetComponent<Image>().DOColor(a, MoveTime);
                    //    Items[i].transform.SetSiblingIndex(this.transform.childCount - 3);
                        break;
                    case 1:
                     //   Items[i].transform.DOScale(0.5f, MoveTime);
                        if (ThisImage < AllCards.Count && Items[i] != null && Items[i].transform.localScale != new Vector3(0, 0, 0))
                            Items[i].transform.DOScale(0.5f, MoveTime);
                        else
                        {
                         //   Destroy(Items[i].gameObject);
                        }
                     //   Items[i].transform.SetSiblingIndex(this.transform.childCount - 4);
                        break;
                    case 2:

                        if (ThisImage < AllCards.Count && Items[i] != null && Items[i].transform.localScale != new Vector3(0, 0, 0))
                          Items[i].transform.DOScale(0.4f, MoveTime);
                      else
                      {
                         // Destroy(Items[i].gameObject);
                      }
                   //     Items[i].transform.SetSiblingIndex(this.transform.childCount - 5);
                        break;
                    case 3:
                     //   Items[i].GetComponent<Image>().color = d;
                     //   Items[i] = null;
                   //     Debug.Log("ThisImage is "+ThisImage);
                        Items[i].transform.DOScale(0.3f, MoveTime);
                        if (ThisImage + 1 < AllCards.Count && AllCards[ThisImage + 1] != null)
                        {
                            Destroy(Items[i].gameObject);
                            Items[i] = GameObject.Instantiate(AllCards[ThisImage + 1], Node[Node_Items[i]].transform.position, AllCards[ThisImage + 1].transform.rotation) as GameObject;
                            NS_PD.EquipInfors(Items[i]);
                            Items[i].transform.SetParent(this.transform);
                            Items[i].transform.DOScale(0.3f, 0);
                        }
                        else
                        {
                        //    Debug.Log("Is All Over");i
                            Destroy(Items[i].gameObject);
                           Items[i]=new GameObject();
                        }
                  //      Items[i].transform.DOScale(0.3f, 0);
                    //    Items[i].transform.SetSiblingIndex(this.transform.childCount - 6);
                        break;
                    default:
                        break;
                }
            }
            WhichImage_Left += 1;
            WhichImage_Right += 1;
            if (WhichImage_Left > 6)
                WhichImage_Left = 0;
            if (WhichImage_Right > 6)
                WhichImage_Right = 0;
        }
    }

    /// <summary>
    /// 向右的按钮
    /// </summary>
    public void ToRight()
    {
        if (ThisImage - 2 > 0 && CanMove_Right)
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
                    //    Items[i] = null;
                        Items[i].transform.DOScale(0f, MoveTime);
                     //   Items[i].GetComponent<Image>().DOColor(d, MoveTime);
                     //   Items[i].transform.SetSiblingIndex(this.transform.childCount - 6);
                        break;
                    case 2:
                        Items[i].transform.DOScale(0.4f, MoveTime);
                      if (AllCards[ThisImage] != null)
                           Items[i].GetComponent<Image>().DOColor(c, MoveTime);
                   //     Items[i].transform.SetSiblingIndex(this.transform.childCount - 5);
                        break;
                    case 1:
                        Items[i].transform.DOScale(0.5f, MoveTime);
                   //     Items[i].GetComponent<Image>().DOColor(b, MoveTime);
                   //     Items[i].transform.SetSiblingIndex(this.transform.childCount - 4);
                        break;
                    case 0:
                        Items[i].transform.DOScale(0.6f, MoveTime);
                    //    Items[i].GetComponent<Image>().DOColor(a, MoveTime);
                    //    Items[i].transform.SetSiblingIndex(this.transform.childCount - 3);
                        break;
                    case -1:

                        if (Items[i] != null && Items[i].transform.localScale != new Vector3(0, 0, 0))
                            Items[i].transform.DOScale(0.5f, MoveTime);
                 //       Items[i].transform.SetSiblingIndex(this.transform.childCount - 4);
                        break;
                    case -2:
          
                       if (Items[i]!= null&&Items[i].transform.localScale!=new Vector3(0,0,0))
                           Items[i].transform.DOScale(0.4f, MoveTime);
                  //      Items[i].transform.SetSiblingIndex(this.transform.childCount - 5);
                        break;
                    case -3:
                        //   Items[i].GetComponent<Image>().color = d;
                      //  Items[i] = null;

                        if (ThisImage - 4 > 0)
                        {
                            Items[i] = GameObject.Instantiate(AllCards[ThisImage - 5], Node[Node_Items[i]].transform.position, AllCards[ThisImage - 5].transform.rotation) as GameObject;
                            NS_PD.EquipInfors(Items[i]);
                            Items[i].transform.SetParent(this.transform);
                        }
                        else
                        {
                            Destroy(Items[i].gameObject);
                            Items[i] = new GameObject();
                        }
                       Items[i].transform.DOScale(0.3f, 0);
                  //     Items[i].transform.SetSiblingIndex(this.transform.childCount - 6);
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
