using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BrickColor
{
    Red,
    Green,
    Blue,
    Yellow,
    Purple
}

public class BrickSpawner : MonoBehaviour
{
    
    [Tooltip("砖块预制体")]
    public GameObject brickPrefab;
    [Tooltip("砖块行数")]
    public int rows=5;
    [Tooltip("砖块列数")]
    public int columns=10;
    [Tooltip("砖块水平间距")]
    public float spacingX=1.5f;
    [Tooltip("砖块垂直间距")]
    public float spacingY=1f;
    [Tooltip("砖块的起始位置")]
    public Vector3 startPosition=new Vector3(-7f, 4f, 0f);
    [Tooltip("砖块的父对象")]
    public Transform bricksParent;
    private BrickColor colorBrick=BrickColor.Red;

    void Start()
    {
         if(brickPrefab==null)
        {
            Debug.LogError("砖块预制体未设置！");
            return;
        }
        SpawnBricks();
    }

    public void SpawnBricks()
    {
        for(int row = 0; row < rows; row++)
        {
            
            for(int col = 0; col < columns; col++)
            {
                Vector3 position=new Vector3(
                    startPosition.x + col * spacingX,
                    startPosition.y - row * spacingY,
                    startPosition.z
                );
                GameObject brick= Instantiate(brickPrefab, position, Quaternion.identity, bricksParent);
                brick.GetComponent<SpriteRenderer>().color=GetColorFromEnum(colorBrick);
                GameManager.Instance.RegesterBrickEvent(brick.GetComponent<Brick>());
            }
            colorBrick+=1;
        }
    }

    private Color GetColorFromEnum(BrickColor color)
    {
        switch(color)
        {
            case BrickColor.Red:
                return Color.red;
            case BrickColor.Green:
                return Color.green;
            case BrickColor.Blue:
                return Color.blue;
            case BrickColor.Yellow:
                return Color.yellow;
            case BrickColor.Purple:
                return Color.magenta;
            default:
                return Color.white;
        }
    }
}
