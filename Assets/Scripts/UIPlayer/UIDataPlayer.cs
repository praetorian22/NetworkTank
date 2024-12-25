using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Unity.Collections.LowLevel.Unsafe;


public class UIDataPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(SyncName))] public string playerName;
    [SyncVar(hook = nameof(SyncType))] public typeTank playerType;
    public Text nameText;
    public Image color;
    public Button buttonChangeType;
    private bool initOK;

    private void Update()
    {
        SetColor();
        SetName();
    }
    private void SetColor()
    {
        if (playerType == typeTank.blue)
        {
            color.color = Color.blue;
        }
        else
        {
            color.color = Color.red;
        }
    }
    private void SetName()
    {
        nameText.text = playerName;
    }
    void SyncName(string oldvalue, string newValue)
    {
        this.playerName = newValue;
        //SetName();
    }
    void SyncType(typeTank oldvalue, typeTank newValue)
    {
        this.playerType = newValue;
        //SetColor();
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
        GameManager.singleton.type = type;
        if (isServer) ChangeType(type);
        else CmdChangeType(type);
    }

    public void Init()
    {
        if (initOK) return;
        ClientServerChangeName(GameManager.singleton.playerName);
        ClientServerChangeType(GameManager.singleton.type);
        buttonChangeType.onClick.RemoveAllListeners();
        if (isOwned)
        {
            buttonChangeType.onClick.AddListener(() =>
            {
                if (playerType == typeTank.red)
                    ClientServerChangeType(typeTank.blue);
                else
                    ClientServerChangeType(typeTank.red);
            });
        }
        initOK = true;
    }
    
    [Server]
    public void ChangeName(string newValue)
    {
        SyncName("", newValue);
    }
    [Command(requiresAuthority = false)]
    public void CmdChangeName(string newValue)
    {
        ChangeName(newValue);
    }
    [Server]
    public void ChangeType(typeTank newValue)
    {
        SyncType(typeTank.red, newValue);
    }
    [Command(requiresAuthority = false)]
    public void CmdChangeType(typeTank newValue)
    {
        ChangeType(newValue);
    }    
}
