using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEditor.ShaderGraph.Internal;
using Unity.Netcode;
using Unity.VisualScripting;
using System;
using Unity.Mathematics;

public static class LoadChunks 
{
    public struct ChunkBlock : INetworkSerializable
    {
        public int BlockType;
        public int BlockHeight;
        public int BlockRotation;
        public int BlockOre;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref BlockType);
            serializer.SerializeValue(ref BlockHeight);
            serializer.SerializeValue(ref BlockRotation);
            serializer.SerializeValue(ref BlockOre);
        }
    }
    public struct Chunk : INetworkSerializable
    {
        public ChunkBlock[] Blocks;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (Blocks != null && Blocks.Length != 0)
            {
                for (int i = 0; i < Blocks.Length; i++)
                {
                    ChunkBlock block = Blocks[i];
                    serializer.SerializeValue(ref block);

                }
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public static void LoadNewChunksServerRpc(Vector3 PlayerPosition, List<Vector2> LoadedChunks, List<Vector2> RequestedChunks, int ChunkSize, int NumberOfChunks, ulong ClientId,int Seed)
    {   
        List<Vector2> chunksToLoad = TrackUnloadedChunks.GetUnloadedChunks(PlayerPosition, ChunkSize, NumberOfChunks,LoadedChunks,RequestedChunks);
        RequestedChunks.AddRange(chunksToLoad);
        foreach (Vector2 chunk in chunksToLoad)
        {
            float[,] Data = WorldGenerater.GenerateChunk(chunk,ChunkSize,Seed);
            if(ChunkInstantiate.instance == null)
            {
                RequestedChunks = TrackUnloadedChunks.RemoveRequestChunks(RequestedChunks,chunksToLoad);
                return;
            }
            ChunkInstantiate.instance.InstantiateChunkServerRpc(chunk,ChunkSize,Data,ClientId);
            LoadedChunks.Add(chunk);
        }
        if (chunksToLoad.Count > 0)
        {
            Debug.Log(chunksToLoad.Count);
            ReturnChunksListsClientRpc(LoadedChunks, RequestedChunks, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { ClientId } } });

        }
    }
    [ClientRpc]
    public static void ReturnChunksListsClientRpc(List<Vector2> LoadedChunksList, List<Vector2> RequestedChunksList, ClientRpcParams clientRpcParams)
    {
        TrackUnloadedChunks.SetLoadedChunks(LoadedChunksList);
        TrackUnloadedChunks.SetRequestedChunks(RequestedChunksList);

    }

    [ClientRpc]
    public static bool CheckPlayerHasInstantiatedClientRpc(ClientRpcParams clientRpcParams)
    {
        if (PlayerSetUp.instance == null) return false;
        return true;
    }
    public static List<Vector2> GetNearChunks(Vector3 PlayerPosition, int ChunkSize, int NumberOfChunks)
    {
        return TrackUnloadedChunks.GetNearChunks(PlayerPosition, ChunkSize, NumberOfChunks);
    }



}
