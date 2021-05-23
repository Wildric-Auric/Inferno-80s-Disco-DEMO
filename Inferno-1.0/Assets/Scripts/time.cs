using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class time : MonoBehaviour
{

    CustomInputs CI;
    AudioManager AM;
    MusicManager MM;
    bool canScaleTime = true;
    [SerializeField] float speed;
    [SerializeField] float itime;
    [SerializeField] float minScale = 0.1f;
    [SerializeField] float deadTime = 5f;
    [SerializeField] GameObject canScaleTimeIcon;

    private void Start()
    {
        CI = GetComponent<CustomInputs>();
        AM = GetComponent<AudioManager>();
        MM = GetComponent<MusicManager>();
    }
    void Update()
    {
        canScaleTimeIcon.SetActive(canScaleTime);
        if (CI.timeScale && canScaleTime)
        {
            StartCoroutine(ScaleTime(speed));
            canScaleTime = false;
        }
    }
    IEnumerator ScaleTime(float time)
    {
        var i = 1f;
        var audioSource = AM.GetAudioSource(MM.currentSound);
        while (i > minScale)
        {

            i =  Mathf.Max(minScale,i-Time.deltaTime * speed);
            Time.timeScale = i;
            audioSource.pitch = i;
            if (!CI.timeScale)
            {
                time = 0f;
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(time);
        while (i <1)
        {
            i = Mathf.Min(1, i + Time.deltaTime * speed);
            Time.timeScale = i;
            audioSource.pitch = i;
            yield return null;
        }
        yield return new WaitForSeconds(deadTime);
        canScaleTime = true;

        
    }
}
