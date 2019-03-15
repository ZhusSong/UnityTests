using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System.Data;
/// <summary>
/// 装备数据的基础管理脚本
/// </summary>
public class EquipsData : MonoBehaviour {
    private MySqlite SQL_EquipsData;
	// Use this for initialization
	void Start () {
        SQL_EquipsData = new MySqlite("data source=CardsAndDices.db");
        //↓创建戒指表
        string sql_01 = "CREATE TABLE IF NOT EXISTS Rings(id INTEGER,Name TEXT,LimitLevel INTEGER,LimitAttribute_01 TEXT,LimitAttributeNumber_01 INTEGER,LimitAttribute_02 TEXT,LimitAttributeNumber_02 INTEGER,LimitAttribute_03 TEXT,LimitAttributeNumber_03 INTEGER,SpecialField_01 TEXT,SpecialField_02 TEXT,SpecialField_03 TEXT,SpecialField_04 TEXT,SpecialField_05 TEXT,Introduce TEXT,SerialNumber TEXT)";
        SQL_EquipsData.ExecuteQuery(sql_01);
        //↓创建项链表
        string sql_02 = "CREATE TABLE IF NOT EXISTS Necklace(id INTEGER,Name TEXT,LimitLevel INTEGER,LimitAttribute_01 TEXT,LimitAttributeNumber_01 INTEGER,LimitAttribute_02 TEXT,LimitAttributeNumber_02 INTEGER,LimitAttribute_03 TEXT,LimitAttributeNumber_03 INTEGER,SpecialField_01 TEXT,SpecialField_02 TEXT,SpecialField_03 TEXT,SpecialField_04 TEXT,SpecialField_05 TEXT,Introduce TEXT,SerialNumber TEXT)";
        SQL_EquipsData.ExecuteQuery(sql_02);
        //↓创建武器表
        string sql_03 = "CREATE TABLE IF NOT EXISTS Weapon(id INTEGER,Name TEXT,LimitLevel INTEGER,LimitAttribute_01 TEXT,LimitAttributeNumber_01 INTEGER,LimitAttribute_02 TEXT,LimitAttributeNumber_02 INTEGER,LimitAttribute_03 TEXT,LimitAttributeNumber_03 INTEGER,SpecialField_01 TEXT,SpecialField_02 TEXT,SpecialField_03 TEXT,SpecialField_04 TEXT,SpecialField_05 TEXT,Introduce TEXT,SerialNumber TEXT)";
        SQL_EquipsData.ExecuteQuery(sql_03);
        //↓创建头盔表
        string sql_04 = "CREATE TABLE IF NOT EXISTS Helmet(id INTEGER,Name TEXT,LimitLevel INTEGER,LimitAttribute_01 TEXT,LimitAttributeNumber_01 INTEGER,LimitAttribute_02 TEXT,LimitAttributeNumber_02 INTEGER,LimitAttribute_03 TEXT,LimitAttributeNumber_03 INTEGER,SpecialField_01 TEXT,SpecialField_02 TEXT,SpecialField_03 TEXT,SpecialField_04 TEXT,SpecialField_05 TEXT,Introduce TEXT,SerialNumber TEXT)";
        SQL_EquipsData.ExecuteQuery(sql_04);
        //↓创建盔甲表
        string sql_05 = "CREATE TABLE IF NOT EXISTS Chestplate(id INTEGER,Name TEXT,LimitLevel INTEGER,LimitAttribute_01 TEXT,LimitAttributeNumber_01 INTEGER,LimitAttribute_02 TEXT,LimitAttributeNumber_02 INTEGER,LimitAttribute_03 TEXT,LimitAttributeNumber_03 INTEGER,SpecialField_01 TEXT,SpecialField_02 TEXT,SpecialField_03 TEXT,SpecialField_04 TEXT,SpecialField_05 TEXT,Introduce TEXT,SerialNumber TEXT)";
        SQL_EquipsData.ExecuteQuery(sql_05);
        //↓创建裤子表
        string sql_06 = "CREATE TABLE IF NOT EXISTS Pants(id INTEGER,Name TEXT,LimitLevel INTEGER,LimitAttribute_01 TEXT,LimitAttributeNumber_01 INTEGER,LimitAttribute_02 TEXT,LimitAttributeNumber_02 INTEGER,LimitAttribute_03 TEXT,LimitAttributeNumber_03 INTEGER,SpecialField_01 TEXT,SpecialField_02 TEXT,SpecialField_03 TEXT,SpecialField_04 TEXT,SpecialField_05 TEXT,Introduce TEXT,SerialNumber TEXT)";
        SQL_EquipsData.ExecuteQuery(sql_06);
        //↓创建材料表
        string sql_07 = "CREATE TABLE IF NOT EXISTS Material(id INTEGER,Name TEXT,Introduce TEXT,SerialNumber TEXT)";
        SQL_EquipsData.ExecuteQuery(sql_07);
        //↓创建消耗品表
        string sql_08 = "CREATE TABLE IF NOT EXISTS Consumables(id INTEGER,Name TEXT,SpecialField_01 TEXT,SpecialField_02 TEXT,SpecialField_03 TEXT,Introduce TEXT,SerialNumber TEXT)";
        SQL_EquipsData.ExecuteQuery(sql_08);
        //↓创建食品表
        string sql_09 = "CREATE TABLE IF NOT EXISTS Foods(id INTEGER,Name TEXT,SpecialField_01 TEXT,SpecialField_02 TEXT,SpecialField_03 TEXT,Introduce TEXT,SerialNumber TEXT)";
        SQL_EquipsData.ExecuteQuery(sql_09);

        /*   string sql_10 = "ALTER TABLE Weapon ADD COLUMN KIND TEXT ";
          SQL_EquipsData.ExecuteQuery(sql_10);
         string sql_11 = "ALTER TABLE Weapon ADD COLUMN ATK_Max INTEGER ";
          SQL_EquipsData.ExecuteQuery(sql_11);
          string sql_12 = "ALTER TABLE Helmet ADD COLUMN DEF_Min INTEGER ";
          SQL_EquipsData.ExecuteQuery(sql_12);
          string sql_13 = "ALTER TABLE Helmet ADD COLUMN DEF_Max INTEGER ";
          SQL_EquipsData.ExecuteQuery(sql_13);
          string sql_14 = "ALTER TABLE Chestplate ADD COLUMN DEF_Min INTEGER ";
          SQL_EquipsData.ExecuteQuery(sql_14);
          string sql_15 = "ALTER TABLE Chestplate ADD COLUMN DEF_Max INTEGER ";
          SQL_EquipsData.ExecuteQuery(sql_15);
          string sql_16 = "ALTER TABLE Pants ADD COLUMN DEF_Min INTEGER ";
          SQL_EquipsData.ExecuteQuery(sql_16);
          string sql_17 = "ALTER TABLE Pants ADD COLUMN DEF_Max INTEGER ";
          SQL_EquipsData.ExecuteQuery(sql_17);*/

        int ID = SQL_EquipsData.GetIDs("Helmet");
        string[] str = SQL_EquipsData.CreateString(ID, "Test001",0, "STR", 0, "INT", 0, "CON", 1, "NULL", "NULL", "NULL", "NULL", "NULL", "This is the test", "HEL_00000001",1,3);
 
        AddDataInEquip("Helmet", "Test001", "HEL_00000001", str);
      
      /*  int ID_1 = SQL_EquipsData.GetIDs("Pants");
        string[] str_1 = SQL_EquipsData.CreateString(ID_1, "Test001", 0, "STR", 0, "INT", 0, "CON", 1, "NULL", "NULL", "NULL", "NULL", "NULL", "This is the test", "PAN_00000001", 1, 3);
        AddDataInEquip("Pants", "Test001", "PAN_00000001", str_1);

        int ID_2 = SQL_EquipsData.GetIDs("Weapon");
       string[] str_2 = SQL_EquipsData.CreateString(ID_2, "Test001", 0, "STR", 0, "INT", 0, "CON", 1, "NULL", "NULL", "NULL", "NULL", "NULL", "This is the test", "WEA_00000001", 2, 5,"Weapon");
       AddDataInEquip("Weapon", "Test001", "WEA_00000001", str_2);

       int ID_3 = SQL_EquipsData.GetIDs("Weapon");
       string[] str_3 = SQL_EquipsData.CreateString(ID_3, "Test002", 0, "STR", 0, "INT", 0, "CON", 1, "NULL", "NULL", "NULL", "NULL", "NULL", "This is the test", "WEA_00000002", 2, 5, "Shield");
       AddDataInEquip("Weapon", "Test002", "WEA_00000002", str_3);*/
        SQL_EquipsData.CloseConnection();
    }

    /// <summary>
    /// 向道具库中添加数据
    /// </summary>
    /// <param name="which">哪个库</param>
    /// <param name="name">道具名</param>
    /// <param name="serialNumber">道具序列号</param>
    /// <param name="data">数据数组</param>
    public void AddDataInEquip(string which,string name,string serialNumber,string[] data)
    {
        SqliteCommand com = new SqliteCommand();
        bool flag = SQL_EquipsData.IfRepeat(which, name, serialNumber);
        if (!flag)
            SQL_EquipsData.InsertValues(which, data);
        SQL_EquipsData.CloseConnection();
    }

}
