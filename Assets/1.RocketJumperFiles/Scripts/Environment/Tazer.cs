using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tazer : MonoBehaviour
{

    [SerializeField] private Transform block1;
    [SerializeField] private Transform block2;
    [SerializeField] private LineRenderer myLR;
    private ContactFilter2D cf;
    List<RaycastHit2D> hits = new List<RaycastHit2D>();
    [SerializeField] private LayerMask playerMask;

    private void Start()
    {
        cf.useLayerMask = true;
        cf.SetLayerMask(playerMask);
        myLR.SetPosition(0, block1.localPosition);
        myLR.SetPosition(1, block2.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.Raycast(block1.position, (block2.position - block1.position).normalized, cf, hits, Vector2.Distance(block1.position, block2.position));

        if (hits.Count > 0)
        {
            if(hits[0].transform.GetComponent<CharacterController>())
                hits[0].transform.GetComponent<CharacterController>().playerDead();
        }

    }
}
