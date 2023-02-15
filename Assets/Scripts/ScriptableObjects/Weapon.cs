using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireType
{
    None = 0,
    Semi,
    Auto
}
[CreateAssetMenu(menuName = "Objects/Weapon")]
public class Weapon : ScriptableObject
{
    public new string name;
    public FireType fireType;
    public float timeToReload;
    public int bulletCount;
    public float timeToShoot;
    public float damage;
    public float range;
    public int magazineSize;
    public GameObject prefab;
}