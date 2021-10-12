using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTile : Tile
{
    public GameObject playerObj = null;

    protected override void Start()
    {
        Instantiate(playerObj, transform.position, Quaternion.identity);
    }
}
