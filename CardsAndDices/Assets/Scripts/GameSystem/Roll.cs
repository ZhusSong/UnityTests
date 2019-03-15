using UnityEngine;
using System.Collections;
//using UnityEngine.UI;
using DG.Tweening;

public class Roll {
    private int TensDidgt;      //十位显示数字
    private int OnesDidgt;      //个位显示数字
    void Start () { 
	}
    //↓普通2d10（R100）事件结果，count为补正值
    public int NormalEvent(int count )
    {
        int Result = Random.Range(0, 101);
        Result += count;
        Debug.Log("This Roll Value is " + Result);
        return Result;
    }
    //伤害判定结果，Limit为武器最大伤害值
    public int Damage(int limit)
    {
        int Result=Random.Range(0,25);
        if (Result >= limit)
            return limit;
        else
            return Result;
    }
}
