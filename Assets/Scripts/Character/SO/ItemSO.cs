using UnityEngine;


[CreateAssetMenu(menuName = "Collectables")]
public class ItemSO : ScriptableObject
{
    public AbilitySO abilityToGrant;
    public Sprite collectableSprite;
    public string collectableName;
    public string description;
    public GameObject collectedItem;
}
