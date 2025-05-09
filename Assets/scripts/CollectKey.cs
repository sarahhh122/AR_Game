using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CollectKey : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI keysRemainingText;
    public GameObject winPanel; // UI panel/text to show on win
    public AudioClip collectSound;
    public AudioSource audioSource;

    public int score = 0;
    public int totalKeys = 10;

    void Start()
    {
        UpdateScoreUI();
        if (winPanel != null)
            winPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Key"))
                {
                    Debug.Log("Key collected!");
                    Destroy(hit.collider.gameObject);
                    score++;

                    if (audioSource != null && collectSound != null)
                        audioSource.PlayOneShot(collectSound);

                    UpdateScoreUI();

                    if (score >= totalKeys)
                    {
                        Win();
                    }
                }
            }
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Keys Collected: " + score;

        if (keysRemainingText != null)
            keysRemainingText.text = "Keys Remaining: " + (totalKeys - score);
    }

    void Win()
    {
        if (winPanel != null)
            winPanel.SetActive(true);

        Debug.Log("You win!");
        Invoke(nameof(LoadNextLevel), 3f); 
    }

    void LoadNextLevel()
    {
       SceneManager.LoadScene(3);
    }

public void StealCollectedKeys(int amount)
{
    if (score <= 0) return;
    int stolen = Mathf.Min(score, amount);
    score -= stolen;
    totalKeys+=stolen;
    UpdateScoreUI();
}


}
