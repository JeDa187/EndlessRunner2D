using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerCharacter", menuName = "PlayerCharacter")]
public class PlayerCharacterSO : ScriptableObject
{

    public string characterName;
    public Sprite characterMenuSprite;
    public Sprite characterPlayableSprite;
    public float jumpForce;
    public int inventorySize;
    public int unlockScore;

    // Voit lis‰t‰ muita ominaisuuksia pelaajahahmolle tarpeen mukaan.
    public void ApplyCharacter(DragonflyController dragonFlyController)
    {
        // P‰ivitet‰‰n pelaajan sprite
        dragonFlyController.playerSpriteRenderer.sprite = characterPlayableSprite;

        // P‰ivitet‰‰n pelaajan hypp‰‰miskorkeus
        dragonFlyController.jumpForce = jumpForce;

        // P‰ivitet‰‰n pelaajan inventaarion koko
        InventoryManager.Instance.SetInventorySize(inventorySize);
        // T‰ss‰ voit lis‰t‰ muita p‰ivityksi‰ pelaajahahmolle tarpeen mukaan.
    }
}