using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAi : BaseAi
{
    private EnemyInfo myInfo;

    private void Start()
    {
        myInfo = GetComponent<EnemyInfo>();
    }


    // Update is called once per frame
    void Update()
    {
        if (isAlive(myInfo.health))
        {
            if (!detectPlayer(transform.GetChild(0), myInfo.detectionRange, myInfo.playerMask) && !myInfo.haveDetectedPlayer)
                patrolling();
            else
            {
                myInfo.haveDetectedPlayer = true;
                if (detectPlayer(transform.GetChild(0), myInfo.attackRange, myInfo.playerMask))
                {

                }
                else
                {
                    moveTowardPlayer(myInfo.myRB2D, FindObjectOfType<CharacterController>().transform, myInfo.movementSpeed);
                }
            }
        }
        else
        {
            myInfo.myRB2D.velocity = Vector3.zero;
            Dead(myInfo.myRB2D, myInfo.myAnimator, myInfo.myCol, "Dead");
        }
    }

    void patrolling()
    {
        if (checkForward(transform.GetChild(0).transform, myInfo.forwardDetectionOrigin, myInfo.changeMovingDirectionDtectionRange, myInfo.forwardDetectionLayermasks) || checkForwardDown(transform.GetChild(0).transform, myInfo.forwardGroundDetectionOrigin, myInfo.forwardGroundDetectionRange, myInfo.forwardDetectionLayermasks))
            transform.GetChild(0).transform.Rotate(new Vector3(0, 180, 0));
        basicMovement(myInfo.myRB2D, transform.GetChild(0).transform, myInfo.movementSpeed);
    }
}
