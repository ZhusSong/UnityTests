using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System;
/// <summary>
/// 储存怪物数据的库
/// </summary>
public class MonsterData : MonoBehaviour {
    private MySqlite SQL_MonsterData;
	// Use this for initialization
    public void L_Start()
    {
    //    SQL_MonsterData = new MySqlite("data source=CardsAndDices.db");
     //   string sql_01 = "CREATE TABLE IF NOT EXISTS Monsters(id INTEGER,Name TEXT,Kind TEXT,Level TEXT,Constitution INTEGER,Strength INTEGER,Dexterity INTEGER,RareLevel TEXT,SpecialField_01 TEXT,SpecialField_02 TEXT,SpecialField_03 TEXT,SpecialField_04 TEXT,SpecialField_05 TEXT)";
     //   SQL_MonsterData.ExecuteQuery(sql_01);
    //   int[] DATA= GiveMonsterData("MON_00000001");
   //    Debug.Log("Level is " + DATA[0] + " HP is " + DATA[1] + " ActNum is " + DATA[2] + " ATK is " + DATA[3] + " DEF is " + DATA[4]+" CRI IS "+DATA[5]);
      
	}
    /// <summary>
    /// 根据序列号得到怪物数据
    /// </summary>
    /// <param name="serialNumber"></param>
    public int[]  GiveMonsterData(string serialNumber)
    {
        MySqlite thisSQL= new MySqlite("data source=CardsAndDices.db");
        SqliteDataReader reader_GMD;
        //基础数据 0:等级 1:体质 2:力量 3: 敏捷 4:行动力
        int[] BasicData=new int[5];
        //计算后的数据: 0:等级  1:HP 2:行动力 3:攻击力 4:防御力 5:暴击
        int[] thisData = new int[6];

        string sql = "SELECT * FROM Monsters WHERE SerialNumber = '"+serialNumber+"'";
        reader_GMD = thisSQL.ExecuteQuery(sql);
        while (reader_GMD.Read())
        {
            BasicData[0] =Convert.ToInt32(reader_GMD[3].ToString());
            BasicData[1] = Convert.ToInt32(reader_GMD[4].ToString());
            BasicData[2] = Convert.ToInt32(reader_GMD[5].ToString());
            BasicData[3] = Convert.ToInt32(reader_GMD[6].ToString());
            BasicData[4] = Convert.ToInt32(reader_GMD[13].ToString());
        }
        Debug.Log( "Level is "+BasicData[0] +"Con is "+ BasicData[1] +"Str is "+ BasicData[2]+"Dex is "+ BasicData[3] +"ActNum is "+BasicData[4]);
        reader_GMD.Close();

        //根据得到的基础数据计算二级数据
        thisData[0] = BasicData[0];
        thisData[1] = BasicData[1];
        thisData[2] = BasicData[4];
        thisData[3] = BasicData[2];
        thisData[4] = (int)BasicData[1]/5;
        thisData[5] = BasicData[3]*5;

        thisSQL.CloseConnection();

        return thisData;

      
    }
}


