using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button startBtn;
    public Button quitBtn;
    public Button settingsBtn;
    public Button backBtn;
    public Button returnToMenu;
    public Button resumeBtn;

    [Header("Menu Canvases")]
    public GameObject mainMenuCanvas;
    public GameObject settingsCanvas;
    public GameObject pauseMenuCanvas;
    public GameObject hudCanvas;

    [Header("Text")]
    public TMP_Text volSliderText;
    public TMP_Text livesText;

    [Header("Sliders")]
    public Slider volSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startBtn) startBtn.onClick.AddListener(() => ChangeScene("Game"));
        if (quitBtn) quitBtn.onClick.AddListener(QuitGame);
        if (settingsBtn) settingsBtn.onClick.AddListener(() => SetMenus(settingsCanvas, mainMenuCanvas));
        if (resumeBtn) resumeBtn.onClick.AddListener(resumeGame);
       

        if (backBtn)
        {
            if (SceneManager.GetActiveScene().name == "Game")
                backBtn.onClick.AddListener(() => SetMenus(hudCanvas, pauseMenuCanvas)); //I'm pretty sure this is redunant because I added a resume button

            else
                backBtn.onClick.AddListener(() => SetMenus(mainMenuCanvas, settingsCanvas));
        }

        if (returnToMenu) returnToMenu.onClick.AddListener(() => ChangeScene("Title"));

        if (volSlider)
        {
            volSlider.onValueChanged.AddListener(OnVolSliderChanged);
            OnVolSliderChanged(volSlider.value); // Initialize the text with the current slider value
        }

        if (livesText)
        {
            GameManager.Instance.OnLivesChanged += UpdateLivesText;
            UpdateLivesText(GameManager.Instance.Lives); // Initialize the text with the current lives value
        }
    }

    private void UpdateLivesText(int value)
    {
        livesText.text = $"Lives: {value}";
    }

    private void OnVolSliderChanged(float valueChanged)
    {
        float roundedValue = Mathf.Round(valueChanged * 100);
        if (volSliderText) volSliderText.text = $"{roundedValue}%";
    }

    

    public void ChangeScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is null or empty.");
            return;
        }
        GameManager.isPaused = false; //When changing scenes will make sure the game is no longer paused
        Time.timeScale = 1; //Will also set timescale back to 1 

        SceneManager.LoadScene(sceneName);
        Debug.Log("Changing scene to: " + sceneName);
    }

    //platform specific so that if you are in the editor, it will stop the editor play mode otherwise it will quit the application

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    

    private void SetMenus(GameObject canvasToActivate, GameObject canvasToDeactivate)
    {
        if (canvasToActivate) canvasToActivate.SetActive(true);
        if (canvasToDeactivate) canvasToDeactivate.SetActive(false);
    }

    private void resumeGame()
    {
        SetMenus(hudCanvas, pauseMenuCanvas);
        Time.timeScale = 1;
        GameManager.isPaused = false;
    }

    private void Update()
    {
        if (!pauseMenuCanvas) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseMenuCanvas.activeSelf)
            {
                SetMenus(hudCanvas, pauseMenuCanvas);
                Time.timeScale = 1;
                GameManager.isPaused = false;
            }
            else
            {
                SetMenus(pauseMenuCanvas, hudCanvas);
                Time.timeScale = 0;
                GameManager.isPaused = true;
                
            }
        }
    }



}
