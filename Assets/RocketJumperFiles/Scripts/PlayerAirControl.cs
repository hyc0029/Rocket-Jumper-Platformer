using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirControl : MonoBehaviour
{

    Rigidbody2D playerRb;
    [SerializeField] private float airControlForce;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float leftRight = Input.GetKey(KeyCode.D) ? 1 : 0 - (Input.GetKey(KeyCode.A) ? 1 : 0);

        playerRb.AddForce(transform.right * leftRight * airControlForce);

    }

}
