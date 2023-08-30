using UnityEngine;

public enum AbilityType { FireBreath /*, Lis‰‰ muita kykytyyppej‰ tarvittaessa*/ }

[CreateAssetMenu(fileName = "New Ability", menuName = "Custom/Ability")]
public class AbilitySO : ScriptableObject/*IAbilityActivator*/
{
    // Lis‰‰ tarvittavia ominaisuuksia kyvylle
    // Muuttujat kyvyn arvoja varten
    public string abilityName;
    public string description;
    public Sprite icon;
    public float screenSpeedMultiplier;
    public float abilityDuration = 5.0f;
    public GameObject abilityEffect;
    public ParticleSystem fireBreathParticles;

    public AbilityType abilityType;

    //public void UseAbility(DragonflyController player)
    //{
    //    //DragonflyController player = FindObjectOfType<DragonflyController>();

    //    switch (abilityType)
    //    {
    //        case AbilityType.FireBreath:
    //            Debug.Log("aso");
    //            player.UseAbility(this); // Kutsu DragonflyControllerin metodia ja v‰lit‰ AbilitySO
    //            break;
    //            // K‰sittele muita kykytyyppej‰ tarvittaessa
    //    }
    //}

}
