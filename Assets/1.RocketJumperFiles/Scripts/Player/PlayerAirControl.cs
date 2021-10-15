using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirControl : MonoBehaviour
{

    Rigidbody2D playerRb;
    CharacterController cc;
    [SerializeField] private float airControlForce;
    [SerializeField] private float groundedMoveSpeed;
    [SerializeField] private ParticleSystem forwardThrustParticle;
    [SerializeField] private ParticleSystem backwardThrustParticle;
    [SerializeField] private AudioSource thrustSound;

    float leftRight;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cc.myCol.enabled)
        {
            leftRight = Input.GetKey(KeyCode.D) ? 1 : 0 - (Input.GetKey(KeyCode.A) ? 1 : 0);

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                if (!thrustSound.isPlaying)
                    thrustSound.Play();
                if (leftRight > 0)
                {
                    forwardThrustParticle.Play();
                    backwardThrustParticle.Stop();
                }
                else if (leftRight < 0)
                {
                    backwardThrustParticle.Play();
                    forwardThrustParticle.Stop();
                }
            }
            if (leftRight == 0)
            {
                forwardThrustParticle.Stop();
                backwardThrustParticle.Stop();
                thrustSound.Stop();
            }
            if (cc.groundCheck() && leftRight != 0 && playerRb.velocity.magnitude < groundedMoveSpeed)
            {
                playerRb.AddForce(transform.right * leftRight * 1000);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!cc.groundCheck())
        {
            Vector2 velocity = playerRb.velocity;
            velocity.x += leftRight*((airControlForce + 0.1f * playerRb.velocity.x) * Time.fixedDeltaTime);
            playerRb.velocity = velocity;
        }
    }

}
