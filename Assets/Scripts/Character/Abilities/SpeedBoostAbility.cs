//using UnityEngine;
//using System.Collections;

//public class SpeedBoostAbility : MonoBehaviour, IAbility
//{
//    public float abilityDuration;
//    public float immortalTimer = 0;
//    private bool isMultiplierActive = false;
//    private RigidbodyConstraints2D originalConstraints;
//    private Rigidbody2D rb;
//    private InfiniteParallaxBackground parallaxBG;
//    private AbilitySO abilitySO;
//    private Coroutine speedBoostCoroutine;

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        parallaxBG = FindObjectOfType<InfiniteParallaxBackground>();
//    }
//    public void ActivateSpeedBoost()
//    {
//        if (immortalTimer <= 0)
//        {
//            StartCoroutine(SpeedBoostCoroutine());
//        }
//    }
//    public void SetAbilitySO(AbilitySO ability)
//    {
//        abilitySO = ability;
//    }
//    public bool IsSpeedBoostActive()
//    {
//        return isMultiplierActive;
//    }

//    private IEnumerator SpeedBoostCoroutine()
//    {
//        Debug.Log("coroutine");
//        bool originalKinematicState = rb.isKinematic;
//        ManipulateCameraSpeed(true);
//        ActivateImmortality();

//        isMultiplierActive = true;
//        yield return new WaitForSeconds(abilitySO.abilityDuration);

//        isMultiplierActive = false;
//        ManipulateCameraSpeed(false);

//        // Restore player's ability to move here
//        rb.constraints = originalConstraints;
//        rb.isKinematic = true;
//        rb.isKinematic = originalKinematicState;
//        yield return new WaitForSeconds(2.4f);
//        DeactivateImmortality();
//    }

//    private void ManipulateCameraSpeed(bool increase)
//    {
//        if (increase)
//        {
//            // Kiihdytä kameraa
//            parallaxBG.CameraSpeed *= 8f;
//        }
//        else
//        {
//            // Palauta kameran nopeus alkuperäiseen
//            parallaxBG.CameraSpeed /= 8f;
//        }
//    }

//    private void ActivateImmortality()
//    {
//        immortalTimer = abilitySO.abilityDuration;
//        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
//        GetComponent<Collider2D>().enabled = false;
//    }

//    private void DeactivateImmortality()
//    {
//        immortalTimer = 0;
//        rb.constraints = originalConstraints;
//        GetComponent<Collider2D>().enabled = true;
//    }

//    public int GetScoreMultiplier()
//    {
//        return isMultiplierActive ? 2 : 1;
//    }

//    public void Activate(DragonflyController controller)
//    {
//        speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine());
//    }

//    public void Deactivate(DragonflyController controller)
//    {
//        if (isMultiplierActive)
//        {
//            StopCoroutine(speedBoostCoroutine);        
//        }
//        isMultiplierActive = false;
//        ManipulateCameraSpeed(false);
//        DeactivateImmortality();
//        // Kaikki muu siivouskoodi
//    }
//}
