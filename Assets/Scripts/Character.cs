using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum CharacterAction
{
    Nothing,
    MeleeAttack,
    RangedAttack,
    HealSelf,
    Heal
}

public class Character : MonoBehaviour
{
    [Header("Current Position")]
    [SerializeField] private Transform grillPosition;

    [Header("Stats")]
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private int currentHealth;
    [SerializeField] private int velocity = 3;
    [SerializeField] private int meleeDamage = 2;
    [SerializeField] private int rangedDamage = 0;
    [SerializeField] private int healAmount = 2;
    [SerializeField] private int remainingSteps;

    [Header("Action Selection")]
    [SerializeField] private Character currentTarget;
    [SerializeField] private CharacterAction currentAction;
    [SerializeField] private List<CharacterAction> characterActions = new();
    private int currentActionIndex;

    public event Action<int> OnStepUpdate = delegate { };
    public event Action<int> OnHealthUpdate = delegate { };
    public event Action<int> OnCurrentActionIndexUpdate = delegate { };

    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int CurrentHealth
    {
        get => currentHealth;

        set
        {
            currentHealth = value;
            OnHealthUpdate?.Invoke(currentHealth);
        }
    }
    public int Velocity { get => velocity; set => velocity = value; }
    public int RemainingSteps 
    { 
        get => remainingSteps; 
        
        set 
        { 
            remainingSteps = value;
            OnStepUpdate?.Invoke(remainingSteps);
        } 
    }

    public int CurrentActionIndex
    {
        get => currentActionIndex;
        set
        {
            currentActionIndex = value;
            OnCurrentActionIndexUpdate?.Invoke(currentActionIndex);
        }
    }

    public CharacterAction CurrentAction { get => currentAction; set => currentAction = value; }

    public List<CharacterAction> CharacterActions { get => characterActions; set => characterActions = value; }

    private void Start()
    {
        currentHealth = maxHealth;
        remainingSteps = velocity;
    }

    public void HandleSelectedAction()
    {
        switch (currentAction)
        {
            case CharacterAction.Nothing:
                Debug.Log($"{name}: Does nothing!");
                break;
            case CharacterAction.MeleeAttack:
                currentTarget.ReceiveDamage(meleeDamage);
                Debug.Log($"{name}: attack {currentTarget}!");
                break;
            case CharacterAction.RangedAttack:
                currentTarget.ReceiveDamage(rangedDamage);
                Debug.Log($"{name}: attack {currentTarget}!");
                break;
            case CharacterAction.Heal:
                Heal(currentTarget, healAmount);
                Debug.Log($"{name}: heal {currentTarget}!");
                break;
            case CharacterAction.HealSelf:
                Heal(this, healAmount);
                Debug.Log($"{name}: heals itself!");
                break;  
        }
    }

    public void Heal(Character target, int damageHealed)
    {
        target.CurrentHealth += (target.CurrentHealth + damageHealed >= target.MaxHealth) ? target.MaxHealth : damageHealed; 
    }

    public void ReceiveDamage(int damageAmount)
    {
        currentHealth -= Mathf.Max(currentHealth -= damageAmount, 0);

        if (currentHealth <= 0)
            Die();
    }

    public void MovePosition(Transform grillPosition)
    {
        transform.SetParent(grillPosition);
        transform.position = grillPosition.position;
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
