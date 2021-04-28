using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[CreateAssetMenu(fileName = "newDialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    [Header("Type dialogues here, after selecting how many case in the array")]
    [TextArea(1, 15)]
    public string[] dia;

    [Header("Font")]
    public TMP_FontAsset font;
    public FontStyles fs;
    public int minSize;
    public int maxSize;
    public Color FontColor;
    [Header("Panel")]
    public float xPosition;
    public float yPosition;
    public Color PanelColor;
    public Sprite Image;
    [Header("Sound while displaying text")]
    public Sound sound;
    public float minPitch;
    public float maxPitch;
    [Header("Speed")]
    public float waiting;
    public float waitingForComa;
    public float waitingForOther;
    public int wordsFrequency = 1;
}
