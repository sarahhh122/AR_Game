using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enablePhysicsOnEvent : MonoBehaviour
{
    [SerializeField] private Rigidbody rb; 

    void Start()
    {
         if (rb == null)
            rb = GetComponent<Rigidbody>();
        UIButtonHandler.OnUIStartButtonClicked += StartPhysicsOnButtonClicked;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void StartPhysicsOnButtonClicked()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private void OnDestroy()
    {
        UIButtonHandler.OnUIStartButtonClicked -= StartPhysicsOnButtonClicked;
    }
    
 private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Optional: add particle or sound effect here

            // Add score safely
            var scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.AddPoint();
            }

            Destroy(gameObject); // remove this target
        }
    }
}
