using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class EnemyShot : NetworkBehaviour
{
    [SerializeField] private List<WeaponScript> weaponScripts;

    private void OnEnable()
    {
        ActivateCannon(0);
        //GetComponentInChildren<UpgradeTank>().upgradeEvent += ActivateCannon;
    }
    private void OnDisable()
    {
        //GetComponentInChildren<UpgradeTank>().upgradeEvent -= ActivateCannon;
    }

    private void Update()
    {
        foreach (WeaponScript weaponScript in weaponScripts)
        {
            if (weaponScript.enabled)
                weaponScript.ShotNow(transform.rotation);
        }         
    }

    private void ActivateCannon(int level)
    {
        if (level == 0)
        {
            weaponScripts[0].enabled = true;
            weaponScripts[1].enabled = false;
            weaponScripts[2].enabled = false;
            weaponScripts[3].enabled = false;
        }
        if (level == 1)
        {
            weaponScripts[0].enabled = true;
            weaponScripts[1].enabled = false;
            weaponScripts[2].enabled = false;
            weaponScripts[3].enabled = false;
        }
        if (level == 2)
        {
            weaponScripts[0].enabled = false;
            weaponScripts[1].enabled = true;
            weaponScripts[2].enabled = true;
            weaponScripts[3].enabled = true;
        }
    }
}
