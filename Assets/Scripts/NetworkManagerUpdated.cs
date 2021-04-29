using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManagerUpdated : NetworkManager
{
    public static Dictionary<int, GameObject> playerObjects; // Used to store each player's game object

    public override void OnStartHost()
    {
        base.OnStartHost();

        playerObjects = new Dictionary<int, GameObject>();
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        playerObjects[conn.connectionId] = player; // Add the player to our dictionary

        player.GetComponent<PlayerID>().ID = conn.connectionId; // Store the player's ID

        // instantiating a "Player" prefab gives it the name "Player(clone)"
        // => appending the connectionId is WAY more useful for debugging!
        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
        NetworkServer.AddPlayerForConnection(conn, player);

        // Loop through each of the currently connected players, and update their health display
        foreach (KeyValuePair<int, GameObject> playerObject in playerObjects)
        {
            // We don't need to sync our own data as it is already loaded
            if (playerObject.Key != conn.connectionId)
            {
                // Try to get the player's health object
                if (playerObject.Value.TryGetComponent(out Health health))
                {
                    health.UpdateHealthDisplay(); // Update the player's health display
                }
            }
        }
    }

    // Remove player's from the dictionary on disconnect
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        if (playerObjects.ContainsKey(conn.connectionId))
        {
            playerObjects.Remove(conn.connectionId);
        }
    }
}
