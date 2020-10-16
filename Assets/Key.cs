using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;

public class Key : MonoBehaviour
{
    public bool isControlDoor;
    public bool isControlFloor;
    public GameObject keyOpen;
    public GameObject tropFloor;
    public TropFloor[] tropFloors;
    Rigidbody2D rig;
    public DoorOpen door;
    int relayLayer;
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
        RaycastHit2D hit = CheckCollision.CheckUpCollison(rig, relayLayer, 0.1f);
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
                    tropFloors[i].canDestory = true;
                    tropFloors[i].liveTime = i * 0.4f + 0.4f;
                }
            }
            GameObject.Instantiate(keyOpen, transform.position,Quaternion.identity,transform.parent);
            GameObject.Destroy(gameObject);    
        }
    }
}
