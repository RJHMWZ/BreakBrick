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

    [Tooltip("当前游戏状态")]
    public E_GameState currentGameState=E_GameState.Ready;

    private int life=3;
    public int Life
    {
        get { return life; }
        set
        {
            life = value;
            if (life <= 0)
            {
                life = 0;
            }
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
            Debug.Log("游戏开始！");
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
            Life += amount;
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
            Life -= amount;
            if (Life <= 0)
            {
                GameOver();
            }
        }
    }




}
