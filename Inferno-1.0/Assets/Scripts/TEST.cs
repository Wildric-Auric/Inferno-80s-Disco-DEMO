using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public bool trig;
    public float rot;
    // Update is called once per frame
    void Update()
    {
        if (trig)
        {
            transform.rotation = Quaternion.Euler(0, 0, rot);
            trig = false;
        }
    }
}
