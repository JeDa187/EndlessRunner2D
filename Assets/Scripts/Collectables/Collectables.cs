using UnityEngine;

public class Collectables : MonoBehaviour
{
    public ItemSO collectedItem; // ScriptableObject, joka tallentaa objektin tiedot
    private AbilityManager abilityManager;
    public bool isScoreMultiplier; // Uusi muuttuja ScriptableObjektissa m‰‰ritt‰m‰‰n onko esine pistekerroin
    public int multiplierValue = 2; // Kerroin, joka sovelletaan, kun t‰m‰ esine ker‰t‰‰n. Oletuksena 2.

    private void Awake()
    {
        abilityManager = FindObjectOfType<AbilityManager>(); // Etsi AbilityManager Awake-metodissa
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int currentInventorySize = InventoryManager.Instance.GetCollectedItems().Count;
            int inventoryCapacity = InventoryManager.Instance.GetInventoryCapacity();

            if (currentInventorySize < inventoryCapacity)
            {
                // T‰ss‰ voit suorittaa toiminnon objektin ker‰‰misen yhteydess‰
                InventoryManager.Instance.AddItem(collectedItem);
                abilityManager.SetCurrentAbility(collectedItem.abilityToGrant);
                // Lopuksi voit poistaa objektin pelimaailmasta
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Your inventory is full, can not collect more collectables");
                // Voit halutessasi lis‰t‰ muun k‰sitellyn logiikan, kuten ilmoituksen pelaajalle, jos inventaari on t‰ynn‰.
            }
        }
    }
}
