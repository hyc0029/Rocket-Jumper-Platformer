using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvulnerable : MonoBehaviour
{
    [Header("Invulnerable Shield")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Vector2 shieldLocalPos;
    [SerializeField] private float shieldSize;
    [SerializeField] private SpriteRenderer shield;
    [HideInInspector] public bool shieldOn;

    [Header("Invulnerable Duration")]
    [SerializeField] private float invulnerableDuration;
    [HideInInspector] public float invulnerableTimer;
    private float secondsLeftToBlink = 1;

    [HideInInspector] public bool blinking;

    // Start is called before the first frame update
    void Start()
    {
        shield.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shieldOn)
        {
            if (gameObject.layer == LayerMask.NameToLayer("player"))
            {
                gameObject.layer = LayerMask.NameToLayer("PlayerInvulnerable");
            }

            if (gameObject.layer == LayerMask.NameToLayer("PlayerInvulnerable"))
            {
                if (invulnerableTimer > invulnerableDuration)
                {
                    shield.enabled = false;
                    gameObject.layer = LayerMask.NameToLayer("player");
                    invulnerableTimer = 0;
                    shieldOn = false;
                    blinking = false;
                }
                else
                    invulnerableTimer += Time.deltaTime;

                if (invulnerableDuration - invulnerableTimer <= secondsLeftToBlink && !blinking)
                {
                    StartCoroutine(shieldBlink());
                }

                if (!blinking)
                {
                    shield.enabled = true;
                }
            }
        }
    }

    IEnumerator shieldBlink()
    {
        blinking = true;
        while (blinking)
        {
            shield.enabled = false;
            yield return new WaitForSeconds(0.15f);
            shield.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }
        shield.enabled = false;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.TransformPoint(shieldLocalPos), shieldSize);
    }

}
