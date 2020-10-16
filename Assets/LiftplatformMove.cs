using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftplatformMove : MonoBehaviour
{
    public float minHeight;
    public float maxHeight;
    public float speed;
    float curSpeed;
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
    
    public void OnLiftPlat(Player player)
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
    void Move()
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
