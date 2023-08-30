using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public AbilityManager abilityManager;
    public static UIManager Instance;
    public Sprite icon;
    public Text collectedItemsText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Aseta Instance arvoksi this (UIManager)
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void UpdateCollectedItemsText()
    {
        List<ItemSO> collectedItems = InventoryManager.Instance.GetCollectedItems();

        StringBuilder sb = new StringBuilder("Collected Items:\n");
        foreach (ItemSO item in collectedItems)
        {
            sb.Append(item.collectableName).Append("\n");
        }

        collectedItemsText.text = sb.ToString();
    }

    private void Start()
    {
        UpdateCollectedItemsText(); // P‰ivit‰ tekstikentt‰ alussa
    }

    //Voit kutsua t‰t‰ metodia esimerkiksi aina kun ker‰ttyj‰ objekteja p‰ivitet‰‰n
    public void UpdateUI()
    {
        UpdateCollectedItemsText();
        // Voit lis‰t‰ muita p‰ivityksi‰ tarpeesi mukaan.
    }
}
