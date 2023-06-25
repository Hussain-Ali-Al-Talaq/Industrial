using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CreateMenu : MonoBehaviour
{
    [SerializeField] private Button CreateButton;
    [SerializeField] private TMP_InputField WorldName;
    [SerializeField] private TMP_InputField Seed;

    string Numbers = "1234567890";

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
            seed += Numbers[Random.Range(0, Numbers.Length - 1)];
        }
        Seed.text = seed;
    }
    public void CreateGame()
    {
        FindObjectOfType<CreateGame>().Create();
    }

}
