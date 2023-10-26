using UnityEngine;

public enum AbilityType { SpeedBoost, ScoreMultiplier/*, Lis‰‰ muita kykytyyppej‰ tarvittaessa*/ }

[CreateAssetMenu(fileName = "New Ability", menuName = "Custom/Ability")]
public class AbilitySO : ScriptableObject
{
    // Lis‰‰ tarvittavia ominaisuuksia kyvylle
    // Muuttujat kyvyn arvoja varten
    public string abilityName;
    public string description;
    public Sprite icon;
    public float abilityDuration = 5.0f;
    public ParticleSystem particleEffect;

    public AbilityType abilityType;

    public void UseAbility(DragonflyController player)
    {
        //DragonflyController player = FindObjectOfType<DragonflyController>();

        switch (abilityType)
        {
            case AbilityType.SpeedBoost:
                Debug.Log("aso");
                player.UseAbility(this); // Kutsu DragonflyControllerin metodia ja v‰lit‰ AbilitySO
                break;
                // K‰sittele muita kykytyyppej‰ tarvittaessa
        }
    }

}
