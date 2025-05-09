using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyThief : MonoBehaviour
{
    public GameObject thiefPrefab;
    public float spawnInterval = 15f;
    public float stayDuration = 3f;
    public AudioClip stealSound;
    private List<GameObject> currentKeys;
    private Transform player;

    void Start()
    {
        player = Camera.main.transform;
        InvokeRepeating(nameof(SpawnThief), spawnInterval, spawnInterval);
    }

    void SpawnThief()
    {
        currentKeys = new List<GameObject>(GameObject.FindGameObjectsWithTag("Key"));
        if (currentKeys.Count == 0) return;

        Vector3 spawnPos = player.position + player.forward * 1.2f + Vector3.up * -0.5f;
        Quaternion spawnRot = Quaternion.LookRotation(player.position - spawnPos);
        GameObject thief = Instantiate(thiefPrefab, spawnPos, spawnRot);

        
        AudioSource audioSource = thief.GetComponent<AudioSource>();
        if (audioSource != null && stealSound != null)
        {
            audioSource.PlayOneShot(stealSound);
        }
        StartCoroutine(RemoveKeysAndDisappear(thief));
    }

    IEnumerator RemoveKeysAndDisappear(GameObject thief)
    {
        yield return new WaitForSeconds(1f); 

        currentKeys = new List<GameObject>(GameObject.FindGameObjectsWithTag("Key"));
        int removed = 0;

        while (removed < 2 && currentKeys.Count > 0)
        {
            int randomIndex = Random.Range(0, currentKeys.Count);
            Destroy(currentKeys[randomIndex]);
            currentKeys.RemoveAt(randomIndex);
            removed++;
        }

        yield return new WaitForSeconds(stayDuration);
        Destroy(thief);
    }
}
