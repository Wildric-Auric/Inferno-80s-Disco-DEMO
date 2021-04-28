using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Book", menuName = "Book")]
public class Book : Item
{
   [Header("Book Properties")]
   public int NumberOfPages;
}
