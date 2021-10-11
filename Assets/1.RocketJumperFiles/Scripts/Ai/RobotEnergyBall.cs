using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotEnergyBall : MonoBehaviour
{
    private ContactFilter2D cf; 

    public RobotAi myRobot;
    [SerializeField] private Rigidbody2D myRB2D;


    [Header("Hit Detection")]
    [SerializeField] private float sizeOfCast;
    [SerializeField] private LayerMask masks;

    Vector2 oldPos;
    // Start is called before the first frame update
    void Start()
    {
        myRB2D.velocity = transform.right * myRobot.projectileSpeed;
        cf.useLayerMask = true;
        cf.SetLayerMask(masks);
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        hitDetection();
    }

    void hitDetection()
    {
        List<RaycastHit2D> circleHits = new List<RaycastHit2D>();
        //Physics2D.CircleCast(transform.position - transform.right * rlc.lengthOfDetectionRay, rlc.sizeOfCast, transform.right, cf, circleHits, rlc.lengthOfDetectionRay);
        float lengthOfDetectionRay = Vector2.Distance(oldPos, transform.position);
        Physics2D.CircleCast(oldPos, sizeOfCast, transform.right, cf, circleHits, lengthOfDetectionRay);
        if (circleHits.Count > 0)
        {
            //explosionPoint = circleHits[0].point;
            //RocketExplosion();
            //RocketExplosionEffect();
            if (circleHits[0].transform.GetComponent<CharacterController>())
            {
                circleHits[0].transform.GetComponent<CharacterController>().playerDead();
            }
            Destroy(gameObject);
        }
        oldPos = transform.position;
    }
}
