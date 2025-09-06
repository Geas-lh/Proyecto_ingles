using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroller : MonoBehaviour
{
    public RectTransform creditsText; // arrastra tu TextMeshPro aquí
    public float scrollSpeed = 50f;
    public float endY = 1000f;
    public string menuSceneName = "MenuScene";

    void Update()
    {
        if (creditsText != null)
        {
            creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            if (creditsText.anchoredPosition.y >= endY)
            {
                SceneManager.LoadScene(menuSceneName);
            }
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
