using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootWeapon : Loot
{
    [SerializeField] private weaponType weaponType;
    public override void ActivateEffect(UpgradeTank tank)
    {
        tank.UpgaredWeapon(weaponType);
    }    
}
