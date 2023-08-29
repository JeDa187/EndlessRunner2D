//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Item : MonoBehaviour
//{
//    public ItemSO collectible;
//    public AbilityManager abilityManager;

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        Item item = collision.GetComponent<Item>();
//        if (item != null)
//        {
//            AbilitySO ability = GetAbilityForItem(item);
//            abilityManager.UseCurrentAbility();
//            Destroy(collision.gameObject);
//        }
//    }

//    public AbilitySO GetAbilityForItem(Item item)
//    {
//        // Tämä on vain esimerkki. Voit muokata tätä osaa tarpeidesi mukaan.
//        if (item.name == "FireGem")
//        {
//            return new AbilitySO { name = "FireBreath", description = "Breathe fire!", speedMultiplier = 10 };
//        }
//        return new AbilitySO();
//    }

//}
