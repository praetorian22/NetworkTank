using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Linq;

public class GameManager : GenericSingletonClass<GameManager>
{
    //public GameObject coinPrefab;

    //[SyncVar] public int globalCoins;

    //public Text globalCoinsText;
    //public Text coinsText;
    /*
    private void Start()
    {
        if (isServer)
        {
            for(int  i = 0; i < 5; i++)
            {
                Vector2 position = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);
                NetworkServer.Spawn(coin);
            }
        }
    }
    
    public void StopGame()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            if (NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopClient();
            }
            else
            {
                if (NetworkServer.active)
                {
                    NetworkManager.singleton.StopServer();
                }
            }
        }
    }
    */
    
    [Server]
    public void DestroyTank(GameObject gameObject, typeTank typeTank)
    {
        EffectManage.Instance.Explosion(gameObject.transform.position);
        NetworkServer.Destroy(gameObject);
    }
       
}
