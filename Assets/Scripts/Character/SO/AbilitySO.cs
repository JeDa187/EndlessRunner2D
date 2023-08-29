using UnityEngine;

public enum AbilityType { FireBreath, JumpBoost /*, Lis‰‰ muita kykytyyppej‰ tarvittaessa*/ }

[CreateAssetMenu(fileName = "New Ability", menuName = "Custom/Ability")]
public class AbilitySO : ScriptableObject/*IAbilityActivator*/
{
    public string abilityName;
    public string description;
    public Sprite icon;
    public GameObject abilityEffectPrefab;
    public ParticleSystem fireBreathParticles;
    // Lis‰‰ tarvittavia ominaisuuksia kyvylle
    public AbilityType abilityType;

    // Muuttujat kyvyn arvoja varten
    public float speedMultiplier;
    public float abilityDuration = 5.0f;
    public float jumpHeight;

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
