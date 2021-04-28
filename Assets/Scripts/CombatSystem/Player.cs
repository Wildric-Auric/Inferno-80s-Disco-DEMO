using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    Slider slider;
    Energy playerEnergy;
    PlayerController2D player;
    CombatManager CM;
    PauseMenu PM;
    GameObject gameOverObj;
    bool once = false;

    void Start()
    {
        slider = GameObject.Find("Energy bar").GetComponent<Slider>();
        playerEnergy = GetComponent<Energy>();
        player = GetComponent<PlayerController2D>();
        CM = FindObjectOfType<CombatManager>();
        PM = FindObjectOfType<PauseMenu>();
        gameOverObj = GameObject.Find("UI").transform.Find("Score").gameObject;
        slider.maxValue = playerEnergy.maxEnergy;
        
    }
    private void Update()
    {
        slider.value = playerEnergy.currentEnergy;
        player.isControlled = CM.hasStarted && !PM.isPaused;
        if (playerEnergy.currentEnergy <= 0 && !once)
        {
            CM.hasStarted = false;
            player.transform.position = new Vector2(0, 0);
            once = true;
            GameOver();
        }
    }
    void GameOver()
    {
        FindObjectOfType<AudioManager>().PlaySoundWithpitch("GameOver", 1f, 1f);
        gameOverObj.GetComponent<Animator>().Play("GameOver");
    }
}
