using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject levelSelectPanel;
    public Button playButton; 

    public void OnPlayPressed()
    {
        levelSelectPanel.SetActive(true);       
        playButton.gameObject.SetActive(false);  
    }

    public void OnBackPressed()
    {
        levelSelectPanel.SetActive(false);       
        playButton.gameObject.SetActive(true);   
    }

    public void OnLevel1Pressed() => LoadLevelByIndex(1);
    public void OnLevel2Pressed() => LoadLevelByIndex(2);
    public void OnLevel3Pressed() => LoadLevelByIndex(3);
    public void OnLevel4Pressed() => LoadLevelByIndex(4);

    void LoadLevelByIndex(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
