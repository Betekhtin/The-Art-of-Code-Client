using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using Quobject.SocketIoClientDotNet.Client;

public class LocationController : MonoBehaviour {

    private JsonData locationData;
    static private float _tile_size_ = 1f; //wtf
    static private string _sprite_path_ = "Sprites/";
    private Tile[,] location;
    private Socket socket;

    public GameObject tilePrefab;
    //public GameObject testObject;
    
    // Load location
    void Start () {
        
        string json = File.ReadAllText(Application.dataPath + "/Resources/test_location.json");
        socket = GameObject.Find("SocketObject").GetComponent<SocketController>().getSocket();

        socket.On(Socket.EVENT_CONNECT, () =>
        {
            socket.Emit("auth", json);
        });

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
                tile.GetComponent<SpriteRenderer>().sprite = 
                    Resources.Load(_sprite_path_ + (string)locationData["matrix"]["textures"][x][y], typeof(Sprite)) as Sprite;
                location[x, y] = new Tile(tile);
            }
        }

        //test
        //location[5, 5].setSprite(Resources.Load<Sprite>("Sprites/red_dot"));
        //location[4, 4].setObject(testObject);
    }

}
