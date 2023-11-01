using UnityEngine;

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
}

