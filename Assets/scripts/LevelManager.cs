using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadLevel2()
    {
        // Could also activate Level 2 content if in same scene
        Debug.Log("Loading Level 2...");
        // SceneManager.LoadScene("Level2");
    }
}
