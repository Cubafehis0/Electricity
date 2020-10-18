using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelayMove : MonoBehaviour
{
    //用来控制继电器在被拾取时水平方向上的移动和放下时竖直方向上的运动
    public Relay relay;
    int mapLayer;
    int player1Layer;
    int player2Layer;
    Vector2 relayExtents;
    private void Start()
    {
        mapLayer = LayerMask.NameToLayer("Map");
        player1Layer = LayerMask.NameToLayer("Player1");
        player2Layer = LayerMask.NameToLayer("Player2");
        relayExtents = relay.rig.GetComponent<Collider2D>().bounds.extents;
        
    }
    private void FixedUpdate()
    {
        Move();
    }
    
    void Move()
    {
        if (relay.hasPick)
        {
            relay.rig.MovePosition((Vector2)(relay.picker.rig.transform.position) + relay.offSet + new Vector2(0, relayExtents.y));
        }
        if (!relay.hasPick)
        {
            if (relay.speed.y < 0)
            {
                VerticalMove();
            }
        }
    }
        

    void VerticalMove()
    {
        //与玩家1的碰撞检测
        RaycastHit2D hit = CheckCollision.CheckDownCollison(relay.rig, player1Layer, Mathf.Abs(relay.speed.y) * Time.deltaTime);
        if(hit.collider!=null)
        {
            return;
        }
        //与玩家2的碰撞检测

        hit = CheckCollision.CheckDownCollison(relay.rig, player2Layer, Mathf.Abs(relay.speed.y) * Time.deltaTime);
        if(hit.collider!=null)
        {
            return;
        }
        //着地碰撞检测
        hit = CheckCollision.CheckDownCollison(relay.rig, mapLayer, Mathf.Abs(relay.speed.y) * Time.deltaTime);
        if (hit.collider != null)
        {
            relay.rig.MovePosition(new Vector2(relay.rig.transform.position.x,hit.point.y+relayExtents.y));
            relay.speed.y = 0;
        }   
        else//没有着地继续移动
        {
            relay.rig.MovePosition(relay.speed * Time.deltaTime + (Vector2)relay.transform.position);
        }
    }
}
