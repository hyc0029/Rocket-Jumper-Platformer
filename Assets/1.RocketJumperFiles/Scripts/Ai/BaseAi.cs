using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAi : MonoBehaviour
{
    private ContactFilter2D cf;
    private ContactFilter2D raycastFilter;

    public bool groundCheck(Transform transform, Vector2 position, Vector2 size, LayerMask groundMask)
    {
        if (Physics2D.OverlapBox(transform.TransformPoint(position), size, 0, groundMask))
            return true;
        else
            return false;
    }

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

    public bool checkForwardDown(Transform transform, Vector2 origin, float distance, LayerMask layers)
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        cf.useLayerMask = true;
        cf.SetLayerMask(layers);
        Physics2D.Raycast(transform.TransformPoint(origin), -transform.up, cf, hits, distance);

        if (hits.Count > 0)
            return false;
        else
            return true;
    }

    public void basicMovement(Rigidbody2D rb2d, Transform transform, float speed)
    {
        rb2d.velocity = new Vector3(transform.right.x * speed, rb2d.velocity.y, 0);
    }

    public void moveTowardPlayer(Rigidbody2D rb2d, Transform target, float speed)
    {
        float xDir = (rb2d.position.x < target.position.x ? 1 : 0) - (rb2d.position.x > target.position.x ? 1 : 0);
        rb2d.velocity = new Vector3(xDir * speed, rb2d.velocity.y, 0);
        if (rb2d.transform.GetChild(0).InverseTransformPoint(target.position).x < 0)
            rb2d.transform.GetChild(0).Rotate(new Vector3(0, 180, 0));
    }

    public bool detectPlayer(Transform transform, Transform eyePos, float distance, LayerMask player)
    {
        if (Physics2D.OverlapCircle(transform.position, distance, player))
        {
            RaycastHit2D[] rayHits2D;

            Vector2 directionToPlayer = FindObjectOfType<CharacterController>().transform.position - eyePos.position;
            rayHits2D = Physics2D.RaycastAll(eyePos.position, directionToPlayer.normalized, directionToPlayer.magnitude);
            if (rayHits2D.Length > 0)
            {
                if (rayHits2D[0].transform.GetComponent<CharacterController>())
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        else
            return false;

        //return Physics2D.OverlapCircle(transform.position, distance, player);
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
        }
    }


}
