using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animCycle : MonoBehaviour
{
//THINK ABOUT IMPROVING THIS LATER USING EVENT SYSTEM!
    [SerializeField] Animator anim;
    [SerializeField] float timeofCycle = 10f;
    [SerializeField] int offset = 3;
    bool time = true;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (time)
        {
            StartCoroutine(enclache(timeofCycle + Random.Range(-offset,offset)));
            time = false;
        }
        
    }
    IEnumerator enclache(float timeofCycle)
    {
        anim.Play("Guy sitting on bench");
        yield return new WaitForSeconds(timeofCycle);
        time = true;
    }

}
