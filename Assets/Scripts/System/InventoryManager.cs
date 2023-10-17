using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] List<ItemSO> collectedItems = new();

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(ItemSO item)
    {
        if (collectedItems.Count < 3)
        {
            collectedItems.Add(item);
            // Voit suorittaa lisätoimia, kuten päivittää käyttöliittymää inventaarion kanssa.
            UIManager.Instance.UpdateUI();
        }
        else
        {
            Debug.Log("Inventaario on jo täynnä!");
            // Voit halutessasi lisätä muun käsitellyn logiikan, kuten ilmoituksen pelaajalle, jos inventaari on täynnä.
        }
    }

    public List<ItemSO> GetCollectedItems()
    {
        return collectedItems;
    }

    public void UseAbilityAndClearInventory(AbilitySO ability)
    {
        ItemSO itemToRemove = collectedItems.Find(item => item.abilityToGrant == ability);
        if (itemToRemove != null)
        {
            collectedItems.Remove(itemToRemove);
            UIManager.Instance.UpdateUI();
        }
    }

}
