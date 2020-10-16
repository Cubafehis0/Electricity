using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class PlayerControl : MonoBehaviour
{
    public Player player1;
    public Player player2;

    public float initialHorizontalSpeed;
    public float initialVerticalSpeed;
    public float verticalAcc;
    public Relay[] relays = new Relay[4];
    int relaySize;
    private bool isFinishInitialDrop;//是否完成初始下落，没有则为false且不能操作。
    public LiftplatformMove[] liftPlats;

    bool isDownS;
    bool isDownDown;

    int mapLayer;
    int relayLayer;

    Animator animator1, animator2;
    private void Start()
    {
        isFinishInitialDrop = false;
        player1.IsFloat = true;
        player2.IsFloat = true;
        player1.IsOnPlat = false;
        player2.IsOnPlat = false;
        relayLayer = LayerMask.NameToLayer("Relay");
        mapLayer = LayerMask.NameToLayer("Map");
        player1.Layer = LayerMask.NameToLayer("Player1");
        player2.Layer = LayerMask.NameToLayer("Player2");
        player1.Collider2DExtents = player1.rig.GetComponent<Collider2D>().bounds.extents;
        player2.Collider2DExtents = player2.rig.GetComponent<Collider2D>().bounds.extents;
        animator1 = player1.transform.GetComponent<Animator>();
        animator2 = player2.transform.GetComponent<Animator>();
        isDownS = false;
        isDownDown = false;
        relaySize = relays.Length;

    }
    private void FixedUpdate()
    {
        OnFloorCollision();
        OnGroundCollision();//放在前面，在后面会跳不起来

        if (!player1.IsReachDes && !player2.IsReachDes)
            TVVerticalCollision();

        CheckMapLayer();
        if (isFinishInitialDrop)
            Control();
        CheckHorizontalCollision();//在前面墙挡不住
        if (!player1.IsReachDes  && !player2.IsReachDes)
            TVHorizontalCollision();
        Animanage();

        Move(player2);
        Move(player1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            isDownDown = true;
        if (Input.GetKeyDown(KeyCode.S))
            isDownS = true;
    }
    void OnGroundCollision()
    {
        GroundCollision(player1, mapLayer, "map");
        GroundCollision(player2, mapLayer, "map");
        if (player1.IsFloat)
            GroundCollision(player1, relayLayer, "map");
        if (player2.IsFloat)
            GroundCollision(player2, relayLayer, "map");
        if (player1.IsFloat)
            GroundCollision(player1, mapLayer, "conveyBelt");
        if (player2.IsFloat)
            GroundCollision(player2, mapLayer, "conveyBelt");

    }
    void OnFloorCollision()
    {
        if (player1.speed.y > 0)//防止电视机在传送带上互相穿过
        {
            FloorCollision(player1, mapLayer, "map");
            FloorCollision(player1, relayLayer, "map");
        }

        if (player2.speed.y > 0)
        {
            FloorCollision(player2, mapLayer, "map");
            FloorCollision(player2, relayLayer, "map");
        }

    }
    void TVHorizontalCollision()
    {
        if (player1.speed.x < 0)
        {
            LeftCollision(player1, player2.Layer, "Player");
        }
        else if (player1.speed.x > 0)
        {
            RightCollision(player1, player2.Layer, "Player");
        }
        if (player2.speed.x < 0)
        {
            LeftCollision(player2, player1.Layer, "Player");
        }
        else if (player2.speed.x > 0)
        {
            RightCollision(player2, player1.Layer, "Player");
        }
        CheckConveyBelt(player1);
        CheckConveyBelt(player2);

    }
    void CheckConveyBelt(Player player)
    {
        RaycastHit2D hit = CheckCollision.CheckDownCollison(player.rig, mapLayer, (player.speed.y) * Time.deltaTime);
        if(hit.collider!=null)
        {
            if (hit.collider.tag == "conveyBelt")
            {
                player.speed.x += 1.5f;
            }
        }
    }
    void TVVerticalCollision()
    {
        if (player1.IsFloat)
        {
            GroundCollision(player1, player2.Layer, "Player");
        }
        if (player2.IsFloat)
            GroundCollision(player2, player1.Layer, "Player");
        if (player1.speed.y > 0)
        {
            FloorCollision(player1, player2.Layer, "Player");
        }

        if (player2.speed.y > 0)
        { 
            FloorCollision(player2, player1.Layer, "Player");
        }
    }
    public void Move(Player player)
    {

        player.rig.MovePosition(player.speed * Time.deltaTime + (Vector2)player.rig.transform.position);
        if (player.IsFloat)
            player.speed.y -= verticalAcc * Time.deltaTime;
        else player.speed.y = 0;

    }
    void CheckMapLayer()
    {
        RaycastHit2D hit = CheckCollision.CheckDownCollison(player1.rig, mapLayer, 0.01f);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "prickle")
            {
                GameStatus.IsAlive = false;
            }
        }

        hit = CheckCollision.CheckDownCollison(player2.rig, mapLayer, 0.01f);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "prickle")
            {
                GameStatus.IsAlive = false;
            }
        }

        if(liftPlats.Length>0)
        {
            player1.IsOnPlat = false;
            for (int i = 0; i < liftPlats.Length && !player1.IsOnPlat; i++)
            {
                liftPlats[i].OnLiftPlat(player1);
            }

            player2.IsOnPlat = false;
            for (int i=0;i<liftPlats.Length && !player2.IsOnPlat;i++)
            {
                liftPlats[i].OnLiftPlat(player2);
            }
            if(player1.isOnOther && player2.IsOnPlat)
            {
                player1.speed.y = player2.speed.y;
            }
            if(player2.isOnOther && player1.IsOnPlat)
            {
                player2.speed.y = player1.speed.y;
            }
        }

    }
    void Animanage()
    {
        animator1.SetBool("isJump", false);
        animator1.SetBool("isFaceRight", false);
        animator1.SetBool("isFaceLeft", false);
        if (player1.IsFloat)
        {
            animator1.SetBool("isJump", true);
        }
        else if (player1.speed.x > 0)
        {
            animator1.SetBool("isFaceRight", true);
        }
        else if (player1.speed.x < 0)
        {
            animator1.SetBool("isFaceLeft", true);
        }


        animator2.SetBool("isJump", false);
        animator2.SetBool("isFaceRight", false);
        animator2.SetBool("isFaceLeft", false);
        if (player2.IsFloat)
        {
            animator2.SetBool("isJump", true);
        }
        else if (player2.speed.x > 0)
        {
            animator2.SetBool("isFaceRight", true);
        }
        else if (player2.speed.x < 0)
        {
            animator2.SetBool("isFaceLeft", true);
        }

    }
    void CheckHorizontalCollision()
    {
        if (player1.speed.x < 0)
        {
            LeftCollision(player1,mapLayer,"map");
            LeftCollision(player1, relayLayer, "map");
        }
        else if (player1.speed.x > 0)
        {
            RightCollision(player1,mapLayer,"map");
            RightCollision(player1,relayLayer, "map");
        }
        if (player2.speed.x < 0)
        {
            LeftCollision(player2,mapLayer,"map");
            LeftCollision(player2,relayLayer, "map");
        }
        else if (player2.speed.x > 0)
        {
            RightCollision(player2, mapLayer, "map");
            RightCollision(player2,relayLayer, "map");
        }
    }
    void GroundCollision(Player player,int layerMask,string tag)
    {
        RaycastHit2D hit = CheckCollision.CheckDownCollison(player.rig, layerMask, Mathf.Abs(player.speed.y * Time.deltaTime)+0.01f);
        if (hit.collider != null)
        {
            if (hit.collider.tag == tag)
            {

                if (player.speed.y < -20)
                {
                    GameObject dropEffect = GameObject.Instantiate(player.dropDustEffect, player.transform.position + new Vector3(0.15f, -0.70f, 0), Quaternion.identity);
                    GameObject.Destroy(dropEffect, 0.3f);
                }
                player.speed.y = (hit.point.y - player.rig.GetComponent<Collider2D>().bounds.center.y + player.Collider2DExtents.y) / Time.deltaTime;
                player.IsFloat = false;
                isFinishInitialDrop = true;
                if (hit.collider.tag == player.tag)
                {
                    player.isOnOther = true;
                }
                    
            }
        }
        else
        {
            player.isOnOther = false;
            player.IsFloat = true;
        }
            /*player.ColCen = player.rig.GetComponent<Collider2D>().bounds.center;
            Vector2 offSet = -player.Collider2DExtents;//坐标偏移量,左下角
            Vector2 raySourseLeftDown = player.ColCen + offSet;//射线射出位置
            RaycastHit2D hit = Physics2D.Raycast(raySourseLeftDown, Vector2.down, Mathf.Abs(player.speed.y * Time.deltaTime), 1 << mapLayer);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "map")
                {
                    player.speed.y = (hit.point.y - raySourseLeftDown.y) / Time.deltaTime;
                    if (player.speed.y < -20)
                    {
                        GameObject dropEffect = GameObject.Instantiate(player.dropDustEffect, player.transform.position + new Vector3(0.15f, -0.70f, 0), Quaternion.identity);
                        GameObject.Destroy(dropEffect, 0.3f);
                    }
                    player.speed.y = (hit.point.y - raySourseLeftDown.y) / Time.deltaTime;
                    player.IsFloat = false;
                    isFinishInitialDrop = true;
                }

            }
            else
            {

                offSet = new Vector2(player.Collider2DExtents.x, -player.Collider2DExtents.y);
                Vector2 raySourseRightDown = player.ColCen + offSet;
                hit = Physics2D.Raycast(raySourseRightDown, Vector2.down, Mathf.Abs(player.speed.y * Time.deltaTime), 1 << mapLayer);
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "map")
                    {
                        player.speed.y = (hit.point.y - raySourseRightDown.y) / Time.deltaTime;
                        player.IsFloat = false;
                        isFinishInitialDrop = true;
                    }
                }
                else player.IsFloat = true;
            }*/
        }
    void FloorCollision(Player player,int layerMask,string tag)
    {
        RaycastHit2D hit = CheckCollision.CheckUpCollison(player.rig, layerMask, Mathf.Abs(player.speed.y * Time.deltaTime));
        if (hit.collider != null)
        {
            if (hit.collider.tag == tag)
            {
                player.speed.y = 0;
            }
        }
        /*player.ColCen = player.rig.GetComponent<Collider2D>().bounds.center;
        Vector2 offSet = player.Collider2DExtents;//坐标偏移量,右上角
        Vector2 raySourseRightUp = player.ColCen + offSet;//射线射出位置
        RaycastHit2D hit = Physics2D.Raycast(raySourseRightUp, Vector2.up, Mathf.Abs(player.speed.y) * Time.deltaTime, 1 << mapLayer);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "map")
            {
                player.rig.MovePosition(hit.point - offSet);
                player.speed.y = 0;
            }
        }
        else
        {
            offSet = new Vector2(-player.Collider2DExtents.x, player.Collider2DExtents.y);
            Vector2 raySourseLeftUp = player.ColCen + offSet;
            hit = Physics2D.Raycast(raySourseLeftUp, Vector2.up, Mathf.Abs(player.speed.y) * Time.deltaTime, 1 << mapLayer);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "map")
                {
                    player.rig.MovePosition(hit.point - offSet);
                    player.speed.y = 0;
                }
            }
        }*/
    }
    void LeftCollision(Player player,int layerMask,string tag)
    {
        RaycastHit2D hit = CheckCollision.CheckLeftCollison(player.rig, layerMask, Mathf.Abs(player.speed.x)*Time.deltaTime);
        if(hit.collider!=null)
        {
            if(hit.collider.tag==tag)
                player.speed.x = 0;
        }
        /*player.ColCen = player.rig.GetComponent<Collider2D>().bounds.center;
        Vector2 offSet = new Vector2(-player.Collider2DExtents.x, -player.Collider2DExtents.y + 0.01f);//左下角
        Vector2 raySoursLeftUp = player.ColCen + offSet;//射线射出位置
        RaycastHit2D hit = Physics2D.Raycast(raySoursLeftUp, Vector2.left, Mathf.Abs(player.speed.x * Time.deltaTime), 1 << mapLayer);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "map")
            {
                player.rig.MovePosition(hit.point - offSet);
                player.speed.x = 0;
            }
        }
        else
        {
            offSet = new Vector2(-player.Collider2DExtents.x, player.Collider2DExtents.y - 0.01f);//左上角
            Vector2 raySourseLeftDown = player.ColCen + offSet;
            hit = Physics2D.Raycast(raySourseLeftDown, Vector2.left, Mathf.Abs(player.speed.x * Time.deltaTime), 1 << mapLayer);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "map")
                {
                    player.rig.MovePosition(hit.point - offSet);
                    player.speed.x = 0;
                }
            }
        }*/
    }
    void RightCollision(Player player,int layerMask,string tag)
    {
        RaycastHit2D hit = CheckCollision.CheckRightCollison(player.rig, layerMask, Mathf.Abs(player.speed.x) * Time.deltaTime);
        if (hit.collider != null)
        {
            if (hit.collider.tag == tag)
            {
                
                player.speed.x = 0;
            }
                
        }

        /*player.ColCen = player.rig.GetComponent<Collider2D>().bounds.center;
        Vector2 offSet = new Vector2(player.Collider2DExtents.x, -player.Collider2DExtents.y + 0.01f);//右下角
        Vector2 ray = player.ColCen + offSet;//射线射出位置
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.right, Mathf.Abs(player.speed.x * Time.deltaTime), 1 << layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.tag == tag)
            {
                player.rig.MovePosition(hit.point - offSet);
                player.speed.x = 0;
            }

        }
        else
        {

            offSet = new Vector2(player.Collider2DExtents.x, +player.Collider2DExtents.y - 0.01f);//右上角
            ray = player.ColCen + offSet;
            hit = Physics2D.Raycast(ray, Vector2.right, Mathf.Abs(player.speed.x * Time.deltaTime) + 0.01f, 1 << layerMask);
            if (hit.collider != null)
            {
                if (hit.collider.tag == tag)
                {
                    player.rig.MovePosition(hit.point - offSet);
                    player.speed.x = 0;
                }
            }
        }*/
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
            /*if (isDownUp)
            {
                Debug.Log("yes jump");
                player1.speed.y = initialVerticalSpeed;
                player1.IsFloat = true;

            }*/
            if(Input.GetKey(KeyCode.UpArrow))
            {
                player1.speed.y = initialVerticalSpeed;
                player1.IsFloat = true;
            }
        }
        
        if (isDownDown)
        {
            Pick(player1);
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

        if (isDownS)
        {
            Pick(player2);
        }      
        isDownDown = false;
        isDownS = false;
    }
    void Pick(Player player)
    {
        if (!player.HasPick)
        {
            float min = 10f;
            int recentRigIndex =-1;
            for (int i = 0; i < relaySize; i++)
            {
                if (!relays[i].hasPick)
                {
                    float distance = ((Vector2)(relays[i].rig.transform.position - player.transform.position)).magnitude;
                    if ( distance< 1.5f)
                    {
                        if(distance<min)
                        {
                            recentRigIndex = i;
                            min = distance;
                        }
                            
                    }
                } 
            }
            if (recentRigIndex>=0)
            {
                if(relays[recentRigIndex].transform.position.x<player.transform.position.x)
                {
                    relays[recentRigIndex].transform.Translate(new Vector3(0, player.Collider2DExtents.y, 0f) + player.transform.position - relays[recentRigIndex].transform.position);
                    relays[recentRigIndex].offSet = new Vector2(0, player.Collider2DExtents.y);
                }
                else
                {
                    relays[recentRigIndex].transform.Translate(new Vector3(0, player.Collider2DExtents.y, 0f) + player.transform.position - relays[recentRigIndex].transform.position);
                    relays[recentRigIndex].offSet = new Vector2(0, player.Collider2DExtents.y);
                }
                    
                relays[recentRigIndex].picker = player;
                relays[recentRigIndex].tag = "relay";
                //relays[recentRigIndex].transform.parent = player.transform;
                player.HasPick = true;
                player.PickRelay = relays[recentRigIndex];
                relays[recentRigIndex].hasPick = true;
            }
        }
        else
        {
            player.HasPick = false;
            //player.PickRelay.transform.parent = map.transform;
            player.PickRelay.tag = "map";
            player.PickRelay.hasPick = false;
            player.PickRelay.speed.y = -3;
            
        }
    }
}
