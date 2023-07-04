using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMenu : MonoBehaviour
{
    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject EndMenu;

    public void OnButton()
    {
        EndMenu.SetActive(true);
        StartMenu.SetActive(false);
    }
}
