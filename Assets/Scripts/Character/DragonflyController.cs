using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class DragonflyController : MonoBehaviour
{

    private Vector2 originalPosition; // Original position of the dragonfly for resetting  
    public float jumpForce;
    private bool canMove = false;
    private InfiniteParallaxBackground parallaxBG;
    private Rigidbody2D rb;
    public SpriteRenderer playerSpriteRenderer;
    private float immortalTimer = 0;
    private AbilityManager abilityManager;
    private InputHandling inputHandling;
    private AbilitySO abilitySO;
    private bool isMultiplierActive = false;
    private RigidbodyConstraints2D originalConstraints;

    //private float[] originalSpeeds;

    private void Awake()
    {
        // Etsi InfiniteParallaxBackground-olio pelimaailmasta
        parallaxBG = FindObjectOfType<InfiniteParallaxBackground>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>(); // Initialize the Rigidbody2D component here instead of Start
        originalConstraints = rb.constraints;
    }

    private void Start()
    {
        originalPosition = transform.position; // Save the original position
        abilityManager = GetComponent<AbilityManager>();
        inputHandling = GetComponent<InputHandling>();
    }

    private void Update()
    {
        if (!canMove) return; // If canMove is false, exit the function
        inputHandling.HandleInput();        // Check and process player input
    }

    public void Jump()
    {
        rb.velocity = Vector2.up * jumpForce;
    }

    // IPlayerInteraction implementation
    public void EnablePlayerMovement(bool enable)
    {
        ToggleRigidbodyMovement(enable);
    }

    public void ToggleRigidbodyMovement(bool allowMovement)
    {
        canMove = allowMovement; // Set the movement flag
        rb.isKinematic = !allowMovement; // Toggle kinematic state based on movement

        if (!allowMovement) rb.velocity = Vector2.zero; // Stop any movement
    }

    public void ResetDragonfly()
    {
        gameObject.SetActive(true); // Activate the dragonfly object
        transform.position = originalPosition; // Reset position
        rb.velocity = Vector2.zero; // Stop any movement
    }

    // Kutsutaan kun pelaaja k‰ytt‰‰ kyky‰
    public void UseAbility(AbilitySO currentAbility)
    {
        if (currentAbility != null && immortalTimer <= 0)
        {
            InventoryManager.Instance.UseAbilityAndClearInventory(currentAbility);
            abilitySO = currentAbility;

            if (InventoryManager.Instance.GetCollectedItems().Count == 0)
            {
                abilityManager.SetCurrentAbility(null);
                Debug.Log("Inventory on tyhj‰");
            }

            StartCoroutine(SpeedBoostCoroutine());
        }
    }
    public bool IsSpeedBoostActive()
    {
        // Return the state of the speed boost (true if active, false otherwise)
        return isMultiplierActive;
    }
    private void ManipulateCameraSpeed(bool increase)
    {
        if (increase)
        {
            // Kiihdyt‰ kameraa
            parallaxBG.CameraSpeed *= 8f;
        }
        else
        {
            // Palauta kameran nopeus alkuper‰iseen
            parallaxBG.CameraSpeed /= 8f;
        }
    }

    private void ActivateImmortality()
    {
        immortalTimer = abilitySO.abilityDuration;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        GetComponent<Collider2D>().enabled = false;
    }

    private void DeactivateImmortality()
    {
        immortalTimer = 0;
        rb.constraints = originalConstraints;
        GetComponent<Collider2D>().enabled = true;
    }

    private IEnumerator SpeedBoostCoroutine()
    {
        Debug.Log("coroutine");

        bool originalKinematicState = rb.isKinematic;       

        ManipulateCameraSpeed(true);

        // Immortal effect
        ActivateImmortality();

        // Score Multiplier
        isMultiplierActive = true;

        // Wait for ability duration
        yield return new WaitForSeconds(abilitySO.abilityDuration);

        // Score Multiplier
        isMultiplierActive = false;

        ManipulateCameraSpeed(false);

     
        // Restore player's ability to move here
        rb.constraints = originalConstraints;
        rb.isKinematic = true;
        rb.isKinematic = originalKinematicState;
        

        yield return new WaitForSeconds(2.4f);

        // Reset Immortal effect
        DeactivateImmortality();

    }

    public int GetScoreMultiplier()
    {
        // Return 2 if multiplier is active, otherwise return 1
        return isMultiplierActive ? 2 : 1;
    }
}
