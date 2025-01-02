
using UnityEngine;
using System.Collections;


public class HealingBread : MonoBehaviour
{
    public float bobHeight = 0.2f; 
    public float bobSpeed = 3f;   
    public float despawnTime = 30f; 
    public float flashStartTime = 20f; 
    public float flashInterval = 0.2f; 

    private Vector3 startPosition;
    private SpriteRenderer spriteRenderer;
    private float elapsedTime = 0f; // Track time since bread was created
    private bool isFlashing = false;

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("HealingBread requires a SpriteRenderer component.");
        }

        Destroy(gameObject, despawnTime); // Automatically destroy after despawnTime
    }

    void Update()
    {
        // Handle bobbing animation
        transform.position = startPosition + new Vector3(0, Mathf.Sin(Time.time * bobSpeed) * bobHeight, 0);

        // Increment elapsed time
        elapsedTime += Time.deltaTime;

        // Start flashing if elapsed time exceeds flashStartTime
        if (elapsedTime >= flashStartTime && !isFlashing)
        {
            isFlashing = true;
            StartCoroutine(FlashEffect());
        }
    }

    private IEnumerator FlashEffect()
    {
        while (elapsedTime < despawnTime)
        {
            if (spriteRenderer != null)
            {
                // Toggle visibility
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }
            yield return new WaitForSeconds(flashInterval);
        }

        // Ensure sprite is visible at the end
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
    }
}


