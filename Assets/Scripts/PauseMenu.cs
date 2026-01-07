using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pauseMenu;

    private bool isPauseMenuActive;
    private string currentScene;

    private void Start()
    {
        isPauseMenuActive = false;
    }

    public void TogglePauseMenu()
    {
        isPauseMenuActive = !isPauseMenuActive;

        if (pauseButton.activeSelf)
        {
            pauseButton.SetActive(false);
            Time.timeScale = 0f;
        }

        if (pauseMenu.activeSelf)
        {
            pauseButton.SetActive(true);
            Time.timeScale = 1f;
        }

        pauseMenu.SetActive(isPauseMenuActive);
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
