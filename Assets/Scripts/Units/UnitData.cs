using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Units", order = 1)]
public class UnitData : ScriptableObject
{
    public int hp;
    public int damage;
    public float speed;
    public float attackSpeed;
    public float attackDistance;
    public float fieldOfViewRadius;
    public bool isBum;
    public bool isTurnedLeft;
}
