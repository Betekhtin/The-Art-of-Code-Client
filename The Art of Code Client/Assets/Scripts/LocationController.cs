using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class LocationController : MonoBehaviour {

    private JsonData locationData;
    static private float _tile_size_ = 2.5f;
    static private string _sprite_path_ = "Sprites/";
    private Tile[,] location;

    public GameObject tilePrefab;
    
    // Load location
    void Start () {

        // Get location json
        locationData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Resources/test_location.json"));
        
        // Generate tiles
        int size_x = (int)locationData["matrix"]["size_x"];
        int size_y = (int)locationData["matrix"]["size_y"];
        location = new Tile[size_x, size_y];
   
        for (int x = 0; x < size_x; ++x) {
            for (int y = 0; y < size_y; ++y) {
                GameObject tile = Instantiate(tilePrefab);
                tile.transform.SetParent(transform, false);
                tile.transform.position = new Vector3(x * _tile_size_, y * _tile_size_, 0);
                Debug.Log(_sprite_path_ + (string)locationData["matrix"]["textures"][x][y]);
                tile.GetComponent<SpriteRenderer>().sprite = 
                    Resources.Load(_sprite_path_ + (string)locationData["matrix"]["textures"][x][y], typeof(Sprite)) as Sprite;
                location[x, y] = new Tile(tile);
            }
        }
    }

}
