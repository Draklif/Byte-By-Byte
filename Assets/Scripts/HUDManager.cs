using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI RemainingTimer;
    [SerializeField] public TextMeshProUGUI ActualTimer;
    [SerializeField] public TextMeshProUGUI HappinessValue;
    [SerializeField] public TextMeshProUGUI HungerValue;
    [SerializeField] public TextMeshProUGUI StressValue;
    [SerializeField] public TextMeshProUGUI WeightValue;
    [SerializeField] public TextMeshProUGUI KnowledgeValue;
    [SerializeField] public Button SkipButton;
}
