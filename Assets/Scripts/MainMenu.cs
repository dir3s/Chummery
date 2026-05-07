using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string startSceneName = "StartScene";
    [SerializeField] private string storySceneName = "StoryScene";
    [SerializeField] private List<GameObject> allPanels;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        foreach (GameObject panel in allPanels)
        {
            if (panel != null) panel.SetActive(false);
        }

        if (volumeSlider != null)
            volumeSlider.value = AudioListener.volume;
    }

    public void OpenPanel(GameObject panelToOpen)
    {
        if (panelToOpen != null)
            panelToOpen.SetActive(true);
    }

    public void ClosePanel(GameObject panelToClose)
    {
        if (panelToClose != null)
            panelToClose.SetActive(false);
    }

    public void OnStartButtonClick()
    {

        SaveLastNode.DeleteSave();
        GameState.loadFromSave = false;


        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadScene(startSceneName);
        }
        else
        {
            Debug.LogError("SceneTransitionManager not found!");
        }
    }

    public void OnStoryButtonClick()
    {
        if (MenuCameraController.Instance != null)
        {
            MenuCameraController.Instance.MoveToLeftPanel();
        }
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }


    public void LoadGame()
    {
        GameState.loadFromSave = true;
        SceneManager.LoadScene("StartScene");
    }
}