using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using Unity.Mathematics;
using TMPro;

public class JoinGame : NetworkBehaviour
{
    private UnityTransport unityTransport;

    private NetworkVariable<string> HostVersion = new NetworkVariable<string>(null,NetworkVariableReadPermission.Owner,NetworkVariableWritePermission.Server);
    public async void Join(string JoinCode)
    {
        unityTransport = FindObjectOfType<UnityTransport>();

        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(JoinCode);

        unityTransport.SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);

        NetworkManager.Singleton.StartClient();
        
        GetHostVersionServerRpc();
        Debug.Log(HostVersion);
    }
    [ServerRpc(RequireOwnership = false)]
    private void GetHostVersionServerRpc()
    {
        string StringVersion = Application.version;
        HostVersion = new NetworkVariable<string>(StringVersion);
    }
}
