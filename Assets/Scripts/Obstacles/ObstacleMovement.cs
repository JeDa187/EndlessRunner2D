using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    private float speed;
    private float destructionXPosition;

    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x < destructionXPosition)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetDestructionXPosition(float position)
    {
        destructionXPosition = position;
    }
}
