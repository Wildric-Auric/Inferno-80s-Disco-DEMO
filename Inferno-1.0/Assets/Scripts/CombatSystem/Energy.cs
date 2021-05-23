using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Energy : MonoBehaviour
{
    public float maxEnergy = 100f;

    SpriteRenderer sr;
    public float currentEnergy;

    [Header("Hit Parameters")]
    [SerializeField] Color hitColor;
    [SerializeField] Vector3 screenShake;
    [SerializeField] float timeofSecurity; // If object is hitten you can't hit it again until a time amount has finished
    [SerializeField] float timeofColorChange;

    [Header("Points Drop")]
    public int normalDropNumber = 1;
    [SerializeField] float dropForce = 10f;
    public float normalGain = 1f;
    [SerializeField] float highlightMax;
    [SerializeField] float highlightDuration;
    [SerializeField] float bigPointNumber;
    public float bigGain;

    Light2D light1;
    Transform pointsContainer;
    Transform bigPointsContainer;
    int originalLayer;
    ScreenShake SS;

    bool isRunning = false;
    public bool isDead = false;

    private void OnEnable()
    {
        isDead = false;
        isRunning = false;
    }
    void Start()
    {
        light1 = transform.Find("Highlight").GetComponent<Light2D>();
        currentEnergy = maxEnergy;
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = GetComponentInChildren<SpriteRenderer>(); // Only the player hasn't sprite renderer on main object so i'm doing this to get the sprite, HOWEVER THE ORDER OF SPRITE IN CHILDREN SHOULD NOT CHANGE
        }
        SS = FindObjectOfType<ScreenShake>();
        pointsContainer = GameObject.Find("Points Container").transform;
        bigPointsContainer = GameObject.Find("Big Points Container").transform;



    }
    public void onHit(float damage, Transform target)
    {
        SS.CameraTwerk(screenShake.x, screenShake.y, screenShake.z);
        currentEnergy -= damage;
        sr.color = hitColor;
        if (currentEnergy <= 0 && !isDead)
        {
            bigPointDrop(target);
            isDead = true;
        }
        else if (currentEnergy >0)
        {
            dropPoint(target);
        }
        StartCoroutine(colorChange(timeofColorChange));
        if (timeofSecurity > 0)
        {
            originalLayer = gameObject.layer;
            gameObject.layer = LayerMask.NameToLayer("secure");
            StartCoroutine(sec(timeofSecurity));
        }
    }
    public void dropPoint(Transform target)
    {
       for (int i = 0; i < normalDropNumber; i++)
        {
            if (pointsContainer.childCount > 0)
            {
                var point = pointsContainer.GetChild(0).gameObject;
                var pointRb = point.GetComponent<Rigidbody2D>();
                point.transform.parent = null;
                point.SetActive(true);
                point.transform.position = this.transform.position;
                var vec = Random.insideUnitCircle;
                pointRb.AddForce(vec * dropForce, ForceMode2D.Impulse) ;
                var drop = point.GetComponent<dropPoints>();
                drop.target = target;
                drop.gainPoints = normalGain;
            }
            else break;
        }
    }
    public void bigPointDrop(Transform target)
    {
        for (int i = 0; i < bigPointNumber; i++)
        {
            if (bigPointsContainer.childCount > 0)
            {
                var point = bigPointsContainer.GetChild(0).gameObject;
                var pointRb = point.GetComponent<Rigidbody2D>();
                point.SetActive(true);
                point.transform.parent = null;
                point.transform.position = this.transform.position;
                var vec = Random.insideUnitCircle;
                pointRb.AddForce(vec * dropForce, ForceMode2D.Impulse);
                var drop = point.GetComponent<dropPoints>();
                drop.target = target;
                drop.gainPoints = bigGain;
            }
            else break;
        }
    }
    public void high()
    {
        if (!isRunning)
        {
            StartCoroutine(Highlight());  //I'M USING THIS BECAUSE DISABLING A SCRIPT FROM WHERE COROUTINE  IS CALLED STOP AUTOMATICALLY THAT COROUTINE
        }
    }

    IEnumerator sec(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.layer = originalLayer;
    }
    IEnumerator colorChange(float time)
    {
        yield return new WaitForSeconds(time);
        sr.color = Color.white;
    }
    public IEnumerator Highlight()
    {
        isRunning = true;
            while (light1.intensity < highlightMax)
            {
                light1.intensity += Time.deltaTime * highlightDuration;

                yield return new WaitForEndOfFrame();
            }
            while (light1.intensity != 0)
            {
                var newInt = light1.intensity - Time.deltaTime * highlightDuration;
                 light1.intensity = Mathf.Max(0, newInt);
                if (light1.intensity < 0.02)
                {
                light1.intensity = 0;
                }
                

                yield return new WaitForEndOfFrame();
            }
        isRunning = false;
    }
}
