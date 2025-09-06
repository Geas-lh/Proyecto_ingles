using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectorUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelSelector;
    public GameObject panelOpciones;

    void Start()
    {
        // Aseguramos estado inicial
        panelSelector.SetActive(true);
        panelOpciones.SetActive(false);
    }

    public void OpenOptions()
    {
        panelSelector.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void CloseOptions()
    {
        panelOpciones.SetActive(false);
        panelSelector.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene"); // Cambia por el nombre exacto de tu menú
    }
}
