using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompKill : MonoBehaviour
{
    CharacterController CC;

    private void Start()
    {
        CC = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CC.onEnemyCheck();
        if (CC.stompedEnemies.Length > 0)
        {
            foreach (Collider2D enemy in CC.stompedEnemies)
                enemy.GetComponent<EnemyInfo>().health = 0;
        }
    }

}
