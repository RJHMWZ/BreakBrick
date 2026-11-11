using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [Tooltip("砖块的分数")]
    private int hitScore=10;
    public int HitScore { get { return hitScore; } set { hitScore = value; } }

    /// <summary>
    /// 销毁砖块
    /// </summary>
    public void DestroyBrick()
    {
        Debug.Log("砖块被销毁，获得分数:"+hitScore);
        Destroy(gameObject);
    }

    public void HitBrick()
    {
        //后续逻辑补充
        Destroy(gameObject);
    }
}
