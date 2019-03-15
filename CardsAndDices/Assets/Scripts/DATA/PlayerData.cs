using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System;
//↓进行与玩家数据相关的操作
public class PlayerData : MonoBehaviour {
    //↓五项基本属性
    private int Constitution;              //体质
    private int Strength;                  //力量
    private int Intelligence;             //智力
    private int Dexterity;                  //敏捷
    private int Level;                       //等级
    
    //↓11项衍生属性

    public int HitPoint;                    //血量
    public int Defense;                     //防御力
    public int AttactDamage;              //物理伤害
    public int ParryPoint;                  //格挡减伤值
    public int MagicNumber;                //法术个数
    public int AbilityDamage;             //法术伤害
    public int Critial;                    //暴击率
    public int Accuracy;                 //命中率
    public int ActionNumber;                  //行动力
    public int Foods;                       //食物数量
    public int Satiety;                     //饱食度

    //↓属性的偏差值,此偏差值为装备技能等等影响后的数值，仅用于当次游戏
    public int A_Constitution;              //偏差体质
    public int A_Strength;                  //偏差力量
    public int A_Intelligence;             //偏差智力
    public int A_Dexterity;                  //偏差敏捷
    public int A_Level;                       //偏差等级
    public int A_HitPoint;                    //偏差血量
    public int A_Defense;                     //偏差防御力
    public int A_AttactDamage;              //偏差物理伤害
    public int A_ParryPoint;                  //偏差格挡减伤值
    public int A_MagicNumber;                //偏差法术个数
    public int A_AbilityDamage;             //偏差法术伤害
    public int A_Critial;                    //偏差暴击率
    public int A_Accuracy;                 //偏差命中率
    public int A_HitNumber;                  //偏差攻击次数
    public int A_Foods;                       //偏差食物数量
    public int A_Satiety;                     //偏差饱食度

    public static int HP=0;
    public static int Atk=0;
    public static int Def=0;
    public static int ActNum=0;
    public static int Cri=0;
    public static int Sat=0;
    public static int Battle_Damage;      //战斗所受伤害

    private int HP_NOW;                   //当前HP

    private int EQU_ATK = 0;              //角色装备提供的攻击力
    private int EQU_DEF = 0;              //角色装备提供的防御力
    private int EQU_HP = 0;               //角色装备提供的血量
    private int EQU_CRI = 0;              //角色装备提供的暴击率            
    private int EQU_AN = 0;               //角色装备提供的行动力
    //角色装备图片信息 0:左手戒指 1:右手戒指 2:项链 3:左手武器 4:右手武器 5:头盔 6:盔甲 7:裤子 
    public Sprite[] PlayerEquip = new Sprite[9];   //角色穿戴图片信息
    public Sprite[] PlayerBag = new Sprite[9];     //角色背包图片信息
    public Sprite[] PlayerMysCon = new Sprite[9];  //角色神秘状态图片信息
    public Sprite[] PlayerSkills = new Sprite[9];  //角色技能图片信息

    //角色装备序列号信息 0:左手戒指 1:右手戒指 2:项链 3:左手武器 5:右手武器 6:头盔 7:盔甲 8:裤子 
    public string[] PlayerEquip_SN = new string[9];
   // private string[] PlayerSkill_SN = new string[9]; //角色装备技能序列号信息
  //  private string[] PlayerMysCon_SN = new string[9];//角色神秘状态序列号信息

    private string PlayerEquipData_LeftRing="ShiPin";        //角色穿戴数据_左手戒指
    private string PlayerEquipData_RightRing = "ShiPin";        //角色穿戴数据_右手戒指
    private string PlayerEquipData_Necklace = "ShiPin";         //角色穿戴数据_项链
    private string PlayerEquipData_LeftHandWeapon = "WuQi";   //角色穿戴数据_左手武器
    private string PlayerEquipData_RightHandWeapon = "WuQi";   //角色穿戴数据_右手武器
    private string PlayerEquipData_Helmet = "Tou";            //角色穿戴数据_头盔
    private string PlayerEquipData_Chestplate = "KuiJia";        //角色穿戴数据_盔甲
    private string PlayerEquipData_Pants = "KuZi";             //角色穿戴数据_裤子

    public List<string> PlayerBagData = new List<string>();     //角色背包序列号数据
   // private List<string> PlayerMysConData = new List<string>();  //角色神秘状态序列号数据
  //  private List<string> PlayerSkillsData = new List<string>();  //角色所有技能序列号数据
  

    private string saveTime;
    private MySqlite PlayerSQL ;

    private RoleInfor PD_RI;
    private MissionInfor PD_MI;
  
