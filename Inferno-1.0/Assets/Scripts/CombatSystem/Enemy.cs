using System.Collections;
using System; 
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    firing enemyFiring;
    Energy energy;
    Transform target;
    Rigidbody2D rb;
    CombatManager CM;

    bool flying;
    [SerializeField] float restSeconds;
    [SerializeField] float restRandomness;
    public Collider2D moveArea;
    [HideInInspector]public float speed;

    public static int defeatedEnemies;

    private Transform origin;
    float sizeX;
    float sizeY;
    float randomX;
    float randomY;
    bool isFree = true;
    bool coroutineIsRunning;
    bool death = false;

    //About Initialization of enemies;
    [SerializeField] float minSpd = 1.4f;
    [SerializeField] float maxSpd = 3.3f;
    [SerializeField] float creepySpd = 10f;
    [SerializeField] float minBulletDamge = 2f;
    [SerializeField] float maxBulletDamage = 2f;
    [SerializeField] float randomOffset = 0.1f;
    [SerializeField] float minEnergy = 1;
    [SerializeField] float maxEnergy = 10;
    [SerializeField] float minDeadTime = 0.2f;
    [SerializeField] float maxDeadTime = 1.5f;
    [SerializeField] float minBigGain = 5f;
    [SerializeField] float maxBigGain = 25f;
    void OnEnable()
    {
        ///I just figured out that instances aren't reset when they are disabled
        death = false;
        coroutineIsRunning = false;
        isFree = true;
 
        CM = FindObjectOfType<CombatManager>();
        enemyFiring = GetComponent<firing>();
        target = GameObject.Find("player").transform;
        rb = GetComponent<Rigidbody2D>();
        origin = moveArea.transform;
        sizeX = moveArea.bounds.size.x * 0.5f;
        sizeY = moveArea.bounds.size.y*.5f;

        //Init a random enemy it's here where everything is played
        //Type of enemy
        flying = Convert.ToBoolean((int)us.RandomPick(0, 1, CM.difficulty + UnityEngine.Random.Range(-.2f, 0))); //As difficulty increase more flying enemies appear
        rb.gravityScale = Convert.ToInt16(!flying); // if enemy flies he is not affected by gravity
        speed = us.RandomPick(us.RandomPick(0, UnityEngine.Random.Range(minSpd, maxSpd), CM.difficulty + UnityEngine.Random.Range(0, 0.3f)), creepySpd, Mathf.Max(0, Convert.ToInt16(CM.difficulty >= .5f) - 0.7f)) ;; //Creepy enemy  can appear only when difficly is higher than 0.5
      
        enemyFiring.gravityScale = us.RandomPick(0, 1, 0.1f) * Convert.ToInt16(!flying); //Gravity bullets
        enemyFiring.bulletDamage = us.RandomPick(minBulletDamge, UnityEngine.Random.Range(minBulletDamge+1, maxBulletDamage), CM.difficulty);
        var randomDir = UnityEngine.Random.Range(0.3F, 0.5F); // offset of gravity bullets
        var normalRandomDir = UnityEngine.Random.Range(0, randomOffset); // Offset of normal bullets
        enemyFiring.offsetX = randomDir * ((int)(enemyFiring.gravityScale)) + normalRandomDir;
        enemyFiring.offsetY = randomDir * ((int)(enemyFiring.gravityScale)) + normalRandomDir;
        enemyFiring.deadTime = us.RandomPick(UnityEngine.Random.Range(minDeadTime * 0.5F, minDeadTime * 1.5F), UnityEngine.Random.Range(maxDeadTime * 0.5f, maxDeadTime * 1.5f), 0.65f);

        energy = GetComponent<Energy>();
        energy.maxEnergy = Mathf.Max(minEnergy, maxEnergy * CM.difficulty);
        energy.currentEnergy = Mathf.Max(minEnergy, maxEnergy * CM.difficulty);
        energy.normalGain = 1;
        energy.bigGain = UnityEngine.Random.Range(minBigGain, maxBigGain);
        energy.normalDropNumber =(int) us.RandomPick(1f, 2f, 0.5f);
    }
    void FixedUpdate()
    {
        if (energy.currentEnergy <= 0 && !death)
        {
            StartCoroutine(Death(0.2f));  //0.2 is the legth of animation, not a good paractice to do it like that
        }
        enemyFiring.target = target.position;
        enemyFiring.onFire = CM.hasStarted; //To stop firing when gameHasfinished;
        //Pick random point
        if (isFree)
        {
            isFree = false;
            randomX = origin.position.x + UnityEngine.Random.Range(-sizeX, sizeX);
            randomY = origin.position.y + UnityEngine.Random.Range(-sizeY, sizeY);
            
        }
        if (flying == true)
        {
            if (Vector3.Distance(transform.position, new Vector3(randomX, randomY, 0)) > 0.5f)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(randomX, randomY), speed * Time.fixedDeltaTime);
            }
            else
            {
                rb.velocity = Vector2.zero;
                if (!coroutineIsRunning)
                { StartCoroutine(Rest(restSeconds + UnityEngine.Random.Range(0, restRandomness)));}
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, new Vector3(randomX, transform.position.y, 0))>0.5f)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(randomX, transform.position.y), speed* Time.fixedDeltaTime);
            }
            else {
                rb.velocity = Vector2.zero;
                if (!coroutineIsRunning)
                {
                    StartCoroutine(Rest(restSeconds + UnityEngine.Random.Range(0, restRandomness)));
                }
            }
        }
        
    }
    IEnumerator Rest(float time)
    {
        coroutineIsRunning = true;
        yield return new WaitForSeconds(time);
        coroutineIsRunning = false;
        isFree = true;
    }
    IEnumerator Death(float time)
    {
        FindObjectOfType<GainTextManager>().gain(maxEnergy);//To update total on points
        death = true;
        energy.isDead = true;
        var ring = transform.Find("ring").gameObject;
        ring.SetActive(true);
        yield return new WaitForSeconds(time);
        ring.SetActive(false);
        transform.SetParent(GameObject.Find("Enemy Container").transform);
        gameObject.SetActive(false);
        


    }
}
