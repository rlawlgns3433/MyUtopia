using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalStat", menuName = "Animal/Default")]
public class AnimalStat : ScriptableObject
{
    [Range(0f, 10f)]
    public float walkSpeed;
    [Range(0f, 10f)]
    public float runSpeed;
    [Range(0f, 10f)]
    public float idleTime;
}