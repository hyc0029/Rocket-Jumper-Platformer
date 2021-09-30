using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]

public class CameraController : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private Rigidbody2D playerRigidbody2D;
    private CharacterController player;

    [Header("Camera size")]
    [SerializeField] private float baseCameraSize;
    [SerializeField] [Range(0, 1)] private float cameraMoveInPercentage;
    private float delayBeforeChangingSize = 0.5f;
    private float delayBeforeChangingSizeTimer;
    private float sizeChangeSpeed = 0.5f;
    private float sizeChangeTimer;

    [Header("Camera Position")]
    [SerializeField] private float amountOfCameraLead;
    [SerializeField] private float idleUpwardOffset;
    [SerializeField] [Range(0, 1)] private float distanceFromMouseModifier;
    private float positionChangeSpeed = 3f;

    [Header("Mouse Deadzones")]
    [SerializeField] private float minXMouseDeadzone;
    [SerializeField] private float maxXMouseDeadzone;
    [SerializeField] private float minYMouseDeadzone;
    [SerializeField] private float maxYMouseDeadzone;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GetComponent<Camera>();
        //playerRigidbody2D = FindObjectOfType<CharacterController>().GetComponent<Rigidbody2D>();
        player = FindObjectOfType<CharacterController>().GetComponent<CharacterController>();
        mainCam.orthographicSize = baseCameraSize * (1 - cameraMoveInPercentage);
    }

    // Update is called once per frame
    void Update()
    {
        CameraSizeController();
        CameraPositionController();
    }

    void CameraSizeController()
    {
        if (player.groundCheck())
        {
            if (delayBeforeChangingSizeTimer > delayBeforeChangingSize)
            {
                CameraSizeChange(mainCam, baseCameraSize, baseCameraSize * (1 - cameraMoveInPercentage));
            }
            else
                    delayBeforeChangingSizeTimer += Time.deltaTime;
        }
        else
            CameraSizeChange(mainCam, baseCameraSize * (1 - cameraMoveInPercentage), baseCameraSize);
    }

    void CameraSizeChange(Camera cam, float currentsize, float newSize)
    {
        if (cam.orthographicSize != newSize)
        {
            cam.orthographicSize = Mathf.Lerp(currentsize, newSize, sizeChangeTimer / sizeChangeSpeed);
            sizeChangeTimer += Time.deltaTime;
        }
        else
        {
            sizeChangeTimer = 0;
            delayBeforeChangingSizeTimer = 0;
        }
    }

    void CameraPositionController()
    {
        CameraPositionChange(mainCam, mainCam.transform.position, CameraPositionCalculator(mainCam, amountOfCameraLead));
    }

    private Vector3 CameraPositionCalculator(Camera cam ,float camLead)
    {
        Vector3 newCameraPosition = Vector3.zero;

        Vector3 playerPosition = playerRigidbody2D.position;

        Vector3 playerVelocity = playerRigidbody2D.velocity;

        Vector3 mouseLocalPosition = playerRigidbody2D.transform.InverseTransformPoint(mainCam.ScreenToWorldPoint(Input.mousePosition));

        if (playerVelocity.magnitude > 5)
        {
            newCameraPosition = (Vector3)playerRigidbody2D.position + ((Vector3.right * playerVelocity.normalized.x) * camLead);
        }
        else
        {
            newCameraPosition = (Vector3)playerRigidbody2D.position + (Vector3.up * idleUpwardOffset);
            if (MouseDeadzoneForCamera(minXMouseDeadzone, maxXMouseDeadzone, Vector2.right))
                newCameraPosition.x += mouseLocalPosition.x * distanceFromMouseModifier;
        }
        if (MouseDeadzoneForCamera(minYMouseDeadzone, maxYMouseDeadzone, Vector2.up))
            newCameraPosition.y += mouseLocalPosition.y * distanceFromMouseModifier;

        newCameraPosition.z = -1;

        return newCameraPosition;

    }

    void CameraPositionChange(Camera cam, Vector3 startPos, Vector3 newPos)
    {
        cam.transform.position = Vector3.Lerp(startPos, newPos, positionChangeSpeed * Time.deltaTime);
    }

    bool MouseDeadzoneForCamera(float min, float max, Vector2 axisToCheck)
    {
        Vector2 playerPosition = playerRigidbody2D.position;

        Vector2 mouseLocalPositionToPlayer = playerRigidbody2D.transform.InverseTransformPoint(mainCam.ScreenToWorldPoint(Input.mousePosition));

        if (axisToCheck == Vector2.right && mouseLocalPositionToPlayer.x < min || axisToCheck == Vector2.right && mouseLocalPositionToPlayer.x > max)
            return true;
        else if (axisToCheck == Vector2.up && mouseLocalPositionToPlayer.y < min || axisToCheck == Vector2.up && mouseLocalPositionToPlayer.y > max)
            return true;
        else
            return false;


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(playerRigidbody2D.transform.TransformPoint(new Vector3(minXMouseDeadzone, maxYMouseDeadzone, 0)), playerRigidbody2D.transform.TransformPoint(new Vector3(maxXMouseDeadzone, maxYMouseDeadzone, 0)));
        Gizmos.DrawLine(playerRigidbody2D.transform.TransformPoint(new Vector3(minXMouseDeadzone, minYMouseDeadzone, 0)), playerRigidbody2D.transform.TransformPoint(new Vector3(maxXMouseDeadzone, minYMouseDeadzone, 0)));
        Gizmos.DrawLine(playerRigidbody2D.transform.TransformPoint(new Vector3(minXMouseDeadzone, maxYMouseDeadzone, 0)), playerRigidbody2D.transform.TransformPoint(new Vector3(minXMouseDeadzone, minYMouseDeadzone, 0)));
        Gizmos.DrawLine(playerRigidbody2D.transform.TransformPoint(new Vector3(maxXMouseDeadzone, maxYMouseDeadzone, 0)), playerRigidbody2D.transform.TransformPoint(new Vector3(maxXMouseDeadzone, minYMouseDeadzone, 0)));
    }
}
