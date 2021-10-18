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
    [SerializeField] ParticleSystem rocketParticleSystem;
    [SerializeField] ParticleSystem explosionParticleSystem;
    [SerializeField] AudioSource explosionSound;
    float maxVolume = 0.3f;
    float maxHearingDistance = 150;
    Vector2 oldPos;
    //[SerializeField] private LayerMask layersToDetect;
    // Start is called before the first frame update
    void Start()
    {
        myRB2D = GetComponent<Rigidbody2D>();
        rlc = FindObjectOfType<RocketLauncherControl>();
        myRB2D.velocity = transform.right * rlc.rocketSpeed;
        cf.useLayerMask = true;
        cf.SetLayerMask(rlc.layerToCollideWith);
        explosionParticleSystem.gameObject.SetActive(false);
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (rlc.GetComponent<CharacterController>().myCol.enabled)
        {
            if (!exploded)
                RocketHitDetection();
            else
            {
                if (!explosionParticleSystem.IsAlive())
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (!explosionParticleSystem.IsAlive())
                Destroy(gameObject);
        }
    }

    void RocketHitDetection()
    {
        List<RaycastHit2D> circleHits = new List<RaycastHit2D>();
        //Physics2D.CircleCast(transform.position - transform.right * rlc.lengthOfDetectionRay, rlc.sizeOfCast, transform.right, cf, circleHits, rlc.lengthOfDetectionRay);
        Physics2D.CircleCast(oldPos, rlc.sizeOfCast, transform.right, cf, circleHits, rlc.lengthOfDetectionRay*2);
        if (circleHits.Count > 0)
        {
            explosionPoint = circleHits[0].point;
            RocketExplosion();
            RocketExplosionEffect();
        }
        oldPos = transform.position;
    }

    void RocketExplosion()
    {
        Collider2D[] CanBeHit = Physics2D.OverlapCircleAll(explosionPoint, rlc.explosionRadius, rlc.ExplosionCanHit);
        myRB2D.velocity = Vector2.zero;
        if (CanBeHit.Length > 0)
        {
            foreach (Collider2D col in CanBeHit)
            {
                if (col.GetComponent<Rigidbody2D>() && !col.GetComponent<EnemyInfo>())
                {
                    CharacterController cc = col.GetComponent<CharacterController>();
                    Rigidbody2D thisRB2D = col.GetComponent<Rigidbody2D>();
                    Vector2 playerCenterOffset = Vector2.zero;
                    if (cc.walledCheck())
                    {
                        playerCenterOffset = Vector2.up * rlc.wallJumpModifier;
                    }
                    else
                    {
                        playerCenterOffset = rlc.playerCenterOffset;
                    }
                    Vector2 explosionDir = (thisRB2D.position + playerCenterOffset) - explosionPoint;
                    float explosionDistance = explosionDir.magnitude;
                    float explosionDot = Vector2.Dot(explosionDir, Vector3.up);

                    if (rlc.upwardsModifier == 0)
                        explosionDir /= explosionDistance;
                    else if (cc.walledCheck() && !cc.groundCheck() && explosionDot > 0 && explosionDot < 4)
                    {
                        explosionDir.y += rlc.WalledAngleUpwardModifier;
                        explosionDir.Normalize();
                    }
                    else
                    {
                        explosionDir.y += rlc.upwardsModifier;
                        explosionDir.Normalize();
                    }

                    if (cc.resetVerticalVelocityCheck())
                        thisRB2D.velocity = new Vector3(thisRB2D.velocity.x, 0, 0);
                    else if (cc.groundCheck())
                        thisRB2D.velocity = new Vector3(0, 0, 0);

                    //Vector2 explosionForce = Mathf.Lerp(0, rlc.forceToApply, (rlc.explosionRadius - explosionDistance)) * explosionDir;
                    Vector2 explosionForce = rlc.forceToApply * explosionDir;
                    thisRB2D.drag = 0;
                    thisRB2D.AddForce(explosionForce, ForceMode2D.Impulse);

                }
                else if (col.GetComponent<EnemyInfo>())
                {
                    col.GetComponent<EnemyInfo>().health -= 1;
                }
                else if (col.GetComponent<DestructableEnvironment>())
                {
                    col.GetComponent<DestructableEnvironment>().destroyEnvrionment();
                }
            }
            exploded = true;
        }
    }

    void RocketExplosionEffect()
    {
        explosionSound.volume = Mathf.Lerp(0, maxVolume, 1 - Mathf.Clamp(Vector2.Distance(rlc.gameObject.transform.position, explosionPoint) / maxHearingDistance, 0 , 1));
        explosionSound.Play();
        rocketParticleSystem.transform.parent.gameObject.SetActive(false);
        transform.position = explosionPoint;
        explosionParticleSystem.gameObject.SetActive(true);
        explosionParticleSystem.Play();
    }




    private void OnDrawGizmos()
    {
        if (rlc != null)
        {
            List<RaycastHit2D> circleHits = new List<RaycastHit2D>();
            Physics2D.CircleCast(transform.position - transform.right * rlc.lengthOfDetectionRay, rlc.sizeOfCast, transform.right, cf, circleHits, rlc.lengthOfDetectionRay);
            Gizmos.color = Color.green;
            if (circleHits.Count > 0)
            {
                Gizmos.DrawLine(transform.position - transform.right * rlc.lengthOfDetectionRay, circleHits[0].point);
                Gizmos.DrawWireSphere(transform.position + transform.right * (circleHits[0].point - (Vector2)transform.position).magnitude, rlc.sizeOfCast);
                Gizmos.DrawWireSphere(explosionPoint, rlc.explosionRadius);
            }
        }
    }


}
