using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Tooltip("总分数")]
    private int SumScore=0;

    /// <summary>
    /// 增加分数
    /// </summary>
    /// <param name="num"></param>
    public void AddScore(int num)
    {
        SumScore+=num;
        PrintScore();
    }

    /// <summary>
    /// 减少分数
    /// </summary>
    /// <param name="num"></param>
    public void subtractScore(int num)
    {
        SumScore-=num;
        PrintScore();
    }

    /// <summary>
    /// 打印当前总分
    /// </summary>
    public void PrintScore()
    {
        Debug.Log("现在的总分为:"+SumScore);
    }
}
