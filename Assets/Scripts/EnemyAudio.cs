using UnityEngine;
using System.Collections;

public class EnemyAudio : MonoBehaviour
{
    public AudioSource audioSource; // Assign this in the Inspector or dynamically
    public AudioClip[] gruntClips; // Array of grunt clips
    public Transform player; // Reference to the player's Transform
    public float minGruntDelay = 5f; // Minimum delay between grunts
    public float maxGruntDelay = 15f; // Maximum delay between grunts
    public float maxDistance = 20f; // Max distance at which the sound is audible
    public float minDistance = 3f; // Distance at which the sound is strongest

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }

        if (gruntClips.Length > 0)
        {
            StartCoroutine(PlayGruntSound());
        }
    }

    private IEnumerator PlayGruntSound()
    {
        while (true)
        {
            // Calculate distance to the player
            float playerDistance = Vector3.Distance(transform.position, player.position);

            // Adjust delay based on proximity
            float adjustedDelay = Mathf.Lerp(maxGruntDelay, minGruntDelay, 1f - Mathf.Clamp01(playerDistance / maxDistance));
            yield return new WaitForSeconds(adjustedDelay);

            // Adjust volume and play grunt sound if within range
            if (playerDistance <= maxDistance)
            {
                if (gruntClips.Length > 0)
                {
                    AudioClip selectedClip = gruntClips[Random.Range(0, gruntClips.Length)];
                    audioSource.clip = selectedClip;

                    // Adjust volume based on proximity
                    float volumeFactor = Mathf.InverseLerp(maxDistance, minDistance, playerDistance);
                    audioSource.volume = volumeFactor;

                    // Optionally adjust pitch for variety
                    audioSource.pitch = Random.Range(0.9f, 1.1f);

                    audioSource.Play();
                }
            }
        }
    }
}
