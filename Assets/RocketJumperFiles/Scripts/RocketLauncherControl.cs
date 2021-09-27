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

    [Header("Firing Rocket")]
    [SerializeField] private GameObject rocketToBeFired;
    [SerializeField] private Vector2 rocketSpawnOffset;

    [Header("Rocket")]
    public float rocketSpeed;
    public float[] forces;
    [System.NonSerialized]
    public float forceToApply;
    public float explosionRadius;
    public float lengthOfDetectionRay;
    public float sizeOfCast;
    public float upwardsModifier;
    public LayerMask layerToCollideWith;
    public LayerMask ExplosionCanHit;
    [SerializeField] private float maxHoldTime;
    private float currentHoldTime;
    private float timePerIncreaseForce;
    private int currentSelectedForce;

    // Start is called before the first frame update
    void Start()
    {
        rightShoulderInitialAngle = rightShoulder.eulerAngles.z;
        leftShoulderInitialAngle = leftShoulder.eulerAngles.z;
        leftShoulderAngleDiff = rightShoulderInitialAngle - leftShoulderInitialAngle;
        timePerIncreaseForce = maxHoldTime / forces.Length;
        Debug.Log(timePerIncreaseForce);
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
        if (Input.GetMouseButton(0) && currentHoldTime < maxHoldTime)
        {
            currentHoldTime += Time.deltaTime;
            if (currentHoldTime > timePerIncreaseForce * (currentSelectedForce + 1))
                currentSelectedForce += 1;
        }
        else if (Input.GetMouseButtonUp(0) || currentHoldTime >= maxHoldTime)
        {
            if (currentSelectedForce >= forces.Length)
                currentSelectedForce = forces.Length - 1;
            forceToApply = forces[currentSelectedForce];
            Debug.Log(currentSelectedForce);
            Instantiate(rocketToBeFired, rocket.TransformPoint((Vector3) rocketSpawnOffset), rocket.rotation);
            currentHoldTime = 0;
            currentSelectedForce = 0;
        }
    }
}
