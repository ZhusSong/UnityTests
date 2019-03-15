using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Battle : MonoBehaviour {
   
    private GameObject Role;
    private string[] MonsterName= new string[3];    //存储本次的怪物名
    private int ThisNumber=10;                      //这次的怪物数量


    private SignAbove Battle_SA;                    //调用SignAbove()
    private Booty Battle_Booty;                     //调用Booty()
    //↓实例化
    void Start()
    {
        Battle_Booty = GameObject.Find("BG_Booty").GetComponent<Booty>();
            Battle_SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
    }
    //↓得到本次出现的怪物数量,当怪物数为0时，调用Booty()中CreateDown()出现战利品栏
    //  被从CreateMonster()中CanCreateMonster()调用
    public void GetMonstersNumber(int i)           
    {
        ThisNumber = i;
    }
    //↓得到本次出现的怪物名字
    //  被从CreateMonster()中createMonster()调用
    public void GetMonsterName(string Name,int i)
    {
        MonsterName[i] = Name;
    }
    //↓进行射线检测
    //  当鼠标抬起时，若碰到怪物，则判定为作出决定，当怪物数量为0时，延时0.5s调用ToBooty()
    void Update() 
    {
          Ray ray_Battle = Camera.main.ScreenPointToRay(Input.mousePosition);
   RaycastHit hit_Battle = new RaycastHit();
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray_Battle, out hit_Battle))
            {
                for (int i = 0; i < 3; i++)
                {
                    if (hit_Battle.transform.name.Replace("(Clone)", "") == MonsterName[i])
                        Debug.Log("Monster Name is " + hit_Battle.transform.name.Replace("(Clone)", ""));
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Physics.Raycast(ray_Battle, out hit_Battle))
            {
                for (int i = 0; i < 3; i++)
                {
                    if (hit_Battle.transform.name.Replace("(Clone)", "") == MonsterName[i])
                    {
                        Destroy(hit_Battle.transform.GetComponent<Collider>().gameObject);
                        ThisNumber--;
                    }
                  
                }
            }
        }
        if (ThisNumber == 0)
        {
            ThisNumber = 10;
            Invoke("ToBooty", 0.5f);
        }
    }
    //↓通知Booty()进行战利品牌子的落下
   // public void ToBooty()
   // {
  //      Battle_Booty.RotateDown();
  //  }
}
