using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏状态枚举
/// </summary>
 public enum E_GameState
{
    Ready,
    Playing,
    GameOver,
    Victory
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance=> instance;
    public int maxLife=3;
    [Tooltip("当前游戏状态")]
    private E_GameState currentGameState=E_GameState.Ready;
    public E_GameState CurrentGameState { get { return currentGameState; } }
    private int life;
    public int Life{get { return life; }}

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        life=maxLife;
    }

    public void SetGameState(E_GameState newState)
    {
        currentGameState = newState;
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        if(currentGameState == E_GameState.Ready)
        {
            SetGameState(E_GameState.Playing);
            Debug.Log("游戏开始!");
        }
        else
        {
            Debug.Log("当前游戏状态为不是Ready,无法开始游戏");
        }
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    public void GameOver()
    {
        SetGameState(E_GameState.GameOver);
        Debug.Log("游戏结束！");
    }

    /// <summary>
    /// 游戏胜利
    /// </summary>
    public void Victory()
    {
        SetGameState(E_GameState.Victory);
        Debug.Log("游戏胜利！");
    }

    /// <summary>
    /// 增加生命值
    /// </summary>
    /// <param name="amount">增加量</param>
    public void AddLife(int amount)
    {
        if(currentGameState == E_GameState.Playing)
        {
            life += amount;
            life=Mathf.Clamp(life,0,maxLife);
            Debug.Log("扣血成功，现在的血量为："+life);
        }
    }

    /// <summary>
    /// 减少生命值
    /// </summary>
    /// <param name="amount">减少量</param>
    public void SubtractLife(int amount)
    {
        if(currentGameState == E_GameState.Playing)
        {
            life -= amount;
            life=Mathf.Clamp(life,0,maxLife);
            if (life <= 0)
            {
                GameOver();
            }
            else
            {
                SetGameState(E_GameState.Ready);
            }
        }
    }
}
