using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class DragonflyController : MonoBehaviour /*IAbilityActivator*/
{
    [SerializeField] private CameraController cameraController;
    private float originalCameraSpeed;
    private Vector2 originalPosition; // Original position of the dragonfly for resetting  

    [SerializeField] private float jumpForce;
    private bool canMove = false; 
    private Rigidbody2D rb;      

    private bool isImmortal = false;
    private float immortalTimer = 0;
    private Coroutine fireBreathCoroutine;
    private AbilityManager abilityManager;
    private InputHandling inputHandling;
    private AbilitySO abilitySO;

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
            Debug.Log("ss");
            Debug.Log("useability");
            InventoryManager.Instance.UseAbilityAndClearInventory(currentAbility);
            abilitySO = currentAbility; //
            StartCoroutine(FireBreathCoroutine());
        }
        //Debug.Log("ss");
        //Debug.Log("useability");
        //InventoryManager.Instance.UseAbilityAndClearInventory(currentAbility);
        //StartCoroutine(FireBreathCoroutine());
    }

    private IEnumerator FireBreathCoroutine()
    {
        Debug.Log("korokoro");
        //isImmortal = true;
        immortalTimer = abilitySO.abilityDuration;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;

        if (abilitySO.fireBreathParticles != null)
        {
            abilitySO.fireBreathParticles.Play();
        }

        yield return new WaitForSeconds(abilitySO.abilityDuration);

        //isImmortal = false;
        immortalTimer = 0;
        rb.isKinematic = false;
        if (abilitySO.fireBreathParticles != null)
        {
            abilitySO.fireBreathParticles.Stop();
        }
    }
}
