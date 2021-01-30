using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropsGenerator : MonoBehaviour
{
    private List<GameObject> Walls;
    private Dictionary<string, List<GameObject>> Prefabs = new Dictionary<string, List<GameObject>>();

    public string SpaceName;

    void Start()
    {
        InitInteriorPrefabs();

        Walls = new List<GameObject>();
        Transform GeometryItems = transform.Find("Geometry");
        foreach (Transform ch in GeometryItems)
        {
            if (ch.gameObject.CompareTag("Wall"))
                Walls.Add(ch.gameObject);
        }

        foreach (var wall in Walls)
        {
            Debug.Log("Comparison: SpaceName is " + SpaceName);
            if (string.Compare(SpaceName, "RoomSpace") == 0)
            {
                int r = Random.Range(0, 8);
                GameObject bathItem = Prefabs[SpaceName][r % Prefabs[SpaceName].Count];
                var b = Instantiate(Prefabs[SpaceName][r % Prefabs[SpaceName].Count], wall.transform.position, wall.transform.rotation, gameObject.transform);
                b.transform.Translate(wall.transform.right * 0.5f, Space.World);
            } else if (string.Compare(SpaceName, "CoridorSpace") == 0)
            {
                SpawnPainting(wall.transform);
            } else
            {
                Debug.Log("SpaceName is " + SpaceName);
            }
        }
    }

    void InitInteriorPrefabs()
    {
        List<GameObject> TempPrefabs = new List<GameObject>();
        TempPrefabs.Add(Resources.Load("Models/Props/Painting") as GameObject);
        Prefabs.Add("CoridiorSpace", TempPrefabs);

        TempPrefabs.Clear();
        TempPrefabs.Add(Resources.Load("Models/Props/Bath") as GameObject);
        TempPrefabs.Add(Resources.Load("Models/Props/Sink") as GameObject);
        Prefabs.Add("RoomSpace", TempPrefabs);
    }

    void SpawnPainting(Transform wall)
    {
        var prop = Instantiate(Prefabs[SpaceName][0], wall.transform);
    }
}
