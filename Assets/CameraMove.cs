﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player1;
    public Transform player2;
    public Transform carema;
    Vector2 tranlation;
    private void FixedUpdate()
    {
        tranlation = (player1.position + player2.position) / 2- carema.position;
        carema.Translate(tranlation );
    }
}
