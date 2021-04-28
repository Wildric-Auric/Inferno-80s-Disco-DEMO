using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropPoints : MonoBehaviour
{
    AudioManager AM;

    public Transform target;
    Rigidbody2D rb;
    [SerializeField] Transform parent;
    [Header("Force applied when going to the object that did damage")]
    [SerializeField] float attractingForce = 200f;
    [SerializeField] float power = -1;
    
    [HideInInspector] public float gainPoints;
    void Awake()
    {
        AM = FindObjectOfType<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        
    }
    void FixedUpdate()
    {
        if (target == null)
        {
            this.enabled = false;
        }
        var inverseDistance = Mathf.Pow((-transform.position + target.position).magnitude, power);
        rb.AddForce((-transform.position + target.position).normalized * attractingForce * inverseDistance * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetInstanceID() == target.gameObject.GetInstanceID())
        {
            var trans = transform;
            var targetEnergy = target.GetComponent<Energy>();
            trans.SetParent(parent);
            if (targetEnergy.maxEnergy > targetEnergy.currentEnergy) targetEnergy.currentEnergy = Mathf.Min(gainPoints + targetEnergy.currentEnergy, targetEnergy.maxEnergy);
            if (!AM.IsPlaying("EnergyGain")) AM.PlaySoundWithpitch("Energy Gain",Random.Range(0.7f,1.3f),1f);
            targetEnergy.high();
            trans.gameObject.SetActive(false);


        }
    }
}
