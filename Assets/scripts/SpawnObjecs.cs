using UnityEngine;

public class SpawnObjecrs : MonoBehaviour
{

    public GameObject[] quizItemPrefabs; // Drag your 5 prefabs here
    public float spacing = 0.4f; 

    void Start()
    {
        SpawnQuizItems();
    }

    void SpawnQuizItems()
    {
        Transform cam = Camera.main.transform;
        int count = quizItemPrefabs.Length;

        for (int i = 0; i < count; i++)
        {
            Vector3 basePos = cam.position + cam.forward * 1.5f + new Vector3(0, 0.05f, 0);

            Vector3 offset = cam.right * ((i - count / 2f) * spacing);
            Vector3 spawnPos = basePos + offset;

            Quaternion spawnRot = Quaternion.LookRotation(cam.position - spawnPos);

            Instantiate(quizItemPrefabs[i], spawnPos, spawnRot);
        }
    }
}