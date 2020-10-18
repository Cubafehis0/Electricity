using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    //负责门的开关，被Key脚本调用
    public Transform upDoor;
    public Transform downDoor;
    public bool isOpen=false;//控制门的开关
    public float liveTime;//门延迟销毁的时间
    public float speed;//门打开后上下移动的速度
    float time = 0;//控制门的延迟销毁
    bool isPlay;//是否播放开门音乐；

    private void Start()
    {
        isPlay = false;
    }
    private void FixedUpdate()
    {
        if(isOpen)
        {
            if(!isPlay)
            {
                isPlay = true;
                AudioSource source = GetComponent<AudioSource>();
                source.Play();
            }
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
