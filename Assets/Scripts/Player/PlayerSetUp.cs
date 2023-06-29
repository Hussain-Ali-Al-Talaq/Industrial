using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static LoadChunks;

public class PlayerSetUp : NetworkBehaviour
{
    public static PlayerSetUp instance;
    public Chunk[,] ChunksListPP = new Chunk[1, 1];
    public Chunk[,] ChunksListPN = new Chunk[1, 1];
    public Chunk[,] ChunksListNP = new Chunk[1, 1];
    public Chunk[,] ChunksListNN = new Chunk[1, 1];
    public LoadChunks.Chunk[] ChunkDataBuffer = new LoadChunks.Chunk[25];
    public bool[] EmptyBufferSlot = new bool[25];
    public Vector2[] ChunkDataIndex = new Vector2[25];

    public int ChunkSize = 64;


    private void Start()
    {
        if (!IsLocalPlayer) return;
        instance = this;
        FillBufferWithEmpty();
    }
    private void Update()
    {
        if (!IsLocalPlayer) return;
        instance = this;
        LoadNewChunksServerRpc(transform.position,TrackUnloadedChunks.GetLoadedChunks(), TrackUnloadedChunks.GetRequestedChunks(), ChunkSize, 5, OwnerClientId, SceneManager.instence.Seed.Value);

    }
    private void LateUpdate()
    {
        if (!IsLocalPlayer) return;
        LoadNearChunks();
    }
    public void AddChunkToChunksList(Chunk chunk,Vector2 ChunkLocation)
    {
        if(ChunkLocation.x >= 0 && ChunkLocation.y >= 0) 
        {
            ChunksListPP = CheckIfChunkInBound(ChunksListPP, ChunkLocation);
            ChunksListPP[(int)ChunkLocation.x, (int)ChunkLocation.y] = chunk;
        }
        else if(ChunkLocation.x >= 0 && ChunkLocation.y < 0)
        {
            ChunksListPN = CheckIfChunkInBound(ChunksListPN, new Vector2((int)ChunkLocation.x, (int)MathF.Abs(ChunkLocation.y)));
            ChunksListPN[(int)ChunkLocation.x, (int)MathF.Abs(ChunkLocation.y)] = chunk;
        }
        else if (ChunkLocation.x < 0 && ChunkLocation.y >= 0)
        {
            ChunksListNP = CheckIfChunkInBound(ChunksListNP, new Vector2((int)MathF.Abs(ChunkLocation.x), (int)ChunkLocation.y));
            ChunksListNP[(int)MathF.Abs(ChunkLocation.x), (int)ChunkLocation.y] = chunk;
        }
        else if (ChunkLocation.x < 0 && ChunkLocation.y < 0)
        {
            ChunksListNN = CheckIfChunkInBound(ChunksListNN, new Vector2((int)MathF.Abs(ChunkLocation.x), (int)MathF.Abs(ChunkLocation.y)));
            ChunksListNN[(int)MathF.Abs(ChunkLocation.x), (int)MathF.Abs(ChunkLocation.y)] = chunk;
        }
    }
    private void LoadNearChunks()
    {
        foreach (Vector2 chunk in GetNearChunks(transform.position, ChunkSize, 5))
        {
            Chunk chunkData = GetChunk(chunk);
            if(chunkData != null)
            {
                chunkData.EnableChunk();
            }
        }


    }

