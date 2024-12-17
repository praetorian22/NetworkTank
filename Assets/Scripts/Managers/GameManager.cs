using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

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
    private GameObject _player;

    public void SetPlayer(GameObject player)
    {
        if (_player != null)
        {
            _player.GetComponent<HealthScript>().deadEvent -= PlayerDead;
        }
        _player = player;        
        _player.GetComponent<HealthScript>().deadEvent += PlayerDead;
    }

    private void PlayerDead(GameObject gameObject, typeTank typeTank)
    {
        if (isServer)
        {
            gameObject.SetActive(false);
            RpcPlayerDead(gameObject, typeTank);
        }
        else
        {
            CmdPlayerDead(gameObject, typeTank);
        }
    }
    [Command(requiresAuthority = false)]
    private void CmdPlayerDead(GameObject gameObject, typeTank typeTank)
    {
        gameObject.SetActive(false);
        RpcPlayerDead(gameObject, typeTank);
    }
    [ClientRpc]
    private void RpcPlayerDead(GameObject gameObject, typeTank typeTank)
    {
        gameObject.SetActive(false);
    }
}
