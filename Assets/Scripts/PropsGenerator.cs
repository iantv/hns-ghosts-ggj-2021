using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropsGenerator : MonoBehaviour
{
    private List<GameObject> Walls;
    private GameObject Floor;
    public Dictionary<string, List<GameObject>> Prefabs = new Dictionary<string, List<GameObject>>();

    public string SpaceName;
    private string Subplace = "Bathroom";

    void Start()
    {
        InitInteriorPrefabs();

        Walls = new List<GameObject>();

        Transform GeometryItems = transform.Find("Geometry");
        foreach (Transform ch in GeometryItems.GetComponentsInChildren<Transform>())
        {
            if (ch.gameObject.CompareTag("Wall"))
                Walls.Add(ch.gameObject);
            else if (ch.gameObject.CompareTag("Floor"))
            {
                Floor = ch.gameObject;
            }
        }
        
        if (string.Compare(SpaceName, "RoomSpace") == 0)
        {
            Floor.GetComponent<MeshRenderer>().material = Resources.Load("Materials/BathroomFloor") as Material;
            InteriorInteriorStyle("Materials/BathroomWall");
            foreach (var wall in Walls)
            {
                int r = Random.Range(0, 8);
                var b = Instantiate(Prefabs[SpaceName][r % Prefabs[SpaceName].Count], wall.transform.position, wall.transform.rotation, gameObject.transform);
                b.transform.Translate(wall.transform.right * 0.5f, Space.World);
            }
        } else if (string.Compare(SpaceName, "CoridorSpace") == 0)
        {
            SpawnPainting();
        }
        else if (string.Compare(SpaceName, "EndRoomSpace") == 0)
        {
            Floor.GetComponent<MeshRenderer>().material = Resources.Load("Materials/BathroomFloor") as Material;
            
            GameObject bathItem = Prefabs[SpaceName][0];
            var b = Instantiate(Prefabs[SpaceName][0], Floor.transform.position, Floor.transform.rotation, gameObject.transform);
            b.transform.Translate(Floor.transform.up * 0.5f, Space.World);
            b.transform.Translate(Floor.transform.forward * 4, Space.World);
            b.transform.Rotate(new Vector3(0f, -90f, 0f));

            InteriorInteriorStyle("Materials/BathroomWall");
        }
        else if (string.Compare(SpaceName, "LivingRoomSpace") == 0)
        {
            //Floor.GetComponent<MeshRenderer>().material = Resources.Load("Materials/BathroomFloor") as Material;

            GameObject bathItem = Prefabs[SpaceName][0];
            var b = Instantiate(Prefabs[SpaceName][0], Floor.transform.position, Floor.transform.rotation, gameObject.transform);
            b.transform.Translate(Floor.transform.up * 0.5f, Space.World);
            //b.transform.Translate(Floor.transform.forward * 4, Space.World);
            //b.transform.Rotate(new Vector3(0f, -90f, 0f));

            //InteriorInteriorStyle("Materials/BathroomWall");
        }
        else if (string.Compare(SpaceName, "BigHallSpace") == 0)
        {
            //Floor.GetComponent<MeshRenderer>().material = Resources.Load("Materials/BathroomFloor") as Material;

            GameObject bathItem = Prefabs[SpaceName][0];
            var b = Instantiate(Prefabs[SpaceName][0], Floor.transform.position, Floor.transform.rotation, gameObject.transform);
            b.transform.Translate(Floor.transform.up * 0.5f, Space.World);
            //b.transform.Translate(Floor.transform.forward * 4, Space.World);
            //b.transform.Rotate(new Vector3(0f, -90f, 0f));

            //InteriorInteriorStyle("Materials/BathroomWall");
        }
        else
        {
            Debug.Log("SpaceName is " + SpaceName);
        }
    }

    void InitInteriorPrefabs()
    {
        List<GameObject> CoridorPrefabs = new List<GameObject>();
        CoridorPrefabs.Add(Resources.Load("Models/Props/Painting") as GameObject);
        Prefabs.Add("CoridiorSpace", CoridorPrefabs);

        List<GameObject> RoomSpacePrefabs = new List<GameObject>();
        RoomSpacePrefabs.Add(Resources.Load("Models/Props/Bath") as GameObject);
        RoomSpacePrefabs.Add(Resources.Load("Models/Props/Sink") as GameObject);
        Prefabs.Add("RoomSpace", RoomSpacePrefabs);

        List<GameObject> EndRoomPrefabs = new List<GameObject>();
        EndRoomPrefabs.Add(Resources.Load("Models/Props/Bathroom_all_v1") as GameObject);
        Prefabs.Add("EndRoomSpace", EndRoomPrefabs);

        List<GameObject> LivingRoomPrefabs = new List<GameObject>();
        LivingRoomPrefabs.Add(Resources.Load("Models/Props/LivingRoom_all_v1") as GameObject);
        Prefabs.Add("LivingRoomSpace", LivingRoomPrefabs);

        List<GameObject> BigHallPrefabs = new List<GameObject>();
        LivingRoomPrefabs.Add(Resources.Load("Models/Props/LivingRoom_set_v2") as GameObject);
        Prefabs.Add("BigHallSpace", LivingRoomPrefabs);
    }

    void SpawnPainting()
    {
        foreach (var wall in Walls)
            Instantiate(Prefabs[SpaceName][0], wall.transform);
    }

    void InteriorInteriorStyle(string WallsMat)
    {
        foreach (var wall in Walls)
        {
            wall.GetComponent<MeshRenderer>().material = Resources.Load("Materials/BathroomWall") as Material;
        }
    }
}
