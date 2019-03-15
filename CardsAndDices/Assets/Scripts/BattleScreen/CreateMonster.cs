using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
//↓进行战斗前的发怪物牌，玩家准备好后通知BattleSystem进行战斗
public class CreateMonster : MonoBehaviour {
    public GameObject MonsterPoint;                //怪物卡牌出现的起点物体，用于记录起点坐标
    public GameObject[] Monsters;                  //本次的怪物
    public GameObject FinishPoint;                 //怪物卡牌的终点坐标
    public GameObject monster;                     //存储怪物信息的对象
    public GameObject DelayObject;                 //延时物体，用于进行DoTween动作达到延时目的
    public GameObject Node;                        //怪物牌旋转节点
    public GameObject CloseButton;                 //关闭展示按钮
    private float[] X_FP=new float[3];             //终点的X坐标
    private float[] Y_FP = new float[3];           //终点的Y坐标
    private float[] Cards_Rotate = new float[3];   //卡牌出现时的旋转角度
    private bool CanCreate = false;                 //是否可进行创建
    private int LorMorR;                           //左或中或右
    private int Number = 0;                        //已创建的怪物数量
    private int Which = 0;                         //创建的是哪个怪物
    private int ThisNumber;                        //本次需要创建的怪物数量
    private int ShowNumber;                        //本次展示的怪物是第几个
    private Vector3 ShowPoint;                     //本次展示的怪物原坐标
    private bool CanShow = false;                  //是否可进行点击检测

    //怪物数据 0:等级 1:HP 2:行动力 3:攻击力 4:防御力 5:暴击
    public int[][] MonsterData_CM= new int[3][];
    private int[] Max = new int[3];                //怪物出现的顺序数组

    public BattleSystem CrMon_BS;                   //调用Battle()
    public AllMonsters CrMon_Mon;                 //调用AllMonsters()
    public NewButton CrMon_NewBut;                //调用NewButton()
    public ScriptsManager CrMon_SM;
   public  MonsterData CrMon_MD;

    public TextMesh[][] MonstersText=new TextMesh[3][];
    //↓实例化
   public  void L_Start()
    {
        CrMon_SM = GameObject.Find("Main Camera").GetComponent<ScriptsManager>();
        monster = GameObject.Find("monster");
        CrMon_BS = CrMon_SM.BS;
        CrMon_Mon = monster.GetComponent<AllMonsters>();
        CrMon_NewBut = CrMon_SM.NewBut;
        CrMon_MD = CrMon_SM.MD;

        CloseButton.transform.DOScale(0, 0.3f);
        /////以下是测试数据
    //    GetMonstersData_CM(0, 5, 5, 5, 5, 10);
     //   GetMonstersData_CM(1, 4, 4,4, 4, 10);
    }

