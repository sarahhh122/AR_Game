using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public void OnMainMenuPressed() => SceneManager.LoadScene(0);  
}
