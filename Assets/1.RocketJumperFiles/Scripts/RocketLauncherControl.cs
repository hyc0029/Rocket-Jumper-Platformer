using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherControl : MonoBehaviour
{
    [Header("Rocket Aimming")]
    [SerializeField] private Transform rightShoulder;
    [SerializeField] private Transform leftShoulder;
    [SerializeField] private Transform rocket;
    private float rightShoulderInitialAngle;
    private float leftShoulderInitialAngle;
    private float leftShoulderAngleDiff;

    [Header("Rocket Charge")]
    [SerializeField] private float maxHoldTime;
    [SerializeField] private UnityEngine.UI.Image chargeImg;
    [SerializeField] private ParticleSystem chargingRocketParticle;
    [SerializeField] private AudioSource chargingRocketSound;
    private float currentHoldTime;
    private float timePerIncreaseForce;
    private int currentSelectedForce;

    [Header("Firing Rocket")]
    [SerializeField] private GameObject rocketToBeFired;
    [SerializeField] private Vector2 rocketSpawnOffset;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private AudioSource fireRocketSound;
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
    public LayerMask ExplosionCanHit;
    [System.NonSerialized]public float forceToApply;

    // Start is called before the first frame update
    void Start()
    {
        rightShoulderInitialAngle = rightShoulder.eulerAngles.z;
        leftShoulderInitialAngle = leftShoulder.eulerAngles.z;
        leftShoulderAngleDiff = rightShoulderInitialAngle - leftShoulderInitialAngle;
        timePerIncreaseForce = maxHoldTime / forces.Length;
        chargingRocketParticle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        rocketAiming();
        firingRocket();
    }

    private void rocketAiming()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 vectorToTarget = mousePos - (Vector2)rocket.position;

        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + rightShoulderInitialAngle;

        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        rightShoulder.rotation = Quaternion.Slerp(rightShoulder.rotation, rot, 1);

        float leftAngle = angle - leftShoulderAngleDiff;

        Quaternion lRot = Quaternion.AngleAxis(leftAngle, Vector3.forward);

        leftShoulder.rotation = Quaternion.Slerp(leftShoulder.rotation, lRot, 1);
    }

    private void firingRocket()
    {
        if (canFireRocket)
        {
            if (Input.GetMouseButton(0) && currentHoldTime < maxHoldTime)
            {
                chargingRocketParticle.Play();
                chargeImg.fillAmount = currentHoldTime / maxHoldTime;
                currentHoldTime += Time.deltaTime;
                if (currentHoldTime > timePerIncreaseForce * (currentSelectedForce + 1))
                    currentSelectedForce += 1;
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
                forceToApply = forces[currentSelectedForce];
                Instantiate(rocketToBeFired, rocket.TransformPoint((Vector3)rocketSpawnOffset), rocket.rotation);
                currentHoldTime = 0;
                currentSelectedForce = 0;
                chargeImg.fillAmount = 0;
                timeBetweenShotsTimer = 0;
                chargingRocketParticle.Stop();
                canFireRocket = false;
            }
        }
        else
        {
            if (timeBetweenShotsTimer < timeBetweenShots)
                timeBetweenShotsTimer += Time.deltaTime;
            else
                canFireRocket = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(rocket.TransformPoint((Vector3)rocketSpawnOffset), sizeOfCast);
    }
}
