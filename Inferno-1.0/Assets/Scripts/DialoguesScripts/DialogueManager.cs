using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameController player;
    public Dialogue dl;
    [SerializeField] GameObject canva;
    TextMeshProUGUI txt;
    GameObject Panel;
    GameObject textObj;
    CustomInputs CI;

    bool started = false;
    int currentLetter = 0;
    int frequency;
   
    public int sAcc; //Start index of accessible text in a dialogue
    public int fAcc = 2; //Final Index of accessible text in dl
    public int currentPar = 0; // The phrase number
    string currentText; //That is modified

    bool shake;
    int shakeStartIndex = 0;
    int shakeEndIndex = 10;
    public float cosFrequency = 5f;
    public float cosMul = 10;
    public float sinFrequency;
    public float sinMul;
    //Next Part is for setting dialogue for each dialogue script
    public bool available = true; //To prevent problem if charachters are near each others...
    public bool triggered; 

    void Start()
    {
        CI = FindObjectOfType<CustomInputs>();
    }
    void Update()
    {
        if (available && !canva.activeSelf && triggered)
        {
            player.isControlled = false;
            triggered = false;
            available = false;
            canva.SetActive(true);
            FindObjectOfType<Transition>().OpenPanel();
            SetUpCanva();
            started = true;
            frequency = dl.wordsFrequency;
            currentText = dl.dia[currentPar];
            while (currentText.Contains("<"))
            {
                currentText = us.RemoveSub(currentText, currentText.IndexOf("<"), currentText.IndexOf(">"));
            }
        }
        else if (canva.activeSelf && CI.talk && txt.text.Length >= currentText.Length && currentPar != fAcc)
        {
            currentLetter = 0;
            frequency = dl.wordsFrequency;
            txt.text = "";
            shake = false;

            currentPar += 1;
            started = true;
            currentText = dl.dia[currentPar];
            while (currentText.Contains("<"))
            {
                currentText = us.RemoveSub(currentText, currentText.IndexOf("<"), currentText.IndexOf(">"));
            }

        }
        else if (canva.activeSelf && CI.talk && txt.text.Length >= currentText.Length && currentPar == fAcc)
        {
            FindObjectOfType<Transition>().ClosePanel();
            StartCoroutine(close());
        }
        if (started)
        {
            if (txt.text.Length < currentText.Length) //It's meant to start coroutine and to write gradully the text, started is modified in coroutine
            {
                if ((currentLetter + frequency) <= currentText.Length)
                {
                    frequency = dl.wordsFrequency;
                }
                else
                { frequency = currentText.Length - currentLetter; }
                StartCoroutine(talk(currentLetter));
                currentLetter += frequency;
            }
            else
            { //In the end rich text is applied and word shaking start
                var unwantedIndex = "<shake>".Length ;
                txt.text = dl.dia[currentPar];
                var txt1 = "";
                if (txt.text.IndexOf("<shake>") != -1)
                {
                    txt1 = txt.text.Substring(0, txt.text.IndexOf("<shake>"));
                }
                var last1 = "";
                var count1 = 0;
                while (txt1.Contains("<")) //Rich text make problem of index that's why i'm doing this now
                { //Find 'true' index of first word to shake
                    last1 = txt1;
                    txt1 = us.RemoveSub(txt1, txt1.IndexOf("<"), txt1.IndexOf(">"));
                    count1 += last1.Length - txt1.Length;
                }
                shakeStartIndex = txt.text.IndexOf("<shake>") - count1;

                txt1 = "";
                if (txt.text.IndexOf("<stopShake>") != -1)  //txt.IndexOf will return -1 if there no <Shake>
                {
                    txt1 = txt.text.Substring(0, txt.text.IndexOf("<stopShake>"));
                }
                last1 = "";
                count1 = 0;
                while (txt1.Contains("<"))
                { //Find 'true' index of Last word to shake
                    last1 = txt1;
                    txt1 = us.RemoveSub(txt1, txt1.IndexOf("<"), txt1.IndexOf(">"));
                    count1 += last1.Length - txt1.Length;
                }
                shakeEndIndex = txt.text.IndexOf("<stopShake>") - count1;
                txt.text = txt.text.Replace("<shake>", "");
                txt.text = txt.text.Replace("<stopShake>", "");
                shake = true;
            } 
            started = false;
        }

        
        if (shake)
        {
            txt.ForceMeshUpdate();
            var Info = txt.textInfo;
            for (int i = shakeStartIndex; i < shakeEndIndex; i++) // To update mesh position
            {
                var charInfo = Info.characterInfo[i];
                if (!charInfo.isVisible) { continue; }
                var verts = Info.meshInfo[charInfo.materialReferenceIndex].vertices;
                for (int j = 0; j < 4; j++)
                {
                    var orig = verts[charInfo.vertexIndex + j];
                    verts[charInfo.vertexIndex + j] = orig + new Vector3(0,Mathf.Cos(Time.time * cosFrequency + orig.x * 0.01f) * cosMul + Mathf.Sin(Time.time * sinFrequency + orig.x * 0.01f) * sinMul, 0);
                }
            }
            for (int i = 0; i < Info.meshInfo.Length; i++) //To apply it on actual vertices
            {
                var meshInfo = Info.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                txt.UpdateGeometry(meshInfo.mesh, i);
            }
        }
    }

    IEnumerator talk(int current)
    {
        if ( dl.sound != null && !FindObjectOfType<AudioManager>().IsPlaying(dl.sound.Name))
        {
            FindObjectOfType<AudioManager>().PlaySoundWithpitch(dl.sound.Name, UnityEngine.Random.Range(dl.minPitch,dl.maxPitch), 1);
        }
        yield return new WaitForSeconds(Time.deltaTime*dl.waiting);
        txt.text = String.Concat(txt.text, currentText.Substring(current, Mathf.Min(currentText.Length,frequency)));
        started = true;
        
    }

    void SetUpCanva()
    {
        Panel = canva.transform.Find("Panel").gameObject;
        textObj = Panel.transform.Find("Text").gameObject;
        txt = textObj.GetComponent<TextMeshProUGUI>();
        var imag = Panel.GetComponent<Image>();
        imag.color = dl.PanelColor;
        imag.sprite = dl.Image;
        txt.font = dl.font;
        txt.fontStyle = dl.fs;
        txt.fontSizeMin = dl.minSize;
        txt.fontSizeMax = dl.maxSize;
        txt.color = dl.FontColor;
        txt.text = "";
    }

    IEnumerator close()
    {
        player.isControlled = true;
        yield return new WaitForSeconds(0.2f);
        currentPar = sAcc;
        currentLetter = 0;
        frequency = dl.wordsFrequency;
        started = false;
        txt.text = "";
        shake = false;
        available = true;
        canva.SetActive(false);
    }
}
