using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteController : MonoBehaviour
{
    public string speakerId;   // ID que debe coincidir con DialogueLine.speakerId
    public Image portraitImage;

    [Header("Efectos visuales")]
    public float activeScale = 1.1f;
    public float inactiveScale = 0.9f;
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    private void Awake()
    {
        if (portraitImage == null)
            portraitImage = GetComponent<Image>();
    }

    public void SetActiveSpeaker(bool isActive)
    {
        if (portraitImage == null) return;

        // Escala
        transform.localScale = isActive ? Vector3.one * activeScale : Vector3.one * inactiveScale;

        // Color
        portraitImage.color = isActive ? activeColor : inactiveColor;
    }
}
