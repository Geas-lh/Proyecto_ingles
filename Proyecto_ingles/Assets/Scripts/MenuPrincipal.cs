using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelMenu;
    public GameObject panelSelectorNiveles;
    public GameObject panelOpciones; // nuevo panel de opciones

    [Header("Opciones de menú (Botones)")]
    public Button[] botonesMenu;      // botones del panel principal
    public Button[] botonesOpciones;  // botones del panel de opciones
    private int indiceActual = 0;
    private Button[] botonesActivos;  // botones que actualmente se navegan

    [Header("Parpadeo")]
    public float velocidadParpadeo = 3f;

    void Start()
    {
        AbrirMenuPrincipal(); // activa el panel principal al inicio
    }

    void Update()
    {
        if (botonesActivos == null || botonesActivos.Length == 0) return;

        // Mover con flechas
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            indiceActual--;
            if (indiceActual < 0) indiceActual = botonesActivos.Length - 1;
            ActualizarSeleccion();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            indiceActual++;
            if (indiceActual >= botonesActivos.Length) indiceActual = 0;
            ActualizarSeleccion();
        }

        // Activar con Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            botonesActivos[indiceActual].onClick.Invoke();
        }

        // Parpadeo del botón seleccionado
        TextMeshProUGUI texto = botonesActivos[indiceActual].GetComponentInChildren<TextMeshProUGUI>();
        if (texto != null)
        {
            float alpha = (Mathf.Sin(Time.time * velocidadParpadeo) + 1f) / 2f;
            Color c = texto.color;
            c.a = alpha;
            texto.color = c;
        }
    }

    void ActualizarSeleccion()
    {
        if (botonesActivos == null) return;

        foreach (var boton in botonesActivos)
        {
            TextMeshProUGUI texto = boton.GetComponentInChildren<TextMeshProUGUI>();
            if (texto != null)
            {
                Color c = texto.color;
                c.a = 1f; // reset alpha
                texto.color = c;
            }
        }
    }

    // 🔹 Métodos para abrir/cerrar paneles
    public void AbrirMenuPrincipal()
    {
        panelMenu.SetActive(true);
        panelSelectorNiveles.SetActive(false);
        panelOpciones.SetActive(false);

        botonesActivos = botonesMenu;
        indiceActual = 0;
        ActualizarSeleccion();
    }

    public void AbrirSelectorNiveles()
    {
        panelMenu.SetActive(false);
        panelSelectorNiveles.SetActive(true);
        panelOpciones.SetActive(false);

        botonesActivos = botonesMenu; // si quieres navegar por los botones del selector, crea otro array
        indiceActual = 0;
        ActualizarSeleccion();
    }

    public void AbrirOpciones()
    {
        panelMenu.SetActive(false);
        panelSelectorNiveles.SetActive(false);
        panelOpciones.SetActive(true);

        botonesActivos = botonesOpciones;
        indiceActual = 0;
        ActualizarSeleccion();
    }
    public void VerCreditos()
    {
        // Cambia "CreditsScene" por el nombre exacto de tu escena de créditos
        SceneManager.LoadScene("CreditsScene");
    }

    public void Salir()
    {
        Debug.Log("Salir del juego");
        Application.Quit();

    }
    public void CerrarOpciones()
{
    if (panelOpciones != null) panelOpciones.SetActive(false);
    if (panelMenu != null) panelMenu.SetActive(true);

    // Actualizar botones activos para navegación con flechas
    if (botonesMenu != null && botonesMenu.Length > 0)
    {
        botonesActivos = botonesMenu;
        indiceActual = 0;
        ActualizarSeleccion();
    }
}
}
