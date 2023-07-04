using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinMenu : MonoBehaviour
{
    [SerializeField] private Button JoinButton;
    [SerializeField] private TMP_InputField JoinCode;

    private void Awake()
    {
        UpdateJoinButton();
    }
    public void UpdateJoinButton()
    {
        if(ValidateString(JoinCode.text))
        {
            JoinButton.interactable = true;
        }
        else
        {
            JoinButton.interactable = false;

        }
    }
    private bool ValidateString(string str)
    {
        if (str.Length < 0)
        {
            return false;
        }
        foreach (char letter in str)
        {
            if (letter != " ".ToCharArray()[0])
            {
                return true;
            }
        }
        return false;
    }
    public void JoinGame()
    {
        FindObjectOfType<JoinGame>().Join(JoinCode.text);
    }
}
