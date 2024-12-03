using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;


public class UIDataPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(SyncName))] public string playerName;
    [SyncVar(hook = nameof(SyncType))] public typeTank playerType;
    public Text nameText;
    public Image color;
    public Button buttonChangeType;

    void SyncName(string oldvalue, string newValue)
    {
        nameText.text = newValue;
    }
    void SyncType(typeTank oldvalue, typeTank newValue)
    {
        if (newValue == typeTank.blue)
        {
            color.color = Color.blue;
        }
        else
        {
            color.color = Color.red;
        }
    }
    public void ClientServerChangeName(string name)
    {
        if (isServer) ChangeName(name);
        else
        { 
            CmdChangeName(name); 
        }
    }
    public void ClientServerChangeType(typeTank type)
    {
        DataPlayer.Instance.type = type;
        if (isServer) ChangeType(type);
        else CmdChangeType(type);
    }

    private void Start()
    {
        ClientServerChangeName(DataPlayer.Instance.playerName);
        ClientServerChangeType(DataPlayer.Instance.type);
        buttonChangeType.onClick.RemoveAllListeners();
        buttonChangeType.onClick.AddListener(() =>
        {
            if (playerType == typeTank.red)
                ClientServerChangeType(typeTank.blue);
            else
                ClientServerChangeType(typeTank.red);
        });
    }
    public override void OnStartAuthority() 
    {
        
    }
    public override void OnStopAuthority()
    {
        
    }
    [Server]
    public void ChangeName(string newValue)
    {
        playerName = newValue;
    }
    [Command]
    public void CmdChangeName(string newValue)
    {
        ChangeName(newValue);
    }
    [Server]
    public void ChangeType(typeTank newValue)
    {
        playerType = newValue;
    }
    [Command]
    public void CmdChangeType(typeTank newValue)
    {
        ChangeType(newValue);
    }
}
