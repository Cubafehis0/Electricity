using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class PlayerControl : MonoBehaviour
{
    //处理玩家的移动控制和碰撞检测
    public Player player1;//玩家1
    public Player player2;//玩家2

    public float initialHorizontalSpeed;//初始水平速度
    public float initialVerticalSpeed;//初始垂直速度
    public float verticalAcc;//垂直加速度
    public Relay[] relays = new Relay[4];//记录场景继电器信息
    private bool isFinishInitialDrop;//是否完成初始下落，没有则为false且不能操作。
    public LiftplatformMove[] liftPlats;//记录升降台信息

    bool isDownS;//控制拾取
    bool isDownDown;//控制拾取

    int mapLayer;//map层
    int relayLayer;//继电器层

    AudioSource audioSourse;
    Animator animator1, animator2;//两个玩家的动画控制器
    int relaySize;//继电器的数目
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
        audioSourse = GetComponent<AudioSource>();
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
            isDownDown = true;//监控DownArrow键是否按下
        if (Input.GetKeyDown(KeyCode.S))
            isDownS = true;//监控S键是否按下
    }
    void OnGroundCollision()
    {
        GroundCollision(player1, mapLayer, "map");//对地图层的碰撞检测
        GroundCollision(player2, mapLayer, "map");
        if (player1.IsFloat)
            GroundCollision(player1, relayLayer, "map");//继电器层的碰撞检测
        if (player2.IsFloat)
            GroundCollision(player2, relayLayer, "map");
        if (player1.IsFloat)
            GroundCollision(player1, mapLayer, "conveyBelt");//对传送带的碰撞检测
        if (player2.IsFloat)
            GroundCollision(player2, mapLayer, "conveyBelt");

    }
    void OnFloorCollision()
    {
        //顶部碰撞检测
        if (player1.speed.y > 0)
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
        //玩家之间水平方向上的碰撞检测
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
        //传送带加减速
        RaycastHit2D hit = CheckCollision.CheckDownCollison(player.rig, mapLayer, Mathf.Abs(player.speed.y)*Time.deltaTime+0.01f);
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
        //玩家垂直方向的碰撞检测
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
        //移动
        player.rig.MovePosition(player.speed * Time.deltaTime + (Vector2)player.rig.transform.position);
        if (player.IsFloat)
            player.speed.y -= verticalAcc * Time.deltaTime;
        else player.speed.y = 0;

    }
    void CheckMapLayer()
    {
        //地刺和升降台的碰撞检测
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
        //动画管理
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
        //水平方向上碰撞检测
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
        //落地检测
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
    }
    void FloorCollision(Player player,int layerMask,string tag)//顶部碰撞检测
    {
        RaycastHit2D hit = CheckCollision.CheckUpCollison(player.rig, layerMask, Mathf.Abs(player.speed.y * Time.deltaTime));
        if (hit.collider != null)
        {
            if (hit.collider.tag == tag)
            {
                player.speed.y = 0;
            }
        }
        
    }
    void LeftCollision(Player player,int layerMask,string tag)//左侧碰撞检测
    {
        RaycastHit2D hit = CheckCollision.CheckLeftCollison(player.rig, layerMask, Mathf.Abs(player.speed.x)*Time.deltaTime);
        if(hit.collider!=null)
        {
            if(hit.collider.tag==tag)
                player.speed.x = 0;
        }
    }
    void RightCollision(Player player,int layerMask,string tag)//右侧碰撞检测
    {
        RaycastHit2D hit = CheckCollision.CheckRightCollison(player.rig, layerMask, Mathf.Abs(player.speed.x) * Time.deltaTime);
        if (hit.collider != null)
        {
            if (hit.collider.tag == tag)
            {
                
                player.speed.x = 0;
            }
                
        }
    }
    void Control()
    {
        //人物控制
        if (Input.GetKey(KeyCode.LeftArrow))
            player1.speed.x = -initialHorizontalSpeed;
        else if (Input.GetKey(KeyCode.RightArrow))
            player1.speed.x = initialHorizontalSpeed;
        else player1.speed.x = 0;
        if (!player1.IsFloat)//跳跃
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                audioSourse.Play();
                player1.speed.y = initialVerticalSpeed;
                player1.IsFloat = true;
            }
        }
        
        if (isDownDown)//拾取
        {
            Pick(player1);
        }

        if (Input.GetKey(KeyCode.A))
            player2.speed.x = -initialHorizontalSpeed;
        else if (Input.GetKey(KeyCode.D))
            player2.speed.x = initialHorizontalSpeed;
        else player2.speed.x = 0;

        if (!player2.IsFloat)//跳跃
        {
            if (Input.GetKey(KeyCode.W))
            {
                audioSourse.Play();
                player2.speed.y = initialVerticalSpeed;
                player2.IsFloat = true;
            }
        }

        if (isDownS)//拾取
        {
            Pick(player2);
        }      
        isDownDown = false;
        isDownS = false;
    }
    void Pick(Player player)
    {
        //继电器的拾取
        if (!player.HasPick)
        {
            float min = 10f;
            int recentRigIndex =-1;
            for (int i = 0; i < relaySize; i++)//找到最近的继电器
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
                relays[recentRigIndex].tag = "relay";//relay便签不与玩家碰撞检测
                //relays[recentRigIndex].transform.parent = player.transform;
                player.HasPick = true;
                player.PickRelay = relays[recentRigIndex];
                relays[recentRigIndex].hasPick = true;
            }
        }
        else
        {
            player.HasPick = false;
            player.PickRelay.tag = "map";//改变为地图标签，恢复与玩家的碰撞检测
            player.PickRelay.hasPick = false;
            player.PickRelay.speed.y = -3;//给急待你其落下的速度
            
        }
    }
}
