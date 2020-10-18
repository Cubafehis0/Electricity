using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftplatformMove : MonoBehaviour
{
    //控制传送带的移动，和与玩家的碰撞检测，附着在升降台上，并且被playerControl调用
    public float minHeight;//最低高度
    public float maxHeight;//最高高度
    public float speed;//移动速度绝对值
    float curSpeed;//移动速度
    Rigidbody2D rig;
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        curSpeed = speed;
    }
    private void FixedUpdate()
    {
        Move();
        
    }
    
    public void OnLiftPlat(Player player)//检测与玩家的碰撞
    {
        RaycastHit2D hit = CheckCollision.CheckUpCollison(rig, player.Layer, 0.1f);
        if (hit.collider != null)
        {
            player.speed.y = curSpeed+(hit.point.y - player.rig.GetComponent<Collider2D>().bounds.center.y + player.Collider2DExtents.y)/ Time.deltaTime;
            player.IsOnPlat = true;
            player.IsFloat = false;
            
        }
        else player.IsOnPlat = false;
    }

    void Move()//控制升降台的移动
    {
        Vector2 rigPos = rig.transform.position;
        if(rigPos.y>maxHeight)
        {
            curSpeed = -speed;
        }
        else if(rigPos.y<minHeight)
        {
            curSpeed = speed;
        }
        rig.MovePosition(rigPos + new Vector2(0, curSpeed * Time.deltaTime));
        
    }
}
