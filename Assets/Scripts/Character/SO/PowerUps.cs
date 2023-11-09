using UnityEditor.Timeline.Actions;
using UnityEngine;

public abstract class PowerUps : ScriptableObject
{
    public abstract void Apply(GameObject target);
}