    public Chunk GetChunk(Vector2 Chunk)
    {
        if (Chunk.x >= 0 && Chunk.y >= 0)
        {
            ChunksListPP = CheckIfChunkInBound(ChunksListPP, Chunk);
            if (ChunksListPP[(int)Chunk.x, (int)Chunk.y] != null)
            {
                return ChunksListPP[(int)Chunk.x, (int)Chunk.y];
            }
        }
        else if (Chunk.x >= 0 && Chunk.y < 0)
        {
            ChunksListPN = CheckIfChunkInBound(ChunksListPN, Chunk);
            if (ChunksListPN[(int)Chunk.x, (int)MathF.Abs(Chunk.y)] != null)
            {
                return ChunksListPN[(int)Chunk.x, (int)MathF.Abs(Chunk.y)];
            }
        }
        else if (Chunk.x < 0 && Chunk.y >= 0)
        {
            ChunksListNP = CheckIfChunkInBound(ChunksListNP, Chunk);
            if (ChunksListNP[(int)MathF.Abs(Chunk.x), (int)Chunk.y] != null)
            {
                return ChunksListNP[(int)MathF.Abs(Chunk.x), (int)Chunk.y];
            }
        }
        else if (Chunk.x < 0 && Chunk.y < 0)
        {
            ChunksListNN = CheckIfChunkInBound(ChunksListNN, Chunk);
            if (ChunksListNN[(int)MathF.Abs(Chunk.x), (int)MathF.Abs(Chunk.y)] != null)
            {
                return ChunksListNN[(int)MathF.Abs(Chunk.x), (int)MathF.Abs(Chunk.y)];
            }
        }
        return null;
    }
    private Chunk[,] CheckIfChunkInBound(Chunk[,] ChunkArray,Vector2 ChunkPosition)
    {
        Chunk[,] chunkData = CopyChunkToNewChunk(ChunkArray, ChunkArray.GetLength(0), ChunkArray.GetLength(1));

        if (Mathf.Abs(ChunkPosition.x) > (ChunkArray.GetLength(0) -1) && Mathf.Abs(ChunkPosition.y) > (ChunkArray.GetLength(1) -1))
        {
            chunkData = CopyChunkToNewChunk(ChunkArray, (int)Mathf.Abs(ChunkPosition.x) + 1, (int)Mathf.Abs(ChunkPosition.y) + 1);

        }
        else if (Mathf.Abs(ChunkPosition.y) > (ChunkArray.GetLength(1) -1))
        {
            chunkData = CopyChunkToNewChunk(ChunkArray, ChunkArray.GetLength(0), (int)Mathf.Abs(ChunkPosition.y) + 1);
                
        }
        else if (Mathf.Abs(ChunkPosition.x) > (ChunkArray.GetLength(0) -1))
        {
            chunkData = CopyChunkToNewChunk(ChunkArray, (int)Mathf.Abs(ChunkPosition.x) + 1, ChunkArray.GetLength(1));

        }

        return chunkData;
    }
    private Chunk[,] CopyChunkToNewChunk(Chunk[,] OldChunk,int NewChunkWidth,int NewChunkHeight)
    {
        Chunk[,] NewChunk = new Chunk[NewChunkWidth, NewChunkHeight];

        for(int y = 0; y < OldChunk.GetLength(1); y++)
        {
            for (int x = 0; x < OldChunk.GetLength(0); x++)
            {
                NewChunk[x,y] = OldChunk[x,y];
            }
        }
        return NewChunk;
    }
    public int GetEmptyBufferIndex()
    {
        int index = 0;
        foreach(LoadChunks.Chunk chunk in ChunkDataBuffer)
        {
            if(EmptyBufferSlot[index])
            {
                EmptyBufferSlot[index] = false;
                return index;
            }
            index++;
        }
        return -1;
    } 
    public LoadChunks.Chunk FindAndRemoveChunkData(Vector2 ChunkPosision,out bool Successful)
    {
        int index = 0;
        LoadChunks.Chunk Chunk;
        foreach (Vector2 chunkPosition in ChunkDataIndex)
        {
            if (chunkPosition == ChunkPosision)
            {
                Chunk = ChunkDataBuffer[index];
                EmptyBufferSlot[index] = true;
                ChunkDataIndex[index] = Vector2.zero;

                Successful = true;
                return Chunk;
            }
            index++;
        }
        Successful = false;
        return new LoadChunks.Chunk();
    }
    private void FillBufferWithEmpty()
    {
        for (int i=0;i<ChunkDataBuffer.Length;i++)
        {
            EmptyBufferSlot[i] = true;
        }
    } 

}
