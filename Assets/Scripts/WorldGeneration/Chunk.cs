using Unity.Netcode;
using UnityEngine;

public class Chunk : NetworkBehaviour
{
    [SerializeField] private GameObject TilesContainer;
    public LoadChunks.Chunk ChunkTiles;
    public Vector2 Location;
    public bool DisableOnServer;
    private bool FoundData;

    private void Update()
    {   
        if(NetworkManager.LocalClientId != 0)
        {
            if(NetworkManager.LocalClientId != OwnerClientId)
            {
                HideNetworkObjectServerRpc(NetworkManager.LocalClientId);
            }
        }
        if(IsServer || IsHost)
        {
            if (DisableOnServer)
            {
                TilesContainer.SetActive(false);
            }
        }

        if (!IsOwner) return;
        TilesContainer.SetActive(false);

        if (!FoundData)
        {
            ChunkTiles = PlayerSetUp.instance.FindAndRemoveChunkData(Location,out bool successful);

            if (!successful)
            {
                Debug.Log("FoundChunkData And Location Is: " + Location);
                FoundData = true;
            }
        }

    }
    private void Start()
    {
        if (!IsOwner) return;
        Vector3 ChunkPosition = transform.position / PlayerSetUp.instance.ChunkSize;
        Location = new Vector2(ChunkPosition.x, ChunkPosition.z);
        if(!DisableOnServer && OwnerClientId == 0)
        {
            PlayerSetUp.instance.AddChunkToChunksList(this, Location);
        }
    }
    public void EnableChunk()
    {
        TilesContainer.SetActive(true);

    }
    [ServerRpc(RequireOwnership = false)]
    private void HideNetworkObjectServerRpc(ulong clientId)
    {
        if (NetworkObject.IsNetworkVisibleTo(clientId))
        {
            NetworkObject.NetworkHide(clientId);
        }
    }

}   
