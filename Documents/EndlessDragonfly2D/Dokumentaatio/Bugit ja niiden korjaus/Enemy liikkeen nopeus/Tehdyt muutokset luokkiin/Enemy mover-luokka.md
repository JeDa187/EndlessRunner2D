
Enemy mover luokassa vaihdettu nopeuteen vaikuttavat tekijät luokan sisällä oleviin muuttujiin. Aikaisemmin nopeuteen vaikuttivat pelaajan nopeuksiin liittyvät tekijät.
Alla alkuperäinen luokka ja siihen tehdyt muutokset. 

Alkuperäinen Scripti:

````csharp
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private DragonflyController dragonflyController;

    private void Start()
    {
        dragonflyController = FindObjectOfType<DragonflyController>();
        if (!dragonflyController)
        {
            Debug.LogError("DragonflyController was not found in the scene!");
        }
    }

    private void LateUpdate()
    {
        ////Check if SpeedBoost is active
        //float speedMultiplier = (dragonflyController && dragonflyController.IsSpeedBoostActive()) ? 8.0f : 1.0f;

        //// Move the object to the left
        //transform.position += Vector3.left * moveSpeed * speedMultiplier * Time.deltaTime;

        // Check if the object has gone off the left side of the screen
        Vector2 screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        // If the object's viewport position is less than -0.1 in the x axis (off the left side of the screen)
        if (screenPosition.x < -0.1)
        {
            ReturnToPool();
        }
    }

    public void SetSpeed(float cameraSpeed)
    {
        // Modify moveSpeed based on camera speed.
        moveSpeed = moveSpeed * cameraSpeed * 0.5f;
    }

    private void ReturnToPool()
    {
        EnemyPooler.Instance.ReturnToPool(gameObject.tag, gameObject);
    }
}

```

Muutettu Scripti:

````csharp
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] float baseMoveSpeed;
    [SerializeField] float customSpeedFactor = 1.0f;

    private void LateUpdate()
    {
        // Move the object to the left
        transform.position += Vector3.left * GetModifiedMoveSpeed() * Time.deltaTime;

        // Check if the object has gone off the left side of the screen
        Vector2 screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        // If the object's viewport position is less than -0.1 in the x axis (off the left side of the screen)
        if (screenPosition.x < -0.1)
        {
            ReturnToPool();
        }
    }

    public void SetCustomSpeedFactor(float factor)
    {
        customSpeedFactor = factor;
    }

    private float GetModifiedMoveSpeed()
    {
        return baseMoveSpeed * customSpeedFactor;
    }

    private void ReturnToPool()
    {
        EnemyPooler.Instance.ReturnToPool(gameObject.tag, gameObject);
    }
}

```