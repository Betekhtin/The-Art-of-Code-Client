using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile{

    private GameObject tile, obj;
    private int id;

    public Tile(GameObject tile, int id = -1, GameObject obj = null) {
        this.tile = tile;
        this.obj = obj;
    }
    
    public void setObject(GameObject obj, int id) {
        this.id = id;
        this.obj = obj;
        this.obj.transform.SetParent(tile.transform, false);
        this.obj.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void setSprite(Sprite sprite) {
        tile.GetComponent<SpriteRenderer>().sprite = sprite;
    }

}
