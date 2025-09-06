using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    public GameObject panelSelector;
    public GameObject panelOpciones;

    void Start()
    {
        // Estado inicial
        panelSelector.SetActive(true);
        panelOpciones.SetActive(false);
    }

    // 👉 Llamar cuando pulses el botón "Opciones"
    public void ShowOptions()
    {
        panelSelector.SetActive(false);
        panelOpciones.SetActive(true);
    }

    // 👉 Llamar cuando pulses el botón "Volver"
    public void ShowSelector()
    {
        panelOpciones.SetActive(false);
        panelSelector.SetActive(true);
    }
}
