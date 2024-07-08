using UnityEngine;
using TMPro;

public class CharacterUI : MonoBehaviour
{
    [Header("Other Scripts Reference")]
    [SerializeField] private Character character;

    [Header("Interface Elements")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI stepsText;
    [SerializeField] private GameObject selectedBorder;
    [SerializeField] private GameObject buttonsContainer;

    private void OnEnable()
    {
        character.OnStepUpdate += HandleStepUpdate;
        character.OnHealthUpdate += HandleHpUpdate;
        character.OnCurrentActionIndexUpdate += HandleActionSelectedUpdate;
    }

    private void OnDisable()
    {
        character.OnStepUpdate -= HandleStepUpdate;
        character.OnHealthUpdate -= HandleHpUpdate;
        character.OnCurrentActionIndexUpdate -= HandleActionSelectedUpdate;
    }

    private void Awake()
    {
        if (!character)
        {
            Debug.LogError($"{name}: Character is missing! Disabling to avoid errors!");
            return;
        }
    }

    private void HandleStepUpdate(int step)
    {
        stepsText.text = step.ToString();
    }

    private void HandleHpUpdate(int health)
    {
        healthText.text = $"HP: {health}";
    }

    private void HandleActionSelectedUpdate(int actionId)
    {
        selectedBorder.transform.position = buttonsContainer.transform.GetChild(actionId).transform.position;
    }
}
