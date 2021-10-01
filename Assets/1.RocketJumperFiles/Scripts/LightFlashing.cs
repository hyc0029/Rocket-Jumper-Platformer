using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlashing : MonoBehaviour
{

    [SerializeField] Material materialToChange;
    [SerializeField] Color[] emissionColors = new Color[2];
    float randomFlashTime;
    float randomFlashTimer;
    float minRandomTime = 0.1f;
    float maxRandomTime = 1f;

    private void Start()
    {
        StartCoroutine(LightFlash());
    }

    IEnumerator LightFlash()
    {
        randomFlashTime = Random.Range(minRandomTime, maxRandomTime);
        while (true)
        {
            if (randomFlashTimer > randomFlashTime)
            {
                materialToChange.SetColor("_EmissionColor", emissionColors[1]);
                randomFlashTime = Random.Range(minRandomTime, maxRandomTime);
                randomFlashTimer = 0;
                yield return new WaitForSeconds(minRandomTime);
                materialToChange.SetColor("_EmissionColor", emissionColors[0]);
            }
            else
            {
                randomFlashTimer += Time.deltaTime;
            }
            yield return null;
        }
    }

}
