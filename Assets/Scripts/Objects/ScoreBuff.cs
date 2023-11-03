using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/ScoreBuff")]
public class ScoreBuff : PowerUpSO
{
    public int amount;

    public override void Apply(GameObject target)
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(amount);
        }
        else
        {
            Debug.LogWarning("ScoreManager instance not found!");
        }
    }
}
