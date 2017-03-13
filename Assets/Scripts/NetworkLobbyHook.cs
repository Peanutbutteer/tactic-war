using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class NetworkLobbyHook : LobbyHook {
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        PlayerMageController mage = gamePlayer.GetComponent<PlayerMageController>();

        mage.color = lobby.playerColor;
        mage.playerName = lobby.name;
        mage.SetPlayerId(lobby.playerId);
    }
}
