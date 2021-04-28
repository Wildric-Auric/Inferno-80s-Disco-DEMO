using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInputs : MonoBehaviour
{
    public bool MoveRight;
    public bool MoveLeft;
    public bool Jump;
    public bool jumpHold;
    public bool Attack;
    public bool Fire;
    public bool timeScale;
    public bool BenshoOne;
    public bool Interact;
    public bool aganor;
    public bool wildric;
    public bool fusion; //A great abandonned idea...
    public bool talk;
    public bool Inventory;
    private bool canTalk = true; //I'm adding this to prevent a bug when you click on talk several times in a small range of time
    PlayerController2D Charachter;

    private void Start()
    {
        Charachter = FindObjectOfType<PlayerController2D>();
    }
    void Update()
    {
        MoveRight = (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))&& Charachter.isControlled;
        MoveLeft = (Input.GetKey("q") || Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow)) && Charachter.isControlled;
        Jump = (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z)) && Charachter.isControlled;
        jumpHold = (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z))&& Charachter.isControlled;
        Attack = Input.GetKeyDown("e");
        Fire = Input.GetMouseButton(0) && Charachter.isControlled; 
        timeScale = Input.GetMouseButton(1) && Charachter.isControlled; ;
        BenshoOne = Input.GetMouseButtonDown(0) && Charachter.isControlled; ;
        Interact = Input.GetKeyDown(KeyCode.Escape);
        aganor = Input.GetKey(KeyCode.Alpha1);
        wildric = Input.GetKey(KeyCode.Alpha2);
        Inventory = Input.GetKeyDown(KeyCode.Tab);

        //if (Input.GetKeyDown(KeyCode.Escape) && canTalk)
        //{
        //    talk = true;
        //    canTalk = false;
        //    StartCoroutine(canTalkAgain());
        //}
        //else {talk = false;}



    }
    IEnumerator canTalkAgain()
    {
        yield return new WaitForSeconds(0.3f);
        canTalk = true;
    }
}
