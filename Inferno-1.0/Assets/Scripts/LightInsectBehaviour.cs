using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightInsectBehaviour : MonoBehaviour
{
    float xSize;
    float ySize;
    float[,] pos;
    [SerializeField] int insectNumber;
    [SerializeField] float insectSpeed;
    void Start()
    {
        var sr = GetComponent<SpriteRenderer>();
        xSize = sr.bounds.size.x/2;
        ySize = sr.bounds.size.y/2;
        var insectModel = transform.GetChild(0).gameObject;
        for (int i = 0; i < insectNumber; i++)
        {
           var temp = Instantiate(insectModel,transform,false);
        }
        var arraySize = transform.childCount;
        pos = new float[arraySize,2];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var i = 0;
        foreach (Transform insect in transform)
        {
            var target = new Vector3(pos[i, 0], pos[i, 1]);
            if ((Vector2.Distance(insect.position, target + this.transform.position) < 0.01) || target == Vector3.zero)
            {
                pos[i, 0] = UnityEngine.Random.Range(-xSize, xSize);
                pos[i,1] = UnityEngine.Random.Range(-ySize, ySize);
            }
            var speed = Time.fixedDeltaTime * insectSpeed;
            insect.transform.position = Vector2.MoveTowards(insect.position,target + this.transform.position, speed);
            i++;
        }
    }
}
