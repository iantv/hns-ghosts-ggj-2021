using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObject/Settings")]
public class CharacterControllerSettings : ScriptableObject
{
    public float radius;
    public float height;
    public float centerY;
}
