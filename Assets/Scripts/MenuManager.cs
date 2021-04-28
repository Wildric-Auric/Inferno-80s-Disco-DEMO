using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    Animator anim;
    private static bool funny = true;
    string[] funnyArray = { "You should respect the rules", "I've told you to not click on!","Funny, isn't it?","My wish is that you don't click on it." };
    TextMeshProUGUI txt;
    private void Start()
    {
        anim = GetComponent<Animator>();
        txt = transform.Find("funnyText").GetComponent<TextMeshProUGUI>();
    }

    public void Quit()
    {
        if (funny)
        {
            txt.text = funnyArray[(int)Random.Range(0f, 3.99f)];
            anim.Play("funny");
            funny = false;
        }
        else Application.Quit();
    }
    public void Play()
    {
        anim.Play("play");
        StartCoroutine(Start(1f));
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator Start(float time)
    {
        yield return new WaitForSeconds(time);
        FindObjectOfType<CombatManager>().hasStarted = true;
    }
}
