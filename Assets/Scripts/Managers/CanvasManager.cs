using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Audio;

public class CanvasManager : MonoBehaviour
{

    public AudioMixer mixer;

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
    public GameObject gameOverCanvas;

    [Header("Sliders")]
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    [Header("Text")]
    public TMP_Text masterVolSliderText;
    public TMP_Text musicVolSliderText;
    public TMP_Text sfxVolSliderText;
    public TMP_Text livesText;

    [Header("Audio")]
    public AudioClip pause;

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

        

        if (livesText)
        {
            GameManager.Instance.OnLivesChanged += UpdateLivesText;
            UpdateLivesText(GameManager.Instance.Lives); // Initialize the text with the current lives value
        }

        if (masterVolSlider)
        {
            SetupSliderInformation(masterVolSlider, masterVolSliderText, "MasterVol");
            OnSliderValueChanged(masterVolSlider.value, masterVolSlider, masterVolSliderText, "MasterVol"); // Initialize the text with the current value
        }
        if (musicVolSlider)
        {
            SetupSliderInformation(musicVolSlider, musicVolSliderText, "MusicVol");
            OnSliderValueChanged(musicVolSlider.value, musicVolSlider, musicVolSliderText, "MusicVol"); // Initialize the text with the current value
        }
        if (sfxVolSlider)
        {
            SetupSliderInformation(sfxVolSlider, sfxVolSliderText, "SFXVol");
            OnSliderValueChanged(sfxVolSlider.value, sfxVolSlider, sfxVolSliderText, "SFXVol"); // Initialize the text with the current value
        }

    }

    private void UpdateLivesText(int value)
    {
        livesText.text = $"Lives: {value}";
    }

   
    private void SetupSliderInformation(Slider slider, TMP_Text sliderText, string mixerParameterName)
    {
        slider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, slider, sliderText, mixerParameterName));
        
    }

    private void OnSliderValueChanged(float value, Slider slider, TMP_Text sliderText, string mixerParameterName)
    {

        if (value == 0)
        {
            value = -80; // Set to minimum to audio mixer
        }

        else
        {
            value = Mathf.Log10(slider.value) * 20; // Convert to decibels
        }

        sliderText.text = (value == -80) ? "0%" : $"{(int)(slider.value * 100)}";
        mixer.SetFloat(mixerParameterName, value);




    }

    public void HideGameOverCanvas()
    {
        gameOverCanvas.SetActive(false);
    }

    public void ShowGameOverCanvas()
    {
        gameOverCanvas.SetActive(true);
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
             
                GetComponent<AudioSource>().PlayOneShot(pause); //Pause sound effect
              

            }
        }

    }



}
