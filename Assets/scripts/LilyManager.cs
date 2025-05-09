using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class LilyManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject lilyPrefab;
    public float spawnDistance = 1.5f;

    [Header("Particle Effects")]
    public ParticleSystem spawnParticles;
    public ParticleSystem hitParticles;
    public ParticleSystem deathParticles;

    [Header("Health")]
    public float maxHealth = 40f;
    public float currentHealth;
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    private Animator lilyAnimator;
    private bool isDying = false;

    public Camera arCamera;

    private GameObject lilyInstance;
    private Collider lilyCollider;
    
    public GameObject winPanel;
    public GameObject mainMenuButton; 

    [Header("Shield Lily")]
    public GameObject shieldPrefab;          
    public float shieldMinInterval = 4f;     
    public float shieldMaxInterval = 9f;     
    public float shieldDuration    = 4.5f;  
    bool shieldActive = false;              
    GameObject shieldInstance;               
 

    void Awake()
    {
        EnhancedTouchSupport.Enable();
       currentHealth = maxHealth;
    }

    void OnDestroy()
    {
        EnhancedTouchSupport.Disable();
    }

    void Start()
    {
        if (arCamera == null)
            arCamera = Camera.main;
        
        
        SpawnCharacter();

        if (healthSlider != null)
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        UpdateHealthUI();
        StartCoroutine(ShieldRoutine()); 
    }

    IEnumerator ShieldRoutine()
    {
    while (!isDying)
    {
        // wait random time before next shield :contentReference[oaicite:2]{index=2}
        float wait = Random.Range(shieldMinInterval, shieldMaxInterval);
        yield return new WaitForSeconds(wait);

        // spawn shield
        shieldActive  = true;
        shieldInstance = Instantiate(
            shieldPrefab,
            lilyInstance.transform.position,
            Quaternion.identity,
            lilyInstance.transform);           

        yield return new WaitForSeconds(shieldDuration); 

        Destroy(shieldInstance);
        shieldActive = false;
    }
}

   void SpawnCharacter()
    {
        Vector3 spawnPos = arCamera.transform.position + arCamera.transform.forward * spawnDistance - new Vector3(0, 0.5f, 0);
        Quaternion spawnRot = new Quaternion(0.04252093f, 0.9915443f, -0.00263807f, 0.12257615f);

        lilyInstance = Instantiate(lilyPrefab, spawnPos, spawnRot);
        lilyCollider = lilyInstance.GetComponent<Collider>();
        lilyAnimator = lilyInstance.GetComponent<Animator>();

        // Spawn effect
        if (spawnParticles != null)
        {
            var ps = Instantiate(spawnParticles, lilyInstance.transform.position, Quaternion.identity);
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }
    }
    void Update()
    {
        var touches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;
        if (touches.Count == 0) return;

        var t = touches[0];
        if (t.phase != UnityEngine.InputSystem.TouchPhase.Ended)
            return;
        Ray ray = arCamera.ScreenPointToRay(t.screenPosition);
       
        if (Physics.Raycast(ray, out var hit) && hit.collider == lilyCollider)
        {
            OnLilyHit(hit.point);
        }
    }

    

    private void OnLilyHit(Vector3 hitPoint)
    {
        if (isDying || shieldActive) return;

        if (lilyAnimator != null)
          lilyAnimator.SetTrigger("Hit");

        if (hitParticles != null)
        {
            var ps = Instantiate(hitParticles, hitPoint, Quaternion.identity);
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }

        // Decrease health
        currentHealth = Mathf.Max(0f, currentHealth - 10f);
        UpdateHealthUI();

        // 3) Death effect & cleanup
        if (currentHealth <= 0f)
        {
            StartCoroutine(PlayDeathAndDestroy());
        }
    }
    private IEnumerator PlayDeathAndDestroy()
    {
        isDying = true;

        if (deathParticles != null)
        {
            var ps = Instantiate(deathParticles, lilyInstance.transform.position, Quaternion.identity);
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }

        // Animator death
        if (lilyAnimator != null)
            lilyAnimator.SetTrigger("Death");

        // wait for animation to finish
        float waitTime = 0.0f;
        if (lilyAnimator != null)
        {
            var clip = lilyAnimator.GetCurrentAnimatorStateInfo(0);
            waitTime = clip.length;
        }
        yield return new WaitForSeconds(waitTime);

        Destroy(lilyInstance);

        yield return new WaitForSeconds(waitTime);

        if (winPanel != null){
            healthSlider.gameObject.SetActive(false);
            winPanel.SetActive(true);
            mainMenuButton.gameObject.SetActive(true);
        }        
        
    }


    private void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;
        if (healthText != null)
            healthText.text = currentHealth.ToString();
    }


    // === Voice‑command damage =================================================
public void OnVoiceAttack()
{
    if (isDying|| shieldActive || currentHealth==0) return;                      

    currentHealth = Mathf.Max(0f, currentHealth - 20f);  
    UpdateHealthUI();

    if (currentHealth <= 0f)
        StartCoroutine(PlayDeathAndDestroy()); 
}

}