	// Use this for initialization
	void Start () {
        PD_MI = GameObject.Find("MissionInfor").GetComponent<MissionInfor>();
        PD_RI = GameObject.Find("RoleInfor").GetComponent<RoleInfor>();
        PlayerBag[0] = Resources.Load<Sprite>("Icons/Food");
        PlayerBag[1] = Resources.Load<Sprite>("Icons/XiaoHao");
        PlayerBag[2] = Resources.Load<Sprite>("Icons/CaiLiao");
        PlayerBag[3] = Resources.Load<Sprite>("Icons/WuQi");
        PlayerBag[4] = Resources.Load<Sprite>("Map/Map");
        PlayerBag[5] = Resources.Load<Sprite>("Icons/ShiPin");
        PlayerBag[6] = Resources.Load<Sprite>("Icons/TouKui");
        PlayerBag[7] = Resources.Load<Sprite>("Icons/KuiJia");
        PlayerBag[8] = Resources.Load<Sprite>("Icons/KuZi");
     //   PlayerMysCon[4] = Resources.Load<Sprite>("Icons/TianFu");

       
        //以下为获取本次游戏玩家数据
      //  BasicValue(10, 3, 5, 5);
        saveTime = GetSaveTime();
        GetPlayerDatas(saveTime);
        HP+=HitPoint;
     Atk+=AttactDamage;
    Def+=Defense;
    ActNum+=ActionNumber;
     Cri+=Critial;
     Sat+= Satiety;

    /////////////////以下为测试数据
   //  HP = 1;
   //   Sat = 0;
    /////////////////




    //    BasicValue(10,3,5,5);
     //   CreateData();
     //   GiveDataCount("PlayerBag",null);
     //   string time = "4/21/2018 2:27:32 PM";
     //   GetPlayerConditions(time);                        
  
     
   //  PlayerBag = null;

  ///////////以下是测试数据

        //给予玩家基础数据
     /*   BasicValue(10, 3, 5, 5);
        //给予玩家背包数据
        PutIntoBag("FOO_00000001");
        PutIntoBag("FOO_00000001");
        PutIntoBag("FOO_00000002");

        //给予玩家装备数据
        PutIntoEquip("WEA_00000001");
        PutIntoEquip("PAN_00000001");
        PutIntoEquip("CHE_00000001");
        PutIntoEquip("HEL_00000001");
        PutIntoEquip("WEA_00000002");
           //得到数据
     //   saveTime = GetSaveTime();
    //    GetPlayerConditions(saveTime);
     */
  //   PutDataIntoDB();
     
	}
    public void AnotherStart()
    {
        PlayerEquip_SN[0] = null;
        PlayerEquip_SN[1] = null;
        PlayerEquip_SN[2] = null;
        PlayerEquip_SN[3] = null;
        PlayerEquip_SN[5] = null;
        PlayerEquip_SN[6] = null;
        PlayerEquip_SN[7] = null;
        PlayerEquip_SN[8] = null;



        PlayerEquip[0] = null;
        PlayerEquip[1] = null;
        PlayerEquip[2] = null;
        PlayerEquip[3] = null;
        PlayerEquip[5] = null;
        PlayerEquip[6] = null;
        PlayerEquip[7] = null;
        PlayerEquip[8] = null;


        PlayerBagData.Clear();


        Constitution =0;
        Strength = 0;
        Intelligence = 0;
        Dexterity = 0;
        Level = 0;
        HitPoint = 0;
        Defense = 0;
        AttactDamage = 0;
        ParryPoint = 0;
        MagicNumber =0;
        AbilityDamage = 0;
        Critial = 0;
        Accuracy = 0;
        ActionNumber = 0;
        Foods = 0;
        Satiety = 0;    

        HP = 0;
        Atk = 0;
        Def = 0;
        ActNum =0;
        Cri = 0;
        Sat =0;
        Debug.Log("PlayerData ReStart");
        saveTime = GetSaveTime();
        GetPlayerDatas(saveTime);
        HP += HitPoint;
        Atk += AttactDamage;
        Def += Defense;
        ActNum += ActionNumber;
        Cri += Critial;
        Sat += Satiety;
        PD_MI.ChangeInfor();


      //  HP = 1;
    }
    /// <summary>
    /// 卸下或更换装备，将装备置入背包
    /// </summary>
  //  public void EquipToBag(int which,string serialNumber)
   // {
   //     PlayerBagData.Add(serialNumber);
  //  }
    /// <summary>
    /// 根据传入的物体名赋予数据库中的信息
    /// </summary>
    /// <param name="name"></param>
    public void EquipInfors(GameObject which)
    {
    //    string Name = name.Replace("(Clone)", "");
        SqliteDataReader reader_EI;
        MySqlite thisSQL;
        thisSQL = new MySqlite("data source=CardsAndDices.db");
        string name=which.name.Replace("(Clone)", "");
       // Debug.Log("name is " + which.name.Replace("(Clone)", ""));
        switch (name.Substring(0, 3))
        {
            case "WEA":
                string sql_01 = "SELECT * FROM Weapon WHERE SerialNumber = '" + name+"'";
                reader_EI = thisSQL.ExecuteQuery(sql_01);
                while (reader_EI.Read())
                {
                    which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                    for (int i = 9; i < 13; i++)
                    {
                        if (reader_EI[i].ToString().Length > 1)
                        {
                            switch (reader_EI[i].ToString().Substring(0, 3))
                            {
                                case "ATK":
                                    which.transform.Find("ATK").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                    break;
                                case "DEF":
                                    which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                            break;
                    }
                }
                reader_EI.Close();
                break;
            case "FOO":
                string sql_02 = "SELECT * FROM Foods WHERE SerialNumber = '" + name + "'";
                reader_EI = thisSQL.ExecuteQuery(sql_02);
                while (reader_EI.Read())
                {
                    which.transform.Find("ATK").GetComponent<TextMesh>().text = reader_EI[3].ToString();
                    which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                }
                reader_EI.Close();
                break;
            case "CON":
                string sql_03 = "SELECT * FROM Consumables WHERE SerialNumber = '" + name + "'";
                reader_EI = thisSQL.ExecuteQuery(sql_03);
                while (reader_EI.Read())
                {
                }
                reader_EI.Close();
                break;
            case "CHE":
                string sql_04 = "SELECT * FROM Chestplate WHERE SerialNumber = '" + name + "'";
                reader_EI = thisSQL.ExecuteQuery(sql_04);
                while (reader_EI.Read())
                {
                    which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                    for (int i = 9; i < 13; i++)
                    {
                        if (reader_EI[i].ToString().Length > 1)
                        {
                            switch (reader_EI[i].ToString().Substring(0, 3))
                            {
                                case "ATK":
                                    which.transform.Find("ATK").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                    break;
                                case "DEF":
                                    which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                            break;
                    }
                }
                reader_EI.Close();
                break;
            case "HEL":
                string sql_05 = "SELECT * FROM Helmet WHERE SerialNumber = '" + name + "'";
                reader_EI = thisSQL.ExecuteQuery(sql_05);
                while (reader_EI.Read())
                {
                    which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                    for (int i = 9; i < 13; i++)
                    {
                        if (reader_EI[i].ToString().Length > 1)
                        {
                            switch (reader_EI[i].ToString().Substring(0, 3))
                            {
                                case "ATK":
                                    which.transform.Find("ATK").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                    break;
                                case "DEF":
                                    which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                            break;
                    }
                }
                reader_EI.Close();
                break;
            case "PAN":
                string sql_06 = "SELECT * FROM Pants WHERE SerialNumber = '" + name + "'";
                reader_EI = thisSQL.ExecuteQuery(sql_06);
                while (reader_EI.Read())
                {
                    which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                    for (int i = 9; i < 13; i++)
                    {
                        if (reader_EI[i].ToString().Length > 1)
                        {
                            switch (reader_EI[i].ToString().Substring(0, 3))
                            {
                                case "ATK":
                                    which.transform.Find("ATK").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                    break;
                                case "DEF":
                                    which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                            break;
                    }
                }
                reader_EI.Close();
                break;
            case "NEC":
                string sql_07 = "SELECT * FROM Necklace WHERE SerialNumber = '" + name + "'";
                reader_EI = thisSQL.ExecuteQuery(sql_07);
                while (reader_EI.Read())
                {
                    which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                    for (int i = 9; i < 13; i++)
                    {
                        if (reader_EI[i].ToString().Length > 1)
                        {
                            switch (reader_EI[i].ToString().Substring(0, 3))
                            {
                                case "ATK":
                                    which.transform.Find("ATK").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                    break;
                                case "DEF":
                                    which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                            break;
                    }
                }
                reader_EI.Close();
                break;
            case "RIN":
                string sql_08 = "SELECT * FROM Rings WHERE SerialNumber = '" + name + "'";
                reader_EI = thisSQL.ExecuteQuery(sql_08);
                while (reader_EI.Read())
                {
                    which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                    for (int i = 9; i < 13; i++)
                    {
                        if (reader_EI[i].ToString().Length > 1)
                        {
                            switch (reader_EI[i].ToString().Substring(0, 3))
                            {
                                case "ATK":
                                    which.transform.Find("ATK").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                    break;
                                case "DEF":
                                    which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                    break;
                                case "ACT":
                                    which.transform.Find("ATK").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                    break;
                                case "CRI":
                                    which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                            break;
                    }
                }
                reader_EI.Close();
                break;
            default :
                break;
        }
        thisSQL.CloseConnection();
    }
    /// <summary>
    /// 根据当前装备信息改变玩家数据
    /// </summary>
    public void ChangeShowData()
    {
        MySqlite thisSQL = new MySqlite("data source=CardsAndDices.db");
        SqliteDataReader reader_CSD;
        HP_NOW = HP;

    //    HP = 0;
        Atk =0;
        Def = 0;
        ActNum = 0;
        Cri =0;

     //   HP += HitPoint;
        Atk += AttactDamage;
        Def += Defense;
        ActNum += ActionNumber;
        Cri += Critial;
      //  Sat += Satiety;
        for (int i = 0; i < PlayerEquip_SN.Length; i++)
        {
            if (i == 4)
                continue;
            if (PlayerEquip_SN[i].Length >3)
            {
                string name = PlayerEquip_SN[i].Substring(0, 3);
             
                switch (name)
                { 
                    case "WEA":
                        string sql_01 = "SELECT * FROM Weapon WHERE SerialNumber = '" + PlayerEquip_SN[i] + "'";
                        reader_CSD = thisSQL.ExecuteQuery(sql_01);
                        while (reader_CSD.Read())
                        {
                            Debug.Log("name is " + name + " serialNumber is " + PlayerEquip_SN[i]);
                            for (int j = 9; j < 13; j++)
                            {
                                if (reader_CSD[j].ToString().Length > 1)
                                {
                                 //   Debug.Log("Atk is " + Convert.ToInt32(reader_CSD[j].ToString().Substring(4)));
                                    switch (reader_CSD[j].ToString().Substring(0, 3))
                                    {
                                        case "ATK":
                                            Atk += Convert.ToInt32(reader_CSD[j].ToString().Substring(4));
                                            
                                            break;
                                        case "DEF":
                                            Def += Convert.ToInt32(reader_CSD[j].ToString().Substring(4));
                                     //       Debug.Log("DEF is " + Convert.ToInt32(reader_CSD[j].ToString().Substring(4)));
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                        reader_CSD.Close();
                        break;

                    case "CHE":
                        string sql_04 = "SELECT * FROM Chestplate WHERE SerialNumber = '" + PlayerEquip_SN[i] + "'";
                        reader_CSD = thisSQL.ExecuteQuery(sql_04);
                        while (reader_CSD.Read())
                        {
                            //   which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                            for (int j = 9; j < 13; j++)
                            {
                                if (reader_CSD[j].ToString().Length > 1)
                                {
                                    switch (reader_CSD[j].ToString().Substring(0, 3))
                                    {
                                        case "ATK":
                                            Atk += Convert.ToInt32(reader_CSD[j].ToString().Substring(4));
                                            break;
                                        case "DEF":
                                            Def += Convert.ToInt32(reader_CSD[j].ToString().Substring(4));
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                        reader_CSD.Close();
                        break;
                    case "HEL":
                        string sql_05 = "SELECT * FROM Helmet WHERE SerialNumber = '" + PlayerEquip_SN[i] + "'";
                        reader_CSD = thisSQL.ExecuteQuery(sql_05);
                        while (reader_CSD.Read())
                        {
                            //   which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                            for (int j = 9; j < 13; j++)
                            {
                                if (reader_CSD[j].ToString().Length > 1)
                                {
                                    switch (reader_CSD[j].ToString().Substring(0, 3))
                                    {
                                        case "ATK":
                                            Atk += Convert.ToInt32(reader_CSD[j].ToString().Substring(4));
                                            break;
                                        case "DEF":
                                            Def += Convert.ToInt32(reader_CSD[j].ToString().Substring(4));
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                        reader_CSD.Close();
                        break;
                    case "PAN":
                        string sql_06 = "SELECT * FROM Pants WHERE SerialNumber = '" + PlayerEquip_SN[i] + "'";
                        reader_CSD = thisSQL.ExecuteQuery(sql_06);
                        while (reader_CSD.Read())
                        {
                            //   which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                            for (int j = 9; j < 13; j++)
                            {
                                if (reader_CSD[j].ToString().Length > 1)
                                {
                                    switch (reader_CSD[j].ToString().Substring(0, 3))
                                    {
                                        case "ATK":
                                            Atk += Convert.ToInt32(reader_CSD[j].ToString().Substring(4));
                                            break;
                                        case "DEF":
                                            Def += Convert.ToInt32(reader_CSD[j].ToString().Substring(4));
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                        reader_CSD.Close();
                        break;
                    case "NEC":
                        string sql_07 = "SELECT * FROM Necklace WHERE SerialNumber = '" + PlayerEquip_SN[i] + "'";
                        reader_CSD = thisSQL.ExecuteQuery(sql_07);
                        while (reader_CSD.Read())
                        {
                            //    which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                            for (int j = 9; j < 13; j++)
                            {
                                if (reader_CSD[j].ToString().Length > 1)
                                {
                                    switch (reader_CSD[j].ToString().Substring(0, 3))
                                    {
                                        case "ATK":
                                            Atk += Convert.ToInt32(reader_CSD[j].ToString().Substring(4));
                                      //      which.transform.Find("ATK").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                            break;
                                        case "DEF":
                                            Def += Convert.ToInt32(reader_CSD[j].ToString().Substring(4));
                                       //     which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                            break;
                                        case "CRI":
                                            Cri += Convert.ToInt32(reader_CSD[j].ToString().Substring(4));
                                            //     which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                        reader_CSD.Close();
                        break;
                    case "RIN":
                        string sql_08 = "SELECT * FROM Rings WHERE SerialNumber = '" + PlayerEquip_SN[i] + "'";
                        reader_CSD = thisSQL.ExecuteQuery(sql_08);
                        while (reader_CSD.Read())
                        {
                            //  which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                            for (int j = 9; j < 13; j++)
                            {
                                if (reader_CSD[j].ToString().Length > 1)
                                {
                                    switch (reader_CSD[j].ToString().Substring(0, 3))
                                    {
                                        case "ATK":
                                            Atk += Convert.ToInt32(reader_CSD[j].ToString().Substring(4));
                                         //   which.transform.Find("ATK").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                            break;
                                        case "DEF":
                                            Def += Convert.ToInt32(reader_CSD[j].ToString().Substring(4));
                                         //   which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                            break;
                                        case "CRI":
                                            Cri += Convert.ToInt32(reader_CSD[j].ToString().Substring(4));
                                            //   which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                            break;
                                        case "ACT":
                                            ActNum += Convert.ToInt32(reader_CSD[j].ToString().Substring(4));
                                            //   which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                        reader_CSD.Close();
                        break;
                    default:
                        break;
                }
            }
        }
        Debug.Log("ATK  is "+Atk+" Def is "+Def);
        thisSQL.CloseConnection();
        PD_MI.ChangeInfor();
    }
    /// <summary>
    /// 进行装备操作
    /// </summary>
    /// <param name="serialNumber"></param>  
    //角色装备序列号信息 0:左手戒指 1:右手戒指 2:项链 3:左手武器 5:右手武器 6:头盔 7:盔甲 8:裤子 
    public void PutIntoEquip(string serialNumber)
    {
        string Kind = serialNumber.Substring(0, 3);
     //   Debug.Log("Kind is "+Kind+" serialNumber is "+serialNumber);
        switch (Kind)
        {
            case "WEA":
                //先判断右手有无武器，无武器装备到右手，有武器装备到左手
                //若都有武器，则将右手武器装备到左手，右手装备新武器
                if (PlayerEquip_SN[5].Length <3)
                {
                 //   Debug.Log("Equip to Right");
                    PlayerEquip_SN[5] = serialNumber;
                    PlayerEquip[5] = Resources.Load<Sprite>("Icons/SmallIcons/" + serialNumber);
                }
                else if (PlayerEquip_SN[3].Length < 3)
                {
                 //   Debug.Log("Equip to Left");
                    PlayerEquip_SN[3] = serialNumber;
                    PlayerEquip[3] = Resources.Load<Sprite>("Icons/SmallIcons/" + serialNumber);
                }
                else if (PlayerEquip_SN[5].Length > 3 && PlayerEquip_SN[3].Length > 3)
                {
               //     Debug.Log("Equip to Right and Left");
                   PutIntoBag(PlayerEquip_SN[3]);
                    PlayerEquip_SN[3] = PlayerEquip_SN[5];
                    PlayerEquip[3] = Resources.Load<Sprite>("Icons/SmallIcons/" + PlayerEquip_SN[5]);
                    PlayerEquip_SN[5] = serialNumber;
                    PlayerEquip[5] = Resources.Load<Sprite>("Icons/SmallIcons/" + serialNumber);
                 //   EquipToBag(3, PlayerEquipData_LeftHandWeapon);
                }

                break;
            case "HEL":
                if (PlayerEquip_SN[6].Length > 3)
                {
                    PutIntoBag(PlayerEquip_SN[6]);
                    PlayerEquip_SN[6] = serialNumber;
                    PlayerEquip[6] = Resources.Load<Sprite>("Icons/SmallIcons/" + serialNumber);
                }
                else
                {
                    PlayerEquip_SN[6] = serialNumber;
                    PlayerEquip[6] = Resources.Load<Sprite>("Icons/SmallIcons/" + serialNumber);
                }
                break;
            case "NEC":
                if (PlayerEquip_SN[2].Length > 3)
                {
                    PutIntoBag(PlayerEquip_SN[2]);
                PlayerEquip_SN[2] = serialNumber;
                PlayerEquip[2] = Resources.Load<Sprite>("Icons/SmallIcons/" + serialNumber);  
                }
                else
                {
                    PlayerEquip_SN[2] = serialNumber;
                    PlayerEquip[2] = Resources.Load<Sprite>("Icons/SmallIcons/" + serialNumber);  
                }
                break;
            case "PAN":
                if (PlayerEquip_SN[8].Length > 3)
                {
                    PutIntoBag(PlayerEquip_SN[8]);
                PlayerEquip_SN[8] = serialNumber;
                PlayerEquip[8] = Resources.Load<Sprite>("Icons/SmallIcons/" + serialNumber);
                }
                else
                {
                    PlayerEquip_SN[8] = serialNumber;
                    PlayerEquip[8] = Resources.Load<Sprite>("Icons/SmallIcons/" + serialNumber);
                }
                break;
            case "RIN":
                //先判断右手有无戒指，无戒指装备到右手，有戒指装备到左手
                //若都有戒指，则将右手戒指装备到左手，右手装备新戒指
                if (PlayerEquip_SN[1].Length < 3)
                {
                    PlayerEquip_SN[1] = serialNumber;
                    PlayerEquip[1] = Resources.Load<Sprite>("Icons/SmallIcons/" + serialNumber);
                }
                else if (PlayerEquip_SN[0].Length < 3)
                {
                    PlayerEquip_SN[0] = serialNumber;
                    PlayerEquip[0] = Resources.Load<Sprite>("Icons/SmallIcons/" + serialNumber);
                }
                else if (PlayerEquip_SN[1].Length > 3 && PlayerEquip_SN[0].Length > 3)
                {
                    PutIntoBag(PlayerEquip_SN[0]);
                    PlayerEquip_SN[0] = PlayerEquip_SN[1];
                    PlayerEquip[0] = Resources.Load<Sprite>("Icons/SmallIcons/" + PlayerEquip_SN[1]);
                    PlayerEquip_SN[1] = serialNumber;
                    PlayerEquip[1] = Resources.Load<Sprite>("Icons/SmallIcons/" + serialNumber);
                }
                break;
            case "CHE":
                if (PlayerEquip_SN[7].Length > 3)
                {
                    PutIntoBag(PlayerEquip_SN[7]);
                    PlayerEquip_SN[7] = serialNumber;
                    PlayerEquip[7] = Resources.Load<Sprite>("Icons/SmallIcons/" + serialNumber);
                }
                else
                {
                    PlayerEquip_SN[7] = serialNumber;
                    PlayerEquip[7] = Resources.Load<Sprite>("Icons/SmallIcons/" + serialNumber);
                }
                break;
            default:
                break;
        }
        DeleteFromBag(serialNumber);
        ChangeShowData();
    }
    /// <summary>
    /// 脱下装备时的响应事件
    /// </summary>
    /// <param name="serialNumber">此装备的序列号</param>
    public void RemoveEquip(string serialNumber)
    {
        for (int i = 0; i < 9; i++)
        {
            if (PD_RI.RI_Icons[i].GetComponent<SpriteRenderer>().sprite.name == serialNumber)
            {
                PD_RI.RI_Icons[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/NewIcon/NUL_00000001");
                PlayerEquip[i] = Resources.Load<Sprite>("Icons/NewIcon/NUL_00000001");
                PlayerEquip_SN[i] ="0";
            }
        }
        PutIntoBag(serialNumber);
        

        MySqlite thisSQL = new MySqlite("data source=CardsAndDices.db");
        SqliteDataReader reader_RE;
            string Name = serialNumber.Substring(0, 3);
            switch (Name)
                {
                    case "WEA":
                        string sql_01 = "SELECT * FROM Weapon WHERE SerialNumber = '" + serialNumber + "'";
                        reader_RE = thisSQL.ExecuteQuery(sql_01);
                        while (reader_RE.Read())
                        {
                            Debug.Log("name is " + name + " serialNumber is " + serialNumber);
                            for (int j = 9; j < 13; j++)
                            {
                                if (reader_RE[j].ToString().Length > 1)
                                {
                                    //   Debug.Log("Atk is " + Convert.ToInt32(reader_CSD[j].ToString().Substring(4)));
                                    switch (reader_RE[j].ToString().Substring(0, 3))
                                    {
                                        case "ATK":
                                            Atk -= Convert.ToInt32(reader_RE[j].ToString().Substring(4));

                                            break;
                                        case "DEF":
                                            Def -= Convert.ToInt32(reader_RE[j].ToString().Substring(4));
                                            //       Debug.Log("DEF is " + Convert.ToInt32(reader_CSD[j].ToString().Substring(4)));
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                        reader_RE.Close();
                        break;

                    case "CHE":
                        string sql_04 = "SELECT * FROM Chestplate WHERE SerialNumber = '" + serialNumber + "'";
                        reader_RE = thisSQL.ExecuteQuery(sql_04);
                        while (reader_RE.Read())
                        {
                            //   which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                            for (int j = 9; j < 13; j++)
                            {
                                if (reader_RE[j].ToString().Length > 1)
                                {
                                    switch (reader_RE[j].ToString().Substring(0, 3))
                                    {
                                        case "ATK":
                                            Atk -= Convert.ToInt32(reader_RE[j].ToString().Substring(4));
                                            break;
                                        case "DEF":
                                            Def -= Convert.ToInt32(reader_RE[j].ToString().Substring(4));
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                        reader_RE.Close();
                        break;
                    case "HEL":
                        string sql_05 = "SELECT * FROM Helmet WHERE SerialNumber = '" + serialNumber + "'";
                        reader_RE = thisSQL.ExecuteQuery(sql_05);
                        while (reader_RE.Read())
                        {
                            //   which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                            for (int j = 9; j < 13; j++)
                            {
                                if (reader_RE[j].ToString().Length > 1)
                                {
                                    switch (reader_RE[j].ToString().Substring(0, 3))
                                    {
                                        case "ATK":
                                            Atk -= Convert.ToInt32(reader_RE[j].ToString().Substring(4));
                                            break;
                                        case "DEF":
                                            Def -= Convert.ToInt32(reader_RE[j].ToString().Substring(4));
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                        reader_RE.Close();
                        break;
                    case "PAN":
                        string sql_06 = "SELECT * FROM Pants WHERE SerialNumber = '" + serialNumber + "'";
                        reader_RE = thisSQL.ExecuteQuery(sql_06);
                        while (reader_RE.Read())
                        {
                            //   which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                            for (int j = 9; j < 13; j++)
                            {
                                if (reader_RE[j].ToString().Length > 1)
                                {
                                    switch (reader_RE[j].ToString().Substring(0, 3))
                                    {
                                        case "ATK":
                                            Atk -= Convert.ToInt32(reader_RE[j].ToString().Substring(4));
                                            break;
                                        case "DEF":
                                            Def -= Convert.ToInt32(reader_RE[j].ToString().Substring(4));
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                        reader_RE.Close();
                        break;
                    case "NEC":
                        string sql_07 = "SELECT * FROM Necklace WHERE SerialNumber = '" + serialNumber + "'";
                        reader_RE = thisSQL.ExecuteQuery(sql_07);
                        while (reader_RE.Read())
                        {
                            //    which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                            for (int j = 9; j < 13; j++)
                            {
                                if (reader_RE[j].ToString().Length > 1)
                                {
                                    switch (reader_RE[j].ToString().Substring(0, 3))
                                    {
                                        case "ATK":
                                            Atk -= Convert.ToInt32(reader_RE[j].ToString().Substring(4));
                                            //      which.transform.Find("ATK").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                            break;
                                        case "DEF":
                                            Def -= Convert.ToInt32(reader_RE[j].ToString().Substring(4));
                                            //     which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                            break;
                                        case "CRI":
                                            Cri -= Convert.ToInt32(reader_RE[j].ToString().Substring(4));
                                            //     which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                        reader_RE.Close();
                        break;
                    case "RIN":
                        string sql_08 = "SELECT * FROM Rings WHERE SerialNumber = '" + serialNumber + "'";
                        reader_RE = thisSQL.ExecuteQuery(sql_08);
                        while (reader_RE.Read())
                        {
                            //  which.transform.Find("Level").GetComponent<TextMesh>().text = reader_EI[2].ToString();
                            for (int j = 9; j < 13; j++)
                            {
                                if (reader_RE[j].ToString().Length > 1)
                                {
                                    switch (reader_RE[j].ToString().Substring(0, 3))
                                    {
                                        case "ATK":
                                            Atk -= Convert.ToInt32(reader_RE[j].ToString().Substring(4));
                                            //   which.transform.Find("ATK").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                            break;
                                        case "DEF":
                                            Def -= Convert.ToInt32(reader_RE[j].ToString().Substring(4));
                                            //   which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                            break;
                                        case "CRI":
                                            Cri -= Convert.ToInt32(reader_RE[j].ToString().Substring(4));
                                            //   which.transform.Find("Def").GetComponent<TextMesh>().text = reader_EI[i].ToString().Substring(4);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                        reader_RE.Close();
                        break;
                    default:
                        break;
                }
                
      
        Debug.Log("ATK  is " + Atk + " Def is " + Def);
        thisSQL.CloseConnection();
        PD_MI.ChangeInfor();
    }
    /// <summary>
    /// 根据传入信息，返回对应数据
    /// </summary>
   /// <param name="which">条目</param>
    public int GivePlayerData(string which)
    {
        switch (which)
        {
            case "HP":  
                return HitPoint;                   //血量
            case "Def":
                return Defense;                    //防御力
            case "Atk": 
                return AttactDamage;               //伤害
            case "ActNum": 
                return ActionNumber;            //行动力
            case "Cri": 
                return Critial;                    //暴击率
            case "Satiety":
                return Satiety;                    //暴击率
            default:
                return 100;
        }
    }
    /// <summary>
    /// 根据传入的检索字符串返回某一条目下的数据数目
    /// </summary>
    /// <param name="Which">条目</param>
    /// <param name="saveTime">保存时间，若没有则自动查询最近的</param>
    /// <param name="field">限制序列号</param>
    public int GiveDataCount(string which,string field,string saveTime)
    {
        List<string> AllData = new List<string>();
        int Count = 0;
         SqliteDataReader reader_GDC;
        PlayerSQL = new MySqlite("data source=CardsAndDices.db");
        if (saveTime == null)
        {
            string sql_GDC_01 = "SELECT SaveTime FROM " + which;
            reader_GDC = PlayerSQL.ExecuteQuery(sql_GDC_01);
            while (reader_GDC.Read())
            {
                AllData.Add(reader_GDC[0].ToString());
            }
            reader_GDC.Close();
            int count = AllData.Count - 1;
            string sql_GDC_02 = "SELECT * FROM " + which + " WHERE SaveTime = '" + AllData[count] + "' AND SerialNumber LIKE " +"'"+field+"%'";
            reader_GDC = PlayerSQL.ExecuteQuery(sql_GDC_02);
            while (reader_GDC.Read())
                Count += 1;
            reader_GDC.Close();
        }
        else
        {
            string sql_GDC_03 = "SELECT * FROM " + which + " WHERE SaveTime = '" + saveTime + "' AND SerialNumber LIKE " + "'" + field + "%'";
            reader_GDC = PlayerSQL.ExecuteQuery(sql_GDC_03);
            while (reader_GDC.Read())
                Count += 1;
            reader_GDC.Close();
        }
        PlayerSQL.CloseConnection();
        return Count;
    }
    /// <summary>
    /// 根据传入的检索字符串返回某一条目下的序列号
    /// </summary>
    /// <param name="Which">条目</param>
    /// <param name="saveTime">保存时间，若没有则自动查询最近的</param>
    /// <param name="field">限制序列号</param>
    public List<string> GiveSerialNumber(string which, string field, string saveTime)
    {
        List<string> AllData = new List<string>();
        List<string> serialNumbers = new List<string>();
        SqliteDataReader reader_GDC;
        PlayerSQL = new MySqlite("data source=CardsAndDices.db");
        if (saveTime == null)
        {
            string sql_GDC_01 = "SELECT SaveTime FROM " + which;
            reader_GDC = PlayerSQL.ExecuteQuery(sql_GDC_01);
            while (reader_GDC.Read())
            {
                AllData.Add(reader_GDC[0].ToString());
            }
            reader_GDC.Close();
            int count = AllData.Count - 1;
            string sql_GDC_02 = "SELECT SerialNumber FROM " + which + " WHERE SaveTime = '" + AllData[count] + "' AND SerialNumber LIKE " + "'" + field + "%'";
            reader_GDC = PlayerSQL.ExecuteQuery(sql_GDC_02);
            while (reader_GDC.Read())
            {
                serialNumbers.Add(reader_GDC[0].ToString());
            }
            reader_GDC.Close();
        }
        else
        {
            string sql_GDC_03 = "SELECT SerialNumber FROM " + which + " WHERE SaveTime = '" + saveTime + "' AND SerialNumber LIKE " + "'" + field + "%'";
            reader_GDC = PlayerSQL.ExecuteQuery(sql_GDC_03);
            while (reader_GDC.Read())
                serialNumbers.Add(reader_GDC[0].ToString());
            reader_GDC.Close();
        }
        PlayerSQL.CloseConnection();
        return serialNumbers;
    }
    /// <summary>
    /// 查询最近的保存时间
    /// </summary>
    /// <returns></returns>
    public string GetSaveTime()
    {
        SqliteDataReader reader_GDC;
        PlayerSQL = new MySqlite("data source=CardsAndDices.db");
        List<string> Times = new List<string>();
        string sql_GDC_01 = "SELECT SaveTime FROM SaveTime";
        reader_GDC = PlayerSQL.ExecuteQuery(sql_GDC_01);
        while (reader_GDC.Read())
        {
            Times.Add(reader_GDC[0].ToString());
        }
        int count = Times.Count - 1;
        reader_GDC.Close();
     

        PlayerSQL.CloseConnection();
        return Times[count];
    }
    /// <summary>
    /// 将数据存入背包
    /// </summary>
    /// <param name="serialNumber">物品序列号</param>
    public void PutIntoBag(string serialNumber)
    {
       
        if (serialNumber!=null)
        PlayerBagData.Add(serialNumber);

      //  foreach (string i in PlayerBagData)
         //   Debug.Log("Player bag has "+i);
    }
    /// <summary>
    /// 随机失去背包物品
    /// </summary>
    public string RandomLoseBag()
    {
        int i = UnityEngine.Random.Range(0, PlayerBagData.Count);
        string which = PlayerBagData[i];
        DeleteFromBag(which);
        return which;
       // DeleteFromBag(which);
    }
    /// <summary>
    /// 将某数据从背包中删除
    /// </summary>
    /// <param name="serialNumber"></param>
    public void DeleteFromBag(string serialNumber)
    {
        if (serialNumber != null)
        PlayerBagData.Remove(serialNumber);
    }

    /// <summary>
    /// 吃掉时的响应事件
    /// </summary>
    /// <param name="serialNumber"></param>
    public void EatFood(string serialNumber)
    {
        MySqlite thisSQL = new MySqlite("data source=CardsAndDices.db");
        string sql = "SELECT * FROM Foods WHERE SerialNumber = '" + serialNumber + "'";
        SqliteDataReader reader_EF = thisSQL.ExecuteQuery(sql);
        while (reader_EF.Read())
        {
            Debug.Log("food is " + serialNumber + "hp is " + Convert.ToInt32(reader_EF[3]) + " Sat is " + Convert.ToInt32(reader_EF[2]));
            HP += Convert.ToInt32(reader_EF[3]);
            Sat += Convert.ToInt32(reader_EF[2]);
        }
        reader_EF.Close();
        DeleteFromBag(serialNumber);
     //   HP += Convert.ToInt32(food.transform.Find("ATK").GetComponent<TextMesh>().text);
      //  Sat += Convert.ToInt32(food.transform.Find("Def").GetComponent<TextMesh>().text);
        PD_MI.ChangeInfor();
        thisSQL.CloseConnection();
    }


   /// <summary>
   /// 根据保存时间得到玩家的装备、背包、技能和状态信息并赋予图片
   /// </summary>
    public void GetPlayerDatas(string saveTime)
    {
        PlayerSQL = new MySqlite("data source=CardsAndDices.db");
        int id = PlayerSQL.GetIDs("Player");
        string nowTime = System.DateTime.Now.ToString();
      //  for (int i = 0; i < 9; i++)
      //  {
       //     PlayerSkillsData.Add("Skills");
      //  }
        string sql_Equip="SELECT * FROM PlayerEquip WHERE SaveTime = '"+saveTime+"'";
        string sql_Bag = "SELECT * FROM PlayerBag WHERE SaveTime = '" + saveTime + "'";
      //  string sql_Skill = "SELECT * FROM PlayerSkills WHERE SaveTime = '" + saveTime + "'";
       // string sql_AllSkill = "SELECT * FROM PlayerAllSkills WHERE SaveTime = '" + saveTime + "'";`
      //  string sql_MysCon = "SELECT * FROM PlyaerMysCon WHERE SaveTime = '" + saveTime + "'";
        string sql_Player = "SELECT * FROM Player WHERE SaveTime = '" + saveTime + "'";

        //↓读取装备信息
        SqliteDataReader reader_Equip = PlayerSQL.ExecuteQuery(sql_Equip);
        while (reader_Equip.Read())
        {
            PlayerEquip_SN[0] = reader_Equip[1].ToString();
            PlayerEquip_SN[1] = reader_Equip[3].ToString();
            PlayerEquip_SN[2] = reader_Equip[2].ToString();
            PlayerEquip_SN[3] = reader_Equip[4].ToString();
            PlayerEquip_SN[5] = reader_Equip[5].ToString();
            PlayerEquip_SN[6] = reader_Equip[6].ToString();
            PlayerEquip_SN[7] = reader_Equip[7].ToString();
            PlayerEquip_SN[8] = reader_Equip[8].ToString();
        }
        reader_Equip.Close();

        PlayerEquip[0] = Resources.Load<Sprite>("Icons/SmallIcons/" + PlayerEquip_SN[0]);
        PlayerEquip[1] = Resources.Load<Sprite>("Icons/SmallIcons/" + PlayerEquip_SN[2]);
        PlayerEquip[2] = Resources.Load<Sprite>("Icons/SmallIcons/" + PlayerEquip_SN[1]);
        PlayerEquip[3] = Resources.Load<Sprite>("Icons/SmallIcons/" + PlayerEquip_SN[3]);
        PlayerEquip[5] = Resources.Load<Sprite>("Icons/SmallIcons/" + PlayerEquip_SN[5]);
        PlayerEquip[6] = Resources.Load<Sprite>("Icons/SmallIcons/" + PlayerEquip_SN[6]);
        PlayerEquip[7] = Resources.Load<Sprite>("Icons/SmallIcons/" + PlayerEquip_SN[7]);
        PlayerEquip[8] = Resources.Load<Sprite>("Icons/SmallIcons/" + PlayerEquip_SN[8]);
        for (int i = 0; i < PlayerEquip.Length; i++)
            if (PlayerEquip[i] == null&&i!=4)
                PlayerEquip[i] = Resources.Load<Sprite>("Icons/NewIcon/NUL_00000001");

        ChangeShowData();
        //↓读取背包信息
        SqliteDataReader reader_Bag = PlayerSQL.ExecuteQuery(sql_Bag);
        while (reader_Bag.Read())
        {
          //  Debug.Log("reader is "+reader_Bag[0].ToString()+" "+reader_Bag[1].ToString()+" "+reader_Bag[2].ToString());
            PlayerBagData.Add(reader_Bag[2].ToString());
        }
        reader_Bag.Close();

        //读取角色基础数据
        SqliteDataReader reader_Player = PlayerSQL.ReadFullTable("Player");
        while (reader_Player.Read())
        {
      //      Debug.Log("Read is " + reader_Player.GetInt32(reader_Player.GetOrdinal("Constitution")));
               Constitution= (int)reader_Player[1];
               Strength = (int)reader_Player[2];
               Intelligence = (int)reader_Player[3];
               Dexterity = (int)reader_Player[4];
               Level = (int)reader_Player[5];
               HitPoint = (int)reader_Player[6];
               Defense = (int)reader_Player[7];
               AttactDamage = (int)reader_Player[8];
               ParryPoint = (int)reader_Player[9];
               MagicNumber = (int)reader_Player[10];
               AbilityDamage = (int)reader_Player[11];
               Critial = (int)reader_Player[12];
               Accuracy = (int)reader_Player[13];
               ActionNumber = (int)reader_Player[14];
               Foods = (int)reader_Player[15];
               Satiety = (int)reader_Player[16];    
        }
        reader_Player.Close();

        

        
        /*  for (int i = 0; i < 9; i++)
          {
              if (i != 4)
                  PlayerSkills[i] = Resources.Load<Sprite>("Icons/" + PlayerSkillsData[i]);
              else
                  PlayerSkills[i] = Resources.Load<Sprite>("Icons/TianFu");
          }
            /*     //↓读取装备技能数据
            SqliteDataReader reader_Skill = PlayerSQL.ExecuteQuery(sql_Skill);
            while (reader_Skill.Read())
            {
                PlayerSkill_SN[0] = reader_Skill[1].ToString();
                PlayerSkill_SN[1] = reader_Skill[2].ToString();
                PlayerSkill_SN[2] = reader_Skill[3].ToString();
                PlayerSkill_SN[3] = reader_Skill[4].ToString();
                PlayerSkill_SN[4] = reader_Skill[5].ToString();
                PlayerSkill_SN[5] = reader_Skill[6].ToString();
                PlayerSkill_SN[6] = reader_Skill[7].ToString();
                PlayerSkill_SN[7] = reader_Skill[8].ToString();
                PlayerSkill_SN[8] = reader_Skill[9].ToString();
            }
            reader_Skill.Close();

            //↓读取所有技能数据
            SqliteDataReader reader_AllSkill = PlayerSQL.ExecuteQuery(sql_AllSkill);
            while (reader_AllSkill.Read())
            {
                PlayerSkillsData.Add(reader_AllSkill[3].ToString());
            }
            reader_AllSkill.Close();*/

            /*  //↓读取神秘状态信息
              SqliteDataReader reader_MysCon = PlayerSQL.ExecuteQuery(sql_MysCon);
              while (reader_MysCon.Read())
              {
                  PlayerMysCon_SN[0] = reader_MysCon[1].ToString();
                  PlayerMysCon_SN[1] = reader_MysCon[2].ToString();
                  PlayerMysCon_SN[2] = reader_MysCon[3].ToString();
                  PlayerMysCon_SN[3] = reader_MysCon[4].ToString();
                  PlayerMysCon_SN[4] = reader_MysCon[5].ToString();
                  PlayerMysCon_SN[5] = reader_MysCon[6].ToString();
                  PlayerMysCon_SN[6] = reader_MysCon[7].ToString();
                  PlayerMysCon_SN[7] = reader_MysCon[8].ToString();
                  PlayerMysCon_SN[8] = reader_MysCon[9].ToString();
              }
              reader_MysCon.Close();*/
                PlayerSQL.CloseConnection();
    }
    /// <summary>
    /// 将当前玩家的所有数据存入数据库
    /// </summary>
    public void PutDataIntoDB()
    {
        PlayerSQL = new MySqlite("data source=CardsAndDices.db");
        string nowTime = System.DateTime.Now.ToString();
        Debug.Log("now time is " + nowTime);

        //↓将玩家装备数据存入数据库
        int id_equip = PlayerSQL.GetIDs("PlayerEquip");
        string[] Equip = PlayerSQL.CreateString(id_equip, PlayerEquip_SN[0], PlayerEquip_SN[1], PlayerEquip_SN[2], PlayerEquip_SN[3], PlayerEquip_SN[5], PlayerEquip_SN[6], PlayerEquip_SN[7], PlayerEquip_SN[8], nowTime);
        PlayerSQL.InsertValues("PlayerEquip", Equip);

        //↓将玩家背包数据存入数据库
        int id_bag = PlayerSQL.GetIDs("PlayerBag");
        string[] Bag=null;
        foreach (object bag in PlayerBagData)
        {
            if (bag != null)
            {
                Bag = PlayerSQL.CreateString(id_bag, nowTime, bag);
                PlayerSQL.InsertValues("PlayerBag", Bag);
            }
        }
      
    /*    //↓将玩家神秘状态数据存入数据库
        int id_mysCon = PlayerSQL.GetIDs("PlyaerMysCon");
        string[] MysCon = PlayerSQL.CreateString(id_mysCon, PlayerMysCon_SN[0], PlayerMysCon_SN[1], PlayerMysCon_SN[2], PlayerMysCon_SN[3], PlayerMysCon_SN[4], PlayerMysCon_SN[5], PlayerMysCon_SN[6], PlayerMysCon_SN[7], PlayerMysCon_SN[8], nowTime);
        PlayerSQL.InsertValues("PlyaerMysCon", MysCon);*/

        //↓将玩家基础数据存入数据库
        int id_player = PlayerSQL.GetIDs("Player");
       // PutBasicValueIntoDB(Constitution, Strength, Intelligence, Dexterity);
        string[] Player = PlayerSQL.CreateString(id_player, Constitution, Strength, Intelligence, Dexterity, Level, HitPoint, Defense, AttactDamage, ParryPoint, MagicNumber, AbilityDamage, Critial, Accuracy, ActionNumber,Foods,Satiety, nowTime);
        PlayerSQL.InsertValues("Player", Player);

        int id_SaveTime = PlayerSQL.GetIDs("SaveTime");
        string[] SaveTime = PlayerSQL.CreateString(id_SaveTime, nowTime);
        PlayerSQL.InsertValues("SaveTime", SaveTime);
    /*    //↓将玩家装备的技能数据存入数据库
        int id_skills = PlayerSQL.GetIDs("PlayerSkills");
        string[] Skills = PlayerSQL.CreateString(id_skills, PlayerSkill_SN[0], PlayerSkill_SN[1], PlayerSkill_SN[2], PlayerSkill_SN[3], PlayerSkill_SN[4], PlayerSkill_SN[5], PlayerEquip_SN[6], PlayerEquip_SN[7], PlayerEquip_SN[8], nowTime);
        PlayerSQL.InsertValues("PlayerSkills", Skills);

        //↓将玩家所有技能数据存入数据库
        int id_allSkill = PlayerSQL.GetIDs("PlayerAllSkills");
        string[] allSkill = null;
        foreach (object allskill in PlayerSkillsData)
        {
            if (allskill != null)
            {
                allSkill = PlayerSQL.CreateString(id_allSkill, nowTime,allskill);
                    PlayerSQL.InsertValues("PlayerAllSkills",allSkill);
            }
        }*/
        PlayerSQL.CloseConnection();
    }
 
    /// <summary>
    /// 根据获得的装备属性或升级，动态改变玩家数据
    /// </summary>
    public void ChangeValue()
    {

    }
    /// <summary>
    /// 对得到的基础属性进行运算
    /// </summary>
    /// <param name="Con">体质</param>
    /// <param name="Str">力量</param>
    /// <param name="Int">智力</param>
    /// <param name="Dex">敏捷</param>
    /// <param name="level">等级</param>
    /// <param name="saveTime">保存时间</param>
    public void BasicValue(int Con,int Str,int Int,int Dex)
    {
        Constitution = Con;
        Strength = Str;
        Intelligence = Int;
        Dexterity = Dex;

        HitPoint = Constitution;
        Defense = (int)Mathf.Floor(Constitution / 10);
        AttactDamage = Strength;
        ParryPoint = (int)Mathf.Floor(Strength / 2);
        MagicNumber = (int)Mathf.Floor(Intelligence / 5)+1;
        AbilityDamage = (int)Mathf.Floor(Intelligence / 3);
        Critial = Dexterity * 2;
        Accuracy = Dexterity * 4;
        ActionNumber = 2;
        Satiety = 12+(int)Mathf.Floor(Constitution / 5);
      //  Debug.Log("HP is "+HitPoint+"Def is "+Defense+"Atk is "+AttactDamage+"Cri is "+Critial+"ActionNumber is "+ActionNumber);
    }
    //↓向有需求的项传递四项基础属性值
    public void GiveData(bool Con,bool Str,bool Int,bool Dex)
    {
        if (Con)
        {
        }
        if (Str)
        {
        }
        if (Int)
        {
        }
        if (Dex)
        {
        }
    }
    //↓得到变化后的衍生值
    public void GetDerivativeValue(int HP,int DEF,int ATD,int PP,int MN,int ABD,int CRI,int HR,int HN,int FOOD,int SAT)
    {
     A_HitPoint=HP;                    //偏差血量
     A_Defense=DEF;                     //偏差防御力
     A_AttactDamage=ATD;              //偏差物理伤害
     A_ParryPoint=PP;                  //偏差格挡减伤值
     A_MagicNumber=MN;                //偏差法术个数
     A_AbilityDamage=ABD;             //偏差法术伤害
     A_Critial=CRI;                    //偏差暴击率
     A_Accuracy = HR;                 //偏差命中率
     A_HitNumber=HN;                  //偏差攻击次数
     A_Foods=FOOD;                       //偏差食物数量
     A_Satiety=SAT;                     //偏差饱食度
    }

    /*↓从外部接收数据操作相关指令并进行内部调用
    //  commandKind 操作类型: 存(T)/取(F)
    //  dataCommand  数据指令
    public void DataOperation(bool commandKind, string dataCommand)
    {
        if (commandKind)
            SaveDataToDB(dataCommand);
        else
            GetDataFromDB(dataCommand);
    }

    //↓将数据存入角色状态数据库
    private void SaveDataToDB(string dataCommand)
    {
        MySqlite SQL_PlayerData;
        SQL_PlayerData = new MySqlite("data source=CardsAndDices.db");


        SQL_PlayerData.CloseConnection();
    }


    //↓从数据库中读出数据
    private void GetDataFromDB(string dataCommand)
    {
        MySqlite SQL_PlayerData;
        SQL_PlayerData = new MySqlite("data source=CardsAndDices.db");


        SQL_PlayerData.CloseConnection();
    }*/



    /// <summary>
    /// 创建数据库
    /// </summary>
    private void CreateData()
    {
        MySqlite SQL_PlayerData;
        SQL_PlayerData = new MySqlite("data source=CardsAndDices.db");
        //↓创建角色基础数据表
        string sql_01 = "CREATE TABLE IF NOT EXISTS Player(id INTEGER,Constitution INTEGER,Strength INTEGER,Intelligence INTEGER,Dexterity INTEGER,Level INTEGER,HitPoint INTEGER,Defense INTEGER,AttactDamage INTEGER,ParryPoint INTEGER,MagicNumber INTEGER,AbilityDamage INTEGER,Critial INTEGER,Accuracy INTEGER,HitNumber INTEGER,Foods INTEGER,Satiety INTEGER)";
        SQL_PlayerData.ExecuteQuery(sql_01);

        /*   int id = SQL_PlayerData.GetIDs("Player");
           string[] Datas={id.ToString(),Constitution.ToString(),Strength.ToString(),Intelligence.ToString(),Dexterity.ToString(),Level.ToString(),HitPoint.ToString(),Defense.ToString(),AttactDamage.ToString(),ParryPoint.ToString(),MagicNumber.ToString(),AbilityDamage.ToString(),Critial.ToString(),HitRating.ToString(),HitNumber.ToString(),Foods.ToString(),Satiety.ToString()};
           SQL_PlayerData.InsertValues("Player", Datas);*/

        //↓创建角色装备表，TEXT为装备序列号
        //  1.LeftRing 左手戒指 2.RightRing 右手戒指 3.Necklace 项链 4.LeftHandWeapon 左手武器 5.RightHandWeapon 右手武器 6.Helmet 头盔 7.Chestplate 盔甲 8.Pants 裤子
        string sql_02 = "CREATE TABLE IF NOT EXISTS PlayerEquip(id INTEGER,LeftRing TEXT,RightRing TEXT,Necklace TEXT,LeftHandWeapon TEXT,RightHandWeapon TEXT,Helmet TEXT,Chestplate TEXT,Pants TEXT)";
        SQL_PlayerData.ExecuteQuery(sql_02);

        //↓创建角色背包表
        //  ItemKind:物品种类 1.Foods 食品 2.consumables 消耗品 3.Material 材料 4.Weapons 武器 5.Map 地图 6.Jewelry 饰品 7.Helmet 头盔 8.Chestplate 盔甲 9. Pants 裤子
        string sql_03 = "CREATE TABLE IF NOT EXISTS PlayerBag(id INTEGER,Items TEXT,ItemKind TEXT)";
        SQL_PlayerData.ExecuteQuery(sql_03);

        //↓创建角色可用技能表
        //  
        string sql_04 = "CREATE TABLE IF NOT EXISTS PlayerSkills(id INTEGER,Skill_02 TEXT,Skill_12 TEXT,Skill_22 TEXT,Skill_01 TEXT,Skill_11 TEXT,Skill_21 TEXT,Skill_00 TEXT,Skill_10 TEXT,Skill_20 TEXT,SaveTime TEXT)";
        SQL_PlayerData.ExecuteQuery(sql_04);

        //↓创建角色神秘状态表
        //  
        string sql_05 = "CREATE TABLE IF NOT EXISTS PlyaerMysCon(id INTEGER,MysCon_02 TEXT,MysCon_12 TEXT,MysCon_22 TEXT,MysCon_01 TEXT,MysCon_11 TEXT,MysCon_21 TEXT,MysCon_00 TEXT,MysCon_10 TEXT,MysCon_20 TEXT,SaveTime TEXT)";
        SQL_PlayerData.ExecuteQuery(sql_05);

        //↓创建角色拥有技能表
        //  1.SkillName 技能名 2. SkillKind 技能种类（1.天赋 2.主动 3.被动） 
        string sql_11 = "CREATE TABLE IF NOT EXISTS PlayerAllSkills(id INTEGER,SkillName TEXT,SkillKind TEXT,SaveTime TEXT)";
        SQL_PlayerData.ExecuteQuery(sql_11);

        //↓创建保存时间表
        //  1.SkillName 技能名 2. SkillKind 技能种类（1.天赋 2.主动 3.被动） 
        string sql_12 = "CREATE TABLE IF NOT EXISTS SaveTime(id INTEGER,SaveTime TEXT)";
        SQL_PlayerData.ExecuteQuery(sql_12);
  /*   string sql_06 = "ALTER TABLE Player ADD COLUMN SerialNumber TEXT";
        SQL_PlayerData.ExecuteQuery(sql_06);

        string sql_07 = "ALTER TABLE PlayerBag ADD COLUMN SerialNumber TEXT";
        SQL_PlayerData.ExecuteQuery(sql_07);

        string sql_08 = "ALTER TABLE PlayerEquip ADD COLUMN SerialNumber TEXT";
        SQL_PlayerData.ExecuteQuery(sql_08);

        string sql_09 = "ALTER TABLE PlayerSkills ADD COLUMN SerialNumber TEXT";
        SQL_PlayerData.ExecuteQuery(sql_09);

        string sql_10 = "ALTER TABLE PlyaerMysticalCondition ADD COLUMN SerialNumber TEXT";
        SQL_PlayerData.ExecuteQuery(sql_10);

        string sql_12 = "ALTER TABLE PlayerAllSkills ADD COLUMN SerialNumber TEXT";
        SQL_PlayerData.ExecuteQuery(sql_12);*/

        SQL_PlayerData.CloseConnection();
    }
}
