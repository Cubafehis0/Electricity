using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Destination : MonoBehaviour
{
    public Player player1;
    public Player player2;
    Vector2 player1Position;
    Vector2 player2Position;
    Vector2 desPos;
    private void Start()
    {
        desPos = transform.position;
        
    }
    private void FixedUpdate()
    {
        ReachDes();
    }
    void ReachDes()
    {
        player1Position = player1.transform.position;
        player2Position = player2.transform.position;
        float distance1 = (player1Position - desPos).magnitude;
        float distance2 = (player2Position - desPos).magnitude;
        if (distance1 < 0.6f)
            player1.IsReachDes = true;
        else player1.IsReachDes = false;
        if (distance2 < 0.6f)
            player2.IsReachDes = true;
        else player2.IsReachDes = false;
        if (player1.IsReachDes && player2.IsReachDes)
            GameStatus.IsPass = true;
    }
    

}
