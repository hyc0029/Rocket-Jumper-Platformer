using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableEnvironment : MonoBehaviour
{
    [SerializeField] private Collider2D col;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem destructionParticle;

    [Header("Camera Shake")]
    [SerializeField] private float magnitude;
    [SerializeField] private float duration;

    public void destroyEnvrionment()
    {
        col.enabled = false;
        spriteRenderer.enabled = false;
        destructionParticle.Play();
        StartCoroutine(CameraShake());
    }

    IEnumerator CameraShake()
    {
        float timer = 0;
        Camera cam = FindObjectOfType<Camera>();
        float x = Random.Range(-1f,1f) * magnitude;
        float y = Random.Range(-1f, 1f) * magnitude;
        while (timer < duration)
        {
            if (x >= 0)
                x = Random.Range(0f, 1f) * magnitude * -1;
            else
                x = Random.Range(0f, 1f) * magnitude;

            if (y >= 0)
                y = Random.Range(0f, 1f) * magnitude * -1;
            else
                y = Random.Range(0f, 1f) * magnitude;

            Vector3 newPos = new Vector3(cam.transform.position.x + x, cam.transform.position.y + y, cam.transform.position.z);

            cam.transform.position = newPos;
            timer += Time.deltaTime;
            yield return null;
        }
    }

}
