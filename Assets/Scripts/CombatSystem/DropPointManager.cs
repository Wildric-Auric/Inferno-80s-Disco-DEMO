using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPointManager : MonoBehaviour
{
    [SerializeField] int TotalOfDropPoints = 50;
    void Awake()
    {
        var point = transform.GetChild(0);
        for (int i = 0; i < TotalOfDropPoints; i++) 
        {
            Instantiate(point, this.transform);
        }
    }
}
