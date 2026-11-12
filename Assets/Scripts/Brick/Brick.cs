using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Brick : MonoBehaviour
{
    [SerializeField]
    [Tooltip("砖块的耐久")]
    private int hpBrick=1;
    [SerializeField]
    [Tooltip("砖块的分数")]
    private int hitScore=10;
    public int HitScore=>hitScore;
    private bool isDestroyed;
    public event Action<Brick,int> OnBrickDestroy;

    /// <summary>
    /// 销毁砖块
    /// </summary>
    public void DestroyBrick()
    {
        if (isDestroyed)
        {
            return;
        }
        isDestroyed = true;
        OnBrickDestroy?.Invoke(this,hitScore);
        Destroy(gameObject);
    }

    /// <summary>
    /// 击打砖块
    /// </summary>
    /// <param name="ballAttack">球的攻击力</param>
    public void HitBrick(int ballAttack)
    {
        if (isDestroyed || ballAttack <= 0)
        {
            return;
        }
        hpBrick -= ballAttack;
        if (hpBrick <= 0)
        {
            DestroyBrick();
        }
    }
}
