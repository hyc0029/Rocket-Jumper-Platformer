using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAi : BaseAi
{
    private EnemyInfo myInfo;

    [Header("Aiming")]
    [SerializeField] private Transform myArm;
    [SerializeField] private float aimTime = 1f;
    private float aimTimer;
    private bool attacking;

    [Header("Firing")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Vector2 projectileOffset;
    [SerializeField] private float delayAfterFiring;
    private float postFiringTimer;

    [Header("Projectile")]
    public float projectileSpeed;

    private void Start()
    {
        myInfo = GetComponent<EnemyInfo>();
    }


    // Update is called once per frame
    void Update()
    {
        if (isAlive(myInfo.health))
        {
            if (!detectPlayer(transform.GetChild(0), myInfo.myEyeTrans, myInfo.detectionRange, myInfo.playerMask) && !myInfo.haveDetectedPlayer)
                patrolling();
            else
            {
                myInfo.haveDetectedPlayer = true;
                if (detectPlayer(transform.GetChild(0), myInfo.myEyeTrans, myInfo.attackRange, myInfo.playerMask))
                {
                    if (!attacking)
                        AimAtPlayer();
                    else
                        postFiring();
                }
                else
                {
                    moveTowardPlayer(myInfo.myRB2D, FindObjectOfType<CharacterController>().transform, myInfo.movementSpeed);
                }
            }
        }
        else
        {
            Dead(myInfo.myRB2D, myInfo.myAnimator, myInfo.myCol, "Dead");
            if (groundCheck(transform.GetChild(0), myInfo.groundCheckPosition, myInfo.groundCheckSize, myInfo.groundMask))
            {
                myInfo.myRB2D.gravityScale = 0;
                myInfo.myRB2D.velocity = Vector2.zero;
            }
            else
                myInfo.myRB2D.gravityScale = 1;
        }
    }

    void patrolling()
    {
        if (checkForward(transform.GetChild(0).transform, myInfo.forwardDetectionOrigin, myInfo.changeMovingDirectionDtectionRange, myInfo.forwardDetectionLayermasks) || checkForwardDown(transform.GetChild(0).transform, myInfo.forwardGroundDetectionOrigin, myInfo.forwardGroundDetectionRange, myInfo.forwardDetectionLayermasks))
            transform.GetChild(0).transform.Rotate(new Vector3(0, 180, 0));
        basicMovement(myInfo.myRB2D, transform.GetChild(0).transform, myInfo.movementSpeed);
    }

    void AimAtPlayer()
    {

        Vector2 playerPos = (Vector2)FindObjectOfType<CharacterController>().transform.position;

        playerPos.y += 5;

        Vector2 vectorToTarget = playerPos - (Vector2)myArm.position;
        float angle;

        if (Mathf.Round(transform.GetChild(0).localEulerAngles.y) == 0)
            angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + 90;
        else
            angle = (Mathf.Atan2(vectorToTarget.x, vectorToTarget.y) * Mathf.Rad2Deg) + 180;

        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        myArm.localRotation = Quaternion.Slerp(myArm.rotation, rot, 1);
        aimTimer += Time.deltaTime;

        if (aimTimer > aimTime)
            ShootAtPlayer();
    }

    void ShootAtPlayer()
    {
        if (!attacking)
        {
            RobotEnergyBall thisEnergyBall =  Instantiate(projectile, myArm.TransformPoint(projectileOffset), myArm.rotation * Quaternion.AngleAxis(-90, Vector3.forward)).GetComponent<RobotEnergyBall>();
            thisEnergyBall.myRobot = this;
            thisEnergyBall.gameObject.SetActive(true);
            postFiringTimer = 0;
            attacking = true;
        }
    }

    void postFiring()
    {
        if (postFiringTimer < delayAfterFiring)
            postFiringTimer += Time.deltaTime;
        else
        {
            aimTimer = 0;
            attacking = false;
            Debug.Log("Reseted");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(myArm.transform.position, (Vector2)FindObjectOfType<CharacterController>().transform.position - (Vector2)myArm.position);

        Gizmos.DrawWireSphere(myArm.TransformPoint(projectileOffset), 1);
    }

}
