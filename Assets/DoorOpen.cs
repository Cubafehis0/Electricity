using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Transform upDoor;
    public Transform downDoor;
    public bool isOpen=false;
    public float liveTime;
    public float speed;
    float time = 0;
    private void FixedUpdate()
    {
        if(isOpen)
        {
            time += Time.deltaTime;
            if(time < liveTime)
            {
                time += Time.deltaTime;
                upDoor.Translate( new Vector2(0, speed * Time.deltaTime));
                downDoor.Translate(new Vector2(0,-speed*Time.deltaTime));
            }
            else
                GameObject.Destroy(gameObject);
        }
    }

}
