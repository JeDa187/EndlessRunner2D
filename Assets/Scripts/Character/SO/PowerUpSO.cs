using UnityEditor.Timeline.Actions;
using UnityEngine;

public abstract class PowerUpSO : ScriptableObject
{
    public abstract void Apply(GameObject target);
}
