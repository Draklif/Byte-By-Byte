using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjects : MonoBehaviour
{
    [SerializeField] public ActivityScriptableObject[] scriptableActivities;
    [SerializeField] public ItemScriptableObject[] scriptableShopItems;
    [SerializeField] public EffectScriptableObject[] scriptableEffects;
    [SerializeField] public JobScriptableObject[] scriptableJobs;
}
