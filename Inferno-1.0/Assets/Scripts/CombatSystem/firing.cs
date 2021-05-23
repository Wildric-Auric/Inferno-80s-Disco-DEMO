using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class firing : MonoBehaviour
{
    CustomInputs CI;
    Transform container;
    ScreenShake SS;
    AudioManager AM;

    Transform fireOrigin;
    [FormerlySerializedAs("distanceFromPlayer")] 
    [SerializeField] float maxDis;
    [SerializeField] float dis = 1f;
    [SerializeField] int maxBullets = 10;
    public float deadTime = 0.1f; //Time Between bullets;
    public float bulletSpeed;
    public float gravityScale;
    public int cadence = 1; //How many bullet in each shot;
    [Header("For random shots")]
    public float offsetX;
    public float offsetY;

    [Header("Screen Shake parameters")]
    [SerializeField] float shakeMagnitude = 0.2f;
    [SerializeField] float shakeDuration = .01f;
    [SerializeField] float shakeFrequency = 2f;

    [Header("Bullet parameters")]
    [SerializeField] Sprite originalBulletSprite;
    public float bulletDamage;
    public float bulletCost = 5f;

    Vector3 initialPosition;
    bool canShoot = true;
    Vector3 scale;

    public bool onFire;
    [HideInInspector] public Vector3 target;
    Transform player; //To prevent searching for it at each frame of update, in the case when this script is related to enemy

    private void OnEnable()
    {
        canShoot = true;
    }
    void Start()
    {
        CI = FindObjectOfType<CustomInputs>();
        SS = FindObjectOfType<ScreenShake>();
        AM = FindObjectOfType<AudioManager>();

        fireOrigin = transform.Find("FireOrigin").transform;

        //Clone the bullet
         container = fireOrigin.Find("BulletContainer");
        var bullet = container.transform.Find("Bullet");
        bullet.gameObject.GetComponent<bullet>().parent = container;
        initialPosition = bullet.transform.position;
        for (int i = 0; i < maxBullets; i++)
        {
            var temp = Instantiate(bullet, container, true);
        }

        scale = fireOrigin.transform.localScale;
            
    }

    void Update()
    {
        //Prevent Problem when player is scaled
        //fireOrigin.localScale = new Vector3(scale.x * Mathf.Sign(transform.localScale.x), scale.y, scale.z);
        //Don't need this anymore I think, I changed scaling player to flip it's sprite rendrerer
        
        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            onFire = CI.Fire;
        }

        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            target = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        }

        fireOrigin.position = transform.position +  (target - transform.position).normalized * dis;
        Quaternion a = (Vector3.Cross(fireOrigin.localPosition, Vector3.right).z) < 0 ?
        fireOrigin.rotation = Quaternion.Euler(0, 0, Vector3.Angle(fireOrigin.localPosition, Vector3.right)) :
        fireOrigin.rotation = Quaternion.Euler(0, 0, -Vector3.Angle(fireOrigin.localPosition, Vector3.right));


        if (onFire && canShoot && container.childCount >= cadence && cadence > 0)
        {
            List<Transform> bullets = new List<Transform>(0);
            if (container.childCount >= cadence)
            {
                for (int i = 0; i < cadence; i++)
                {
                    bullets.Add(container.GetChild(i));
                }
            }
            
            foreach (Transform bul in bullets)
            {
                bul.GetComponent<SpriteRenderer>().sprite = originalBulletSprite;
                var bulletBehaviour = bul.GetComponent<bullet>();
                bulletBehaviour.initialPos = transform.position;
                bulletBehaviour.maxDis = maxDis;
                bul.gameObject.SetActive(true);
                bul.SetParent(null);
                var offset = new Vector3(UnityEngine.Random.Range(-offsetX, offsetX), UnityEngine.Random.Range(-offsetY, offsetY));
                bulletBehaviour.launch(bulletSpeed, ((target - transform.position).normalized + offset).normalized, gravityScale, bulletCost, bulletDamage);
                
            }
            canShoot = false;
            StartCoroutine(shoot(deadTime));
            
            //Some effects with this
            SS.CameraTwerk(shakeMagnitude, shakeDuration, shakeFrequency);
            AM.PlaySoundWithpitch("shot", UnityEngine.Random.Range(0.7f, 1.3f), 1f);
        }
    }
    IEnumerator shoot(float time)
    {
        yield return new WaitForSeconds(time);
        canShoot = true;
        
    }
}
