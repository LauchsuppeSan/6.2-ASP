using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyType")]
public class SO_Enemy : ScriptableObject
{
    [SerializeField] int PhysicalRisistance;
    [SerializeField] int MagicalResistance;
    public int TestDamage;
    [SerializeField] string EnemyName;
    
}
