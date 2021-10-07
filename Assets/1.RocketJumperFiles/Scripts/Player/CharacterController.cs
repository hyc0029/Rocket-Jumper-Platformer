using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



[RequireComponent(typeof(Rigidbody2D))]

public class CharacterController : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;
    private Animator myAnimator;
    public Transform myBodyRotationControl;

    [Header("Ground Check")]
    [SerializeField] private Vector2 groundCheckBoxPos;
    [SerializeField] private Vector2 groundCheckBoxSize;
    [SerializeField] private LayerMask groundMask;

    [Header("Enemy Check")]
    [SerializeField] private Vector2 enemyCheckBoxPos;
    [SerializeField] private Vector2 enemyCheckBoxSize;
    [SerializeField] private LayerMask enemyMask;

    [Header("Reset Vertical Velocity Check")]
    [SerializeField] private Vector2 resetVerticalVelocityCheckPos;
    [SerializeField] private Vector2 resetVerticalVelocityCheckSize;

    [Header("Physics")]
    [SerializeField] private float groundedDrag;
    [SerializeField] private float airDrag;

    [Header("Limiter")]
    [SerializeField] private float maxSpeed;

    [Header("Player Killed")]
    [SerializeField] GameObject UiObject;

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

        
        if (playerRigidbody.velocity.x > 5)
        {
            myBodyRotationControl.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (playerRigidbody.velocity.x < -5)
        {
            myBodyRotationControl.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        

        if (playerRigidbody.velocity.magnitude > maxSpeed)
        {
            playerRigidbody.velocity = playerRigidbody.velocity.normalized * maxSpeed;
        }

        if (UiObject.activeSelf && Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
        }


    }

    public bool groundCheck()
    {
        if (Physics2D.OverlapBox(transform.TransformPoint(groundCheckBoxPos), groundCheckBoxSize, 0, groundMask))
            return true;
        else
            return false;
    }

    public bool onEnemyCheck()
    {
        if (Physics2D.OverlapBox(transform.TransformPoint(enemyCheckBoxPos), enemyCheckBoxSize, 0, enemyMask))
            return true;
        else
            return false;
    }

    public bool resetVerticalVelocityCheck()
    {
        if (Physics2D.OverlapBox(transform.TransformPoint(resetVerticalVelocityCheckPos), resetVerticalVelocityCheckSize, 0, groundMask))
            return true;
        else
            return false;
    }

    public void playerDead()
    {
        UiObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.TransformPoint(groundCheckBoxPos), groundCheckBoxSize);

        Gizmos.DrawWireCube(transform.TransformPoint(resetVerticalVelocityCheckPos), resetVerticalVelocityCheckSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.TransformPoint(enemyCheckBoxPos), enemyCheckBoxSize);

    }

}
