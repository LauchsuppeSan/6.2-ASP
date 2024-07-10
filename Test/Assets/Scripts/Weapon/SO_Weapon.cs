using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Physical,
    Magical,
}
[CreateAssetMenu(menuName = "WeaponType")]

public class SO_Weapon : ScriptableObject
{
    public string Name;
    public int Damage;

    public DamageType damageType;
}