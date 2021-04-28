using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class bullet : MonoBehaviour
{
    Rigidbody2D rb;
    AudioManager AM;

    [HideInInspector] public float maxDis;
    [HideInInspector] public Vector3 initialPos;
    [HideInInspector] public Transform parent;
    [SerializeField] ParticleSystem PS;
    [SerializeField] Transform shotBy;

    [Header("VisualEffects")]
    [SerializeField] GameObject flash;
    [SerializeField] float flashDuration;
    
    [Header("To find this animation length")]
    [SerializeField] AnimationClip animatio;

    [Header("Layers that are affected by the ball")]
    [SerializeField] LayerMask targetLayers;
    [SerializeField] LayerMask collisionMask;

    float damage;

   
    [Header("About Facing direction of movement")]
    Vector3 newPos;
    int temp;
    [SerializeField] int numberOfFrame = 2;


    private void Awake()
    {
        AM = FindObjectOfType<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void launch(float Force, Vector2 Direction, float gravityScale, float cost, float damage)
    {
        flash.SetActive(true);
        rb.AddForce(Force * Direction, ForceMode2D.Impulse);
        rb.gravityScale = gravityScale;
        PS.Play();
        StartCoroutine(StopFlash());

        newPos = transform.position;

        this.damage = damage;
        //Apply energy minus
        shotBy.GetComponent<Energy>().currentEnergy -= cost;
        
    }

    void Update()
    {
        ///I'm doing THIS because sprite cannot be set in script in 2D lights
        var currentSprite = GetComponent<SpriteRenderer>().sprite.name;
        var spriteIndex = int.Parse(currentSprite[currentSprite.Length - 1].ToString());
        var currentChild = transform.GetChild(spriteIndex).gameObject;
        if (!currentChild.activeSelf)
        {
            transform.GetChild(spriteIndex - 1 + 6 * Convert.ToInt16(spriteIndex == 0)).gameObject.SetActive(false);
            currentChild.SetActive(true);
        }

        var difference = transform.position - newPos;
        if (difference != Vector3.zero) //This "if" is meant to update only of varation of position is detected;
        {
            Quaternion a = Vector3.Cross(difference, Vector3.right).z < 0 ?
            transform.rotation = Quaternion.Euler(0f, 0f, Vector3.Angle(difference, Vector3.right)) :
            transform.rotation = Quaternion.Euler(0f, 0f, -Vector3.Angle(difference, Vector3.right));

        }

        if ((transform.position - initialPos).magnitude > maxDis)
        {
            transform.SetParent(parent);
            gameObject.SetActive(false);
            transform.position = parent.transform.position;
        }
        if (temp == numberOfFrame)
        {
            newPos = transform.position;
            temp = 0;
        }
        temp += 1;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var colliderObj = collision.gameObject;
        if (collisionMask.Includes(colliderObj.layer))
        {
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;
            var anim = GetComponent<Animator>();
            anim.Play("Bullet Crash");
            StartCoroutine(end(animatio.length));
        }
        if (targetLayers.Includes(colliderObj.layer))
        {
            var otherEnergy = colliderObj.GetComponent<Energy>();
            otherEnergy.onHit(damage, shotBy);
            AM.PlaySoundWithpitch("hit", UnityEngine.Random.Range(0.5f, 1.5f), 1f);
            
        }
    }
    IEnumerator StopFlash()
    {
        yield return new WaitForSeconds(flashDuration);
        flash.SetActive(false);
    }
    IEnumerator end(float time)
    {
        yield return new WaitForSeconds(time +0.03f);
        gameObject.SetActive(false);
        transform.SetParent(parent);
        transform.position = parent.transform.position;
    }
}
