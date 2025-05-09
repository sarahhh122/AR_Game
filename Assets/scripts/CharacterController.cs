using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    public GameObject characterPrefab;
    private GameObject spawnedCharacter;
    public ScoreManager scoreManager;
    public AudioClip voiceLine;
    public TextMeshProUGUI levelMessageText;


    void Start()
    {
        // Delay spawn by 2 seconds for dramatic effect
        Invoke(nameof(SpawnCharacter), 2f);
     
    }

    void SpawnCharacter()
    {
        // Spawn in front of the AR camera
       // Vector3 spawnPos = Camera.main.transform.position + Camera.main.transform.forward * 1.5f - new Vector3(0, 0.5f, 0);
    Vector3 spawnPos = Camera.main.transform.position + Camera.main.transform.forward * 1.5f - new Vector3(0, 0.5f, 0);

    Quaternion spawnRot = new Quaternion(
        0.04252093f,
        0.9915443f,
       -0.00263807f,
        0.12257615f
    );
    

    spawnedCharacter = Instantiate(characterPrefab, spawnPos, spawnRot);
 var audioSource = spawnedCharacter.GetComponent<AudioSource>();
        if (audioSource != null && voiceLine != null)
        {
            audioSource.PlayOneShot(voiceLine);
        }
        CharacterSpeech speech = spawnedCharacter.GetComponent<CharacterSpeech>();
        speech?.Say("Let's boost your score!");
        // Apply score multiplier while character is active
        if (scoreManager != null)
        {
            scoreManager.scoreMultiplier = 2;
        }

    }

    void Update()
    {
        if (spawnedCharacter != null && scoreManager != null && scoreManager.CurrentScore >= 8)
        {
            Destroy(spawnedCharacter);
            scoreManager.scoreMultiplier = 1;

        levelMessageText.text = "CONGRATULATIONS! You will be moved to Level 2...";
        Invoke(nameof(LoadLevel2), 3f);
        }
    }
    void LoadLevel2()
{
    SceneManager.LoadScene(1);
}

}
