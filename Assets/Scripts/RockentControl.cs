using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockentControl : MonoBehaviour
{

    [SerializeField] private Transform shoulder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rocketAiming();
    }

    private void rocketAiming()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 vectorToTarget = mousePos - (Vector2)shoulder.position;

        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + 90;

        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        shoulder.rotation = Quaternion.Slerp(shoulder.rotation, rot, 1);
    }

}
