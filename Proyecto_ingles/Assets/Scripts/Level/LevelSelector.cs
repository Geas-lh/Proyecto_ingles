using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene"); // 👈 nombre exacto de tu menú principal
    }
}
