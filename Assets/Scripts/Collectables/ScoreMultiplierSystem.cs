using UnityEngine;
using System.Collections;

public class ScoreMultiplierSystem : MonoBehaviour
{
    public static ScoreMultiplierSystem Instance;

    private int multiplier;
    private float duration = 5.0f;
    private Coroutine timerCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectableHit()
    {
        Debug.Log("hitti");
        multiplier += 5; // Tämä on esimerkki, voit muokata logiikkaa tarpeen mukaan
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(ResetMultiplierAfterDelay());
    }

    private IEnumerator ResetMultiplierAfterDelay()
    {
        yield return new WaitForSeconds(duration);
        multiplier = 1;
    }

    public int GetCurrentMultiplier()
    {
        return multiplier;
    }
}
