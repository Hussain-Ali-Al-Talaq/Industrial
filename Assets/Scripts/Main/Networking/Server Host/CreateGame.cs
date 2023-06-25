using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System.Threading.Tasks;
using Unity.Collections;

public class CreateGame : NetworkBehaviour
{
    private UnityTransport unityTransport;
    private int MaxPlayers = 8;

    public async void Create()
    {
        unityTransport = FindObjectOfType<UnityTransport>();

        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(MaxPlayers);
        Debug.Log(await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId));

        unityTransport.SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);

        NetworkManager.Singleton.StartHost();

        NetworkManager.SceneManager.OnLoad += SceneManager_OnLoad;
        NetworkManager.SceneManager.OnLoadComplete += SceneManager_OnLoadComplete;
        NetworkManager.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        NetworkManager.SceneManager.LoadScene("GameWorld", LoadSceneMode.Single);
    }



    private void SceneManager_OnLoad(ulong clientId, string sceneName, LoadSceneMode loadSceneMode, AsyncOperation asyncOperation)
    {
        Debug.Log("Scene Loading");
        NetworkManager.SceneManager.OnLoad -= SceneManager_OnLoad;
    }

    private void SceneManager_OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        Debug.Log("Scene Loaded");
        NetworkManager.SceneManager.OnLoadComplete -= SceneManager_OnLoadComplete;
    }
    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, System.Collections.Generic.List<ulong> clientsCompleted, System.Collections.Generic.List<ulong> clientsTimedOut)
    {
        Debug.Log("Done");
        NetworkManager.SceneManager.OnLoadEventCompleted -= SceneManager_OnLoadEventCompleted;
    }
}
