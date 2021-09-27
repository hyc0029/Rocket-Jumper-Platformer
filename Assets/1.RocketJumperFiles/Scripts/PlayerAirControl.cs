using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirControl : MonoBehaviour
{

    Rigidbody2D playerRb;
    [SerializeField] private float airControlForce;
    [SerializeField] private ParticleSystem forwardThrustParticle;
    [SerializeField] private ParticleSystem backwardThrustParticle;
    [SerializeField] private AudioSource thrustSound;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        float leftRight = Input.GetKey(KeyCode.D) ? 1 : 0 - (Input.GetKey(KeyCode.A) ? 1 : 0);

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            if(!thrustSound.isPlaying)
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

        playerRb.AddForce(transform.right * leftRight * airControlForce);
    }

}
