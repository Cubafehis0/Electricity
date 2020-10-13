using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    int mapLayerMask;
    int movableLayerMask;
    Rigidbody2D rig1;
    Rigidbody2D rig2;
    
    public Player player1;
    public Player player2;
    private void Start()
    {
        movableLayerMask= LayerMask.NameToLayer("Movable");
        mapLayerMask= LayerMask.NameToLayer("Map");
        player1.Collider2DExtents = player1.rig.GetComponent<Collider2D>().bounds.extents;
        player2.Collider2DExtents = player2.rig.GetComponent<Collider2D>().bounds.extents;
    }
    
    private void FixedUpdate()
    {
        CheckMapLayer(player1.rig);
        CheckMapLayer(player2.rig);
        
       

    }
    
    void CheckMapLayer(Rigidbody2D rig)
    {
        RaycastHit2D hit = CheckCollision.CheckDownCollison(rig, mapLayerMask, 0.01f);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "prickle")
            {
                GameStatus.IsAlive = false;
            }
        }
    }
}
