using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;

public class Key : MonoBehaviour
{
    //控制门的开关或者玻璃的破碎，附加在开关-关 上面
    public bool isControlDoor;//控制门的开关
    public bool isControlFloor;//控制玻璃破碎
    public GameObject keyOpen;//开关打开后的预制体
    public GameObject tropFloor;//玻璃集合所在的父物体
    TropFloor[] tropFloors;//玻璃
    Rigidbody2D rig;
    public DoorOpen door;//控制的门
    int relayLayer;//继电器所在的层
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        relayLayer = LayerMask.NameToLayer("Relay");
        if(isControlFloor)
            tropFloors = tropFloor.GetComponentsInChildren<TropFloor>();

    }
    private void FixedUpdate()
    {
        Open();
    }
    void Open()
    {
        RaycastHit2D hit = CheckCollision.CheckUpCollison(rig, relayLayer, 0.1f);//与继电器的碰撞检测
        if(hit.collider!=null)
        {
            if(isControlDoor)
            {
                door.isOpen = true;
            }
            else if(isControlFloor)
            {
                for(int i=0;i<tropFloors.Length;i++)
                {
                    tropFloors[i].canBroken = true;
                    tropFloors[i].liveTime = i * 0.4f + 0.4f;//设置玻璃的剩余时常
                }
            }
            GameObject.Instantiate(keyOpen, transform.position,Quaternion.identity,transform.parent);//建造开关—开
            GameObject.Destroy(gameObject);    
        }
    }
}
