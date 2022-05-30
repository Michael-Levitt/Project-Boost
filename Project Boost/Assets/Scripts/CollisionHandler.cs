using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delayTime = 1f;
    
    [Header("Audio Files")]
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;
    
    [Header("Particles")]
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    bool isTransitioning = false; // Check to stop sounds clips playing multiple times etc.
    bool collisionDisable = false; // To toggle collision

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable; // Toggle collision
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionDisable) { return; }

        switch (collision.gameObject.tag)
        {
             
            // Finishing point collision
            case "Finish":
                StartCoroutine(StartSuccessSequence());
                break;

            // Obstacle/ground collisions
            default:
                StartCoroutine(StartCrashSequence());
                break;
        }
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        // Loads first level once last level is completed
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        
        SceneManager.LoadScene(nextSceneIndex);
    }

    IEnumerator StartSuccessSequence()
    {
        isTransitioning = true;
        StopMovement();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        yield return new WaitForSeconds(delayTime); // Delay before next level
        LoadNextLevel();
    }

    IEnumerator StartCrashSequence()
    {
        isTransitioning = true;
        StopMovement();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        yield return new WaitForSeconds(delayTime); // Delay before reload
        ReloadLevel();
    }

    void StopMovement()
    {
        GetComponent<Movement>().enabled = false; // Stop movement upon collision
        audioSource.Stop(); // Stops thrusting sound
    }
}
