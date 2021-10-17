using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField] private Vector2 physicsCastSize;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Color notActivated;
    [SerializeField] private Color activated;
    private SpriteRenderer checkpointSprite;
    private PlayerRespawn playerRespawn;

    // Start is called before the first frame update
    void Start()
    {
        playerRespawn = FindObjectOfType<PlayerRespawn>();
        checkpointSprite = GetComponentInChildren<SpriteRenderer>();
        checkpointSprite.color = notActivated;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y + physicsCastSize.y / 2), physicsCastSize, 0, playerLayer))
        {
            playerRespawn.lastCheckpoint = transform.position;
            checkpointSprite.color = activated;
            this.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y + physicsCastSize.y / 2), physicsCastSize);
    }




}
