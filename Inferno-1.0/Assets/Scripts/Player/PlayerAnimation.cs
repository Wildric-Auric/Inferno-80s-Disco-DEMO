using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    PlayerController2D player;
    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerController2D>();
    }
    void Update()
    {
        anim.SetBool("isRunning", (player.movement != 0) && player.grounded);
    }
}
