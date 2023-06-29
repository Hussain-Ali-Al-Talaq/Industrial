using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CreateMenu : MonoBehaviour
{
    [SerializeField] private Button CreateButton;
    [SerializeField] private TMP_InputField WorldName;
    [SerializeField] private TMP_InputField Seed;

    string Numbers = "0123456789";

    private void Awake()
    {
        UpdateCreateButton();
    }
    public void UpdateCreateButton()
    {
        if (ValidateString(WorldName.text) && Seed.text.Length > 0)
        {
            CreateButton.interactable = true;
        }
        else
        {
            CreateButton.interactable = false;
        }
    }
    private bool ValidateString(string str)
    {   
        if(str.Length < 0)
        {
            return false;
        }
        foreach(char letter in str)
        {
            if(letter != " ".ToCharArray()[0])
            {
                return true;
            }
        }
        return false;
    }
    public void RandomiesSeed()
    {
        string seed = "";

        for(int i = 0; i < 6; i++)
        {
            seed += Numbers[UnityEngine.Random.Range(0, Numbers.Length - 1)];
        }
        Seed.text = seed;
    }
    private int StringToInt(string String)
    {
        int number = 0;
        int Stringindex = 0;
        foreach(char letter in String)
        {
            int Numberindex = 0;

            foreach (char num in Numbers)
            {
                if(letter == num)
                {
                    number += Numberindex * ((int)Mathf.Pow(10, String.Length - Stringindex - 1));
                }
                Numberindex++;

            }
            Stringindex++;
        }
        return number;
    }
    public void CreateGame()
    {
        SceneManager.instence.Seed = new NetworkVariable<int>(StringToInt(Seed.text),NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
        FindObjectOfType<CreateGame>().Create();
    }

}
