using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using Quobject.SocketIoClientDotNet.Client;
using System.Linq;

public class LocationController : MonoBehaviour {

    private JsonData locationData;
    static private float _tile_size_ = 1f; //wtf
    static private string _sprite_path_ = "Sprites/";
    private Tile[,] location;
    private JsonData objectsInfo;
    private List<JsonData> objects;
    private Socket socket;
    public GameObject player;
    private JsonData heroInfo;
    private bool dataRecieved = false;
    private bool moved = false;
    private bool mobSpawned = false;
    private JsonData mobData;
    private int x, y;
    public GameObject tilePrefab;
    //public GameObject testObject;
    
    // Load location
    void Start () {
        
        socket = GameObject.Find("SocketObject").GetComponent<SocketController>().getSocket();

        socket.Emit("readyForInitData");

        socket.On("initData", (json) =>
        {
            Debug.Log("Got initial data.");
            Debug.Log(json);

            // Get location json
            locationData = JsonMapper.ToObject((string)json);

            dataRecieved = true;
        });

        socket.On("move", (json) =>
        {
            Debug.Log("Moved.");
            Debug.Log(json);

            JsonData moveData = JsonMapper.ToObject((string)json);

            int new_position_x = (int)moveData["newPosition"]["x"];
            int new_position_y = (int)moveData["newPosition"]["y"];

            moved = true;
            x = new_position_x;
            y = new_position_y;

            //location
            /*
            int hero_pos_x = (int)heroInfo["positionX"];
            int hero_pos_y = (int)heroInfo["positionY"];
            location[hero_pos_x, hero_pos_y].setObject(null);
            location[new_position_x, new_position_y].setObject(player);
            heroInfo["positionX"] = new_position_x;
            heroInfo["positionY"] = new_position_y;
            */
        });

        socket.On("moveMob", (json) =>
        {
            Debug.Log("Mob moved.");
            Debug.Log(json);

            JsonData moveData = JsonMapper.ToObject((string)json);

            //Get new positions and mob id
            int mob_id = (int)moveData["id"];
            int new_position_x = (int)moveData["newPosition"]["x"];
            int new_position_y = (int)moveData["newPosition"]["y"];

            //find mob by id in objects list
            JsonData mob = objects.Find(x => (string)x["type"] == "mob" && (int)x["id"] == mob_id);
            //get previous mob position
            int old_position_x = (int)mob["positionX"];
            int old_position_y = (int)mob["positionY"];

            //place old object in a new position
            location[new_position_x, new_position_y].setObject(location[old_position_x, old_position_y].getObject(), mob_id, "mob");
            //remove object from an old tile
            location[old_position_x, old_position_y].setObject(null, -1, null);
        });

        socket.On("mobSpawned", (json) =>
        {
            Debug.Log("Mob spawned.");
            Debug.Log(json);

            mobData = JsonMapper.ToObject((string)json);

            mobSpawned = true;

        });

    }

    void Update()
    {
        //super-puper-kostyl
        if (dataRecieved)
        {

            dataRecieved = false;

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

            /*
   id int unsigned not null auto_increment,
   login varchar(60) not null,
   userId int unsigned not null,
   positionX int unsigned not null,
   positionY int unsigned not null,
   location varchar(60) not null,
   HP decimal(5,2) not null,
   maxHP decimal(6,2) not null,
   attack int unsigned not null,
   defence int unsigned not null,
   exp int unsigned not null,
   lvl int unsigned not null,
   primary key (id) 
             */
            
            heroInfo = locationData["hero"];
            int hero_pos_x = (int)heroInfo["positionX"];
            int hero_pos_y = (int)heroInfo["positionY"];
            location[hero_pos_x, hero_pos_y].setObject(player, (int)heroInfo["id"], "hero");

            /*
  [{"id":1,"positionX":4,"positionY":6,"type":"static","name":"tree"},{"id":2,"positionX":12,"positionY":8,"type":"static","name":"tree"},{"id":3,"positionX":20,"positionY":17,"type":"static","name":"tree"},{"id":4,"positionX":1,"positionY":7,"type":"static","name":"tree"},{"id":5,"positionX":29,"positionY":14,"type":"static","name":"tree"},{"id":6,"positionX":17,"positionY":29,"type":"static","name":"tree"},{"id":7,"positionX":78,"positionY":82,"type":"static","name":"tree"},{"id":8,"positionX":22,"positionY":1,"type":"static","name":"tree"},{"id":9,"positionX":7,"positionY":19,"type":"static","name":"tree"}]
             */
            objectsInfo = locationData["objects"];
            int objects_count = (int)locationData["objectsCount"];
            objects = new List<JsonData>(objects_count);
            for (int i = 0; i < objects_count; ++i)
            {
                JsonData current_elem = locationData["objects"][i];
                objects.Add(current_elem);
                int pos_x = (int)current_elem["positionX"];
                int pos_y = (int)current_elem["positionY"];
                GameObject newObject = new GameObject();
                newObject.AddComponent<SpriteRenderer>();
                newObject.GetComponent<SpriteRenderer>().sprite =
                    Resources.Load(_sprite_path_ + (string)current_elem["name"], typeof(Sprite)) as Sprite;
                location[pos_x, pos_y].setObject(newObject, (int)current_elem["id"], "object");
            }

        }
        if (moved)
        {
            moved = false;
            player.transform.position = new Vector2(x, y);
        }
        if (mobSpawned)
        {
            mobSpawned = false;
            
            int pos_x = (int)mobData["positionX"];
            int pos_y = (int)mobData["positionY"];
            
            GameObject newObject = new GameObject();
            newObject.AddComponent<SpriteRenderer>();
            newObject.GetComponent<SpriteRenderer>().sprite =
                Resources.Load(_sprite_path_ + "Boar", typeof(Sprite)) as Sprite;

            location[pos_x, pos_y].setObject(newObject, (int)mobData["id"], "mob");
        }
    }

}
