using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsGenerator : MonoBehaviour
{
    private GameObject []Walls;
    public GameObject PaintingPrefab;
    void Start()
    {
        Walls = GameObject.FindGameObjectsWithTag("Wall");

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
