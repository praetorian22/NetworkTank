using UnityEngine;
using Mirror;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-room-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkRoomManager.html

	See Also: NetworkManager
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

/// <summary>
/// This is a specialized NetworkManager that includes a networked room.
/// The room has slots that track the joined players, and a maximum player count that is enforced.
/// It requires that the NetworkRoomPlayer component be on the room player objects.
/// NetworkRoomManager is derived from NetworkManager, and so it implements many of the virtual functions provided by the NetworkManager class.
/// </summary>
public class MainNetworkRoomManager : NetworkRoomManager
{    
    private Button startButton;
    public GameObject UIPlayerPrefab;
    private static int startPositionIndexBlue;
    private static int startPositionIndexRed;
    // Overrides the base singleton so we don't
    // have to cast to this type everywhere.
    public static new MainNetworkRoomManager singleton => (MainNetworkRoomManager)NetworkRoomManager.singleton;

    #region Server Callbacks

    /// <summary>
    /// This is called on the server when the server is started - including when a host is started.
    /// </summary>
    public override void OnRoomStartServer() { }

    /// <summary>
    /// This is called on the server when the server is stopped - including when a host is stopped.
    /// </summary>
    public override void OnRoomStopServer() { }

    /// <summary>
    /// This is called on the host when a host is started.
    /// </summary>
    public override void OnRoomStartHost() { }

    /// <summary>
    /// This is called on the host when the host is stopped.
    /// </summary>
    public override void OnRoomStopHost() { }

    /// <summary>
    /// This is called on the server when a new client connects to the server.
    /// </summary>
    /// <param name="conn">The new connection.</param>
    public override void OnRoomServerConnect(NetworkConnectionToClient conn) 
    {        
        
    }

