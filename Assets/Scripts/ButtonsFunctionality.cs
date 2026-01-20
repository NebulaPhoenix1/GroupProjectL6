using UnityEngine;

public class ButtonsFunctionality : MonoBehaviour
{
    // Play game button
    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Game");
    }

    // Quit game button
    public void QuitGame()
    {
        Debug.Log("Exit!");
        Application.Quit();
    }
}
