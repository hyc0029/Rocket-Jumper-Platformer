using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody2D))]

public class CharacterController : MonoBehaviour
{ 
    private Rigidbody2D playerRigidbody;
    private Animator myAnimator;

    [Header("Ground Check")]
    [SerializeField] private Vector2 groundCheckBoxPos;
    [SerializeField] private Vector2 groundCheckBoxSize;
    [SerializeField] private LayerMask groundMask;

    [Header("Physics")]
    [SerializeField] private float groundedDrag;
    [SerializeField] private float airDrag;

    [Header("Limiter")]
    [SerializeField] private float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (groundCheck())
            playerRigidbody.drag = groundedDrag;
        else
            playerRigidbody.drag = airDrag;

        myAnimator.SetBool("Grounded", groundCheck());

        if (playerRigidbody.velocity.magnitude > maxSpeed)
        {
            playerRigidbody.velocity = playerRigidbody.velocity.normalized * maxSpeed;
        }
    }

    private bool groundCheck()
    {
        if (Physics2D.OverlapBox(transform.TransformPoint(groundCheckBoxPos), groundCheckBoxSize, 0, groundMask))
            return true;
        else
            return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.TransformPoint(groundCheckBoxPos), groundCheckBoxSize);
    }

}
