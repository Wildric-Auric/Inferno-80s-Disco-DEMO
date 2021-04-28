using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicManager : MonoBehaviour
{
    AudioManager AM;
    CombatManager CM;

    public string currentSound; //It's public so that the value can be red in time script
    TextMeshProUGUI txt;
    [SerializeField] float apparitionTime = 0.5f;
    [SerializeField] float disappearingTime = 0.5f;
    [SerializeField] float timeOfShowing = 3f;
    [SerializeField] GameObject canva;
    
    private void Start()
    {
        AM = GetComponent<AudioManager>();
        txt = GameObject.Find("Musics Names").GetComponent<TextMeshProUGUI>();
        CM = FindObjectOfType<CombatManager>();
    }

    void Update()
    {
       if ((currentSound == ""||!AM.IsPlaying(currentSound)) && CM.hasStarted)
        {
            var temp = (int)Random.Range(4f, 14.99F); //Pick random number to play music
            currentSound = AM.PlaySoundByIndex(temp);
            StartCoroutine(DisplayMusicName(currentSound));
        }
    }
    IEnumerator DisplayMusicName(string name)
    {
        txt.text = name;
        var i = 0f;
        var canvaGroup = canva.GetComponent<CanvasGroup>();
        while (i<1)
        {
            i += apparitionTime*Time.deltaTime;

            canvaGroup.alpha = i;
            yield return  new WaitForEndOfFrame();

        }
        yield return new WaitForSeconds(timeOfShowing);
        while (i > 0)
        {
            i -= disappearingTime*Time.deltaTime;
            canvaGroup.alpha = i;
            yield return new WaitForEndOfFrame();
        }
       
    }
}
