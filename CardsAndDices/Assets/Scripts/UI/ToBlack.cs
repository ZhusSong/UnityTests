using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class ToBlack : MonoBehaviour {

    [SerializeField]
    public Image Black;                                                      //贴图
    Color co;                                                                //Color变量，控制由白变黑的过程
    private bool Can = false;                                                //控制颜色是否可变
    public CreateMap ToBlack_CM;                                                    //调用CreateMap()
    Move ToBlack_Move;
    NewUIManager ToBlack_NUIM;
    private ScriptsManager TB_SM;
    public MissionInfor TB_MI;
    private BattleSystem TB_BS;
    private PlayerData TB_PD;
    public void L_Start()
    {
        TB_SM = GameObject.Find("Main Camera").GetComponent<ScriptsManager>();
        ToBlack_CM = TB_SM.CM; //得到CreateMap()函数
        ToBlack_Move = TB_SM.Move;
        ToBlack_NUIM = TB_SM.NUIM;
        TB_MI = TB_SM.MI;
        TB_BS = TB_SM.BS;
        TB_PD = TB_SM.PD;
        Black = this.GetComponent<Image>();                                   //得到贴图
        co.a = 1;                                                             //不透明
    }
    //↓控制屏幕变黑，被CardEvents中方式牌处理函数调用
    public void TOBlack(string scriptName)                                                    
    {
        Debug.Log("TooooooooooooooooooooBlack! "+scriptName);
        this.gameObject.SetActive(true);
       co.a = 1;
        Can = true;
        if (Can)
        {
            Tweener TOblack = Black.DOColor(co, 2f);                          //使背景颜色变化
            if (scriptName == "CE")
            {
                TOblack.OnComplete(delegate() {  /*ToBlack_NUIM.GetMap(null);*/ ToBlack_Move.DestroyAndCreate(); ToBlack_CM.InitMap(); });        //完全变黑时，通知CreateMap()创建地图
               
            }
            if (scriptName == "BS")
            {
                TOblack.OnComplete(delegate() { TB_BS.ReturnToMap();});
            }
            if (scriptName == "GameOver")
            {
                PlayerData.HP = 1;
                Debug.Log("OOOOver");
                TOblack.OnComplete(delegate() { TB_SM.GameOver(); });
             //  TB_SM.GameOver();
            }
        }
     
    }
    //↓控制屏幕变白，被CreateMap中InitMap()调用
    public void ToWhite()                                                
    {
      //  Debug.Log("tOOOOOOOObLACK");
        co.a = 0;
        Tweener TOwhite = Black.DOColor(co, 1f);
        Can = false;
        TOwhite.OnComplete(delegate() { this.gameObject.SetActive(false); });
            
    }
}
