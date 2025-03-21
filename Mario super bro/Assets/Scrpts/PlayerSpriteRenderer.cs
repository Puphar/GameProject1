using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    private PlayerMovement movement;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<PlayerMovement>();
    }

    private void LateUpdate()
    {

        if (movement.jumping)
        {
            anim.SetBool("IsJumpping", true);
        }
        else if (movement.sliding)
        {
            anim.SetBool("IsSliding", true);
        }
        else if(movement.running)
        {
            anim.SetBool("IsRunning", true);

        }
    }
}
