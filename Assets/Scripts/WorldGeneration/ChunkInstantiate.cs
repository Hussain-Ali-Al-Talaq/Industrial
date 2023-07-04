using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using static LoadChunks;

public class ChunkInstantiate : NetworkBehaviour
{
    public static ChunkInstantiate instance;
    public static ChunkInstantiate Serverinstance;


    [SerializeField] private GameObject ChunkObject;
    [SerializeField] private Transform ChunksContainer;
    private void Awake()
    {
        instance = this;
    }

    [ServerRpc(RequireOwnership = false)]
    public void InstantiateChunkServerRpc(Vector2 ChunkPosistion,int ChunkSize, float[,] Data,ulong ClientId)
    {
        GameObject Object = Instantiate(ChunkObject, ChunksContainer.position + new Vector3(ChunkPosistion.x,0, ChunkPosistion.y) * ChunkSize, Quaternion.identity, ChunksContainer);

        NetworkObject NetworkObject = Object.GetComponent<NetworkObject>();
        NetworkObject.SpawnWithOwnership(ClientId);
        Object.transform.parent = ChunksContainer;

        if (ClientId != 0)
        {
            Object.GetComponent<Chunk>().DisableOnServer = true;
        }

        NetworkObject.CheckObjectVisibility = (clientId) => {

            if (NetworkManager.Singleton.ConnectedClients[clientId].ClientId == ClientId)
            {
                return true;
            }
            else
            {
                return false;
            }
        };

        LoadChunks.Chunk ChunkData = new LoadChunks.Chunk();
        ChunkData.Blocks = new ChunkBlock[1];
        ChunkData.Blocks[0].BlockType = 1;
        ChunkData.Blocks[0].BlockHeight = 1;
        ChunkData.Blocks[0].BlockRotation = 1;
        ChunkData.Blocks[0].BlockOre = 1;

        SetChunkDataClientRpc(ChunkPosistion, ChunkData, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong>() { ClientId } } });
        

    }
    [ClientRpc]
    public void SetChunkDataClientRpc(Vector2 ChunkPosistion, LoadChunks.Chunk ChunkData, ClientRpcParams clientRpcParams)
    {
        int EmptyBufferIndex = PlayerSetUp.instance.GetEmptyBufferIndex();
        if (EmptyBufferIndex > -1)
        {
            PlayerSetUp.instance.ChunkDataIndex[EmptyBufferIndex] = ChunkPosistion;
            PlayerSetUp.instance.ChunkDataBuffer[EmptyBufferIndex] = ChunkData;
        }
        else
        {
            Debug.LogError("ChunkBufferIsFull index = " + EmptyBufferIndex);
        }

    }


    private void Update()
    {
        instance = this;
    }
}
