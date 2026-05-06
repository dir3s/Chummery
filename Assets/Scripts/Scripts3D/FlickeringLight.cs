using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    public Light lightSource;

    [Header("Intensity")]
    public float minIntensity = 0.2f;
    public float maxIntensity = 1.5f;

    [Header("Timing")]
    public float flickerChance = 0.2f; 
    public float minFlickerTime = 0.05f;
    public float maxFlickerTime = 0.2f;

    public float calmTime = 2f; 

    private void Start()
    {
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
     
            lightSource.intensity = maxIntensity;
            yield return new WaitForSeconds(Random.Range(1f, calmTime));

  
            if (Random.value < flickerChance)
            {
                float flickerDuration = Random.Range(0.5f, 2f);

                float t = 0f;

                while (t < flickerDuration)
                {
                    t += Time.deltaTime;

                    lightSource.intensity = Random.Range(minIntensity, maxIntensity);

                    yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));
                }
            }
        }
    }
}