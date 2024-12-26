using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootEngine : Loot
{
    public override void ActivateEffect(UpgradeTank tank)
    {
        tank.UpgradeEngine();
    }
}
