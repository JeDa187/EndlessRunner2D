using UnityEngine;

public class Collectables : MonoBehaviour
{
    public ItemSO collectedItem; // ScriptableObject, joka tallentaa objektin tiedot
    private AbilityManager abilityManager;

    private void Awake()
    {
        abilityManager = FindObjectOfType<AbilityManager>(); // Etsi AbilityManager Awake-metodissa
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // T‰ss‰ voit suorittaa toiminnon objektin ker‰‰misen yhteydess‰
            // Esimerkiksi lis‰t‰ objektin tiedot ScriptableObjectiin
            InventoryManager.Instance.AddItem(collectedItem);
            abilityManager.SetCurrentAbility(collectedItem.abilityToGrant);
            // Lopuksi voit poistaa objektin pelimaailmasta
            Destroy(gameObject);
        }
    }
}
