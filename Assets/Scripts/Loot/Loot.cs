using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Loot : MonoBehaviour
{
    [SerializeField] private lootType lootType;

    public abstract void ActivateEffect(UpgradeTank tank);    
}

public enum lootType
{
    mainWeapon,
    engine,
    armor,
    special
}
