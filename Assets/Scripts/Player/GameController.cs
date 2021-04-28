using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    [Header("Horizontal speed")]
    Rigidbody2D rb;
    SpriteRenderer sr;
    Transform trans;
    [SerializeField] float spd = 5f;
    [SerializeField] float actualSpd;
    [SerializeField] float stopingFactor = 0f;

    [Header("Parametres for jump force mode")]
    [SerializeField] float jumpForce = 4f;
    [SerializeField] bool JumpForceMode = false;
    [SerializeField] float fallingSpd = 10f;

    [Header("Checkers")]
    [SerializeField] Transform center;
    [SerializeField] float radiusOfGroundChecking = 0.1f;
    [SerializeField] float upRadius = 0.1f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform center1;

    [Header("Air Parameters")]
   //[SerializeField] float smoothReturn = 0.3f;
    [SerializeField] float HowMuchAirSpdIsSlower = 6f;
    [SerializeField] float jumpSpd = 0.2f;
    [HideInInspector] public float vspd = 2f;
    [SerializeField] float gravity = 1f;
    [SerializeField] float jumpBuffer = 1f;
    float actualJumpBuffer;
    bool upCollision;
    bool inAir;

    [Header("Combat")]
    public float interval = 1f;
    public float attackInterval = 0.1f;
    [SerializeField] bool hasAttackAbility = false;
    //AnimationVariables
    [HideInInspector] public int attackMotion = 0;
    int InitialAttack;
    [HideInInspector]public bool isAttacking;

    CustomInputs CI;
    public bool canAttack = true;
    bool once = false;
    float sign;
    public bool grounded;
    bool runningCoroutine = false;

    public bool isControlled = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        trans = GetComponent<Transform>();
        CI = FindObjectOfType<CustomInputs>();
        actualSpd = spd;
        actualJumpBuffer = jumpBuffer;

    }
 
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(center.position, radiusOfGroundChecking, groundMask);
        if (!grounded) { actualJumpBuffer -= Time.fixedDeltaTime; } else if (actualJumpBuffer != jumpBuffer) { actualJumpBuffer = jumpBuffer; }
        if (JumpForceMode)
        {
            if (CI.Jump && grounded)
            {
                rb.AddForce(new Vector2(0, jumpForce * Time.fixedDeltaTime), ForceMode2D.Impulse);
            }
            //Following  condition may be a test case that can be replaced by !gounded
            if (actualJumpBuffer < -0.1f)
            {
                spd = actualSpd / HowMuchAirSpdIsSlower;
                rb.velocity -= new Vector2(0, fallingSpd * Time.fixedDeltaTime);
            }
            else { spd = actualSpd; }
        }
        else
        {
            //Building jump with speed control
            upCollision = Physics2D.OverlapCircle(center1.position, upRadius, groundMask);
            if (actualJumpBuffer >= 0 && CI.Jump)
            {
                actualJumpBuffer = -1f;
                vspd = jumpSpd;
            }
            if ((upCollision || !CI.jumpHold) && vspd > 0)
            {
                vspd = 0;
            }
            vspd -= gravity * Time.fixedDeltaTime;
            if (actualJumpBuffer>= 0 && (!CI.Jump))
            {
                vspd = 0;
            }
            //if (!grounded && !inAir)
            //{
            //    inAir = true;
            //    spd = (float)Decimal.Divide((decimal)spd, (decimal)HowMuchAirSpdIsSlower);
            //}
           //else
            //{
            //    inAir = false;
            //    spd = actualSpd;
            //}
            rb.transform.position += new Vector3(0, vspd * Time.fixedDeltaTime, 0);
        }
        //Horizontal movement
        if (isControlled)
        {
            if (CI.MoveRight)
            {
                once = false;
                rb.velocity = new Vector2(spd * Time.fixedDeltaTime, rb.velocity.y);
                sr.flipX = false;
            }

            if (CI.MoveLeft)
            {
                once = false;
                rb.velocity = new Vector2(-spd * Time.fixedDeltaTime, rb.velocity.y);
                sr.flipX = true;
            }
        }

        if (!CI.MoveRight && !CI.MoveLeft && rb.velocity.x != 0)
        {
            {   //Trying to implement a velocity decrease
                if (once == false) { sign = Mathf.Sign(rb.velocity.x); }
                once = true;
                rb.velocity -= sign * new Vector2(stopingFactor, sign * rb.velocity.y);
            }
            if (Mathf.Sign(rb.velocity.x) != sign)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        if (CI.Attack && hasAttackAbility && isControlled)
        {
            if (canAttack == true)
            {
                attackMotion += 1;
                StartCoroutine(CanAttack());
            }
            if (attackMotion >= 4) { attackMotion = 3;}
            if (runningCoroutine == false)
            {
                StartCoroutine(attacking());
            }

        }


    }

    IEnumerator attacking()
    {
        runningCoroutine = true;
        yield return new WaitForSeconds(interval);
        if (attackMotion != 0)
        { attackMotion = 0;}
        runningCoroutine = false;
    }
    IEnumerator CanAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackInterval);
        canAttack = true;

    }


}