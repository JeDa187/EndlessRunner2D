using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public AbilitySO currentAbility;
    private DragonflyController dragonflyController;
    public AbilitySO fireBreathAbility;  // Tämä on "Fire Breath" -kyky

    private void Start()
    {
        dragonflyController = GetComponent<DragonflyController>();
    }

    public void SetCurrentAbility(AbilitySO ability)
    {
        currentAbility = ability;
    }

    public void UseCurrentAbility()
    {
        if (currentAbility != null)
        {
            dragonflyController.UseAbility(currentAbility);
        }
    }
}
