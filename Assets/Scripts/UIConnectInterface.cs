using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using Mirror.Discovery;
using System;

public class UIConnectInterface : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private GameObject panelServersButton;
    [SerializeField] private GameObject serverButtonPrefab;
    [SerializeField] private InputField inputFieldIPServer;
    readonly Dictionary<long, DiscoveryResponse> discoveredServers = new Dictionary<long, DiscoveryResponse>();

    [SerializeField] private MainNetworkDiscovery networkDiscovery;
    private Coroutine findServersCoro;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (networkDiscovery == null)
        {
            networkDiscovery = GetComponent<MainNetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects(new UnityEngine.Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif
    public void FindServerBegin()
    {
        if (findServersCoro != null) StopCoroutine(findServersCoro);
        findServersCoro = StartCoroutine(FindServersCoroutine());
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
        hostButton.onClick.RemoveAllListeners();
        hostButton.onClick.AddListener(() =>
        {
            discoveredServers.Clear();
            FindServerStop();
            gameObject.GetComponent<MainNetworkRoomManager>().StartHost();
            //MainNetworkRoomManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
        });
    }
    public void FindServerStop()
    {
        if (findServersCoro != null) StopCoroutine(findServersCoro);
        networkDiscovery.StopDiscovery();
    }
    private void OnEnable()
    {
        FindServerBegin();
    }
    private void OnDisable()
    {
        if (findServersCoro != null) StopCoroutine(findServersCoro);
        networkDiscovery.StopDiscovery();
    }
    void Connect(DiscoveryResponse info)
    {
        networkDiscovery.StopDiscovery();
        NetworkManager.singleton.StartClient(info.uri);
    }
    public void StartHost()
    {
        //FindServerStop();
        //gameObject.GetComponent<MainNetworkRoomManager>().StartHost();
    }
    public void StartServer()
    {
        FindServerStop();
        gameObject.GetComponent<MainNetworkRoomManager>().StartServer();
    }
    public void StartClient()
    {
        FindServerStop();
        gameObject.GetComponent<MainNetworkRoomManager>().StartClient();
    }
    public void ChangeIPText()
    {
        if (inputFieldIPServer.text == "")
        {
            clientButton.interactable = false;
        }
        else
        {
            MainNetworkRoomManager.singleton.networkAddress = inputFieldIPServer.text;
            clientButton.interactable = true;            
        }
    }
    private IEnumerator FindServersCoroutine()
    {
        while(true)
        {
            foreach (Transform child in panelServersButton.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (DiscoveryResponse info in discoveredServers.Values)
            {
                GameObject serverButton = Instantiate(serverButtonPrefab, panelServersButton.transform);
                serverButton.GetComponent<Button>().GetComponentInChildren<Text>().text = info.hostPlayerName;//info.EndPoint.Address.ToString();
                serverButton.GetComponent<Button>().onClick.RemoveAllListeners();
                serverButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    inputFieldIPServer.text = info.EndPoint.Address.ToString();                    
                });
            }
            discoveredServers.Clear();
            yield return new WaitForSeconds(3f);            
        }        
    }
    public void OnDiscoveredServer(DiscoveryResponse info)
    {
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        discoveredServers[info.serverId] = info;
        
    }
}
