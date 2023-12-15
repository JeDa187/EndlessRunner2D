using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] float baseMoveSpeed;
    [SerializeField] float customSpeedFactor;
    [SerializeField] float customSpeedAccelerator;
    [SerializeField] float acceleratorForSpeedAccelerator;



    private void Update()
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
        return baseMoveSpeed * customSpeedFactor * AccelerateSpeed();
    }
    private float AccelerateSpeed()
    {
        return acceleratorForSpeedAccelerator + customSpeedAccelerator;
    }

    private void ReturnToPool()
    {
        EnemyPooler.Instance.ReturnToPool(gameObject.tag, gameObject);
    }
}
