using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Tiles : MonoBehaviour
{
    [SerializeField]private float multiplier = 6.7f;
    [SerializeField] private SpriteRenderer mySR;
    [SerializeField] private Vector2 tileSize;

    private enum TileType { Normal, FloorAndCeilingCorner, WallCorner };
    [SerializeField]private TileType currentTile;

    void Update()
    {
        switch (currentTile)
        {
            case TileType.Normal:
                break;

            case TileType.FloorAndCeilingCorner:
                tileSize = new Vector2(0.86f, 1f);
                break;

            case TileType.WallCorner:
                tileSize = new Vector2(1f, 0.86f);
                break;
        }

        if(currentTile != TileType.Normal)
            mySR.sortingLayerName = "Fg_Corner";


        Vector2 newTileSize = new Vector2(tileSize.x * multiplier, tileSize.y * multiplier);
        mySR.size = newTileSize;
    }
}
