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
    [SerializeField] private Vector3 Offset;
    private void Start()
    {
        cf.useLayerMask = true;
        cf.SetLayerMask(playerMask);

        Vector2 block1TargetRot = block2.position - block1.position;
        float angle = Mathf.Atan2(block1TargetRot.y, block1TargetRot.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        block1.rotation = rot;

        Vector2 block2TargetRot = block1.position - block2.position;
        float angle2 = Mathf.Atan2(block2TargetRot.y, block2TargetRot.x) * Mathf.Rad2Deg;
        Quaternion rot2 = Quaternion.AngleAxis(angle2, Vector3.forward);
        block2.rotation = rot2;

        myLR.SetPosition(0, block1.TransformPoint(Offset));
        myLR.SetPosition(1, block2.TransformPoint(Offset));
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
