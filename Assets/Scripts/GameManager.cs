using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Tooltip("Reference to the parallax manager.")]
    public Parallax parallaxManager;

    [Tooltip("Reference to the beverages manager.")]
    public BeveragesManager beverageManager;

    [Tooltip("Reference to the player.")]
    public GameObject mainCharacter;

    public GameObject sideBackground;
    public GameObject endText;
    public GameObject replayButton;
    public GameObject quitButton;

    public GameObject pressAnyKey;

    public GameObject showingPoliceman;

    [SerializeField]
    private float showingPosition = -9.2f;

    [SerializeField]
    private float showingRotation = -35.0f;

    // Limit the pressing of any key.
    private int anyKeyPressedCounter = 0;

    // All distance UI elements.
    private GameObject[] distanceUIElements;

    public bool gameOver = false;

    /// <summary>
    /// Stop all in-game features and actions on start screen.
    /// </summary>
    private void Start()
    {
        parallaxManager.scrollSpeed = 0.0f;
        distanceUIElements = GameObject.FindGameObjectsWithTag("Distance");

        ToggleDistanceUIElements(false);
    }

    /// <summary>
    /// Check whether the user presses any key only once in order to start the game.
    /// </summary>
    private void FixedUpdate()
    {
        if (Input.anyKey && anyKeyPressedCounter == 0)
        {
            PlayGame();

            anyKeyPressedCounter++;
        }
    }

    /// <summary>
    /// Initialize all in-game features and begin the game.
    /// </summary>
    private void PlayGame()
    {
        // Hide Press any key to play text.
        pressAnyKey.SetActive(false);

        // Reveal the distance UI element.
        ToggleDistanceUIElements(true);

        // Start the parallax effect.
        parallaxManager.Initialize();

        // Start the spawning of beverages.
        beverageManager.shouldSpawn = true;

        // Make the player run.
        mainCharacter.GetComponent<Animator>().SetBool("isRunning", true);
        mainCharacter.GetComponent<Player>().shouldDetectForMovementKeys = true;

        // Set the player's in-game position
        Vector3 mainCharacterPosition = mainCharacter.transform.position;
        Vector3 mainCharacterInGamePosition = new Vector3(-7.0f, mainCharacterPosition.y, mainCharacterPosition.z);

        // Smoothly move the player to his in-game position
        float speed = (parallaxManager.scrollSpeed - 1) * Time.deltaTime;
        SmoothlyMoveObjectTo(mainCharacter, mainCharacterInGamePosition, -speed);

        // Smoothly move the policemen to their in-game positions.
        GameObject[] policemen = GameObject.FindGameObjectsWithTag("Policeman");
        foreach (GameObject policeman in policemen)
        {
            Vector3 policemanPosition = policeman.transform.position;
            Vector3 policemanInGamePosition = new Vector3(Mathf.Floor(policemanPosition.x) - 8.0f, policemanPosition.y, policemanPosition.z);
            
            SmoothlyMoveObjectTo(policeman, policemanInGamePosition, -speed);
        }
    }

    /// <summary>
    /// Reveal the distance UI element.
    /// </summary>
    /// <param name="isActive">Whether the UI element is active.</param>
    private void ToggleDistanceUIElements(bool isActive)
    {
        foreach (GameObject element in distanceUIElements)
        {
            element.SetActive(isActive);
        }
    }

    /// <summary>
    /// Smoothly move an object to a desired target position with given speed in time.
    /// </summary>
    /// <param name="objectToMove">The object that needs to be moved.</param>
    /// <param name="targetPosition">The position at which the object should stop translating to the left.</param>
    /// <param name="speed">The speed at which the object is translating to the left.</param>
    private void SmoothlyMoveObjectTo(GameObject objectToMove, Vector3 targetPosition, float speed)
    {
        StartCoroutine(MoveObjectTo(objectToMove, targetPosition, speed));
    }

    /// <summary>
    /// Translate the object until it reaches the target position.
    /// </summary>
    /// <param name="objectToMove">The object that needs to be moved.</param>
    /// <param name="targetPosition">The position at which the object should stop translating to the left.</param>
    /// <param name="speed">The speed at which the object is translating to the left.</param>
    private IEnumerator MoveObjectTo(GameObject objectToMove, Vector3 targetPosition, float speed)
    {
        while (Mathf.Floor(objectToMove.transform.position.x) != targetPosition.x)
        {
            objectToMove.transform.Translate(speed, 0.0f, 0.0f);

            yield return null;
        }
    }

    public void GameOver(bool isCaught, bool isDead)
    {
        parallaxManager.scrollSpeed = 0;
        if (!gameOver)
        {
            if (isDead)
            {
                StartDyingPhase();
                endText.GetComponent<Text>().text = "You died from alcohol";
            }
            else if (isCaught)
            {
                StartCaughtPhase();
                endText.GetComponent<Text>().text = "You got caught";
            }
        }

        mainCharacter.GetComponent<Player>().shouldDetectForMovementKeys = false;
        gameOver = true;
        
        sideBackground.SetActive(true);
        endText.SetActive(true);
        replayButton.SetActive(true);
        quitButton.SetActive(true);
    }

    private void StartDyingPhase()
    {
        mainCharacter.GetComponent<Animator>().SetTrigger("death");
        mainCharacter.GetComponent<Animator>().SetBool("isDead", true);
    }

    private void StartCaughtPhase()
    {
        mainCharacter.GetComponent<Animator>().SetTrigger("caught");
        mainCharacter.GetComponent<Animator>().SetBool("isCaught", true);

        SmoothlyMoveObjectTo(showingPoliceman, new Vector3(Mathf.Floor(showingPosition), showingPoliceman.transform.position.y, showingPoliceman.transform.position.z), 6.0f * Time.deltaTime);
        SmoothlyMoveObjectTo(mainCharacter, new Vector3(Mathf.Floor(mainCharacter.transform.position.x) + 4.0f, mainCharacter.transform.position.y, mainCharacter.transform.position.z), 2.0f * Time.deltaTime);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Replay()
    {
        SceneManager.LoadScene(0);
    }
}
