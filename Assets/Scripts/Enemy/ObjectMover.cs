using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    // Speed at which the object moves
    [SerializeField] float moveSpeed;

    void Update()
    {
        // Move the object to the left
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // Check if the object has gone off the left side of the screen
        // WorldToViewportPoint converts a world space point into a viewport space point
        // In viewport space, (0, 0) is the bottom-left corner of the screen and (1, 1) is the top-right corner
        Vector2 screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        // If the object's viewport position is less than -0.1 in the x axis (off the left side of the screen)
        if (screenPosition.x < -0.1)
        {
            // Destroy the object
            // This removes the object from the scene and frees up memory
            Destroy(gameObject);
        }
    }
    public void SetSpeed(float cameraSpeed)
    {
        // Esimerkiksi, voit tehdä moveSpeedin suuremmaksi kertomalla se kameran nopeudella. 
        // Voit muuttaa kerrointa (tässä 0.5f) säätääksesi kuinka paljon nopeutta lisätään.
        moveSpeed = moveSpeed * cameraSpeed * 0.5f;
    }

}
