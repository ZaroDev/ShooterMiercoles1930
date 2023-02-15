using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Objects/WeaponInventory")]
public class WeaponInventory : ScriptableObject
{
    public List<Weapon> weapons;
}
