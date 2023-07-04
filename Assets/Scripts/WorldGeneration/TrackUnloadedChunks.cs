using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackUnloadedChunks 
{
    [SerializeField] private static List<Vector2> LoadedChunks = new List<Vector2>();
    [SerializeField] private static List<Vector2> RequestedChunks = new List<Vector2>();

    public static Vector2 CalculatePlayerChunk(Vector3 PlayerPosition,int ChunkSize)
    {
        Vector2 playerChunk = new Vector2((int)PlayerPosition.x/ChunkSize, (int)PlayerPosition.z/ChunkSize);

        return playerChunk;
    }

    public static List<Vector2> GetUnloadedChunks(Vector3 PlayerPosition, int ChunkSize,int NumberOfChunks, List<Vector2> LoadedChunks, List<Vector2> RequestedChunks)
    {
        Vector2 PlayerChunk = CalculatePlayerChunk(PlayerPosition, ChunkSize);

        List<Vector2> unLoadedChunks = new List<Vector2>();

        if ((!LoadedChunks.Contains(PlayerChunk)) && (!RequestedChunks.Contains(PlayerChunk)))
        {
            unLoadedChunks.Add(PlayerChunk);
        }

        foreach(Vector2 Chunk in GetNearChunks(PlayerPosition, ChunkSize, NumberOfChunks))
        {
            if ((!LoadedChunks.Contains(Chunk)) && (!RequestedChunks.Contains(Chunk)) && (Chunk != PlayerChunk))
            {
                unLoadedChunks.Add(Chunk);
            }
        }

        return unLoadedChunks;
    }
    public static List<Vector2> GetNearChunks(Vector3 PlayerPosition, int ChunkSize, int NumberOfChunks)
    {
        Vector2 PlayerChunk = CalculatePlayerChunk(PlayerPosition, ChunkSize);

        List<Vector2> nearChunks = new List<Vector2>();

        for (int y = 0; y < NumberOfChunks; y++)
        {
            for (int x = 0; x < NumberOfChunks; x++)
            {
                Vector2 Chunk = new Vector2(x - NumberOfChunks / 2 + PlayerChunk.x, y - NumberOfChunks / 2 + PlayerChunk.y);
                nearChunks.Add(Chunk);
            }
        }
        return nearChunks;
    }

    public static void AddLoadedChunks(List<Vector2> loadedChunks)
    {
        LoadedChunks.AddRange(loadedChunks);
    }
    public static void AddRequestChunks(List<Vector2> requestedChunks)
    {
        RequestedChunks.AddRange(requestedChunks);
    }
    public static List<Vector2> RemoveRequestChunks(List<Vector2> requestedChunksList, List<Vector2> requestedChunks)
    {
        for (int i = 0; i < requestedChunks.Count; i++)
        {
            requestedChunksList.Remove(requestedChunks[i]);
        }
        return requestedChunksList;
    }
    public static List<Vector2> GetLoadedChunks()
    {
        return LoadedChunks;
    }
    public static List<Vector2> GetRequestedChunks()
    {
        return RequestedChunks;
    }
    public static void SetLoadedChunks(List<Vector2> LoadedChunksList)
    {
        LoadedChunks = LoadedChunksList;

    }
    public static void SetRequestedChunks(List<Vector2> RequestedChunksList)
    {
        RequestedChunks = RequestedChunksList;
    }
}
