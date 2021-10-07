using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAi : BaseAi
{
    private EnemyInfo myInfo;
    [Header("Attack")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float upwardModifier;
    private bool attacking = false;

    [Header("Call All Nearby Dogs")]
    [SerializeField] private float callOtherDogsRange;
    [SerializeField] private LayerMask enemies;
    [System.NonSerialized] public bool calledByDog;

    private void Start()
    {
        myInfo = GetComponent<EnemyInfo>();
    }


    // Update is called once per frame
    void Update()
    {
        if (isAlive(myInfo.health))
        {
            if (!detectPlayer(transform.GetChild(0), myInfo.detectionRange, myInfo.playerMask) && !myInfo.haveDetectedPlayer && !calledByDog)
                patrolling();
            else
            {
                ActivateAllNearbyDogs();
                myInfo.haveDetectedPlayer = true;
                myInfo.myAnimator.SetBool("Attack", attacking);
                if (detectPlayer(transform.GetChild(0), myInfo.attackRange, myInfo.playerMask))
                {
                    if (!attacking)
                        jumpToPlayer();
                }
                else
                {
                    moveTowardPlayer(myInfo.myRB2D, FindObjectOfType<CharacterController>().transform, myInfo.movementSpeed);
                }
                if (groundCheck(transform.GetChild(0), myInfo.groundCheckPosition, myInfo.groundCheckSize, myInfo.groundMask) && attacking)
                {
                    attacking = false;
                }
            }
        }
        else
        {
            Dead(myInfo.myRB2D, myInfo.myAnimator, myInfo.myCol, "Dead");
            ActivateAllNearbyDogs();
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

    void jumpToPlayer()
    {

        if (transform.GetChild(0).InverseTransformPoint(FindObjectOfType<CharacterController>().transform.position).x < 0)
            transform.GetChild(0).Rotate(new Vector3(0, 180, 0));

        myInfo.myRB2D.velocity = Vector2.zero;
        
        Vector2 jumpDir = FindObjectOfType<CharacterController>().transform.position - transform.position;
        jumpDir.y += upwardModifier;
        jumpDir.Normalize();

        myInfo.myRB2D.AddForce(jumpDir * jumpForce, ForceMode2D.Impulse);
        
        attacking = true;
    }

    void ActivateAllNearbyDogs()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, callOtherDogsRange, enemies);
        List<DogAi> dogs = new List<DogAi>();
        foreach (Collider2D col in cols)
        {
            if (col.GetComponent<DogAi>() && col.gameObject != this.gameObject)
            {
                dogs.Add(col.GetComponent<DogAi>());
            }
        }
        foreach (DogAi dog in dogs)
        {
            dog.calledByDog = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<CharacterController>() && attacking)
        {
            collision.transform.GetComponent<CharacterController>().playerDead();
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, callOtherDogsRange);
    }

}
