using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider typingSpeedSlider;
    public static bool isPaused = false;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void OnSettingsButtonClick()
    {
        if (settingsPanel != null)
        {
            pauseMenuUI.SetActive(false);
            settingsPanel.SetActive(true);
        }
    }

    public void OnBackButtonClick()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
    }

    public void OnSaveButtonClick() => Debug.Log("Save initiated...");

    public void OnExitButtonClick()
    {
        Time.timeScale = 1f;
        isPaused = false;
        if (SceneTransitionManager.Instance != null) SceneTransitionManager.Instance.LoadScene("SampleScene");
        else SceneManager.LoadScene("SampleScene");
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }

    public void OnTypingSpeedChanged(float value)
    {
        if (DialogueController.Instance != null)
            DialogueController.Instance.SetTypingSpeed(value);
    }
}