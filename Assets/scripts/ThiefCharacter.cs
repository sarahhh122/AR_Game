using UnityEngine;
using TMPro;

public class ThiefCharacter : MonoBehaviour
{

    public GameObject thiefPrefab;
    public AudioClip thiefSound;
    public float stayDuration = 3f;
    public float spawnInterval = 20f;

    private GameObject spawnedThief;
    private Transform player;

    void Start()
    {
        player = Camera.main.transform;
        InvokeRepeating(nameof(SpawnThief), 6f, spawnInterval);

    }

    void SpawnThief()
    {
        if (spawnedThief != null) return;

        // Spawn position 
        Vector3 spawnPos = player.position + player.forward * 1.5f - new Vector3(0, 0.5f, 0);
        Quaternion spawnRot = new Quaternion(
            0.04252093f,
            0.9915443f,
           -0.00263807f,
            0.12257615f
        );
        spawnedThief = Instantiate(thiefPrefab, spawnPos, spawnRot);

        var audioSource = spawnedThief.GetComponent<AudioSource>();
        if (audioSource != null && thiefSound != null)
            audioSource.PlayOneShot(thiefSound);
   
        Invoke(nameof(StealAndDisappear), stayDuration);
    }

void StealAndDisappear()
    {
        CollectKey scoreManager = FindObjectOfType<CollectKey>();
        if (scoreManager != null)
        {
            scoreManager.StealCollectedKeys(1); 
        }

        if (spawnedThief != null)
        {
            Destroy(spawnedThief);
            spawnedThief = null;
        }
    }

}
