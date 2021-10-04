using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public Rigidbody2D myRB2D;
    [Header("Basic Stats")]
    public int health;
    public int movementSpeed;

    [Header("Forward Detection")]
    public Vector2 forwardDetectionOrigin;
    public float changeMovingDirectionDtectionRange;
    public LayerMask forwardDetectionLayermasks;

    [Header("Player Detection")]
    public float detectionRange;
    public LayerMask playerMask;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.GetChild(0).TransformPoint(forwardDetectionOrigin), transform.GetChild(0).right * changeMovingDirectionDtectionRange);

        Gizmos.DrawWireSphere(transform.GetChild(0).position, detectionRange);
    }
}
