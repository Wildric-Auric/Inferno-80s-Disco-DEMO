using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BenchGuyDialogue : MonoBehaviour
{
    DialogueManager DM;
    [SerializeField] int startIndex = 0;
    [SerializeField] int finalIndex = 0;
    [FormerlySerializedAs("Dialogue")]
    [SerializeField] Dialogue dia;
    GameController player;
    CustomInputs CI;

    [Header("Shake Parameters")]
    [SerializeField] float cosFrequency = 5f;
    [SerializeField] float cosMul = 10;
    [SerializeField] float sinFrequency;
    [SerializeField] float sinMul;

    void Start()
    {
        player = FindObjectOfType<GameController>();
        CI = FindObjectOfType<CustomInputs>();
        DM = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CI.talk && DM.available && Vector2.Distance(transform.position, player.transform.position) < 1f)
        {
            DM.triggered = true;
            DM.available = true;
            DM.dl = dia;
            DM.sAcc = startIndex;
            DM.fAcc = finalIndex;
            DM.currentPar = startIndex;
            DM.cosFrequency = cosFrequency;
            DM.cosMul = cosMul;
            DM.sinFrequency = sinFrequency;
            DM.sinMul = sinMul;
        }
    }
}
