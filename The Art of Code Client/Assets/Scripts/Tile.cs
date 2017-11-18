using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile{

    private GameObject tile, obj;
    
    public Tile(GameObject tile, GameObject obj = null) {
        this.tile = tile;
        this.obj = obj;
    }
    
    public void setObject(GameObject obj) {
        this.obj = obj;
    }

    public void setSprite(Sprite sprite) {
        tile.GetComponent<SpriteRenderer>().sprite = sprite;
    }

}
