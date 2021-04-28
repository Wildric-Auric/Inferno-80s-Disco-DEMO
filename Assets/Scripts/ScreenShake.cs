using Cinemachine;
using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    CinemachineBasicMultiChannelPerlin shake;
    float Duration;
    float Frequency;

   private void Start()
    {
      shake = FindObjectOfType<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
   public void CameraTwerk(float magnitude, float duration, float frequency)
    {
        shake.m_AmplitudeGain = magnitude;
        shake.m_FrequencyGain = frequency;
        Duration = duration;
        Frequency = frequency;
        StartCoroutine(shaking());
    }
    IEnumerator shaking()
    {
        yield return new WaitForSeconds(Duration);
        shake.m_AmplitudeGain = 0;
        shake.m_FrequencyGain = 0;
    }
}
