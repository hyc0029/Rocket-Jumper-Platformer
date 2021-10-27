using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherControl : MonoBehaviour
{
    private Transform bodyRot;

    [Header("Rocket Aimming")]
    [SerializeField] private Transform rightShoulder;
    [SerializeField] private Transform leftShoulder;
    public Transform rocket;
    private float rightShoulderInitialAngle;
    private float leftShoulderInitialAngle;
    private float leftShoulderAngleDiff;

    [Header("Rocket Charge")]
    [SerializeField] private float maxHoldTime;
    [SerializeField] private UnityEngine.UI.Image chargeImg;
    [SerializeField] private ParticleSystem chargingRocketParticle;
    [SerializeField] private AudioSource chargingRocketSound;
    [SerializeField] private AudioSource fullyChargeSound;
    [SerializeField] private SpriteRenderer LauncherSR;
    [SerializeField] private Color newClr;
    private Color originalClr;
    [SerializeField] private Color newClrChargeBar;
    private Color flashColor;
    private Color originalClrChargeBar;
    [SerializeField] private AudioSource cantFireClick;
    private float currentHoldTime;
    private float timePerIncreaseForce;
    private int currentSelectedForce;
    private float origialFillAmount;

    [Header("Firing Rocket")]
    [SerializeField] private GameObject rocketToBeFired;
    [SerializeField] private Vector2 rocketSpawnOffset;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float additionalTimeBetweenShots;
    [SerializeField] private AudioSource fireRocketSound;
    private float actualTimeBetweenShots;
    private float timeBetweenShotsTimer;
    private bool canFireRocket = true;

    [Header("Rocket")]
    public float rocketSpeed;
    public LayerMask layerToCollideWith;

    [Header("Rocket Hit Detection")]
    public float lengthOfDetectionRay;
    public float sizeOfCast;

    [Header("Rocket Explosion")]
    public float[] forces;
    public float explosionRadius;
    public float upwardsModifier;
    public float WalledAngleUpwardModifier;
    public Vector2 playerCenterOffset;
    public float wallJumpModifier;
    public LayerMask ExplosionCanHit;
    [System.NonSerialized]public float forceToApply;

    // Start is called before the first frame update
    void Start()
    {
        bodyRot = transform.GetChild(0);
        originalClr = LauncherSR.material.GetColor("_EmissionColor");
        rightShoulderInitialAngle = rightShoulder.eulerAngles.z;
        leftShoulderInitialAngle = leftShoulder.eulerAngles.z;
        leftShoulderAngleDiff = rightShoulderInitialAngle - leftShoulderInitialAngle;
        timePerIncreaseForce = maxHoldTime / (forces.Length-1);
        chargingRocketParticle.Stop();
        originalClrChargeBar = chargeImg.color;
        flashColor = newClrChargeBar;
        flashColor.a = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            rocketAiming();
            firingRocket();
        }
    }

    private void rocketAiming()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 vectorToTarget = mousePos - (Vector2)rocket.position;

        float angle;

        if(bodyRot.localEulerAngles.y == 0)
            angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + rightShoulderInitialAngle;
        else
            angle = (Mathf.Atan2(vectorToTarget.x, vectorToTarget.y) * Mathf.Rad2Deg) + 180;

        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        rightShoulder.localRotation = Quaternion.Slerp(rightShoulder.rotation, rot, 1);

        float leftAngle = angle - leftShoulderAngleDiff;

        Quaternion lRot = Quaternion.AngleAxis(leftAngle, Vector3.forward);

        leftShoulder.localRotation = Quaternion.Slerp(leftShoulder.rotation, lRot, 1);
    }

    private void firingRocket()
    {
        if (canFireRocket)
        {
            chargeImg.color = originalClrChargeBar;
            if (Input.GetMouseButton(0) && currentHoldTime < maxHoldTime)
            {
                currentHoldTime += Time.deltaTime;
                if (currentHoldTime > timePerIncreaseForce * Mathf.Clamp((currentSelectedForce + 1), 0, forces.Length))
                    currentSelectedForce += 1;
                if (currentHoldTime >= maxHoldTime)
                {
                    chargeImg.fillAmount = 1;
                    chargingRocketParticle.Stop();
                    if(!fullyChargeSound.isPlaying)
                        fullyChargeSound.Play();
                }
                else
                {
                    chargingRocketParticle.Play();
                    chargeImg.fillAmount = currentHoldTime / maxHoldTime;
                }
                if(!chargingRocketSound.isPlaying)
                    chargingRocketSound.Play();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (chargingRocketSound.isPlaying)
                    chargingRocketSound.Stop();
                fireRocketSound.Play();
                if (currentSelectedForce >= forces.Length)
                    currentSelectedForce = forces.Length - 1;
                actualTimeBetweenShots = timeBetweenShots + additionalTimeBetweenShots * currentSelectedForce;
                forceToApply = forces[currentSelectedForce];
                Instantiate(rocketToBeFired, rocket.TransformPoint((Vector3)rocketSpawnOffset), rocket.rotation);
                currentHoldTime = 0;
                currentSelectedForce = 0;
                timeBetweenShotsTimer = 0;
                LauncherSR.material.SetColor("_EmissionColor", newClr);
                origialFillAmount = chargeImg.fillAmount;
                chargingRocketParticle.Stop();
                canFireRocket = false;
                chargeImg.color = newClrChargeBar;
            }
        }
        else
        {
            chargeImg.fillAmount = Mathf.Lerp(origialFillAmount, 0, timeBetweenShotsTimer / actualTimeBetweenShots);
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(chargeBarFlash());
            }
            if (timeBetweenShotsTimer < actualTimeBetweenShots)
                timeBetweenShotsTimer += Time.deltaTime;
            else
            {
                canFireRocket = true;
                LauncherSR.material.SetColor("_EmissionColor", originalClr);
            }

            if (Input.GetMouseButtonDown(0))
            {
                cantFireClick.Play();
            }
        }
    }

    public void resetRLC()
    {
        timeBetweenShotsTimer = 0;
        chargeImg.fillAmount = 0;
        currentHoldTime = 0;
        currentSelectedForce = 0;
    }

    IEnumerator chargeBarFlash()
    {
        chargeImg.color = flashColor;
        yield return new WaitForSeconds(0.1f);
        chargeImg.color = newClrChargeBar;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(rocket.TransformPoint((Vector3)rocketSpawnOffset), sizeOfCast);

        Gizmos.DrawWireSphere(transform.position + (Vector3)playerCenterOffset, 0.5f);

        Gizmos.DrawWireSphere(transform.position + Vector3.up * wallJumpModifier, 0.5f);

    }
}
