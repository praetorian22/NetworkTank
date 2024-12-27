using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpecial : Loot
{
    [SerializeField] typeSpecial special;
    public override void ActivateEffect(UpgradeTank tank)
    {
        tank.UpgradeSpecial(special);
    }
}
