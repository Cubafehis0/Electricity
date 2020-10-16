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
        if(relay.hasPick)
        {
            relay.rig.MovePosition((Vector2)(relay.picker.rig.transform.position) + relay.offSet+new Vector2(0,relayExtents.y));
        }
        
        if(!relay.hasPick)
        {
            if(relay.speed.y<0)
            {
                VerticalMove();
            }
           
        }
    }
    
    
        

    void VerticalMove()
    {
        //与玩家1的碰撞检测
        Vector2 offSet = new Vector2(-relayExtents.x, -relayExtents.y);//坐标偏移量,左下角
        Vector2 ray = new Vector2(relay.transform.position.x, relay.transform.position.y - 0.03f) + offSet;//射线射出位置
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.down, Mathf.Abs(relay.speed.y) * Time.deltaTime, 1 << player1Layer);
        if (hit.collider != null)
        {
            return;
        }
        else
        {
            offSet = new Vector2(relayExtents.x, -relayExtents.y);
            ray = new Vector2(relay.transform.position.x, relay.transform.position.y - 0.03f) + offSet; ;//右下角的射线
            hit = Physics2D.Raycast(ray, Vector2.down, Mathf.Abs(relay.speed.y) * Time.deltaTime, 1 << player1Layer);
            
            if (hit.collider != null)
            {
                return;
            }
            else
            {
                offSet = new Vector2(0,-relayExtents.y);
                ray = new Vector2(relay.transform.position.x, relay.transform.position.y - 0.03f) + offSet; ;//中间下角的射线
                hit = Physics2D.Raycast(ray, Vector2.down, Mathf.Abs(relay.speed.y) * Time.deltaTime, 1 << player1Layer);
                if (hit.collider != null)
                {
                    return;
                }
            }
        }
        //与玩家2的碰撞检测

        offSet = new Vector2(-relayExtents.x, -relayExtents.y);//坐标偏移量,左下角
        ray = new Vector2(relay.transform.position.x, relay.transform.position.y - 0.03f) + offSet;//射线射出位置
        hit = Physics2D.Raycast(ray, Vector2.down, Mathf.Abs(relay.speed.y) * Time.deltaTime, 1 << player2Layer);
        if (hit.collider != null)
        {
            return;
        }
        else
        {
            offSet = new Vector2(relayExtents.x,- relayExtents.y);
            ray = new Vector2(relay.transform.position.x, relay.transform.position.y - 0.03f) + offSet; ;//右下角的射线
            hit = Physics2D.Raycast(ray, Vector2.down, Mathf.Abs(relay.speed.y) * Time.deltaTime, 1 << player2Layer);

            if (hit.collider != null)
            {
                return;
            }
            else
            {
                offSet = new Vector2(0, -relayExtents.y);
                ray = new Vector2(relay.transform.position.x, relay.transform.position.y - 0.03f) + offSet; ;//右下角的射线
                hit = Physics2D.Raycast(ray, Vector2.down, Mathf.Abs(relay.speed.y) * Time.deltaTime, 1 << player2Layer);
                if (hit.collider != null)
                {
                    return;
                }
            }
        }
        //着地碰撞检测
        offSet  = new Vector2(-relayExtents.x, -relayExtents.y) ;//坐标偏移量,左下角
        
        //中心在下中心
        ray = new Vector2(relay.transform.position.x, relay.transform.position.y-0.03f)+offSet;//射线射出位置
        hit = Physics2D.Raycast(ray, Vector2.down, Mathf.Abs(relay.speed.y)*Time.deltaTime, 1 << mapLayer);
        if (hit.collider != null)
        {
            relay.rig.MovePosition(hit.point - offSet);
            relay.speed.y = 0;
        }
        else
        {
            offSet = new Vector2(relayExtents.x, -relayExtents.y);
            ray = new Vector2(relay.transform.position.x, relay.transform.position.y - 0.03f) + offSet; ;//右下角的射线
            hit = Physics2D.Raycast(ray, Vector2.down, Mathf.Abs(relay.speed.y) * Time.deltaTime, 1 << mapLayer);
            if (hit.collider != null)
            {
                relay.rig.MovePosition(hit.point - offSet);
                relay.speed.y = 0;
            }
                
            else
            {
                relay.rig.MovePosition(relay.speed * Time.deltaTime + (Vector2)relay.transform.position);
            }
        }
            
    }
        

    
}
