using UnityEngine;


[CreateAssetMenu(menuName = "Collectables")]
public class ItemSO : ScriptableObject
{    
    public AbilitySO abilityToGrant;
    public Sprite collectableSprite;
    public string collectableName;
    public string description;
    public GameObject collectedItem;
    public bool isScoreMultiplier = false; // Uusi attribuutti, joka m‰‰ritt‰‰ onko esine pistekerroin
    public int multiplierValue = 2; // Kerroin, joka sovelletaan, kun t‰m‰ esine ker‰t‰‰n. Oletuksena 2.
}
