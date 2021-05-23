using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class us 
{
    //Here are some useful function that can be called
    ///<summary>
    ///Remove substring given start and last index.
    ///</summary>
    public static string RemoveSub(string str, int startIndex, int endInedex)
    {
        return str.Remove(startIndex, endInedex - startIndex + 1);
    }
    ///<summary>
    ///Pick one of two values, with preponderence, write 100% and it will pick up second value and for 0% it will pick first value all times
    ///</summary>                                                                                                                                   
    public static float RandomPick(float num1, float num2, float preponderence)
    {
        preponderence = Mathf.Max(0,Mathf.Min(1, preponderence)); //Handle invalid input 
        var temp = Mathf.Min(1 - preponderence, preponderence);
        var value = Random.Range(Mathf.Max(preponderence - .5f, 0f), Mathf.Min(1, preponderence + .5f));
        if (value >= 0.5)
        {
            return num2;
        }
        else return num1;
    }
}