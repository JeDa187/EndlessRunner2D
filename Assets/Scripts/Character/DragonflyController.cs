using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class DragonflyController : MonoBehaviour
{
    private Vector2 originalPosition; // Stores the starting position of the dragonfly for later use
    [SerializeField] private float jumpForce; // Amount of force applied when the dragonfly jumps
    private bool canMove = false; // Determines if the dragonfly is allowed to move
    private Rigidbody2D rb; // Reference to the dragonfly's rigidbody

    private float immortalTimer = 0; // Timer used for determining immortal state
    private AbilityManager abilityManager; // Manages the abilities of the dragonfly
    private InputHandling inputHandling; // Handles user input
    private AbilitySO abilitySO; // Represents the current ability

    private bool isMultiplierActive = false; // Is the score multiplier active?

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the rigidbody component attached to the dragonfly
    }

    private void Start()
    {
        originalPosition = transform.position; // Store the initial position of the dragonfly
        abilityManager = GetComponent<AbilityManager>();
        inputHandling = GetComponent<InputHandling>();
    }

    private void Update()
    {
        if (!canMove) return; // Exit early if the dragonfly isn't allowed to move
        inputHandling.HandleInput(); // Check for and process any player input
    }

    public void Jump()
    {
        rb.velocity = Vector2.up * jumpForce; // Make the dragonfly jump by applying an upward force
    }

    public void EnablePlayerMovement(bool enable)
    {
        ToggleRigidbodyMovement(enable); // Enable or disable player movement
    }

    public void ToggleRigidbodyMovement(bool allowMovement)
    {
        canMove = allowMovement; // Update the movement flag
        rb.isKinematic = !allowMovement; // If movement is disallowed, make the rigidbody kinematic (unaffected by forces)

        if (!allowMovement) rb.velocity = Vector2.zero; // If movement is disallowed, halt the dragonfly
    }

    public void ResetDragonfly()
    {
        gameObject.SetActive(true); // Make the dragonfly active in the scene
        transform.position = originalPosition; // Move the dragonfly back to its original position
        rb.velocity = Vector2.zero; // Stop any movement of the dragonfly
    }

    public void UseAbility(AbilitySO currentAbility)
    {
        // If an ability exists and the immortal timer isn't active
        if (currentAbility != null && immortalTimer <= 0)
        {
            InventoryManager.Instance.UseAbilityAndClearInventory(currentAbility);
            abilitySO = currentAbility;

            // Clear the player's current ability
            abilityManager.SetCurrentAbility(null);
            StartCoroutine(FireBreathCoroutine()); // Begin the fire breath ability
        }
    }

    private IEnumerator FireBreathCoroutine()
    {
        Debug.Log("coroutine started");
        InfiniteParallaxBackground parallaxBG = FindObjectOfType<InfiniteParallaxBackground>();
        float[] originalSpeeds = (float[])parallaxBG.LayerScrollSpeeds.Clone(); // Store the original speeds for later

        // Increase the speed of the parallax background
        for (int i = 0; i < parallaxBG.LayerScrollSpeeds.Length; i++)
        {
            parallaxBG.LayerScrollSpeeds[i] *= 20f;
        }

        immortalTimer = abilitySO.abilityDuration;
        RigidbodyConstraints2D originalConstraints = rb.constraints;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY; // Prevent vertical movement
        GetComponent<Collider2D>().enabled = false; // Disable collisions

        if (abilitySO.fireBreathParticles != null) // If there are fire breath particles
        {
            Debug.Log("Fire breath particles found");
            abilitySO.fireBreathParticles.Play(); // Play the particle effect
        }
        else
        {
            Debug.Log("No fire breath particles found");
        }

        isMultiplierActive = true; // The score multiplier is now active

        yield return new WaitForSeconds(abilitySO.abilityDuration); // Wait for the duration of the ability

        isMultiplierActive = false; // The score multiplier is now inactive

        // Restore the original parallax background speeds
        for (int i = 0; i < parallaxBG.LayerScrollSpeeds.Length; i++)
        {
            parallaxBG.LayerScrollSpeeds[i] = originalSpeeds[i];
        }

        immortalTimer = 0;
        rb.constraints = originalConstraints; // Restore the original movement constraints
        GetComponent<Collider2D>().enabled = true; // Re-enable collisions

        if (abilitySO.fireBreathParticles != null)
        {
            abilitySO.fireBreathParticles.Stop(); // Stop the particle effect
        }
    }

    public int GetScoreMultiplier()
    {
        return isMultiplierActive ? 2 : 1; // Return 2 if multiplier is active, otherwise return 1
    }
}
