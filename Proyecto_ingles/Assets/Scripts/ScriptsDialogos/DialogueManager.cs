using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DialogueLine
{   
    public string speakerId;   // ID del personaje (ej: "Heroe", "Aliado")
    [TextArea(2, 5)]
    public string sentence; // Texto que dirá
}

public class DialogueManager : MonoBehaviour
{
    [Header("Configuración")]
    public DialogueLine[] dialogueLines;   // Lista de frases
    public float typingSpeed = 0.05f;      // Velocidad de escritura
    public string nextSceneName = "Nivel1"; // Escena a cargar después

    [Header("Referencias UI")]
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    private int currentLine = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    [Header("Sprites")]
    public SpriteController[] spriteControllers;


    void Start()
    {
        ShowLine();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                // Si aún está escribiendo, mostrar todo de golpe
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogueLines[currentLine].sentence;
                isTyping = false;
            }
            else
            {
                // Si ya terminó de escribir, pasar a la siguiente línea
                NextLine();
            }
        }
    }

    void ShowLine()
    {
    if (currentLine < dialogueLines.Length)
    {
        DialogueLine line = dialogueLines[currentLine];
        speakerText.text = line.speakerId;

        // 👉 Actualizar sprites
        UpdateSprites(line.speakerId);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(line.sentence));
    }
    else
    {
        SceneManager.LoadScene(nextSceneName);
    }
    }

void UpdateSprites(string activeId)
{
    foreach (var sc in spriteControllers)
    {
        sc.SetActiveSpeaker(sc.speakerId == activeId);
    }
}


    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void NextLine()
    {
        currentLine++;
        ShowLine();
    }
}
