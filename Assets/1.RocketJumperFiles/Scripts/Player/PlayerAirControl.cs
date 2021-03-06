using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirControl : MonoBehaviour
{

    Rigidbody2D playerRb;
    CharacterController cc;
    [SerializeField] private float airControlForce;
    [SerializeField] private float groundedMoveSpeed;
    [SerializeField] [Range(0, 1)] private float extraControlPrecentage;
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
        if (cc.myCol.enabled && Time.timeScale != 0)
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
                playerRb.velocity = Vector3.right * leftRight * groundedMoveSpeed;
            }
        }
        else
        {
            forwardThrustParticle.Stop();
            backwardThrustParticle.Stop();
            thrustSound.Stop();
        }
    }

    private void FixedUpdate()
    {
        if (!cc.groundCheck() && cc.myCol.enabled)
        {
            Vector2 velocity = playerRb.velocity;
            if (Vector2.Dot(transform.GetChild(0).right * leftRight, transform.right) > 0)
                velocity.x += leftRight * ((airControlForce + 0.1f * Mathf.Abs(playerRb.velocity.x)) * Time.fixedDeltaTime);
            else if(Vector2.Dot(transform.GetChild(0).right * leftRight, transform.right) < 0)
                velocity.x += leftRight * ((airControlForce + extraControlPrecentage * Mathf.Abs(playerRb.velocity.x)) * Time.fixedDeltaTime);
            playerRb.velocity = velocity;
        }
    }

}
