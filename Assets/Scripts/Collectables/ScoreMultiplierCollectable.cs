using UnityEngine;

public class ScoreMultiplierCollectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ScoreMultiplierSystem.Instance.CollectableHit();
            Destroy(gameObject);
        }
    }
}
