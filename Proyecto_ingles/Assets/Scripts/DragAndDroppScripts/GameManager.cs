using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Base de datos (ItemAssets)")]
    public List<ItemAsset> itemDatabase = new List<ItemAsset>();

    [Header("Rondas / dificultad")]
    public int itemsPerRound = 4;      // cuántos ítems aparecen en el panel (incluye el correcto)
    public int roundsToWin = 0;        // 0 = usar itemDatabase.Count
    public int maxFails = 3;

    [Header("Referencias UI y objetos")]
    public DropZone dropZone;
    public Transform sourcePanel;      // parent donde se instancian los ítems
    public GameObject itemPrefab;      // prefab con DraggableItem
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI failsText;
    public TextMeshProUGUI correctsText;
    public TextMeshProUGUI scoreText;

    [Header("Panels")]
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    public GameObject pausePanel;
    public TextMeshProUGUI victoryScoreText;
    public TextMeshProUGUI defeatScoreText;

    [Header("Score")]
    public int maxScorePerHit = 100; // puntuación máxima por acierto (ajusta)
    [Header("Escenas")]
    public string nivelesSceneName = "SelectorNiveles"; // 👈 cámbialo en el Inspector


    // internals
    private List<ItemAsset> availableItems = new List<ItemAsset>();
    private List<ItemAsset> usedItems = new List<ItemAsset>();

    private int fails = 0;
    private int roundsCompleted = 0;
    private int score = 0;
    private float timer = 0f;
    private float roundStartTime = 0f;
    private bool isPlaying = true;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // inicializar pools
        availableItems = new List<ItemAsset>(itemDatabase);
        usedItems = new List<ItemAsset>();

        if (roundsToWin <= 0) roundsToWin = Mathf.Max(1, itemDatabase.Count);

        // UI / estados
        if (victoryPanel) victoryPanel.SetActive(false);
        if (defeatPanel) defeatPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(false);

        fails = 0;
        roundsCompleted = 0;
        score = 0;
        timer = 0f;
        isPlaying = true;

        UpdateFailsUI();
        UpdateCorrectsUI();
        UpdateScoreUI();

        NextRound();
    }

    void Update()
    {
        if (!isPlaying) return;

        timer += Time.deltaTime;
        if (timerText != null) timerText.text = FormatTime(timer);

        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    // -------------------------
    //  Rondas y spawn
    // -------------------------
    public void NextRound()
    {
        if (!isPlaying) return;

        if (availableItems.Count == 0)
        {
            // reciclar usados si se acabaron
            availableItems = new List<ItemAsset>(usedItems);
            usedItems.Clear();
        }

        if (availableItems.Count == 0)
        {
            Debug.LogWarning("GameManager: itemDatabase vacío.");
            return;
        }

        // elegir el item objetivo de availableItems
        int idx = Random.Range(0, availableItems.Count);
        ItemAsset chosen = availableItems[idx];

        // marcar como usado
        usedItems.Add(chosen);
        availableItems.RemoveAt(idx);

        // setear objetivo en DropZone
        if (dropZone != null) dropZone.SetExpected(chosen.id);

        // marcar tiempo de inicio de la ronda (para penalización por tardanza)
        roundStartTime = timer;

        // poblar source panel con el correcto + distractores
        SpawnSourceItems(chosen);
    }

    void SpawnSourceItems(ItemAsset chosen)
    {
        // limpiar panel
        for (int i = sourcePanel.childCount - 1; i >= 0; i--)
            Destroy(sourcePanel.GetChild(i).gameObject);

        // preparar pool de candidatos (todos los items excepto el elegido)
        List<ItemAsset> pool = new List<ItemAsset>(itemDatabase);
        pool.Remove(chosen);

        // lista final que contendrá el correcto + distractores
        List<ItemAsset> toShow = new List<ItemAsset> { chosen };

        int needed = Mathf.Clamp(itemsPerRound - 1, 0, pool.Count);
        for (int i = 0; i < needed; i++)
        {
            int r = Random.Range(0, pool.Count);
            toShow.Add(pool[r]);
            pool.RemoveAt(r);
        }

        // mezclar orden para que el correcto no siempre esté en la misma posición
        for (int i = 0; i < toShow.Count; i++)
        {
            int r = Random.Range(i, toShow.Count);
            var tmp = toShow[i];
            toShow[i] = toShow[r];
            toShow[r] = tmp;
        }

        // instanciar prefabs
        foreach (var data in toShow)
        {
            GameObject go = Instantiate(itemPrefab, sourcePanel);
            go.name = "Item_" + data.id;
            DraggableItem di = go.GetComponent<DraggableItem>();
            if (di != null)
            {
                di.Setup(data.id, data.icon, data.label);
            }
        }
    }

    // -------------------------
    //  Resultado de interacción
    // -------------------------
    // llamado por DropZone cuando suelta correctamente
    public void OnCorrectPlacement(string id, GameObject placedObject)
    {
        if (placedObject != null)
            Destroy(placedObject);

        roundsCompleted++;

        int gained = CalculateScoreForHit();
        score += gained;

        StartCoroutine(AnimateScoreTextScale());
        UpdateScoreUI();
        UpdateCorrectsUI();

        if (roundsCompleted >= roundsToWin)
            Victory();
        else
            NextRound();
    }

    // llamado por DropZone cuando es incorrecto
    public void RegisterFail()
    {
        fails++;
        UpdateFailsUI();

        if (fails >= maxFails)
            Defeat();
    }

    // -------------------------
    //  Score
    // -------------------------
    int CalculateScoreForHit()
    {
        // penalización por tiempo en la ronda actual
        int penalty = Mathf.FloorToInt(timer - roundStartTime); // segundos transcurridos desde que salió el objetivo
        int points = Mathf.Max(10, maxScorePerHit - penalty); // mínimo 10 por acierto
        return points;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = $"Score: {score}";
    }

    IEnumerator AnimateScoreTextScale()
    {
        if (scoreText == null) yield break;

        Vector3 original = scoreText.rectTransform.localScale;
        Vector3 peak = original * 1.2f;
        float dur = 0.12f;

        float t = 0f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            scoreText.rectTransform.localScale = Vector3.Lerp(original, peak, t / dur);
            yield return null;
        }

        t = 0f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            scoreText.rectTransform.localScale = Vector3.Lerp(peak, original, t / dur);
            yield return null;
        }

        scoreText.rectTransform.localScale = original;
    }

    // -------------------------
    //  UI helpers
    // -------------------------
    void UpdateFailsUI()
    {
        if (failsText != null) failsText.text = $"Fallos: {fails}/{maxFails}";
    }

    void UpdateCorrectsUI()
    {
        if (correctsText != null) correctsText.text = $"Aciertos: {roundsCompleted}/{roundsToWin}";
    }

    string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        int cent = Mathf.FloorToInt((t * 100) % 100);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cent);
    }

    // -------------------------
    //  Estados (victoria/derrota/pausa)
    // -------------------------
    void Victory()
    {
        isPlaying = false;
        Time.timeScale = 0f;
        if (victoryPanel) victoryPanel.SetActive(true);
        if (defeatPanel) defeatPanel.SetActive(false);
        if (victoryScoreText) victoryScoreText.text = $"Tiempo: {FormatTime(timer)}\nFallos: {fails}\nAciertos: {roundsCompleted}\nScore: {score}";
    }

    void Defeat()
    {
        isPlaying = false;
        Time.timeScale = 0f;
        if (defeatPanel) defeatPanel.SetActive(true);
        if (victoryPanel) victoryPanel.SetActive(false);
        if (defeatScoreText) defeatScoreText.text = $"Tiempo: {FormatTime(timer)}\nFallos: {fails}\nAciertos: {roundsCompleted}\nScore: {score}";
    }

    public void TogglePause()
    {
        if (pausePanel == null) return;
        bool active = !pausePanel.activeSelf;
        pausePanel.SetActive(active);
        Time.timeScale = active ? 0f : 1f;
        isPlaying = !active;
    }

    // -------------------------
    //  Botones
    // -------------------------
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
    public void ResumeGame()
{
    if (pausePanel != null)
        pausePanel.SetActive(false);

    Time.timeScale = 1f;
    isPlaying = true;
}

public void RestartFromPause()
{
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}

public void BackToLevels()
{
    if (!string.IsNullOrEmpty(nivelesSceneName))
    {
        Time.timeScale = 1f;
        GameState.openLevelSelector = true; // 👈 forzar abrir selector
        SceneManager.LoadScene(nivelesSceneName);
    }
    else
    {
        Debug.LogWarning("⚠️ No se configuró el nombre de la escena de niveles en GameManager");
    }
}

}
