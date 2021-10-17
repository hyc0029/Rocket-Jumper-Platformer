using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



[System.Serializable]
public class bodyParts
{
    public Collider2D partCollider;
}

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    [System.NonSerialized]public Rigidbody2D playerRigidbody;
    public Animator myAnimator;
    public Collider2D myCol;
    public Transform myBodyRotationControl;
    public RocketLauncherControl myRLC;

    [Header("Ground Check")]
    [SerializeField] private Vector2 groundCheckBoxPos;
    [SerializeField] private Vector2 groundCheckBoxSize;
    [SerializeField] private LayerMask groundMask;

    [Header("Enemy Check")]
    [SerializeField] private Vector2 enemyCheckBoxPos;
    [SerializeField] private Vector2 enemyCheckBoxSize;
    public Collider2D[] stompedEnemies;
    [SerializeField] private LayerMask enemyMask;

    [Header("Walled Check")]
    [SerializeField] private Vector2 walledCheckBoxPos;
    [SerializeField] private Vector2 walledCheckBoxSize;
    [SerializeField] private LayerMask wallCheckMasks;

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
    [SerializeField] Collider2D body;
    [SerializeField] private float explosionForce;
    [SerializeField] private bodyParts[] bodyParts;

    [Header("Reset")]
    private PlayerRespawn playerRespawn;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        playerRespawn = GetComponent<PlayerRespawn>();
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].partCollider.enabled = false;
        }

        myRLC = GetComponent<RocketLauncherControl>();

        myRLC.rocket.GetComponent<Collider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (myCol.enabled)
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
        }
        else
        {
            if (UiObject.activeSelf && Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
            {
                playerRespawn.ResetPlayer();
                Time.timeScale = 1;
                UiObject.SetActive(false);
            }
        }


    }

    public bool groundCheck()
    {
        if (Physics2D.OverlapBox(transform.TransformPoint(groundCheckBoxPos), groundCheckBoxSize, 0, groundMask))
            return true;
        else
            return false;
    }

    public void onEnemyCheck()
    {

        stompedEnemies = Physics2D.OverlapBoxAll(transform.TransformPoint(enemyCheckBoxPos), enemyCheckBoxSize, 0, enemyMask);

        /*
        if (Physics2D.OverlapBox(transform.TransformPoint(enemyCheckBoxPos), enemyCheckBoxSize, 0, enemyMask))
            return true;
        else
            return false;
            */
    }

    public bool walledCheck()
    {
        if (Physics2D.OverlapBox(transform.TransformPoint(walledCheckBoxPos), walledCheckBoxSize, 0, wallCheckMasks) || Physics2D.OverlapBox(transform.TransformPoint(new Vector2(-walledCheckBoxPos.x, walledCheckBoxPos.y)), walledCheckBoxSize, 0, wallCheckMasks))
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
        myCol.enabled = false;
        myAnimator.SetBool("Dead", true);
        playerRigidbody.simulated = false;
        myRLC.enabled = false;
        for (int i = 0; i < bodyParts.Length; i++)
        {

            bodyParts[i].partCollider.transform.SetParent(null);
            bodyParts[i].partCollider.enabled = true;
            Rigidbody2D temp = bodyParts[i].partCollider.gameObject.AddComponent<Rigidbody2D>();
            temp.mass = 25;
            temp.interpolation = RigidbodyInterpolation2D.Extrapolate;

            Vector2 explosionDirection = (Vector2)bodyParts[i].partCollider.transform.position - (Vector2)body.transform.position;
            explosionDirection.y += 1;
            explosionDirection.Normalize();

            temp.AddForce(explosionDirection * explosionForce, ForceMode2D.Impulse);

            temp.AddTorque(Random.Range((int)-1, (int)2) * 500, ForceMode2D.Impulse);

        }

        StartCoroutine(playerDeadScreen());

    }

    IEnumerator playerDeadScreen()
    {
        yield return new WaitForSeconds(5);
        UiObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.TransformPoint(groundCheckBoxPos), groundCheckBoxSize);

        Gizmos.DrawWireCube(transform.TransformPoint(resetVerticalVelocityCheckPos), resetVerticalVelocityCheckSize);

        Gizmos.DrawWireCube(transform.TransformPoint(walledCheckBoxPos), walledCheckBoxSize);
        Gizmos.DrawWireCube(transform.TransformPoint(new Vector2(-walledCheckBoxPos.x, walledCheckBoxPos.y)), walledCheckBoxSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.TransformPoint(enemyCheckBoxPos), enemyCheckBoxSize);

    }

}
