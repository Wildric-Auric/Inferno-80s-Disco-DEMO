using System.Collections;
using UnityEngine;


public class CombatManager : MonoBehaviour
{
    public float difficulty;
    int spawnFrequency = 1;
    [SerializeField] Collider2D spawnArea;
    [SerializeField] int maxEnemies = 20;

    public bool hasStarted;
    Vector3 origin;
    float Xpos;
    float Ypos;

    void Awake()
    {
        //Clone enemy object
        var enemy = transform.GetChild(0).gameObject;
        for (int i = 0; i<maxEnemies; i++)
        {
            Instantiate(enemy, transform);
        }
        origin = spawnArea.transform.position;
        Xpos = spawnArea.bounds.size.x *0.5f;
        Ypos = spawnArea.bounds.size.y * .5f;
    }

    private void OnEnable()
    {
        StartCoroutine(IncreaseDifficulty(35f));
    }
    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            var currentNumber = maxEnemies - transform.childCount;
            if (currentNumber != spawnFrequency - 1 && transform.childCount >= 1)
            {
                var enemy = transform.GetChild(0);
                enemy.position = new Vector3(origin.x + Random.Range(-Xpos, Xpos), origin.y + Random.Range(-Ypos, Ypos), 0);
                enemy.SetParent(null);
                enemy.gameObject.SetActive(true);
            }
        }

    }
    IEnumerator IncreaseDifficulty(float time)
    {
        while (difficulty < 1f)
        {
            if (!hasStarted) yield return null;
            yield return new WaitForSeconds(time);
            time += difficulty * 80;
            difficulty += .1f;
            spawnFrequency = (int) us.RandomPick((difficulty * 10), (difficulty * 10) + 1, 0.25F);
        }
    }

}
