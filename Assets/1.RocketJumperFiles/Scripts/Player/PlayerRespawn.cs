using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRespawn : MonoBehaviour
{
    private float lerpAnimationTime = 3;
    [HideInInspector] public Vector3 lastCheckpoint;
    //[SerializeField] private Transform[] partsTransform;
    [SerializeField] private CharacterController playerCC;
    [SerializeField] private Collider2D[] partsCollider;
    [SerializeField] private Transform[] partsParent;
    [SerializeField] private Vector3[] partsDefaultLocalPosition;
    [SerializeField] private Quaternion[] partsDefaultLocalRotation;


    private Vector3[] startLocalPosition;
    private Quaternion[] startLocalRotation;
    // Start is called before the first frame update
    void Start()
    {
        storePlayerData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void storePlayerData()
    {
        partsCollider = GetComponentsInChildren<Collider2D>();
        List<Collider2D> temp = new List<Collider2D>(partsCollider);
        temp.RemoveAt(0);
        partsCollider = temp.ToArray();
        //partsTransform = new Transform[partsCollider.Length];
        partsParent = new Transform[partsCollider.Length];
        partsDefaultLocalPosition = new Vector3[partsCollider.Length];
        partsDefaultLocalRotation = new Quaternion[partsCollider.Length];
        for (int i = 0; i < partsCollider.Length; i++)
        {
            //partsTransform[i] = partsCollider[i].transform;
            partsParent[i] = partsCollider[i].transform.parent;
            partsDefaultLocalPosition[i] = partsCollider[i].transform.localPosition;
            partsDefaultLocalRotation[i] = partsCollider[i].transform.localRotation;
        }
    }

    public void ResetPlayer()
    {
        startLocalPosition = new Vector3[partsCollider.Length];
        startLocalRotation = new Quaternion[partsCollider.Length];
        for (int i = 0; i < partsCollider.Length; i++)
        {
            partsCollider[i].transform.SetParent(partsParent[i]);
            startLocalPosition[i] = partsCollider[i].transform.localPosition;
            startLocalRotation[i] = partsCollider[i].transform.localRotation;
            Destroy(partsCollider[i].GetComponent<Rigidbody2D>());
            if(partsCollider[i].transform != playerCC.myRLC.rocket)
                partsCollider[i].enabled = false;
        }
        StartCoroutine(playerResetLerp());
    }

    IEnumerator playerResetLerp()
    {
        float timer = 0;
        Camera.main.transform.SetParent(transform);
        Vector3 playerDeathPos = transform.position;
        lastCheckpoint.y += 1f;
        while (timer < lerpAnimationTime)
        {
            transform.position = Vector3.Lerp(playerDeathPos, lastCheckpoint, timer / lerpAnimationTime);
            for (int i = 0; i < partsCollider.Length; i++)
            {
                partsCollider[i].transform.localPosition = Vector3.Lerp(startLocalPosition[i], partsDefaultLocalPosition[i], timer/lerpAnimationTime);
                partsCollider[i].transform.localRotation = Quaternion.Slerp(startLocalRotation[i], partsDefaultLocalRotation[i], timer / lerpAnimationTime);
            }
            timer += Time.deltaTime;
            yield return null;
        }
        playerCC.myCol.enabled = true;
        playerCC.playerRigidbody.simulated = true;
        playerCC.myAnimator.SetBool("Dead", false);
        playerCC.myRLC.enabled = true;
        Camera.main.transform.SetParent(null);
        List<EnemiesSpawner> spawners = new List<EnemiesSpawner>(FindObjectsOfType<EnemiesSpawner>());
        foreach (EnemiesSpawner spawner in spawners)
            spawner.currentCount = 0;

        yield return null;
    }
}
