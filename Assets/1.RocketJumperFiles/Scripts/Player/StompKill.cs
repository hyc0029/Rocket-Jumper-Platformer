using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompKill : MonoBehaviour
{
    [SerializeField] private float directionDiff;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyInfo>())
        {
            Vector2 directionToContact = (Vector2)transform.position - (Vector2)collision.GetContact(0).point;
            directionToContact = directionToContact.normalized;
            if (Vector2.Dot(Vector2.down, directionToContact) <= directionDiff)
            {
                Debug.Log(collision.gameObject.name + " Dead");
                collision.gameObject.GetComponent<EnemyInfo>().health = 0;
            }
        }
    }


}