    /// <summary>
    /// 得到怪物数据
    /// </summary>
    /// <param name="which">下标</param>
    /// <param name="HP">血量</param>
    /// <param name="ActNum">行动力</param>
    /// <param name="Atk">攻击力</param>
    /// <param name="Def">防御力</param>
    /// <param name="Cri">暴击率</param>
    /// <param name="Acc">命中率</param>
  //  public void GetMonstersData_CM(int which, int HP, int actNum, int atk, int def, int cri)
  //  {
  //      MonsterData_CM[which] = new int[] { HP, actNum, atk, def, cri };
        // Debug.Log(which+"'s ActionMunber is"+MonsterData[which][1]);
  //  }
    //↓可以创建怪物，开启协程
   public void CloseThis()
   {
       CloseButton.transform.DOScale(0, 0.4f);
       Monsters[ShowNumber].transform.DOMove(ShowPoint, 0.4f);
       Monsters[ShowNumber].transform.DOScale(0.4f, 0.4f);
       Monsters[ShowNumber].transform.DORotate(new Vector3(0, 0, Cards_Rotate[ShowNumber]), 0.4f);
   }
    public void CanCreateMonster(int number,string[] monsters)
   {
     //  for (int i = 0; i < number; i++)
    //   {
     //      MonsterData_CM[i] = CrMon_MD.GiveMonsterData(monsters[i]);
     //  }

            StartCoroutine(Wait(number, monsters));
    }
    void Update()
    {
   
        if (Input.GetMouseButtonUp(0)&&CanShow)                                    //鼠标抬起时
        {
            Ray ray_CM = Camera.main.ScreenPointToRay(Input.mousePosition);   //发出射线，判断是否点击到了卡牌
            RaycastHit hit_CM = new RaycastHit();                                   //鼠标抬起时不进行拖拽判断
            if (Physics.Raycast(ray_CM,out hit_CM))           //X正方向飞出
            {
                for (int i = 0; i < ThisNumber; i++)
                {
                    if (hit_CM.transform == Monsters[i].transform)
                    {
                        ShowNumber = i;
                        ShowPoint = Monsters[i].transform.position;
                        CloseButton.transform.DOScale(1, 0.4f);
                     //   Debug.Log("hit name is " + hit_CM.transform);
                        Monsters[i].transform.DOMove(new Vector3(0, 0, -3.7f), 0.4f);
                        Monsters[i].transform.DOScale(1,0.4f);
                        Monsters[i].transform.DORotate(new Vector3(0,0,0), 0.4f);
                    }
                }
            }
        }
    }
    //↓依次创建怪物
    //  先确定终点坐标与旋转角度，而后实例化卡牌，并将创建的卡牌名与最大数量传递给Battle()中GetMonsterName()，最后完成位移
    //  当一张卡牌被创建0.5s后，再创建下一张卡牌
    //  被此脚本中CanCreateMonster()与createMonster()调用
    public void createMonster()
    {
                if (Number == 0) 
                {
                    if (Max[0] == 0)
                    {
                        X_FP[Number] = Random.Range(-2.0f, -1.3f);
                        Y_FP[Number] = Random.Range(2.7f, 3.8f);
                        Cards_Rotate[Number] = Random.Range(-45f, 45f);
                        FinishPoint.transform.position = new Vector3(X_FP[Number], Y_FP[Number], -2.5f);

                        MonsterData_CM[Number] = CrMon_MD.GiveMonsterData(Monsters[Which].name);

                        Monsters[Number] = GameObject.Instantiate(Monsters[Which], MonsterPoint.transform.position, Quaternion.Euler(new Vector3(0, 0, Cards_Rotate[Number]))) as GameObject;
                      //  Monsters[Number].name += Number.ToString();
                  
                        Monsters[Number].transform.Find("HPPos").GetComponent<TextMesh>().text = MonsterData_CM[Number][1].ToString();
                        Monsters[Number].transform.Find("ActNum").GetComponent<TextMesh>().text = MonsterData_CM[Number][2].ToString();
                        Monsters[Number].transform.Find("ATK").GetComponent<TextMesh>().text = MonsterData_CM[Number][3].ToString();
                        Monsters[Number].transform.Find("Def").GetComponent<TextMesh>().text = MonsterData_CM[Number][4].ToString();

                      //  CrMon_BS.GetMonsterName(Monsters[Number].name.Replace("(Clone)", ""),Number);
                        Tweener GoTo1 = Monsters[Number].transform.DOMove(FinishPoint.transform.position, 0.5f);
                        GoTo1.SetEase(Ease.OutQuint);
                        Monsters[Number].transform.parent = Node.transform;
                    }
                    else if (Max[0] == 1)
                    {
                        X_FP[Number] = Random.Range(-0.5f, 0.3f);
                        Y_FP[Number] = Random.Range(2.7f, 3.8f);
                        Cards_Rotate[Number] = Random.Range(-45f, 45f);
                        FinishPoint.transform.position = new Vector3(X_FP[Number], Y_FP[Number], -2.5f);

                        MonsterData_CM[Number] = CrMon_MD.GiveMonsterData(Monsters[Which].name);


                        Monsters[Number] = GameObject.Instantiate(Monsters[Which], MonsterPoint.transform.position, Quaternion.Euler(new Vector3(0, 0, Cards_Rotate[Number]))) as GameObject;
                    //    Monsters[Number].name += Number.ToString();

                        Monsters[Number].transform.Find("HPPos").GetComponent<TextMesh>().text = MonsterData_CM[Number][1].ToString();
                        Monsters[Number].transform.Find("ActNum").GetComponent<TextMesh>().text = MonsterData_CM[Number][2].ToString();
                        Monsters[Number].transform.Find("ATK").GetComponent<TextMesh>().text = MonsterData_CM[Number][3].ToString();
                        Monsters[Number].transform.Find("Def").GetComponent<TextMesh>().text = MonsterData_CM[Number][4].ToString();

                        //  CrMon_BS.GetMonsterName(Monsters[Number].name.Replace("(Clone)", ""),Number);
                        Tweener GoTo2 = Monsters[Number].transform.DOMove(FinishPoint.transform.position, 0.5f);
                        GoTo2.SetEase(Ease.OutQuint);
                      //  GoTo2.OnComplete(delegate() { Monsters[Number].transform.parent = Sign.transform; });
                        Monsters[Number].transform.parent = Node.transform;
                    }
                    else if (Max[0] == 2)
                    {
                        X_FP[Number] = Random.Range(1.1f, 1.9f);
                        Y_FP[Number] = Random.Range(2.7f, 3.8f);
                        Cards_Rotate[Number] = Random.Range(-45f, 45f);
                        FinishPoint.transform.position = new Vector3(X_FP[Number], Y_FP[Number], -2.5f);

                        MonsterData_CM[Number] = CrMon_MD.GiveMonsterData(Monsters[Which].name);


                        Monsters[Number] = GameObject.Instantiate(Monsters[Which], MonsterPoint.transform.position, Quaternion.Euler(new Vector3(0, 0, Cards_Rotate[Number]))) as GameObject;
                    //    Monsters[Number].name += Number.ToString();

                        Monsters[Number].transform.Find("HPPos").GetComponent<TextMesh>().text = MonsterData_CM[Number][1].ToString();
                        Monsters[Number].transform.Find("ActNum").GetComponent<TextMesh>().text = MonsterData_CM[Number][2].ToString();
                        Monsters[Number].transform.Find("ATK").GetComponent<TextMesh>().text = MonsterData_CM[Number][3].ToString();
                        Monsters[Number].transform.Find("Def").GetComponent<TextMesh>().text = MonsterData_CM[Number][4].ToString();

              
                        //   CrMon_BS.GetMonsterName(Monsters[Number].name.Replace("(Clone)", ""), Number);
                        Tweener GoTo3 = Monsters[Number].transform.DOMove(FinishPoint.transform.position, 0.5f);
                        GoTo3.SetEase(Ease.OutQuint);
                      //  GoTo3.OnComplete(delegate() { Monsters[Number].transform.parent = Sign.transform; });
                        Monsters[Number].transform.parent = Node.transform;
                    }
                }
                else if (Number == 1)
                {
                    Which = Which + 1;
                    if (Max[1] == 0)
                    {
                        X_FP[Number] = Random.Range(-2.0f, -1.3f);
                        Y_FP[Number] = Random.Range(2.7f, 3.8f);
                        Cards_Rotate[Number] = Random.Range(-45f, 45f);
                        FinishPoint.transform.position = new Vector3(X_FP[Number], Y_FP[Number], -2.5f);

                        MonsterData_CM[Number] = CrMon_MD.GiveMonsterData(Monsters[Which].name);


                        Monsters[Number] = GameObject.Instantiate(Monsters[Which], MonsterPoint.transform.position, Quaternion.Euler(new Vector3(0, 0, Cards_Rotate[Number]))) as GameObject;
                    //    Monsters[Number].name += Number.ToString();

                        Monsters[Number].transform.Find("HPPos").GetComponent<TextMesh>().text = MonsterData_CM[Number][1].ToString();
                        Monsters[Number].transform.Find("ActNum").GetComponent<TextMesh>().text = MonsterData_CM[Number][2].ToString();
                        Monsters[Number].transform.Find("ATK").GetComponent<TextMesh>().text = MonsterData_CM[Number][3].ToString();
                        Monsters[Number].transform.Find("Def").GetComponent<TextMesh>().text = MonsterData_CM[Number][4].ToString();

                        //  CrMon_BS.GetMonsterName(Monsters[Number].name.Replace("(Clone)", ""), Number);
                        Tweener GoTo4 = Monsters[Number].transform.DOMove(FinishPoint.transform.position, 0.5f);
                        GoTo4.SetEase(Ease.OutQuint);
                     //   GoTo4.OnComplete(delegate() { Monsters[Number].transform.parent = Sign.transform; });
                        Monsters[Number].transform.parent = Node.transform;
                    }
                    else if (Max[1] == 1)
                    {
                        X_FP[Number] = Random.Range(-0.5f, 0.3f);
                        Y_FP[Number] = Random.Range(2.7f, 3.8f);
                        Cards_Rotate[Number] = Random.Range(-45f, 45f);
                        FinishPoint.transform.position = new Vector3(X_FP[Number], Y_FP[Number], -2.5f);

                        MonsterData_CM[Number] = CrMon_MD.GiveMonsterData(Monsters[Which].name);


                        Monsters[Number] = GameObject.Instantiate(Monsters[Which], MonsterPoint.transform.position, Quaternion.Euler(new Vector3(0, 0, Cards_Rotate[Number]))) as GameObject;
                    //    Monsters[Number].name += Number.ToString();

                        Monsters[Number].transform.Find("HPPos").GetComponent<TextMesh>().text = MonsterData_CM[Number][1].ToString();
                        Monsters[Number].transform.Find("ActNum").GetComponent<TextMesh>().text = MonsterData_CM[Number][2].ToString();
                        Monsters[Number].transform.Find("ATK").GetComponent<TextMesh>().text = MonsterData_CM[Number][3].ToString();
                        Monsters[Number].transform.Find("Def").GetComponent<TextMesh>().text = MonsterData_CM[Number][4].ToString();

                        //  CrMon_BS.GetMonsterName(Monsters[Number].name.Replace("(Clone)", ""), Number);
                        Tweener GoTo5 = Monsters[Number].transform.DOMove(FinishPoint.transform.position, 0.5f);
                        GoTo5.SetEase(Ease.OutQuint);
                      //  GoTo5.OnComplete(delegate() { Monsters[Number].transform.parent = Sign.transform; });
                        Monsters[Number].transform.parent = Node.transform;
                    }
                    else if (Max[1] == 2)
                    {
                        X_FP[Number] = Random.Range(1.1f, 1.9f);
                        Y_FP[Number] = Random.Range(2.7f, 3.8f);
                        Cards_Rotate[Number] = Random.Range(-45f, 45f);
                        FinishPoint.transform.position = new Vector3(X_FP[Number], Y_FP[Number], -2.5f);

                        MonsterData_CM[Number] = CrMon_MD.GiveMonsterData(Monsters[Which].name);


                        Monsters[Number] = GameObject.Instantiate(Monsters[Which], MonsterPoint.transform.position, Quaternion.Euler(new Vector3(0, 0, Cards_Rotate[Number]))) as GameObject;
                     //   Monsters[Number].name += Number.ToString();

                        Monsters[Number].transform.Find("HPPos").GetComponent<TextMesh>().text = MonsterData_CM[Number][1].ToString();
                        Monsters[Number].transform.Find("ActNum").GetComponent<TextMesh>().text = MonsterData_CM[Number][2].ToString();
                        Monsters[Number].transform.Find("ATK").GetComponent<TextMesh>().text = MonsterData_CM[Number][3].ToString();
                        Monsters[Number].transform.Find("Def").GetComponent<TextMesh>().text = MonsterData_CM[Number][4].ToString();

                        //   CrMon_BS.GetMonsterName(Monsters[Number].name.Replace("(Clone)", ""), Number);
                        Tweener GoTo6 = Monsters[Number].transform.DOMove(FinishPoint.transform.position, 0.5f);
                        GoTo6.SetEase(Ease.OutQuint);
                     //   GoTo6.OnComplete(delegate() { Monsters[Number].transform.parent = Sign.transform; });
                        Monsters[Number].transform.parent = Node.transform;
                    }
                }
                else if (Number == 2)
                {
                    Which = Which + 1;
                    if (Max[2] == 0)
                    {
                        X_FP[Number] = Random.Range(-2.0f, -1.3f);
                        Y_FP[Number] = Random.Range(2.7f, 3.8f);
                        Cards_Rotate[Number] = Random.Range(-45f, 45f);
                        FinishPoint.transform.position = new Vector3(X_FP[Number], Y_FP[Number], -2.5f);

                        MonsterData_CM[Number] = CrMon_MD.GiveMonsterData(Monsters[Which].name);


                        Monsters[Number] = GameObject.Instantiate(Monsters[Which], MonsterPoint.transform.position, Quaternion.Euler(new Vector3(0, 0, Cards_Rotate[Number]))) as GameObject;
                    //    Monsters[Number].name += Number.ToString();

                        Monsters[Number].transform.Find("HPPos").GetComponent<TextMesh>().text = MonsterData_CM[Number][1].ToString();
                        Monsters[Number].transform.Find("ActNum").GetComponent<TextMesh>().text = MonsterData_CM[Number][2].ToString();
                        Monsters[Number].transform.Find("ATK").GetComponent<TextMesh>().text = MonsterData_CM[Number][3].ToString();
                        Monsters[Number].transform.Find("Def").GetComponent<TextMesh>().text = MonsterData_CM[Number][4].ToString();
                        //  CrMon_BS.GetMonsterName(Monsters[Number].name.Replace("(Clone)", ""), Number);
                        Tweener GoTo7 = Monsters[Number].transform.DOMove(FinishPoint.transform.position, 0.5f);
                        GoTo7.SetEase(Ease.OutQuint);
                     //   GoTo7.OnComplete(delegate() { Monsters[Number].transform.parent = Sign.transform; });
                        Monsters[Number].transform.parent = Node.transform;
                    }
                    else if (Max[2] == 1)
                    {
                        X_FP[Number] = Random.Range(-0.5f, 0.3f);
                        Y_FP[Number] = Random.Range(2.7f, 3.8f);
                        Cards_Rotate[Number] = Random.Range(-45f, 45f);
                        FinishPoint.transform.position = new Vector3(X_FP[Number], Y_FP[Number], -2.5f);

                        MonsterData_CM[Number] = CrMon_MD.GiveMonsterData(Monsters[Which].name);


                        Monsters[Number] = GameObject.Instantiate(Monsters[Which], MonsterPoint.transform.position, Quaternion.Euler(new Vector3(0, 0, Cards_Rotate[Number]))) as GameObject;
                    //    Monsters[Number].name += Number.ToString();

                        Monsters[Number].transform.Find("HPPos").GetComponent<TextMesh>().text = MonsterData_CM[Number][1].ToString();
                        Monsters[Number].transform.Find("ActNum").GetComponent<TextMesh>().text = MonsterData_CM[Number][2].ToString();
                        Monsters[Number].transform.Find("ATK").GetComponent<TextMesh>().text = MonsterData_CM[Number][3].ToString();
                        Monsters[Number].transform.Find("Def").GetComponent<TextMesh>().text = MonsterData_CM[Number][4].ToString();
                        //  CrMon_BS.GetMonsterName(Monsters[Number].name.Replace("(Clone)", ""), Number);
                        Tweener GoTo8 = Monsters[Number].transform.DOMove(FinishPoint.transform.position, 0.5f);
                        GoTo8.SetEase(Ease.OutQuint);
                      //  GoTo8.OnComplete(delegate() { Monsters[Number].transform.parent = Sign.transform; });
                        Monsters[Number].transform.parent = Node.transform;
                    }
                    else if (Max[2] == 2)
                    {
                        X_FP[Number] = Random.Range(1.1f, 1.9f);
                        Y_FP[Number] = Random.Range(2.7f, 3.8f);
                        Cards_Rotate[Number] = Random.Range(-45f, 45f);
                        FinishPoint.transform.position = new Vector3(X_FP[Number], Y_FP[Number], -2.5f);

                        MonsterData_CM[Number] = CrMon_MD.GiveMonsterData(Monsters[Which].name);


                        Monsters[Number] = GameObject.Instantiate(Monsters[Which], MonsterPoint.transform.position, Quaternion.Euler(new Vector3(0, 0, Cards_Rotate[Number]))) as GameObject;
                     //   Monsters[Number].name += Number.ToString();

                        Monsters[Number].transform.Find("HPPos").GetComponent<TextMesh>().text = MonsterData_CM[Number][1].ToString();
                        Monsters[Number].transform.Find("ActNum").GetComponent<TextMesh>().text = MonsterData_CM[Number][2].ToString();
                        Monsters[Number].transform.Find("ATK").GetComponent<TextMesh>().text = MonsterData_CM[Number][3].ToString();
                        Monsters[Number].transform.Find("Def").GetComponent<TextMesh>().text = MonsterData_CM[Number][4].ToString();
                        //  CrMon_BS.GetMonsterName(Monsters[Number].name.Replace("(Clone)", ""), Number);
                        Tweener GoTo9 = Monsters[Number].transform.DOMove(FinishPoint.transform.position, 0.5f);
                        GoTo9.SetEase(Ease.OutQuint);
                      //  GoTo9.OnComplete(delegate() { Monsters[Number].transform.parent = Sign.transform; });
                        Monsters[Number].transform.parent = Node.transform;
                    }
                }
                Number++;
                if (Number == ThisNumber)
                {
                    Number = 0;
                    Which = 0;
                }
                else
                    Invoke("createMonster", 0.5f);
     }
    //↓进入战斗场景时，摧毁提供信息的怪物卡牌
    public void DestroyCards()
    {
        CanShow = false;
        for (int i = 0; i < Monsters.Length; i++)
        {
            Destroy(Monsters[i]);
        }
    }
    //↓完成延时，并进行本次怪物的赋值， 生成（0,3）的随机数，确定发牌顺序
    private IEnumerator Wait(int number, string[] monsters)
{
        yield return new WaitForSeconds(1.3f);
        CanShow = true;
        for (int k = 0; k < monsters.Length; k++)
            for (int i = 0; i < CrMon_Mon.MonsterCards.Length; i++)
            {
                if (CrMon_Mon.Monsters[i] != null && monsters[k] == CrMon_Mon.Monsters[i].name)
                {
                    Debug.Log("CanCreate's Name is " + CrMon_Mon.Monsters[i].name);
                    Monsters[k] = CrMon_Mon.MonsterCards[i];
                    break;
                }
            }
        ThisNumber = number;
        int r;
        int j = 0;
        for (j = 0; j < number; j++)
        {
            r = Random.Range(0, number);
            int num = 0;
            for (int k = 0; k < j; k++)
            {
                if (Max[k] == r)
                {
                    num = num + 1;
                }
            }
            if (num == 0)
            {
                Max[j] = r;
            }
            else
            {
                j = j - 1;
            }
        }
        //  CrMon_Battle.GetMonstersNumber(ThisNumber);
        createMonster();
}        
   
}
