using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class DragonflyController : MonoBehaviour /*IAbilityActivator*/
{

    private Vector2 originalPosition; // Original position of the dragonfly for resetting  
    [SerializeField] private float jumpForce;
    private bool canMove = false; 
    private Rigidbody2D rb;

    private float immortalTimer = 0;
    private AbilityManager abilityManager;
    private InputHandling inputHandling;
    private AbilitySO abilitySO;
    private bool isMultiplierActive = false;
    private RigidbodyConstraints2D originalConstraints;

    private float[] originalSpeeds;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Initialize the Rigidbody2D component here instead of Start
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

            // Tyhjenn‰ pelaajan currentAbility
            abilityManager.SetCurrentAbility(null);
            StartCoroutine(FireBreathCoroutine());
        }
    }
    private IEnumerator FireBreathCoroutine()
    {
        Debug.Log("coroutine");

        // Save original parallax speeds
        SaveOriginalParallaxSpeeds();

        // Manipulate Parallax Background
        ManipulateParallaxSpeed(true);

        // Immortal effect
        ActivateImmortality();

        // Fire Breath Particles
        HandleFireBreathParticles(true);

        // Score Multiplier
        isMultiplierActive = true;

        // Wait for ability duration
        yield return new WaitForSeconds(abilitySO.abilityDuration);

        // Reset Parallax Background Speed to original
        ResetParallaxSpeed();

        // Score Multiplier
        isMultiplierActive = false;

        // Reset Parallax Background
        ManipulateParallaxSpeed(false);

        // Reset Immortal effect
        DeactivateImmortality();

        // Stop Fire Breath Particles
        HandleFireBreathParticles(false);
    }
    private void SaveOriginalParallaxSpeeds()
    {
        InfiniteParallaxBackground parallaxBG = FindObjectOfType<InfiniteParallaxBackground>();
        originalSpeeds = (float[])parallaxBG.LayerScrollSpeeds.Clone();
    }
    private void ResetParallaxSpeed()
    {
        ManipulateParallaxSpeed(false);
    }
    private void ManipulateParallaxSpeed(bool increase)
    {
        InfiniteParallaxBackground parallaxBG = FindObjectOfType<InfiniteParallaxBackground>();
        if (increase)
        {
            for (int i = 0; i < parallaxBG.LayerScrollSpeeds.Length; i++)
            {
                parallaxBG.LayerScrollSpeeds[i] *= 20f;
            }
        }
        else
        {
            for (int i = 0; i < parallaxBG.LayerScrollSpeeds.Length; i++)
            {
                parallaxBG.LayerScrollSpeeds[i] = originalSpeeds[i];
            }
        }
    }
    //private void ManipulateParallaxSpeed(bool increase)
    //{
    //    InfiniteParallaxBackground parallaxBG = FindObjectOfType<InfiniteParallaxBackground>();
    //    float multiplier = increase ? 20f : 1f;

    //    for (int i = 0; i < parallaxBG.LayerScrollSpeeds.Length; i++)
    //    {
    //        parallaxBG.LayerScrollSpeeds[i] *= multiplier;
    //    }
    //}

    private void ActivateImmortality()
    {
        immortalTimer = abilitySO.abilityDuration;
        RigidbodyConstraints2D originalConstraints = rb.constraints;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        GetComponent<Collider2D>().enabled = false;
    }

    private void DeactivateImmortality()
    {
        immortalTimer = 0;
        rb.constraints = originalConstraints;
        GetComponent<Collider2D>().enabled = true;
    }

    private void HandleFireBreathParticles(bool shouldPlay)
    {
        Debug.Log("Checking fireBreathParticles");
        if (abilitySO.fireBreathParticles != null)
        {
            Debug.Log("Fire breath particles found");
            if (shouldPlay)
            {
                abilitySO.fireBreathParticles.Play();
            }
            else
            {
                abilitySO.fireBreathParticles.Stop();
            }
        }
        else
        {
            Debug.Log("No fire breath particles found");
        }
    }

    //private IEnumerator FireBreathCoroutine()
    //{
    //    Debug.Log("coroutine");
    //    InfiniteParallaxBackground parallaxBG = FindObjectOfType<InfiniteParallaxBackground>();
    //    float[] originalSpeeds = (float[])parallaxBG.LayerScrollSpeeds.Clone(); // Save the original speeds
    //    for (int i = 0; i < parallaxBG.LayerScrollSpeeds.Length; i++)
    //    {
    //        parallaxBG.LayerScrollSpeeds[i] *= 20f;
    //    }

    //    immortalTimer = abilitySO.abilityDuration;
    //    RigidbodyConstraints2D originalConstraints = rb.constraints;
    //    rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    //    GetComponent<Collider2D>().enabled = false;

    //    Debug.Log("Checking fireBreathParticles");
    //    if (abilitySO.fireBreathParticles != null)
    //    {
    //        Debug.Log("Fire breath particles found");
    //        abilitySO.fireBreathParticles.Play();
    //    }
    //    else
    //    {
    //        Debug.Log("No fire breath particles found");
    //    }

    //    isMultiplierActive = true; // The score multiplier is now active

    //    yield return new WaitForSeconds(abilitySO.abilityDuration);

    //    isMultiplierActive = false;

    //    // Reset the speeds back to the original values
    //    for (int i = 0; i < parallaxBG.LayerScrollSpeeds.Length; i++)
    //    {
    //        parallaxBG.LayerScrollSpeeds[i] = originalSpeeds[i];
    //    }
    //    immortalTimer = 0;
    //    rb.constraints = originalConstraints;
    //    GetComponent<Collider2D>().enabled = true;

    //    if (abilitySO.fireBreathParticles != null)
    //    {
    //        abilitySO.fireBreathParticles.Stop();
    //    }
    //}
    public int GetScoreMultiplier()
    {
        return isMultiplierActive ? 2 : 1; // Return 2 if multiplier is active, otherwise return 1
    }
}
