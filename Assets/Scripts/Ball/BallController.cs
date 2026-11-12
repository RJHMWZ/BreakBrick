using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("球的发射速度")]
    private float launchSpeed=5f;
    [SerializeField]
    [Tooltip("球的最大反射角度")]
    private float maxReflectionAngle=60f;
    [SerializeField]
    [Tooltip("球的攻击力")]
    private int ballAttack=1;
    public GameObject paddle;
    public Vector3 offsetStartPosition=new Vector3(0,0.7f,0);
    private bool isLaunched=false;
    private bool launchRequested = false;
    private Vector2 lastVelocity;
    private Rigidbody2D rb;

    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity=Vector2.zero;
    }

    void Update()
    {
        if(GameManager.Instance.CurrentGameState == E_GameState.Ready&& !isLaunched)
        {
            transform.position=paddle.transform.position+offsetStartPosition;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.StartGame();
                launchRequested=true;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isLaunched&&launchRequested&&GameManager.Instance.CurrentGameState == E_GameState.Playing)
        {
            rb.velocity = Vector2.up * launchSpeed;
            isLaunched = true;
            launchRequested = false;
        }
        lastVelocity = rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (GameManager.Instance.CurrentGameState != E_GameState.Playing)
        {
            return;
        }
        if (collision2D.gameObject.CompareTag("Player"))
        {
            ReflectBall(collision2D);
        }
        else if (collision2D.gameObject.CompareTag("Brick"))
        {
            ReflectBrick(collision2D);
        }
        else if (collision2D.gameObject.CompareTag("Wall"))
        {
            ReflectWall(collision2D);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.Instance.CurrentGameState != E_GameState.Playing)
        {
            return;
        }
        if (other.CompareTag("DeadZone"))
        {
            DeadZone();
        }
    }
    
    /// <summary>
    /// 挡板碰撞处理
    /// </summary>
    /// <param name="collision2D">碰撞信息</param>
    public void ReflectBall(Collision2D collision2D)
    {
        SpecialReflect(collision2D);
    }

    /// <summary>
    /// 砖块碰撞处理
    /// </summary>
    /// <param name="collision2D"></param>
    public void ReflectBrick(Collision2D collision2D)
    {
        NormalReflect(collision2D);
        collision2D.gameObject.GetComponent<Brick>().HitBrick(ballAttack);
    }

    /// <summary>
    /// 墙壁碰撞处理
    /// </summary>
    /// <param name="collision2D"></param>
    public void ReflectWall(Collision2D collision2D)
    {
        NormalReflect(collision2D);
    }

    /// <summary>
    /// 死亡区域触发处理
    /// </summary>
    public void DeadZone()
    {
        GameManager.Instance.SubtractLife(1);
        if (GameManager.Instance.CurrentGameState == E_GameState.GameOver)
        {
            DestroyBall();
        }
        else
        {
            ResetBall();
        }
    }

    /// <summary>
    /// 普通反射处理
    /// </summary>
    /// <param name="collision2D"></param>
    public void NormalReflect(Collision2D collision2D)
    {
        Vector2 normal=collision2D.contacts[0].normal;
        Vector2 direction=Vector2.Reflect(lastVelocity.normalized,normal).normalized;
        rb.velocity=direction*launchSpeed;
    }

    /// <summary>
    /// 特殊反射处理：根据碰撞点距离两边的位置
    /// </summary>
    /// <param name="collision2D"></param>
    public void SpecialReflect(Collision2D collision2D)
    {
        // 获取小球碰撞前的运动方向
        Vector2 incomingDir = rb.velocity.normalized;
        // 计算小球原本的运动角度
        float incomingAngle = Mathf.Atan2(incomingDir.x, -incomingDir.y) * Mathf.Rad2Deg;
        //挡板中心位置
        Vector2 paddlePos=collision2D.gameObject.transform.position;
         //碰撞点位置
        Vector2 hitPoint=collision2D.contacts[0].point;
        //挡板半宽长度
        float paddleHalfWidth=collision2D.collider.bounds.extents.x;
        //碰撞位置的偏移比例
        float offsetRatio= (hitPoint.x-paddlePos.x)/paddleHalfWidth;
        // 根据碰撞位置计算挡板影响角度
        float paddleAngle = offsetRatio * maxReflectionAngle;
        // 混合两个角度：70%保留原方向，30%受挡板位置影响
        float angle = incomingAngle * 0.7f + paddleAngle * 0.3f;
       // 限制最大反弹角度
        angle = Mathf.Clamp(angle, -maxReflectionAngle, maxReflectionAngle);
        // 转换为弧度
        float radAngle = angle * Mathf.Deg2Rad;
        // 计算新的反弹方向
        Vector2 newDirection = new Vector2(Mathf.Sin(radAngle), Mathf.Cos(radAngle));
        rb.velocity = newDirection * launchSpeed;
    }

    /// <summary>
    /// 重置小球
    /// </summary>
    public void ResetBall()
    {
        StopBall();
        rb.position = (Vector2)(paddle.transform.position + offsetStartPosition);
    }

    /// <summary>
    /// 停止小球
    /// </summary>
    public void StopBall()
    {
        isLaunched = false;
        launchRequested = false;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        lastVelocity = Vector2.zero;
    }

    public void DestroyBall()
    {
        ResetBall();
        paddle.GetComponent<PaddleController>().CanInput=false;
        this.gameObject.SetActive(false);
    }
}
