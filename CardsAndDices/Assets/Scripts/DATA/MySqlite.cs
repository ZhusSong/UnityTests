using UnityEngine;
using System.Collections;
using System;
using Mono.Data.Sqlite;
using System.Data;
using System.Collections.Generic;

public class MySqlite 
{
   	/// <summary>
    /// 数据库连接定义
    /// </summary>
    private SqliteConnection DbConnection;

    /// <summary>
    /// SQL命令定义
    /// </summary>
    private SqliteCommand DbCommand;

    /// <summary>
    /// 数据读取定义
    /// </summary>
    private SqliteDataReader DataReader;

    /// <summary>
    /// 构造函数    
    /// </summary>
    /// <param name="connectionString">数据库连接字符串</param>
    public MySqlite(string connectionString)
    {
            try
            {
                //构造数据库连接
                DbConnection = new SqliteConnection(connectionString);
                //打开数据库
                DbConnection.Open();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
    }

    /// <summary>
    /// 执行SQL命令
    /// </summary>
    /// <returns>The query.</returns>
    /// <param name="queryString">SQL命令字符串</param>
    public SqliteDataReader ExecuteQuery(string queryString)
    {
        DbCommand = DbConnection.CreateCommand();
        DbCommand.CommandText = queryString;
        DataReader = DbCommand.ExecuteReader();
        return DataReader;
      
    }
  

    /// <summary>
    /// 关闭数据库连接
    /// </summary>
    public void CloseConnection()
    {
        //销毁Command
        if (DbCommand != null)
        {
            DbCommand.Cancel();
        }
        DbCommand = null;

        //销毁Reader
        if (DataReader != null)
        {
            DataReader.Close();
        }
        DataReader = null;

        //销毁Connection
        if (DbConnection != null)
        {
            DbConnection.Close();
        }
        DbConnection = null;
    }
    /// <summary>
    /// 遍历指定表并返回此次id值
    /// </summary>
    /// <returns></returns>
    public int GetIDs(string tableName)
    {
        List<int> thisList = new List<int>();
        int Num = 0;
        string str = "SELECT id FROM " + tableName;
        SqliteDataReader reader = ExecuteQuery(str);
        while (reader.Read())
        {
            thisList.Add(Convert.ToInt32(reader[0]));
            foreach (int i in thisList)
            {
                if (Num == i)
                    Num += 1;
            }
        }
        foreach (int i in thisList)
        {
            if (Num == i)
                Num += 1;
        }
        return Num;
    }


    /// <summary>
    /// 查询某一表的某一值是否重复，重复时返回真
    /// </summary>
    /// <param name="which">表名</param>
    /// <param name="name">字段名</param>
    /// <returns></returns>
    public bool IfRepeat(string which, string name,string serialNumber)
    {
        SqliteCommand com = new SqliteCommand();
        com.Connection = DbConnection;
        bool flag_01 = false;
        bool flag_02 = false;
        com.CommandText = "SELECT * FROM " + which + " WHERE Name = " + "'" + name + "'";
        SqliteDataReader reader = com.ExecuteReader();
        while (reader.Read())
        {
            flag_01 = true;
        }
        reader.Close();
        com.CommandText = "SELECT * FROM " + which + " WHERE SerialNumber = " + "'" + serialNumber + "'";
         reader = com.ExecuteReader();
        while (reader.Read())
        {
            flag_02 = true;
        }
        reader.Close();
        if (!flag_01 && !flag_02)
            return false;
        else
            return true;
    }
    /// <summary>
    /// 根据输入的变量，创建字符串数组，便于数据库管理函数的使用
    /// </summary>
    /// <param name="list">变量列表</param>
    /// <returns></returns>
    public string[] CreateString(params object[] list)
    {
        string[] str = new string[list.Length];
        int i = 0;
        foreach (object o in list)
        {
            if (o == null)
            {
                str[i] = "'" + o + "'";
                i++;
            }
            else if (o.GetType() == typeof(int))
            {
                str[i] ="'"+ o.ToString()+"'";
                i++;
            }
            else if (o.GetType() == typeof(string))
            {
                str[i] = "'" + (string)o + "'";
                i++;
            }

        }
        return str;
    }
    /// <summary>
    /// 读取整张数据表
    /// </summary>
    /// <returns>The full table.</returns>
    /// <param name="tableName">数据表名称</param>
    public SqliteDataReader ReadFullTable(string tableName)
    {
        string queryString = "SELECT * FROM " + tableName;
        return ExecuteQuery (queryString);
    }
   
    /// <summary>
    /// 向指定数据表中插入数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="values">插入的数值</param>
    public SqliteDataReader InsertValues(string tableName,string[] values)
    {
        //获取数据表中字段数目
        int fieldCount=ReadFullTable(tableName).FieldCount;
    //    Debug.Log("FC IS "+fieldCount);
      //  Debug.Log("VA IS "+values.Length);
        //当插入的数据长度不等于字段数目时引发异常
        if(values.Length!=fieldCount){
            throw new SqliteException("values.Length!=fieldCount");
        }

        string queryString = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for(int i=1; i<values.Length; i++)
        {
          
            queryString+=", " + values[i];
        }
        queryString += ")";
        return ExecuteQuery(queryString);
    }

   
    /// <summary>
    /// 更新指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    /// <param name="key">关键字</param>
    /// <param name="value">关键字对应的值</param>
    public SqliteDataReader UpdateValues(string tableName,string[] colNames,string[] colValues,string key,string operation,string value)
    {
        //当字段名称和字段数值不对应时引发异常
        if(colNames.Length!=colValues.Length) {
            throw new SqliteException("colNames.Length!=colValues.Length");
        }

        string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + colValues[0];
        for(int i=1; i<colValues.Length; i++) 
        {
            queryString+=", " + colNames[i] + "=" + colValues[i];
        }
        queryString += " WHERE " + key + operation + value;
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    public SqliteDataReader DeleteValuesOR(string tableName,string[] colNames,string[] operations,string[] colValues)
    {
        //当字段名称和字段数值不对应时引发异常
        if(colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length) {
            throw new SqliteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
        }

        string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
        for(int i=1; i<colValues.Length; i++) 
        {
            queryString+="OR " + colNames[i] + operations[0] + colValues[i];
        }
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    public SqliteDataReader DeleteValuesAND(string tableName,string[] colNames,string[] operations,string[] colValues)
    {
        //当字段名称和字段数值不对应时引发异常
        if(colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length) {
            throw new SqliteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
        }

        string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
        for(int i=1; i<colValues.Length; i++) 
        {
            queryString+=" AND " + colNames[i] + operations[i] + colValues[i];
        }
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 创建数据表
    /// </summary> +
    /// <returns>The table.</returns>
    /// <param name="tableName">数据表名</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colTypes">字段名类型</param>
    public SqliteDataReader CreateTable(string tableName,string[] colNames,string[] colTypes)
    {
        SqliteDataReader createTableReader;
        int j=0;
      string findString = "select count(*) from sqlite_master where type ='table' and name = '"+tableName+"'";
      createTableReader = ExecuteQuery(findString);
    if (createTableReader.Read())
         j=createTableReader.GetInt32(0);
    Debug.Log("Return value is " + j+" and TableName is "+tableName);
        Debug.Log("colName[0] is "+colNames[0]+" and colType[0] is "+colTypes[0]);
    if (j == 0)
    {
        string queryString = "CREATE TABLE " + tableName + "( " + colNames[0] + " " + colTypes[0];
        for (int i = 1; i < colNames.Length; i++)
        {
            queryString += ", " + colNames[i] + " " + colTypes[i];
        }
        queryString += "  ) ";
        return ExecuteQuery(queryString);
    }
    else
        return null;
    }

    /// <summary>
    /// Reads the table.
    /// </summary>
    /// <returns>The table.</returns>
    /// <param name="tableName">Table name.</param>
    /// <param name="items">Items.</param>
    /// <param name="colNames">Col names.</param>
    /// <param name="operations">Operations.</param>
    /// <param name="colValues">Col values.</param>
    public SqliteDataReader ReadTable(string tableName,string[] items,string[] colNames,string[] operations, string[] colValues)
    {
        string queryString = "SELECT " + items [0];
        for (int i=1; i<items.Length; i++) 
        {
            queryString+=", " + items[i];
        }
        queryString += " FROM " + tableName + " WHERE " + colNames[0] + " " +  operations[0] + " " + colValues[0];
        for (int i=0; i<colNames.Length; i++) 
        {
            queryString+=" AND " + colNames[i] + " " + operations[i] + " " + colValues[0] + " ";
        }
        return ExecuteQuery(queryString);
    }
}
