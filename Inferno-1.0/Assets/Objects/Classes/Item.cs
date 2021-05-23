using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "Item",fileName = "NewItem")]
public class Item : ScriptableObject
{
    [Header("Name Settings")]
    public string Name;
    public TMP_FontAsset NameFont;
    public Color NameColor;

    [Header("Description Settings")]
    [TextArea(1, 20)]
    public string Description;
    public TMP_FontAsset DescriptionFont;
    public Color DescriptionColor;
    [TextArea(1, 10)]
    public string Note;
    public TMP_FontAsset NoteFont;
    public Color NoteColor;

    [Header("Images Settings")]
    public Sprite ItemImage; //Should be 32x32 pixel art
    public Sprite BigImage; //Usually the same as last one
}
