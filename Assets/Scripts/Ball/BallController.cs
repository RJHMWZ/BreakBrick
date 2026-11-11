using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private float launchSpeed=5f;
    [SerializeField]
    private float maxReflectionAngle=60f;

    public ScoreManager scoreManager;
    private bool isLaunched=false;
    private bool launchRequested = false;
    private Vector2 lastVelocity;
    private Rigidbody2D rb;
    private Vector2 startPosition;

    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        startPosition=transform.position;
    }

    void Start()
    {
        rb.velocity=Vector2.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isLaunched)
        {
            launchRequested=true;
        }
    }

    void FixedUpdate()
    {
        if (!isLaunched&&launchRequested)
        {
            rb.velocity = Vector2.up * launchSpeed;

            isLaunched = true;
            launchRequested = false;
        }
        lastVelocity = rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Player"))
        {
            ReflectBall(collision2D);
        }
        else if (collision2D.gameObject.CompareTag("Brick"))
        {
            ReflectBrick(collision2D);
        }
    }
    
    /// <summary>
    /// 挡板碰撞处理
    /// </summary>
    /// <param name="collision2D">碰撞信息</param>
    public void ReflectBall(Collision2D collision2D)
    {
         //挡板中心位置
        Vector2 paddlePos=collision2D.gameObject.transform.position;
        //碰撞点位置
        Vector2 hitPoint=collision2D.contacts[0].point;
        //挡板半宽长度
        float paddleHalfWidth=collision2D.collider.bounds.extents.x;
        //碰撞位置的偏移比例
        float offsetRatio= (hitPoint.x-paddlePos.x)/paddleHalfWidth;
        offsetRatio=Mathf.Clamp(offsetRatio, -1f, 1f);
        //根据偏移比例计算反射角度
        float angle=offsetRatio*maxReflectionAngle;
        //角度转为弧度
        float radAngle=angle*Mathf.Deg2Rad;
        //计算反弹方向
        Vector2 newDirection=new Vector2(Mathf.Sin(radAngle), Mathf.Cos(radAngle));
        rb.velocity = newDirection * launchSpeed;
    }

    /// <summary>
    /// 砖块碰撞处理
    /// </summary>
    /// <param name="collision2D"></param>
    public void ReflectBrick(Collision2D collision2D)
    {
        Vector2 normal=collision2D.contacts[0].normal;
        Vector2 direction=Vector2.Reflect(lastVelocity.normalized,normal).normalized;
        rb.velocity=direction*launchSpeed;
         Brick brick=collision2D.gameObject.GetComponent<Brick>();
        if (brick != null)
        {
            scoreManager.AddScore(brick.HitScore);
            brick.DestroyBrick();
        }
    }
}
