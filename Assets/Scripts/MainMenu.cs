using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayStage1()
    {
        SceneManager.LoadScene(1); // Carga la escena 1
    }

    public void PlayStage2()
    {
        SceneManager.LoadScene(2); // Carga la escena 2
    }


    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0); // Carga la escena 0
    }


    public void QuitGame()
    {
        Application.Quit(); // El juego se cierra
    }
}
