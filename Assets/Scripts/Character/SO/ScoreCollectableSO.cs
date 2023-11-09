using UnityEditor.Timeline.Actions;
using UnityEngine;

public abstract class ScoreCollectableSO : ScriptableObject
{
    public abstract void Apply(GameObject target);
}
