using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerCharacter", menuName = "PlayerCharacter")]
public class PlayerCharacterSO : ScriptableObject
{
    public string characterName;
    public Sprite characterSprite;
    public int jumpHeight;
    public int inventorySize;
    public int unlockScore;

    // Voit lisätä muita ominaisuuksia pelaajahahmolle tarpeen mukaan.
}