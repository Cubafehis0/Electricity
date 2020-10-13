using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelayMove : MonoBehaviour
{
    //用来控制继电器在被拾取时水平方向上的移动和放下时竖直方向上的运动
    public Relay relay;
    int mapLayer;
    int playerLayer;
    Vector2 relayExtents;
    private void Start()
    {
        mapLayer = LayerMask.NameToLayer("Map");
        playerLayer = LayerMask.NameToLayer("Movable");
        relayExtents = relay.rig.GetComponent<Collider2D>().bounds.extents;
        
    }
    private void FixedUpdate()
    {
        if(relay.hasPick)
        {
            relay.rig.MovePosition((Vector2)(relay.picker.rig.transform.position) + relay.offSet);
        }
        if(!relay.hasPick)
        {
            if(relay.speed.y<0)
                VerticalMove();
        }
    }
    
    
        

    void VerticalMove()
    {
        
        Vector2 offSet = new Vector2(-relayExtents.x, 0);//坐标偏移量,左下角
        Vector2 ray = new Vector2(relay.transform.position.x, relay.transform.position.y - 0.03f) + offSet;//射线射出位置
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.down, Mathf.Abs(relay.speed.y) * Time.deltaTime, 1 << playerLayer);
        if (hit.collider != null)
        {
            return;
        }
        else
        {
            offSet = new Vector2(relayExtents.x, 0);
            ray = new Vector2(relay.transform.position.x, relay.transform.position.y - 0.03f) + offSet; ;//右下角的射线
            hit = Physics2D.Raycast(ray, Vector2.down, Mathf.Abs(relay.speed.y) * Time.deltaTime, 1 << playerLayer);
            
            if (hit.collider != null)
            {
                return;
            }
            else
            {
                offSet = new Vector2(0, 0);
                ray = new Vector2(relay.transform.position.x, relay.transform.position.y - 0.03f) + offSet; ;//右下角的射线
                hit = Physics2D.Raycast(ray, Vector2.down, Mathf.Abs(relay.speed.y) * Time.deltaTime, 1 << playerLayer);
                Debug.Log(hit.collider);
                if (hit.collider != null)
                {
                    return;
                }
            }
        }

        
        offSet  = new Vector2(-relayExtents.x, 0) ;//坐标偏移量,左下角
    
        //中心在下中心
        ray = new Vector2(relay.transform.position.x, relay.transform.position.y-0.03f)+offSet;//射线射出位置
        Debug.Log(ray);
        hit = Physics2D.Raycast(ray, Vector2.down, Mathf.Abs(relay.speed.y)*Time.deltaTime, 1 << mapLayer);
        if (hit.collider != null)
        {
            relay.rig.MovePosition(hit.point - offSet);
            relay.speed.y = 0;
        }
        else
        {
            offSet = new Vector2(relayExtents.x, 0);
            ray = new Vector2(relay.transform.position.x, relay.transform.position.y - 0.03f) + offSet; ;//右下角的射线
            Debug.Log(ray); 
            hit = Physics2D.Raycast(ray, Vector2.down, Mathf.Abs(relay.speed.y) * Time.deltaTime, 1 << mapLayer);
            Debug.Log(hit.collider);
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
