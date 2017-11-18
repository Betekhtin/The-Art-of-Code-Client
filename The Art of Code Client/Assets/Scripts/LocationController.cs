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
    private Tile[] objects;
    private Socket socket;
    private bool dataRecieved = false;

    public GameObject tilePrefab;
    //public GameObject testObject;
    
    // Load location
    void Start () {
        
        socket = GameObject.Find("SocketObject").GetComponent<SocketController>().getSocket();

        socket.Emit("readyForInitData");

        socket.On("initData", (json) =>
        {
            Debug.Log("Got initial data.");

            // Get location json
            locationData = JsonMapper.ToObject((string)json);

            dataRecieved = true;
        });

        socket.On("move", (json) =>
        {
            JsonData moveData = JsonMapper.ToObject((string)json);

            int new_position_x = (int)moveData["newPosition"]["x"];
            int new_position_y = (int)moveData["newPosition"]["y"];

            //location

        });

    }

    void Update()
    {
        //super-puper-kostyl
        if (dataRecieved)
        {

            Debug.Log(locationData.ToString());

            // Generate tiles
            int size_x = (int)locationData["locations"]["matrix"]["size_x"];
            int size_y = (int)locationData["locations"]["matrix"]["size_y"];
            location = new Tile[size_x, size_y];

            for (int x = 0; x < size_x; ++x)
            {
                for (int y = 0; y < size_y; ++y)
                {
                    GameObject tile = Instantiate(tilePrefab);
                    tile.transform.SetParent(transform, false);
                    tile.transform.position = new Vector3(x * _tile_size_, y * _tile_size_, 0);
                    tile.GetComponent<SpriteRenderer>().sprite =
                        Resources.Load(_sprite_path_ + (string)locationData["locations"]["matrix"]["textures"][x][y], typeof(Sprite)) as Sprite;
                    location[x, y] = new Tile(tile);
                }
            }



            dataRecieved = false;
        }
    }

}
