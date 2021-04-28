using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GainTextManager : MonoBehaviour
{
    TextMeshProUGUI pointText;
    CombatManager CM;
    AudioManager AM;

    int totalPoints;
    private void Start()
    {
        pointText = GetComponent<TextMeshProUGUI>();
        CM = FindObjectOfType<CombatManager>();
        AM = FindObjectOfType<AudioManager>();
    }
    public void gain(float energy)
    {
        AM.PlaySound("gain");
        totalPoints += (int)energy * 10 + (int)(100*CM.difficulty);
        pointText.text = totalPoints.ToString();
    }
}
