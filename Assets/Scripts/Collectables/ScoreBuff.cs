using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/ScoreBuff")]
public class ScoreBuff : PowerUps
{
    public int amount;

    public override void Apply(GameObject target)
    {
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.AddScore(amount);
        }
    }


}
