using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class PlayerControl : MonoBehaviour
{
    public Player player1;
    public Player player2;

    private bool isFinishInitialDrop;//是否完成初始下落，没有则为false且不能操作。

    public float initialHorizontalSpeed;
    public float initialVerticalSpeed;
    public float verticalAcc;
    
    int mapLayer;
    
    private void Start()
    {
        isFinishInitialDrop = false;
        player1.IsFloat = true;
        player2.IsFloat = true;
        mapLayer = LayerMask.NameToLayer("Map");
        player1.Collider2DExtents = player1.rig.GetComponent<Collider2D>().bounds.extents;
        player2.Collider2DExtents = player2.rig.GetComponent<Collider2D>().bounds.extents;
        
    }
    private void FixedUpdate()
    {
        CheckVerticalCollision();
        if(isFinishInitialDrop)
            Control();
        CheckHorizontalCollision();
        Move(player1);
        Move(player2);
        
    }
    public void Move(Player player)
    {    
        player.rig.MovePosition(player.speed * Time.deltaTime + (Vector2)player.rig.transform.position);
        if (player.IsFloat)
            player.speed.y -= verticalAcc * Time.deltaTime;

    }
    private void OnDrawGizmos()
    {
        player1.Collider2DCenter = player1.rig.GetComponent<Collider2D>().bounds.center;
        Vector2 offSet = new Vector2(-player1.Collider2DExtents.x, player1.Collider2DExtents.y);//左上角
        Vector2 raySoursLeftUp = player1.Collider2DCenter + offSet;//射线射出位置
        Gizmos.DrawLine(raySoursLeftUp, raySoursLeftUp - new Vector2(Mathf.Abs(player1.speed.x * Time.deltaTime), 0));

    }

    void CheckVerticalCollision()
    {
        GroundCollision(player1);
        GroundCollision(player2);
        if(player1.speed.y>0)
            FloorCollision(player1);
        if(player2.speed.y>0)
            FloorCollision(player2);
        
    }
    void CheckHorizontalCollision()
    {
        if(player1.speed.x<0)
        {
            LeftWallCollision(player1);
        }
        else if(player1.speed.x>0)
        {
            RightWallCollision(player1);
        }
        if (player2.speed.x < 0)
        {
            LeftWallCollision(player2);
        }
        else if (player2.speed.x > 0)
        {
            RightWallCollision(player2);
        }
    }
    void GroundCollision(Player player)
    {
        player.Collider2DCenter = player.rig.GetComponent<Collider2D>().bounds.center;
        Vector2 offSet = -player.Collider2DExtents;//坐标偏移量,左下角
        Vector2 raySourseLeftDown = player.Collider2DCenter + offSet;//射线射出位置
        RaycastHit2D hitLeft = Physics2D.Raycast(raySourseLeftDown, Vector2.down,Mathf.Abs(player.speed.y*Time.deltaTime), 1 << mapLayer);
        if (hitLeft.collider != null)
        {
            player.rig.MovePosition(hitLeft.point - offSet);
            player.speed.y = 0;
            player.IsFloat = false;
            isFinishInitialDrop = true;
        }
        else
        {
            offSet = new Vector2(player.Collider2DExtents.x, -player.Collider2DExtents.y);
            Vector2 raySourseRightDown = player.Collider2DCenter + offSet;
            RaycastHit2D hitRight = Physics2D.Raycast(raySourseRightDown, Vector2.down,Mathf.Abs(player.speed.y * Time.deltaTime), 1 << mapLayer);
            if (hitRight.collider != null)
            {
                player.rig.MovePosition(hitRight.point - offSet);
                player.speed.y = 0;
                player.IsFloat = false;
            }
            else
            {
                player.IsFloat = true;
            }
        }
        

        
    }
    void FloorCollision(Player player)
    {
        player.Collider2DCenter = player.rig.GetComponent<Collider2D>().bounds.center;
        Vector2 offSet = player.Collider2DExtents;//坐标偏移量,右上角
        Vector2 raySourseRightUp = player.Collider2DCenter + offSet;//射线射出位置
        RaycastHit2D hitRightUp = Physics2D.Raycast(raySourseRightUp, Vector2.up, Mathf.Abs(player.speed.y) * Time.deltaTime, 1 << mapLayer);
        if (hitRightUp.collider != null)
        {
            player.rig.MovePosition(hitRightUp.point - offSet);
            player.speed.y = 0;
        }
        else
        {
            offSet = new Vector2(-player.Collider2DExtents.x, player.Collider2DExtents.y);
            Vector2 raySourseLeftUp = player.Collider2DCenter + offSet;
            RaycastHit2D hitLeft = Physics2D.Raycast(raySourseLeftUp, Vector2.up, Mathf.Abs(player.speed.y) * Time.deltaTime, 1 << mapLayer);
            if (hitLeft.collider != null)
            {
                player.rig.MovePosition(hitLeft.point - offSet);
                player.speed.y = 0;
            }  
        }
    }
    void LeftWallCollision(Player player)
    {
        player.Collider2DCenter = player.rig.GetComponent<Collider2D>().bounds.center;
        Vector2 offSet = new Vector2(-player.Collider2DExtents.x, player.Collider2DExtents.y-0.01f);//左上角
        Vector2 raySoursLeftUp = player.Collider2DCenter + offSet;//射线射出位置
        Debug.Log(raySoursLeftUp);
        RaycastHit2D hitRight = Physics2D.Raycast(raySoursLeftUp, Vector2.left, Mathf.Abs(player.speed.x * Time.deltaTime), 1 << mapLayer);
        if (hitRight.collider != null)
        {
            Debug.Log("left");
            player.rig.MovePosition(hitRight.point - offSet);
            player.speed.x = 0;
        }
        else
        {
            Debug.Log("left");
            offSet = new Vector2(-player.Collider2DExtents.x,- player.Collider2DExtents.y + 0.01f);//左下角
            Vector2 raySourseLeftDown = player.Collider2DCenter + offSet;
            RaycastHit2D hitLeft = Physics2D.Raycast(raySourseLeftDown, Vector2.left, Mathf.Abs(player.speed.x * Time.deltaTime), 1 << mapLayer);
            if (hitLeft.collider != null)
            {
                player.rig.MovePosition(hitLeft.point - offSet);
                player.speed.x =0;
            }
        }
    }
    void RightWallCollision(Player player)
    {
        player.Collider2DCenter = player.rig.GetComponent<Collider2D>().bounds.center;
        Vector2 offSet = new Vector2(player.Collider2DExtents.x, player.Collider2DExtents.y - 0.01f);//右上角
        Vector2 raySoursRightUp = player.Collider2DCenter + offSet;//射线射出位置
        RaycastHit2D hitUp = Physics2D.Raycast(raySoursRightUp, Vector2.right, Mathf.Abs(player.speed.x * Time.deltaTime), 1 << mapLayer);
        if (hitUp.collider != null)
        {
            Debug.Log(hitUp.collider.name);
            player.rig.MovePosition(hitUp.point - offSet);
            player.speed.x = 0;
        }
        else
        {
            
            offSet = new Vector2(player.Collider2DExtents.x,-player.Collider2DExtents.y+0.01f);//右下角
            Vector2 raySourseRightDown = player.Collider2DCenter + offSet;
            RaycastHit2D hitDown = Physics2D.Raycast(raySourseRightDown, Vector2.right, Mathf.Abs(player.speed.x * Time.deltaTime) + 0.01f, 1 << mapLayer);
            if (hitDown.collider != null)
            {
                player.rig.MovePosition(hitDown.point - offSet);
                player.speed.x = 0;
            }
        }
    }
    void Control()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            player1.speed.x = -initialHorizontalSpeed;
        else if (Input.GetKey(KeyCode.RightArrow))
            player1.speed.x = initialHorizontalSpeed;
        else player1.speed.x = 0;
        if (!player1.IsFloat)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                player1.speed.y = initialVerticalSpeed;
                player1.IsFloat = true;
            }

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            //Pick(player1);
        }

        if (Input.GetKey(KeyCode.A))
            player2.speed.x = -initialHorizontalSpeed;
        else if (Input.GetKey(KeyCode.D))
            player2.speed.x = initialHorizontalSpeed;
        else player2.speed.x = 0;

        if (!player2.IsFloat)
        {
            if (Input.GetKey(KeyCode.W))
            {
                player2.speed.y = initialVerticalSpeed;
                player2.IsFloat = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            //Pick(player2);
        }

    }
    void Pick(Player player)
    {

    }
}
