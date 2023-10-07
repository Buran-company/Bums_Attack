using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Traps", order = 2)]

public class TrapData : ScriptableObject
{
    public int damage;
    public bool isBum;
}
