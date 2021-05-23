using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    CustomInputs CI;
    CombatManager CM;
    [SerializeField] GameObject pause;
    AudioManager AM;
    MusicManager MM;

    public bool isPaused;
    void Start()
    {
        CI = GetComponent<CustomInputs>();
        CM = FindObjectOfType<CombatManager>();
        AM = GetComponent<AudioManager>();
        MM = GetComponent<MusicManager>();
    }

    void Update()
    {
        
        if (CI.Interact && (Time.timeScale == 0 || Time.timeScale == 1) && CM.hasStarted && pause != null)
        {
            isPaused = !isPaused;
            var audio = AM.GetAudioSource(MM.currentSound);

            pause.SetActive(!pause.activeSelf);
        
            if (Time.timeScale == 1)
            {
                audio.volume = 0.4f;
                Time.timeScale = 0;
            }
            else if (Time.timeScale == 0)
            {
                audio.volume = 1f;
                Time.timeScale = 1;
            }
        }
    }
}
