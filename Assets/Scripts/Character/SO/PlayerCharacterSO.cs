using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerCharacter", menuName = "PlayerCharacter")]
public class PlayerCharacterSO : ScriptableObject
{

    public string characterName;
    public Sprite characterMenuSprite;
    public Sprite characterPlayableSprite;
    public float jumpHeight;
    public int inventorySize;
    public int unlockScore;

    public void UpdateAttributesByLevel(int newLevel)
    {
        // T‰‰ll‰ voit p‰ivitt‰‰ hahmon ominaisuuksia uuden tason perusteella.
        // Esimerkiksi voit p‰ivitt‰‰ hyˆkk‰ysvoimaa, kest‰vyytt‰ jne.
        // Esimerkki:
        //this.attackDamage = baseAttackDamage + (newLevel * levelAttackBonus);
        //this.maxHealth = baseMaxHealth + (newLevel * levelHealthBonus);
    }

    // Voit lis‰t‰ muita ominaisuuksia pelaajahahmolle tarpeen mukaan.
}