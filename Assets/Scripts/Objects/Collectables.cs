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
            if (InventoryManager.Instance.GetCollectedItems().Count < 3)
            {
                // Tässä voit suorittaa toiminnon objektin keräämisen yhteydessä
                // Esimerkiksi lisätä objektin tiedot ScriptableObjectiin
                InventoryManager.Instance.AddItem(collectedItem);
                abilityManager.SetCurrentAbility(collectedItem.abilityToGrant);
                // Lopuksi voit poistaa objektin pelimaailmasta
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventaarisi on jo täynnä! Et voi kerätä enää lisää objekteja.");
                // Voit halutessasi lisätä muun käsitellyn logiikan, kuten ilmoituksen pelaajalle, jos inventaari on täynnä.
            }
        }
    }
}
