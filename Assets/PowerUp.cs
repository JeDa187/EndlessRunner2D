using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpSO powerUpEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (powerUpEffect != null)
            {
                powerUpEffect.Apply(collision.gameObject);
            }
            else
            {
                Debug.LogError("PowerUp effect is not assigned!");
            }

            Destroy(gameObject);
        }
    }
}
