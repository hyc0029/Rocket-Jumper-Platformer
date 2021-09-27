using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody2D myRB2D;
    RocketLauncherControl rlc;
    protected ContactFilter2D cf;
    Vector2 explosionPoint;
    bool exploded;
    //[SerializeField] private LayerMask layersToDetect;
    // Start is called before the first frame update
    void Start()
    {
        myRB2D = GetComponent<Rigidbody2D>();
        rlc = FindObjectOfType<RocketLauncherControl>();
        myRB2D.velocity = transform.right * rlc.rocketSpeed;
        cf.useLayerMask = true;
        cf.SetLayerMask(rlc.layerToCollideWith);
    }

    // Update is called once per frame
    void Update()
    {
        if(!exploded)
            RocketHitDetection();
    }

    void RocketHitDetection()
    {
        List<RaycastHit2D> circleHits = new List<RaycastHit2D>();
        Physics2D.CircleCast(transform.position, rlc.sizeOfCast, transform.right, cf, circleHits, rlc.lengthOfDetectionRay);
        if (circleHits.Count > 0)
        {
            explosionPoint = circleHits[0].point;
            RocketExplosion();
        }
    }

    void RocketExplosion()
    {
        Collider2D[] CanBeHit = Physics2D.OverlapCircleAll(explosionPoint, rlc.explosionRadius, rlc.ExplosionCanHit);
        myRB2D.velocity = Vector2.zero;
        if (CanBeHit.Length > 0)
        {
            foreach (Collider2D col in CanBeHit)
            {
                if (col.GetComponent<Rigidbody2D>() != null)
                {
                    Rigidbody2D thisRB2D = col.GetComponent<Rigidbody2D>();
                    Vector2 explosionDir = thisRB2D.position - explosionPoint;
                    float explosionDistance = explosionDir.magnitude;

                    if (rlc.upwardsModifier == 0)
                        explosionDir /= explosionDistance;
                    else
                    {
                        explosionDir.y += rlc.upwardsModifier;
                        explosionDir.Normalize();
                    }

                    //Vector2 explosionForce = Mathf.Lerp(0, rlc.forceToApply, (rlc.explosionRadius - explosionDistance)) * explosionDir;
                    Vector2 explosionForce = rlc.forceToApply * explosionDir;
                    thisRB2D.drag = 0;
                    thisRB2D.AddForce(explosionForce, ForceMode2D.Impulse);
                }
            }
            exploded = true;
        }
    }

    private void OnDrawGizmos()
    {
        
        List<RaycastHit2D> circleHits = new List<RaycastHit2D>();
        Physics2D.CircleCast(transform.position, rlc.sizeOfCast, transform.right, cf, circleHits, rlc.lengthOfDetectionRay);
        Gizmos.color = Color.green;
        if (circleHits.Count > 0)
        {
            Gizmos.DrawLine(transform.position, circleHits[0].point);
            Gizmos.DrawWireSphere(transform.position + transform.right * (circleHits[0].point - (Vector2)transform.position).magnitude, rlc.sizeOfCast);
            Gizmos.DrawWireSphere(explosionPoint, rlc.explosionRadius);
        }
    }


}
