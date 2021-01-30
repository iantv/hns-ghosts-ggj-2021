using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropsGenerator : MonoBehaviour
{
    private List<GameObject> Walls;
    public GameObject PaintingPrefab;
    void Start()
    {
        Walls = new List<GameObject>();
        Walls.AddRange(GameObject.FindGameObjectsWithTag("Wall"));
        foreach (var wall in Walls)
        {
            int r = Random.Range(0, 4);
            if (r % 4 != 0) continue;
            var prop = Instantiate(PaintingPrefab, wall.transform);
        }
    }

    void Update()
    {

    }
}
