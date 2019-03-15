using UnityEngine;
using System.Collections;
/// <summary>
/// 统一脚本获取管理
/// </summary>
public class ScriptsManager: MonoBehaviour{
    public NewUIManager NUIM;
    public PlayerData PD;
    public RayManager RM;
    public RoleInfor RI;
    public CreateMap CM;
    public Move Move;
    public BattleSystem BS;                   //调用Battle()
    public AllMonsters Mon;                 //调用AllMonsters()
    public NewButton NewBut;                //调用NewButton()
    public SignAbove SA;            //调用SignAbove（）
    public ToBlack TB;       //调用ToBlack()
    public CardsEvents CE;
    public ShowAndMove SAM;
    public AllMonsters AllMon;
    public CreateMonster CrMon;
    public MonstersAI MAI;
    public MissionInfor MI;
    public Booty Bo;
    public NewShow NS;
    public MonsterData MD;



	// Use this for initialization
    public delegate void Begin_EventHander();  //定义委托，用于传递每一次的卡排名，事件名和调用次数
    public static event Begin_EventHander CanBegin;

	void Start () {
        RM = GameObject.Find("Main Camera").GetComponent<RayManager>();
       NUIM = GameObject.Find("BG").GetComponent<NewUIManager>();
        PD = GameObject.Find("_PlayerDatas").GetComponent<PlayerData>();
        RI = GameObject.Find("RoleInfor").GetComponent<RoleInfor>();
        Move = GameObject.Find("CreateMap").GetComponent<Move>();
        BS = GameObject.Find("BattleObject").GetComponent<BattleSystem>();
        NewBut = GameObject.Find("NewSignAbove").GetComponent<NewButton>();
        TB = GameObject.Find("ToBlack").GetComponent<ToBlack>();
      //  SAM = GameObject.Find("Show").GetComponent<ShowAndMove>();
        CrMon = GameObject.Find("BG_Battle").GetComponent<CreateMonster>();
        MAI = GameObject.Find("AIManager").GetComponent<MonstersAI>();
        MI = GameObject.Find("MissionInfor").GetComponent<MissionInfor>();
        Bo = GameObject.Find("BattleManager").GetComponent<Booty>();
        CM = GameObject.Find("CreateMap").GetComponent<CreateMap>();
        NS = GameObject.Find("NewShow").GetComponent<NewShow>();
        MD=GameObject.Find("_PlayerDatas").GetComponent<MonsterData>();
        SA = GameObject.Find("SignAbove").GetComponent<SignAbove>();
        CE=new CardsEvents();
        MI.L_Start();
        TB.L_Start();
        RI.L_Start();
        MD.L_Start();
        RM.L_Start();
        BS.L_Start();
      //  SAM.L_Start();
        CrMon.L_Start();
        NUIM.L_Start();
        MAI.L_Start();

        NS.L_Start();
     
     //   CM = this.GetComponent<CreateMap>(); 
	}
    public void GameOver()
    {
        StartCoroutine(ReStart());
        MI.NNNN = 0;
        PD.AnotherStart();
        NUIM.AnotherStart();
        CM.AnotherInitMap();
     
        if (BS.CanBattle)
            NewBut.CanRotate(5);
        else
            NewBut.CanRotate(4);

        BS.ReturnToMap("Over");
        

        CM.First = 0;
        CE.GetCardNameAndEvent("Over", null, 0);
        RI.ReStart();
      
    }
    IEnumerator ReStart()
    {
        yield return new WaitForSeconds(2f);
        TB.ToWhite();
    }
	// Update is called once per frame
	void Update () {
    //    if (PlayerData.HP <= 0)
      //      Debug.Log("游戏结束！");
	}
}
