using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody2D))]

public class CharacterController : MonoBehaviour
{ 
    private Rigidbody2D playerRigidbody;

    [Header("Movement")]
    [SerializeField] private float maxSpeed;
    private Vector2 playerVelocity;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    
    [Header("Ground Check")]
    [SerializeField] private Vector2 groundCheckBoxPos;
    [SerializeField] private Vector2 groundCheckBoxSize;
    [SerializeField] private LayerMask groundMask;
    private bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        groundCheck();
        movement();
    }

    private void movement()
    {
        float leftRight = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        if (leftRight != 0)
            playerVelocity.x = leftRight * maxSpeed;
        else
            playerVelocity.x = 0;

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
            playerRigidbody.velocity = playerRigidbody.velocity + Vector2.up * jumpForce;

        playerVelocity.y = playerRigidbody.velocity.y;

        playerRigidbody.velocity = playerVelocity;
    }

    private void groundCheck()
    {
        if (Physics2D.OverlapBox(transform.TransformPoint(groundCheckBoxPos), groundCheckBoxSize, 0, groundMask))
            grounded = true;
        else
            grounded = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.TransformPoint(groundCheckBoxPos), groundCheckBoxSize);
    }

}
