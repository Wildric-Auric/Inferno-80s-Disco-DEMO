using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class Transition : MonoBehaviour
{

    [Header("Panel Animation")]
    [SerializeField] GameObject PanelCanva;
    Animator PanelAnimator;


    private void Start()
    {
        PanelAnimator = PanelCanva.GetComponent<Animator>(); 
    }

    public void OpenPanel()
    {
        PanelAnimator.Play("OpenDialogue");
    }
    public void ClosePanel()
    {
        PanelAnimator.Play("CloseDialogue");
    }


}
