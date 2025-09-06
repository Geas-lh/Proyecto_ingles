using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject panelInicio;
    public GameObject panelMenu;

    void Start()
    {
        panelInicio.SetActive(true);   // Mostrar solo inicio
        panelMenu.SetActive(false);    // Ocultar menú
    }

    void Update()
    {
        if (panelInicio.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            // Cambiar de panel inicio → panel menú
            panelInicio.SetActive(false);
            panelMenu.SetActive(true);
        }
    }
}
