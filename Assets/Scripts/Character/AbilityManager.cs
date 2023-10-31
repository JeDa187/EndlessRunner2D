using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public AbilitySO currentAbility;
    private DragonflyController dragonflyController;
    public static AbilityManager Instance { get; private set; }

    private void Awake()
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
