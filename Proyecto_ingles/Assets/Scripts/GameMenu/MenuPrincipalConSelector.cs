using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalConSelector : MonoBehaviour
{
    public GameObject panelMenu;
    public GameObject panelSelectorNiveles;

    void Start()
    {
        // 👇 Revisar si venimos desde el juego y queremos abrir el selector directamente
        if (GameState.openLevelSelector)
        {
            panelMenu.SetActive(false);
            panelSelectorNiveles.SetActive(true);
            GameState.openLevelSelector = false; // resetear
        }
        else
        {
            panelMenu.SetActive(true);
            panelSelectorNiveles.SetActive(false);
        }
    }

    public void IniciarJuego()
    {
        panelMenu.SetActive(false);
        panelSelectorNiveles.SetActive(true);
    }

    public void ContinuarJuego()
    {
        Debug.Log("Continuar juego (aquí va el sistema de guardado)");
    }

    public void AbrirOpciones()
    {
        Debug.Log("Abrir menú de opciones");
    }

    public void VerCreditos()
    {
        Debug.Log("Mostrar créditos");
    }

    public void Salir()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }
}
