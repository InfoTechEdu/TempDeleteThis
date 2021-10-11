using System;

public enum OperationTypes
{
    plus,
    minus,
    multiply,
    divide
}

[Serializable]
public class Expression
{
    public int startValue;
    public Operation operation;

    public Expression(int startValue, Operation operation)
    {
        this.startValue = startValue;
        this.operation = operation;
    }

    public int GetResult()
    {
        return operation.ApplyOperation(startValue);
    }
}

[Serializable]
public class Operation
{
    public int value;
    public OperationTypes operationType;

    public Operation(int value, OperationTypes operationType)
    {
        this.value = value;
        this.operationType = operationType;
    }

    public int ApplyOperation(int startValue)
    {
        return operationType switch
        {
            OperationTypes.plus => startValue + value,
            OperationTypes.minus => startValue - value,
            OperationTypes.multiply => startValue * value,
            OperationTypes.divide => startValue / value,
            _ => 0,
        };
    }

    public string GetOperationSymbol()
    {
        return operationType switch
        {
            OperationTypes.plus => "+",
            OperationTypes.minus => "-",
            OperationTypes.multiply => "*",
            OperationTypes.divide => "/",
            _ => "",
        };
    }
}

public static class MathematicalOperationsManager
{
    public static Operation GetRandomOperation(int operationValueMin, int operationValueMax)
    {
        Array operationTypesValues = Enum.GetValues(typeof(OperationTypes));


        OperationTypes operationType = (OperationTypes)operationTypesValues.GetValue(UnityEngine.Random.Range(0, operationTypesValues.Length));
        int operationValue = UnityEngine.Random.Range(operationValueMin, operationValueMax);

        return new Operation(operationValue, operationType);
    }

    public static Operation[] GetArrayOfRandomOperations(int arrayLength, int operationValueMin, int operationValueMax, int startValue)
    {
        Operation[] operations = new Operation[arrayLength];

        Array operationTypesValues = Enum.GetValues(typeof(OperationTypes));

        int value = startValue;

        for (int i = 0; i < arrayLength; i++)
        {
            OperationTypes operationType = (OperationTypes)operationTypesValues.GetValue(UnityEngine.Random.Range(0, operationTypesValues.Length));
            int operationValue = UnityEngine.Random.Range(operationValueMin, operationValueMax);

            Operation generatedOperation = new Operation(operationValue, operationType);

            if (generatedOperation.ApplyOperation(value) < 0)
            {
                i--;
                continue;
            }

            value = generatedOperation.ApplyOperation(value);

            operations[i] = new Operation(operationValue, operationType);
        }

        return operations;
    }
}