    /// <summary>
    /// This is called on the server when a client disconnects.
    /// </summary>
    /// <param name="conn">The connection that disconnected.</param>
    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn) { }

    /// <summary>
    /// This is called on the server when a networked scene finishes loading.
    /// </summary>
    /// <param name="sceneName">Name of the new scene.</param>
    public override void OnRoomServerSceneChanged(string sceneName) 
    {
        if (sceneName == RoomScene)
        {
            startButton = GameObject.FindWithTag("startButton").GetComponent<Button>();
            startButton.GetComponent<Image>().color = Color.white;
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(() =>
            {
                if (allPlayersReady)
                {
                    // set to false to hide it in the game scene
                    startButton.interactable = false;
                    ServerChangeScene(GameplayScene);
                }
            });            
            startButton.interactable = false;
        }
    }

    /// <summary>
    /// This allows customization of the creation of the room-player object on the server.
    /// <para>By default the roomPlayerPrefab is used to create the room-player, but this function allows that behaviour to be customized.</para>
    /// </summary>
    /// <param name="conn">The connection the player object is for.</param>
    /// <returns>The new room-player object.</returns>
    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
    {
        return null;        
    }

    /// <summary>
    /// This allows customization of the creation of the GamePlayer object on the server.
    /// <para>By default the gamePlayerPrefab is used to create the game-player, but this function allows that behaviour to be customized. The object returned from the function will be used to replace the room-player on the connection.</para>
    /// </summary>
    /// Это позволяет настроить создание объекта GamePlayer на сервере.
    ///  По умолчанию для создания игрока используется gamePlayerPrefab, но эта функция позволяет настроить такое поведение.Объект, 
    ///  возвращаемый функцией, будет использоваться для замены игрока в комнате при подключении.
    /// <param name="conn">The connection the player object is for.</param>
    /// <param name="roomPlayer">The room player object for this connection.</param>
    /// <returns>A new GamePlayer object.</returns>
    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        return base.OnRoomServerCreateGamePlayer(conn, roomPlayer);
    }

    /// <summary>
    /// This allows customization of the creation of the GamePlayer object on the server.
    /// <para>This is only called for subsequent GamePlay scenes after the first one.</para>
    /// <para>See OnRoomServerCreateGamePlayer to customize the player object for the initial GamePlay scene.</para>
    /// </summary>
    /// <param name="conn">The connection the player object is for.</param>
    public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnRoomServerAddPlayer(conn);
    }
    public Transform GetStartPosition(typeTank typeTank = typeTank.blue)
    {
        // first remove any dead transforms
        startPositions.RemoveAll(t => t == null);

        if (startPositions.Count == 0)
            return null;

        if (typeTank == typeTank.blue)
        {
            List<Transform> startPositionsBlue = startPositions.Select(e => e).Where(e => e.gameObject.GetComponent<PointSpawn>().typeTank == typeTank.blue).ToList();
            if (playerSpawnMethod == PlayerSpawnMethod.Random)
                return startPositionsBlue[UnityEngine.Random.Range(0, startPositionsBlue.Count)];
            else
            {
                Transform startPosition = startPositionsBlue[startPositionIndexBlue];
                startPositionIndexBlue = (startPositionIndexBlue + 1) % startPositionsBlue.Count;
                return startPosition;
            }
        }
        else
        {
            List<Transform> startPositionsRed = startPositions.Select(e => e).Where(e => e.gameObject.GetComponent<PointSpawn>().typeTank == typeTank.red).ToList();
            if (playerSpawnMethod == PlayerSpawnMethod.Random)
                return startPositionsRed[UnityEngine.Random.Range(0, startPositionsRed.Count)];
            else
            {
                Transform startPosition = startPositionsRed[startPositionIndexRed];
                startPositionIndexRed = (startPositionIndexRed + 1) % startPositionsRed.Count;
                return startPosition;
            }
        }
    }
    /// <summary>
    /// This is called on the server when it is told that a client has finished switching from the room scene to a game player scene.
    /// <para>When switching from the room, the room-player is replaced with a game-player object. This callback function gives an opportunity to apply state from the room-player to the game-player object.</para>
    /// </summary>
    /// <param name="conn">The connection of the player</param>
    /// <param name="roomPlayer">The room player object.</param>
    /// <param name="gamePlayer">The game player object.</param>
    /// <returns>False to not allow this player to replace the room player.</returns>
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        if (gamePlayer.GetComponent<GamePlayer>() != null)
        {
            typeTank typeTank = roomPlayer.GetComponent<MainNetworkRoomPlayer>().uiDataPlayer.playerType;
            gamePlayer.transform.position = GetStartPosition(typeTank).position;
            gamePlayer.GetComponent<GamePlayer>().ChangeTypeTank(typeTank);
            //gamePlayer.GetComponent<PlayerController>().Init(typeTank);
        }
        return true;
    }

    /// <summary>
    /// This is called on server from NetworkRoomPlayer.CmdChangeReadyState when client indicates change in Ready status.
    /// </summary>
    public override void ReadyStatusChanged()
    {
        base.ReadyStatusChanged();
    }
    
    /// <summary>
    /// This is called on the server when all the players in the room are ready.
    /// <para>The default implementation of this function uses ServerChangeScene() to switch to the game player scene. By implementing this callback you can customize what happens when all the players in the room are ready, such as adding a countdown or a confirmation for a group leader.</para>
    /// </summary>
    public override void OnRoomServerPlayersReady()
    {
        if (Utils.IsHeadless())
        {
            base.OnRoomServerPlayersReady();            
        }
        else
        {
            if (startButton != null) startButton.interactable = true;            
        }        
    }

    /// <summary>
    /// This is called on the server when CheckReadyToBegin finds that players are not ready
    /// <para>May be called multiple times while not ready players are joining</para>
    /// </summary>
    public override void OnRoomServerPlayersNotReady() 
    {
        if (startButton != null) startButton.interactable = false;
    }

    #endregion

    #region Client Callbacks

    /// <summary>
    /// This is a hook to allow custom behaviour when the game client enters the room.
    /// Это функция, которая позволяет настроить поведение при входе игрового клиента в комнату.
    /// </summary>
    public override void OnRoomClientEnter() 
    {
        
    }

    /// <summary>
    /// This is a hook to allow custom behaviour when the game client exits the room.
    /// </summary>
    public override void OnRoomClientExit() { }

    /// <summary>
    /// This is called on the client when it connects to server.
    /// </summary>
    public override void OnRoomClientConnect() { }

    /// <summary>
    /// This is called on the client when disconnected from a server.
    /// </summary>
    public override void OnRoomClientDisconnect() { }

    /// <summary>
    /// This is called on the client when a client is started.
    /// </summary>
    public override void OnRoomStartClient() { }

    /// <summary>
    /// This is called on the client when the client stops.
    /// </summary>
    public override void OnRoomStopClient() { }

    /// <summary>
    /// This is called on the client when the client is finished loading a new networked scene.
    /// </summary>
    public override void OnRoomClientSceneChanged() { }

    #endregion

    #region Optional UI

    public override void OnGUI()
    {
        base.OnGUI();        
    }

    #endregion
}

public enum typeTank
{
    red,
    blue,
}