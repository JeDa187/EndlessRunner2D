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

    public int GetInventoryCapacity()
    {
        return collectedItems.Capacity;
    }

    public void SetInventorySize(int size)
    {
        if (collectedItems.Capacity != size)
        {
            if (collectedItems.Count > size)
            {
                Debug.LogWarning("Inventory size set smaller than current item count. Removing excess items.");
                collectedItems.RemoveRange(size, collectedItems.Count - size);
            }
            collectedItems.Capacity = size;
        }
    }

    public void AddItem(ItemSO item)
    {
        if (collectedItems.Count < collectedItems.Capacity)
        {
            collectedItems.Add(item);
            // Voit suorittaa lis�toimia, kuten p�ivitt�� k�ytt�liittym�� inventaarion kanssa.
            UIManager.Instance.UpdateUI();
        }
        else
        {
            Debug.Log("Inventaario on jo t�ynn�!");
            // Voit halutessasi lis�t� muun k�sitellyn logiikan, kuten ilmoituksen pelaajalle, jos inventaari on t�ynn�.
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
