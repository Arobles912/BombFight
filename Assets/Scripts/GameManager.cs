using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject[] players; // Contiene los jugadores

    // Función que comprueba la condición de "victoria"
    public void CheckWinState()
    {
        int alivePlayerCount = 0;

        foreach (GameObject player in players)
        {
            if (player.activeSelf)
            {
                alivePlayerCount++; // Asigna cuantos jugadores siguen "vivos"
            }
        }

        if (alivePlayerCount <= 1) // En caso de que haya solo un jugador o 0, reinicia la escena
        {
            Invoke(nameof(NewRound), 3f);
        }
    }

    // Reincia la escena
    private void NewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
