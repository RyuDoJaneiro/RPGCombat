using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    [Header("Current Character")]
    [SerializeField] private int currentCharacterIndex;
    [SerializeField] private Character currentCharacter;
    [SerializeField] private List<Character> charactersGroup = new();

    [Header("Grid Position")]
    [SerializeField] private List<Transform> gridGroup = new();
    [SerializeField] private int currentPosIndex = 0;
    private const int gridWidth = 6;
    private int movementLimit;

    private int currentActionIndex = 0;

    private void OnEnable()
    {
        inputReader.OnCharacterMovement += HandleMovementUpdate;
        inputReader.OnActionSelected += ActionSelected;
    }
    
    private void OnDisable()
    {
        inputReader.OnCharacterMovement -= HandleMovementUpdate;
        inputReader.OnActionSelected -= ActionSelected;
    }

    private void Start()
    {
        if (charactersGroup.Count == 0)
        {
            Debug.LogError($"{name}: The characters list is null! Disabling to avoid errors!");
            enabled = false;
            return;
        }

        // Moves characters to random positions
        for (int i = 0; i < charactersGroup.Count; i++)
        {
            charactersGroup[i].MovePosition(gridGroup[UnityEngine.Random.Range(0, gridGroup.Count)].transform);
        }
        currentCharacter = charactersGroup[currentCharacterIndex];
        currentPosIndex = currentCharacter.transform.parent.GetSiblingIndex();
    }

    private void HandleMovementUpdate(Vector2 inputValue)
    {
        // If this turn is over, waits for the player to choose an action
        if (movementLimit >= currentCharacter.Velocity)
        {
            ChooseAction(inputValue);
            return;
        }

        if (inputValue.y > 0 && currentPosIndex - gridWidth >= 0) // UP
        {
            if (IsTileOccupied(currentPosIndex - gridWidth))
                return;
            currentPosIndex -= gridWidth;
        }
        else if (inputValue.y < 0 && currentPosIndex + gridWidth < gridGroup.Count) // DOWN
        {
            if (IsTileOccupied(currentPosIndex + gridWidth))
                return;
            currentPosIndex += gridWidth;
        }
        else if (inputValue.x < 0 && currentPosIndex % gridWidth > 0) // LEFT
        {
            if (IsTileOccupied(currentPosIndex - 1))
                return;
            currentPosIndex--;
        }
        else if (inputValue.x > 0 && currentPosIndex % gridWidth < gridWidth - 1) // RIGHT
        {
            if (IsTileOccupied(currentPosIndex + 1))
                return;
            currentPosIndex++;
        }

        movementLimit++;
        currentCharacter.RemainingSteps = currentCharacter.Velocity - movementLimit;
        currentCharacter.MovePosition(gridGroup[currentPosIndex]);
    }

    private void NextTurn()
    {
        currentCharacterIndex++;
        if (currentCharacterIndex >= charactersGroup.Count) currentCharacterIndex = 0;
        currentCharacter = charactersGroup[currentCharacterIndex];
        currentPosIndex = currentCharacter.transform.parent.GetSiblingIndex();
    }

    private void ChooseAction(Vector2 inputValue)
    {
        List<CharacterAction> actions = currentCharacter.CharacterActions;

        if (inputValue.y < 0)
        {
            currentActionIndex--;
            if (currentActionIndex < 0)
            {
                currentActionIndex = actions.Count - 1;
            }
        } 
        else if (inputValue.y > 0)
        {
            currentActionIndex++;
            if (currentActionIndex >= actions.Count)
            {
                currentActionIndex = 0;
            }
        }

        currentCharacter.CurrentActionIndex = currentActionIndex;
        currentCharacter.CurrentAction = actions[currentActionIndex];
    }

    public void ActionSelected()
    {
        currentCharacter.HandleSelectedAction();
        movementLimit = 0;
        currentCharacter.RemainingSteps = currentCharacter.Velocity;
        NextTurn();
    }

    private bool IsTileOccupied(int indexPosition)
    {
        if (gridGroup[indexPosition].childCount >= 1)
            return true;
        else
            return false;
    }

    private int[,] DetectTargets()
    {
        throw new NotImplementedException();
    }
}
