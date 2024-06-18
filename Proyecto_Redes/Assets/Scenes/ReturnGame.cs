using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class ReturnGame : MonoBehaviour
{
    
    // Método para ser llamado por el botón "Back"
    public void BackToGame()
    {
        SceneManager.LoadScene("Juego");
    }
}
