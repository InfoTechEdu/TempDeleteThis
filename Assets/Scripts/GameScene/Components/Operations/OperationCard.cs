using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OperationCard : MonoBehaviour
{
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Image valueImage;
    [SerializeField] private Image operationImage;

    private OperationCardSettings operationCardSettings;

    public void SetOperationCardSettings(OperationCardSettings settings)
    {
        operationCardSettings = settings;
    }

    public void ShowOperation(OperationTypes operationType, int value)
    {
        operationImage.enabled = true;

        valueText.text = value.ToString();
        valueImage.enabled = false;

        SetOperationImage(operationType);
    }

    public void ShowOperation(OperationTypes operationType, Sprite valueSprite)
    {
        valueText.text = string.Empty;
        valueImage.enabled = true;
        valueImage.sprite = valueSprite;

        SetOperationImage(operationType);
    }

    public void ShowExpression(string expression)
    {
        operationImage.enabled = false;
        valueImage.enabled = false;

        valueText.text = expression;
    }

    private void SetOperationImage(OperationTypes operationType)
    {
        switch (operationType)
        {
            case OperationTypes.plus:
                operationImage.sprite = operationCardSettings.plusSprite;
                operationImage.color = operationCardSettings.plusSpriteColor;
                break;
            case OperationTypes.minus:
                operationImage.sprite = operationCardSettings.minusSprite;
                operationImage.color = operationCardSettings.minusSpriteColor;
                break;
            case OperationTypes.multiply:
                operationImage.sprite = operationCardSettings.multiplySprite;
                operationImage.color = operationCardSettings.multiplySpriteColor;
                break;
            case OperationTypes.divide:
                operationImage.sprite = operationCardSettings.divideSprite;
                operationImage.color = operationCardSettings.divideSpriteColor;
                break;
        }
    }
}
