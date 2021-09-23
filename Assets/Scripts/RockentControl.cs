using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockentControl : MonoBehaviour
{

    [SerializeField] private Transform rightShoulder;
    [SerializeField] private Transform leftShoulder;
    private float rightShoulderInitialAngle;
    private float leftShoulderInitialAngle;
    private float leftShoulderAngleDiff;
    [SerializeField] private Transform rocket;

    // Start is called before the first frame update
    void Start()
    {
        rightShoulderInitialAngle = rightShoulder.eulerAngles.z;
        leftShoulderInitialAngle = leftShoulder.eulerAngles.z;
        leftShoulderAngleDiff = rightShoulderInitialAngle - leftShoulderInitialAngle;
    }

    // Update is called once per frame
    void Update()
    {
        rocketAiming();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rocket.position, rocket.right * 100);
    }

}
