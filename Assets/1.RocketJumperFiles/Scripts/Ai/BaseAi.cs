using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAi : MonoBehaviour
{
    protected ContactFilter2D cf;

    public bool checkForward(Transform transform, Vector2 origin, float distance, LayerMask layers)
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        cf.useLayerMask = true;
        cf.SetLayerMask(layers);
        Physics2D.Raycast(transform.TransformPoint(origin), transform.right, cf, hits, distance);
        if (hits.Count > 1)
            return true;
        else
            return false;
    }

    public void basicMovement(Rigidbody2D rb2d, Transform transform, float speed)
    {
        rb2d.velocity = new Vector3(transform.right.x * speed, rb2d.velocity.y, 0);
    }

    public bool detectPlayer(Transform transform, float distance, LayerMask player)
    {
        return Physics2D.OverlapCircle(transform.position, distance, player);
    }

    public bool isAlive(int currentHealth)
    {
        return currentHealth > 0;
    }

    public void Dead(Rigidbody2D rb2d, Animator animator, Collider2D col ,string boolName)
    {
        if (!animator.GetBool(boolName))
        {
            animator.SetBool(boolName, true);
            col.enabled = false;
            rb2d.gravityScale = 0;
            Debug.Log("Dead");
        }
    }


}
