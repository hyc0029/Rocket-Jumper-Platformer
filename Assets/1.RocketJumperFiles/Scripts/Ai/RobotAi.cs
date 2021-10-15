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

    [Header("Projectile")]
    public float projectileSpeed;

    bool blinking;

    private void Start()
    {
        myInfo = GetComponent<EnemyInfo>();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        if (isAlive(myInfo.health))
        {

            myInfo.myAnimator.SetFloat("xVelocity", Mathf.Abs(myInfo.myRB2D.velocity.x));

            if (FindObjectOfType<CharacterController>().myCol.enabled)
            {
                if (!detectPlayer(transform.GetChild(0), myInfo.myEyeTrans, myInfo.detectionRange, myInfo.playerMask) && !myInfo.haveDetectedPlayer)
                    patrolling();
                else
                {
                    myInfo.haveDetectedPlayer = true;
                    if (detectPlayer(transform.GetChild(0), myInfo.myEyeTrans, myInfo.attackRange, myInfo.playerMask))
                    {
                        AimAtPlayer();
                    }
                    else
                    {
                        if (groundCheck(transform.GetChild(0), myInfo.groundCheckPosition, myInfo.groundCheckSize, myInfo.groundMask))
                            moveTowardPlayer(myInfo.myRB2D, FindObjectOfType<CharacterController>().transform, myInfo.movementSpeed);
                    }
                }
            }
            else
            {
                patrolling();
            }
        }
        else
        {
            Dead(myInfo.myRB2D, myInfo.myAnimator, myInfo.myCol, "Dead");
            if (groundCheck(transform.GetChild(0), myInfo.groundCheckPosition, myInfo.groundCheckSize, myInfo.groundMask))
            {
                myInfo.myRB2D.gravityScale = 0;
                myInfo.myRB2D.velocity = Vector2.zero;
                if (!blinking)
                {
                    StartCoroutine(blinkDestroy(gameObject));
                    blinking = true;
                }
            }
            else
                myInfo.myRB2D.gravityScale = 1;
        }
    }

    void patrolling()
    {
        if (checkForward(transform.GetChild(0).transform, myInfo.forwardDetectionOrigin, myInfo.changeMovingDirectionDtectionRange, myInfo.forwardDetectionLayermasks) || checkForwardDown(transform.GetChild(0).transform, myInfo.forwardGroundDetectionOrigin, myInfo.forwardGroundDetectionRange, myInfo.downwardDetectionLayerMasks))
        {
            if (groundCheck(transform.GetChild(0), myInfo.groundCheckPosition, myInfo.groundCheckSize, myInfo.groundMask))
                transform.GetChild(0).transform.Rotate(new Vector3(0, 180, 0));
        }
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
        RobotEnergyBall thisEnergyBall =  Instantiate(projectile, myArm.TransformPoint(projectileOffset), myArm.rotation * Quaternion.AngleAxis(-90, Vector3.forward)).GetComponent<RobotEnergyBall>();
        thisEnergyBall.myRobot = this;
        thisEnergyBall.gameObject.SetActive(true);
        aimTimer = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<EnemyInfo>())
        {
            collision.transform.GetChild(0).transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(myArm.TransformPoint(projectileOffset), 1);
    }

}
