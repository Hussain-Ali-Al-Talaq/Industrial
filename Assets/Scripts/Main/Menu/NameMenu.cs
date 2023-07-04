using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputfield;
    [SerializeField] private TextMeshProUGUI text;

    private string Chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890";
    private string Name;
    void Start()
    {
        Name = LoadSaveName.TryLoadName();
        if(Name == null)
        {
            for (int i = 0; i < 7; i++)
            {
                Name += Chars[Random.Range(0, Chars.Length - 1)];
            }
            LoadSaveName.WriteName(Name);
        }
        inputfield.text = Name;
        text.text = "Name: " + Name;
    }
    public void UpdateNameText()
    {
        if (ValidateString(inputfield.text))
        {
            text.text = "Name: " + inputfield.text;

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
    public void WriteName()
    {
        if (inputfield.text != Name)
        {
            LoadSaveName.WriteName(inputfield.text);
        }
    }
}
