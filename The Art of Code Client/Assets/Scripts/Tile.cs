using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{

    private GameObject tile, obj;
    private int id;
    private string type;
    private bool todel = false;
    public Tile(GameObject tile, int id = -1, string type = null, GameObject obj = null)
    {
        this.tile = tile;
        this.obj = obj;
    }

    public void setObject(GameObject obj, int id, string type)
    {
        try
        {
            this.id = id;
            this.type = type;
            this.obj = obj;
            this.obj.transform.SetParent(tile.transform, false);
            this.obj.transform.localPosition = new Vector3(0, 0, 0);
            this.obj.transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    public GameObject getObject()
    {
        return this.obj;
    }

    public void delObject()
    {
        todel = true;
    }
    public void setSprite(Sprite sprite)
    {
        tile.GetComponent<SpriteRenderer>().sprite = sprite;
    }
    void Update()
    {
        if (todel)
        {
            todel = false;
            Debug.Log(obj);
            obj.GetComponent<SpriteRenderer>().sprite = null;
            //MonoBehaviour.Destroy(obj.GetComponent<SpriteRenderer>());
            //MonoBehaviour.Destroy(obj);
        }
    }
}
