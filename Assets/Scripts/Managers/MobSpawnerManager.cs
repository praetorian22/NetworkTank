using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MobSpawnerManager : GenericSingletonClass<MobSpawnerManager>
{
    [SerializeField] private GameObject _blueTankPrefab;
    [SerializeField] private GameObject _redTankPrefab;    
    [SerializeField] private Transform _parent;
    public List<GameObject> BlueTanks = new List<GameObject>();
    public List<GameObject> RedTanks = new List<GameObject>();       
    

    [Server]
    public void SpawnTank(typeTank typeTank, Vector3 position)
    {
        GameObject tank;
        if (typeTank == typeTank.blue)
        {
            tank = Instantiate(_blueTankPrefab, position, Quaternion.identity, _parent);            
            NetworkServer.Spawn(tank, connectionToClient);            
            BlueTanks.Add(tank);
            //tankCountChangeEvent?.Invoke(typeTank, BlueTanks.Count);
        }
        else
        {
            tank = Instantiate(_redTankPrefab, position, Quaternion.identity, _parent);            
            NetworkServer.Spawn(tank, connectionToClient);
            RedTanks.Add(tank);
            //tankCountChangeEvent?.Invoke(typeTank, RedTanks.Count);
        }
        tank.GetComponent<HealthScript>().ChangeEnemySet(typeTank);
        tank.GetComponent<HealthScript>().Init();        
        //tank.GetComponent<HealthScript>().deadEvent += DestroyTank;
        //tank.GetComponent<HealthScript>().shotEvent += _effectManager.ExplosionMini;
        //WeaponScript[] weaponScripts = tank.GetComponentsInChildren<WeaponScript>();
        //for (int i = 0; i < weaponScripts.Length; i++)
        //{
        //    weaponScripts[i].shotEvent += _effectManager.MakeEnemyShotSound;
        //}
    }

    public void DestroyTank(GameObject gameObject, typeTank typeTank)
    {
        if (typeTank == typeTank.red)
        {
            RedTanks.Remove(gameObject);
            //tankCountChangeEvent?.Invoke(typeTank, RedTanks.Count);
        }
        if (typeTank == typeTank.blue)
        {
            BlueTanks.Remove(gameObject);
            //tankCountChangeEvent?.Invoke(typeTank, BlueTanks.Count);
        }
        //tankDeadEvent?.Invoke(gameObject.transform.position);
        //if (RedTanks.Count == 0) noRedTanksEvent?.Invoke();
        Destroy(gameObject);
    }

    public void DestroyAllTank()
    {
        List<GameObject> BlueTanksCopy = new List<GameObject>(BlueTanks);
        List<GameObject> RedTanksCopy = new List<GameObject>(RedTanks);
        foreach (GameObject tank in BlueTanksCopy)
        {
            Destroy(tank);
            BlueTanks.Remove(tank);
        }
        foreach (GameObject tank in RedTanksCopy)
        {
            Destroy(tank);
            RedTanks.Remove(tank);
        }
    }    
}