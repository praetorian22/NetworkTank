using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaranScript : MonoBehaviour
{
    [SerializeField] private HealthScript _healthScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthScript healthScript = collision.GetComponent<TaranScript>()._healthScript;

        if (healthScript != null && healthScript.IsEnemy != this._healthScript.IsEnemy)
        {
            healthScript.Damage(1);
            this._healthScript.Damage(1);
        }
    }
}
