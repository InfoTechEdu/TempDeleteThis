using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct OperationCardSettings 
{
    public Color plusSpriteColor;
    public Sprite plusSprite;
    public Color minusSpriteColor;
    public Sprite minusSprite;
    public Color multiplySpriteColor;
    public Sprite multiplySprite;
    public Color divideSpriteColor;
    public Sprite divideSprite;
}

public class OperationCardsController : Controller
{
    [SerializeField] private GameControllersStorage gameControllersStorage;
    [SerializeField] private GameObject operationCardPrefab;
    [SerializeField] private GameObject emptyPrefab;
    [SerializeField] private OperationCardSettings operationCardSettings;
    [SerializeField] private NumberSpritesListSO numberSpritesList;
    [SerializeField] private List<Transform> numberSpritesPositions;

    [SerializeField] [ReadOnly] private OperationCard currentOperationCard;

    private BezierTranslateController bezierTranslateController;

    public void SetCardsToPositions(Expression expression, Operation firstOperation, Operation secondOperation)
    {
        bezierTranslateController.SetTransformsToPoints(SpawnOperationCard(secondOperation), SpawnOperationCard(firstOperation), SpawnExpressionCard(expression));
    }

    public void AddNewOperationCardInSequence(Operation operation)
    {
        if (bezierTranslateController.CanTranslate)
            bezierTranslateController.AddTransformInSequence(SpawnOperationCard(operation).transform);
    }

    public void AddEmptyTransformInSequence()
    {
        if(bezierTranslateController.CanTranslate)
            bezierTranslateController.AddTransformInSequence(SpawnEmptyTransform());
    }

    public void ShowExpressionOnFocusCard(string expression)
    {
        if(currentOperationCard)
            currentOperationCard.ShowExpression(expression);
    }

    public void HideAllCards()
    {
        bezierTranslateController.HideAllTransforms();
    }

    public void ShowNumberSprites(int count)
    {
        if (count > numberSpritesPositions.Count)
            return;

        NumberSprite[] selectedNumberSprites = new NumberSprite[count];

        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, numberSpritesList.list.Count);

            NumberSprite numberSprite = numberSpritesList.list[randomIndex];

            if (selectedNumberSprites.Contains(numberSprite))
            {
                i--;

                continue;
            }

            selectedNumberSprites[i] = numberSprite;
        }

        for (int i = 0; i < count; i++)
        {
            SpawnNumberSprite(selectedNumberSprites[i]).position = numberSpritesPositions[i].position;
        }
    }

    private Transform SpawnOperationCard(Operation operation)
    {
        OperationCard spawnedOperationCardComponent = Instantiate(operationCardPrefab).GetComponent<OperationCard>();

        spawnedOperationCardComponent.SetOperationCardSettings(operationCardSettings);
        spawnedOperationCardComponent.ShowOperation(operation.operationType, operation.value);

        return spawnedOperationCardComponent.transform;
    }

    private Transform SpawnExpressionCard(Expression expression)
    {
        OperationCard spawnedExpressionCardComponent = Instantiate(operationCardPrefab).GetComponent<OperationCard>();

        spawnedExpressionCardComponent.SetOperationCardSettings(operationCardSettings);
        spawnedExpressionCardComponent.ShowExpression($"{expression.startValue} {expression.operation.GetOperationSymbol()} {expression.operation.value}");

        return spawnedExpressionCardComponent.transform;
    }

    private Transform SpawnNumberSpriteCard()
    {
        return transform;
    }

    private Transform SpawnNumberSprite(NumberSprite numberSprite)
    {
        return transform;
    }

    private void OnTransformFinishedBezier(Transform transform)
    {
        if(transform)
            Destroy(transform.gameObject);
    }

    private void OnChangeTransformOnFocusPoint(Transform transform)
    {
        if (transform)
        {
            OperationCard operationCard;

            operationCard = transform.GetComponent<OperationCard>();

            if (operationCard)
            currentOperationCard = operationCard;
        }
        else
        {
            currentOperationCard = null;
        }
    }

    private Transform SpawnEmptyTransform()
    {
        return Instantiate(emptyPrefab).transform;
    }

    protected override void OnInitialize()
    {
        bezierTranslateController = gameControllersStorage.bezierTranslateController;
        bezierTranslateController.OnChangeTransformOnFocusPoint += OnChangeTransformOnFocusPoint;
        bezierTranslateController.OnTransformFinishedBezier += OnTransformFinishedBezier;
    }

    protected override void OnActivate()
    {

    }

    protected override void OnDiactivate()
    {
        bezierTranslateController.OnChangeTransformOnFocusPoint -= OnChangeTransformOnFocusPoint;
        bezierTranslateController.OnTransformFinishedBezier -= OnTransformFinishedBezier;
    }
}
