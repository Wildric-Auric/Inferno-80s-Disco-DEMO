using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Experimental.Rendering.Universal;

public class NeonFlickering : MonoBehaviour
{
    SpriteRenderer sr;
    Color InitialColor;
    Light2D lighting;
    [SerializeField] float InitialIntensity;
    [SerializeField] float InitialRadius;
    [SerializeField] float RadiusMagnitude; //Inner Radius
    [SerializeField] float RadiusFrequency;
    [Header("Sinusoidal variation Mode")]
    [SerializeField] float magnitude = 64f;
    [SerializeField] float intensityMagnitude = 0.3f;
    [SerializeField] float frequency = 1f ;
    [SerializeField] float randomOffset;
    [Header("SQUARE SIGNAL MODE")]
    [FormerlySerializedAs("TimeOfLittingOn")]
    [SerializeField] float frequency1;
    [FormerlySerializedAs("TimeOfLittingOff")]
    [SerializeField] float frequency2;
    [SerializeField] float minValueForLight;
    [SerializeField] float minValueForLamp;
    [SerializeField] float randomOffset1;
    [SerializeField] float randomOffset2;
    bool isOff;
    [Header("Choose Mode")]
    [SerializeField] bool SqaureMode;
    [SerializeField] bool SinusoidalMode;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        InitialColor = sr.color;
        lighting = GetComponentInChildren<Light2D>();
        InitialIntensity = lighting.intensity;
        InitialRadius = lighting.pointLightInnerRadius;
    }
    void Update()
    {
        if (SinusoidalMode)
        {
            var F = Time.time * 2.0f * Mathf.PI * (frequency + Random.Range(-randomOffset, randomOffset));
            var Formula = Time.time * Mathf.Sin(Mathf.Exp(Mathf.Cos(F)));
            sr.color = InitialColor + (new Color(Formula * magnitude, Formula * magnitude, Formula * magnitude, 0f));
            lighting.intensity = InitialIntensity + Formula * intensityMagnitude;
        }
        else if (SqaureMode && !isOff)
        {
            var F = (Mathf.Max(0,frequency1 + Random.Range(-randomOffset1, randomOffset1)));
            var F1 = (Mathf.Max(0, frequency2 + Random.Range(-randomOffset2, randomOffset2)));
            isOff = true;
            StartCoroutine(litOff(F,F1));
        }

        lighting.pointLightInnerRadius = InitialRadius + RadiusMagnitude*Mathf.Sin(Time.time * RadiusFrequency);
    }
    IEnumerator litOff(float freq, float freq1)
    {
        lighting.intensity = InitialIntensity;
        sr.color = InitialColor;
        yield return new WaitForSeconds(freq);
        lighting.intensity = minValueForLight;
        sr.color = new Color(minValueForLamp, minValueForLamp, minValueForLamp, 1f);
        yield return new WaitForSeconds(freq1);
        isOff = false;


    }
}
