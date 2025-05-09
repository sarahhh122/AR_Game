using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class RandomKeyPlacer : MonoBehaviour
{
    public GameObject keyPrefab;
    public ARRaycastManager raycastManager;
    public int numberOfKeys = 50;
    public float maxScatterDistance = 5f;
    public float minKeySpacing = 0.5f;
    private List<Vector3> placedKeys = new List<Vector3>();
    private bool keysPlaced = false;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        if (keysPlaced) return;

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose basePose = hits[0].pose;
            int counter =0;
            int maxAttempts = numberOfKeys * 5; 
           while (placedKeys.Count < numberOfKeys && counter < maxAttempts)
            {
                counter ++;
                Vector3 randomOffset = Random.insideUnitSphere * maxScatterDistance;
                randomOffset.y = 0f; 
                Vector3 spawnPosition = basePose.position + randomOffset;

                bool close = false;
                foreach (Vector3 existing in placedKeys){
                   if (Vector3.Distance(spawnPosition, existing) < minKeySpacing) {
                    close=true;
                    break;
                   }
                }
                 if (!close){
                    Instantiate(keyPrefab, spawnPosition, Quaternion.identity);
                    placedKeys.Add(spawnPosition);

                 }
            }

            keysPlaced = true;
           
        }
    }
}
