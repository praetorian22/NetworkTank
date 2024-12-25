using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootArmor : Loot
{
    public override void ActivateEffect(UpgradeTank tank)
    {
        tank.UpgradeArmor();
    }
}
