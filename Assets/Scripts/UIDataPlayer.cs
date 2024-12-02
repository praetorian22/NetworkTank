using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UIDataPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(SyncName))] public string playerName;
    public Text nameText;    

    void SyncName(string oldvalue, string newValue)
    {
        nameText.text = newValue;
    }
    public void ClientServerChangeName(string name)
    {
        if (isServer) ChangeName(name);
        else CmdChangeName(name);
    }

    private void Start()
    {
        ClientServerChangeName(DataPlayer.Instance.playerName);
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
}
