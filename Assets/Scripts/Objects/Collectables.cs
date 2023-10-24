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
            int currentInventorySize = InventoryManager.Instance.GetCollectedItems().Count;
            int inventoryCapacity = InventoryManager.Instance.GetInventoryCapacity();

            if (currentInventorySize < inventoryCapacity)
            {
                // T�ss� voit suorittaa toiminnon objektin ker��misen yhteydess�
                InventoryManager.Instance.AddItem(collectedItem);
                abilityManager.SetCurrentAbility(collectedItem.abilityToGrant);
                // Lopuksi voit poistaa objektin pelimaailmasta
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventaarisi on jo t�ynn�! Et voi ker�t� en�� lis�� objekteja.");
                // Voit halutessasi lis�t� muun k�sitellyn logiikan, kuten ilmoituksen pelaajalle, jos inventaari on t�ynn�.
            }
        }
    }
}
