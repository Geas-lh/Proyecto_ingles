using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextoParpadeo : MonoBehaviour
{
    public TextMeshProUGUI texto;
    public float velocidad = 3f;   // velocidad del parpadeo
    public float escalaExtra = 1.1f; // cuanto se agranda en el "pico"
    public Color colorBase = Color.white;
    public Color colorDestacado = Color.yellow;

    private Vector3 escalaInicial;

    void Start()
    {
        escalaInicial = texto.transform.localScale;
    }

    void Update()
    {
        // Oscilación con seno
        float t = (Mathf.Sin(Time.time * velocidad) + 1f) / 2f;

        // Alpha (fade in/out)
        Color c = Color.Lerp(colorBase, colorDestacado, t);
        c.a = t; 
        texto.color = c;

        // Escala (zoom suave)
        float escala = Mathf.Lerp(1f, escalaExtra, t);
        texto.transform.localScale = escalaInicial * escala;
    }
}
