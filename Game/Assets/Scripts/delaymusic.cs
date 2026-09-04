
using System.Collections;
using UnityEngine;

public class DelayedAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float delayInSeconds = 2.0f; // Set your delay time here

    void Start()
    {
        // If not assigned in Inspector, try to get it automatically
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Start the timer to play the music
        StartCoroutine(PlayMusicWithDelay());
    }

    private IEnumerator PlayMusicWithDelay()
    {
        // Wait for the specified number of seconds
        yield return new WaitForSeconds(delayInSeconds);

        // Play the audio
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}

