using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TropFloor : MonoBehaviour
{
    public bool canDestory;
    public float liveTime;
    public GameObject pieces;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canDestory)
        {
            GameObject pieceEffect=GameObject.Instantiate(pieces, transform.position+new Vector3(-1.434f,0.439f,0), transform.rotation);
            GameObject.Destroy(pieceEffect, 0.2f);
            GameObject.Destroy(gameObject);
        }
    }
}
