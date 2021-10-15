using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public Rigidbody2D myRB2D;
    public Animator myAnimator;
    public Collider2D myCol;

    [Header("Basic Stats")]
    public int health;
    public int movementSpeed;

    [Header("Ground Check")]
    public Vector2 groundCheckPosition;
    public Vector2 groundCheckSize;
    public LayerMask groundMask;

    [Header("Forward Detection")]
    public Vector2 forwardDetectionOrigin;
    public float changeMovingDirectionDtectionRange;
    public Vector2 forwardGroundDetectionOrigin;
    public float forwardGroundDetectionRange;
    public LayerMask forwardDetectionLayermasks;
    public LayerMask downwardDetectionLayerMasks;

    [Header("Player Detection")]
    public Transform myEyeTrans;
    public float detectionRange;
    public LayerMask playerMask;
    public bool haveDetectedPlayer;

    [Header("Attack Player")]
    public float attackRange;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.GetChild(0).TransformPoint(forwardDetectionOrigin), transform.GetChild(0).right * changeMovingDirectionDtectionRange);

        Gizmos.DrawWireSphere(transform.GetChild(0).position, detectionRange);

        Gizmos.DrawRay(transform.GetChild(0).TransformPoint(forwardGroundDetectionOrigin), -transform.GetChild(0).up * forwardGroundDetectionRange);

        Gizmos.DrawWireCube(transform.GetChild(0).TransformPoint(groundCheckPosition), groundCheckSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.GetChild(0).position, attackRange);


    }
}
