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

//    public virtual void Activate(DragonflyController controller)
//    {
//        // T‰h‰n voi lis‰t‰ perustoimintoja, jotka soveltuvat kaikille kyvyille
//        // Esimerkiksi partikkeliefektin k‰ynnist‰minen
//        if (particleEffect != null)
//        {
//            // Luo ja toista partikkeliefekti
//        }

//        Debug.Log($"{abilityName} activated.");
//    }

//}
