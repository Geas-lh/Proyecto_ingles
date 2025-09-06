using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement;

public class SelectorDeNiveles : MonoBehaviour
{
    [System.Serializable]
    public class Nivel
    {
        public string nombre;              // Nombre del nivel
        public string nombreEscena;        // Nombre exacto de la escena
        public RectTransform imagenUI;     // Imagen del nivel en la lista
        public RectTransform puntoMapa;    // Empty en el mapa
    }

    public Nivel[] niveles;
    public TextMeshProUGUI textoNivel;     // Texto donde aparece el nombre
    public RectTransform marcador;         // El marcador en el mapa

    public float escalaNormal = 1f;
    public float escalaSeleccionada = 1.2f;
    public float velocidadMarcador = 5f;

    private int indiceActual = 0;
    private Vector2 destinoMarcador;

    void OnEnable()
    {
        if (niveles == null || niveles.Length == 0)
        {
            Debug.LogWarning("⚠ No hay niveles configurados en el Inspector.");
            return;
        }

        indiceActual = 0;
        ActualizarSeleccion();
    }

    void Update()
    {
        if (niveles == null || niveles.Length == 0) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            indiceActual--;
            if (indiceActual < 0) indiceActual = niveles.Length - 1;
            ActualizarSeleccion();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            indiceActual++;
            if (indiceActual >= niveles.Length) indiceActual = 0;
            ActualizarSeleccion();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            string escena = niveles[indiceActual].nombreEscena;
            if (!string.IsNullOrEmpty(escena))
            {
                SceneManager.LoadScene(escena);
            }
            else
            {
                Debug.LogWarning("⚠ No hay escena asignada a este nivel.");
            }
        }

        // Mover marcador suavemente
        marcador.anchoredPosition = Vector2.Lerp(
            marcador.anchoredPosition,
            destinoMarcador,
            Time.deltaTime * velocidadMarcador
        );
    }

    void ActualizarSeleccion()
    {
        for (int i = 0; i < niveles.Length; i++)
        {
            niveles[i].imagenUI.localScale = (i == indiceActual)
                ? Vector3.one * escalaSeleccionada
                : Vector3.one * escalaNormal;
        }

        textoNivel.text = niveles[indiceActual].nombre;
        destinoMarcador = niveles[indiceActual].puntoMapa.anchoredPosition;
    }
}
