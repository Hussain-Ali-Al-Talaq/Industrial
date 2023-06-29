using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class SceneManager : NetworkBehaviour
{
    public static SceneManager instence;

    public NetworkVariable<int> Seed;
    public NetworkVariable<FixedString32Bytes> JoinCode;
    private void Awake()
    {
        if(instence == null)
        {
            instence = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (instence == null)
        {
            instence = this;
        }
    }


}
