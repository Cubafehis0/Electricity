using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManage : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player1;
    public Player player2;
    Animator animator1, animator2;
    void Start()
    {
        animator1 = player1.transform.GetComponent<Animator>();
        animator2 = player2.transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Animanage();

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


}
