using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    
    [SerializeField]
    private float speed=10f;

    [SerializeField]
    private float minX=-7f;

    [SerializeField]
    private float maxX=7f;

    private float horizontalInput;
    public bool CanInput=true;

    void Update()
    {
        horizontalInput=CanInput?Input.GetAxisRaw("Horizontal"):0f;
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + horizontalInput * speed * Time.fixedDeltaTime, minX, maxX), transform.position.y, transform.position.z);
    }
}
