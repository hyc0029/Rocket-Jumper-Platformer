using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompKill : MonoBehaviour
{
    CharacterController CC;
    [SerializeField] private AudioSource stompSound;

    private void Start()
    {
        CC = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CC.onEnemyCheck();
        if (CC.myCol.enabled)
        {
            if (CC.stompedEnemies.Length > 0 && !CC.groundCheck())
            {
                stompSound.Play();
                foreach (Collider2D enemy in CC.stompedEnemies)
                    enemy.GetComponent<EnemyInfo>().health = 0;
                Vector3 playerVel = CC.playerRigidbody.velocity;
                playerVel.y = 30;
                CC.playerRigidbody.velocity = playerVel;
            }
        }
    }

}
