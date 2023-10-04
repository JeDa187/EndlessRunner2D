using UnityEngine;

public class ObstacleDestruction : MonoBehaviour
{
    private float destructionXPosition;

    private void Update()
    {
        if (transform.position.x < destructionXPosition)
        {
            Destroy(gameObject);
        }
    }

    public void SetDestructionXPosition(float position)
    {
        destructionXPosition = position;
    }
}
