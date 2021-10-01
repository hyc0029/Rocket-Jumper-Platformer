using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAi : BaseAi
{
    private EnemyInfo myInfo;

    private void Start()
    {
        myInfo = GetComponent<EnemyInfo>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!detectPlayer(transform.GetChild(0), myInfo.detectionRange, myInfo.playerMask))
            patrolling();
        else
            myInfo.myRB2D.velocity = new Vector3(0, myInfo.myRB2D.velocity.y, 0);
    }

    void patrolling()
    {
        if (checkForward(transform.GetChild(0).transform, myInfo.forwardDetectionOrigin, myInfo.changeMovingDirectionDtectionRange, myInfo.forwardDetectionLayermasks))
            transform.GetChild(0).transform.Rotate(new Vector3(0, 180, 0));
        basicMovement(myInfo.myRB2D, transform.GetChild(0).transform, myInfo.movementSpeed);
    }

}
