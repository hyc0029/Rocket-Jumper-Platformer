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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyInfo>() && CC.onEnemyCheck())
        {
            collision.gameObject.GetComponent<EnemyInfo>().health = 0;
        }
    }


}
