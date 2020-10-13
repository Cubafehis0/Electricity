using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatusManage : MonoBehaviour
{
    
    private static void Start()
    {
        GameStatus.Lever = 0;
        GameStatus.IsPass =false;
        GameStatus.IsAlive = true;
    }
    void Update()
    {
        if(!GameStatus.IsAlive)
        {   
            GameStatus.IsAlive = true;
            SceneManager.LoadScene(GameStatus.Lever);
            
        }
        if(GameStatus.IsPass)
        {
            GameStatus.Lever++;
            GameStatus.IsPass = false;
            SceneManager.LoadScene(GameStatus.Lever);
        }
    }
}
