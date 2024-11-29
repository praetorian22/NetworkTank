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
        if (isServer) ChangeHealthValue(name);
        else CmdChangeHealth(name);
    }

    private void Start()
    {
        ClientServerChangeName(DataPlayer.Instance.playerName);
    }

    [Server]
    public void ChangeHealthValue(string newValue)
    {
        playerName = newValue;
    }
    [Command]
    public void CmdChangeHealth(string newValue)
    {
        ChangeHealthValue(newValue);
    }
}
