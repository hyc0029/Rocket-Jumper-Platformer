using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompKill : MonoBehaviour
{
    CharacterController CC;
    [SerializeField] private float directionDiff;

    private void Start()
    {
        CC = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CC.onEnemyCheck();
        if (CC.stompedEnemies.Length > 0 && CC.playerRigidbody.velocity.y < 0)
        {
            foreach (Collider2D col in CC.stompedEnemies)
                col.gameObject.GetComponent<EnemyInfo>().health = 0;

            Vector3 vel = CC.playerRigidbody.velocity;
            vel.y = 30;
            CC.playerRigidbody.velocity = vel;
        }
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyInfo>() && CC.onEnemyCheck())
        {
            collision.gameObject.GetComponent<EnemyInfo>().health = 0;
        }
    }
    */

}
