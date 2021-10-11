using System;

[Serializable]
public struct IntegerRange
{
    public int min;
    public int max;

    public IntegerRange(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public int Length
    {
        get
        {
            return max - min;
        }
    }

    public int GetRangomValue()
    {
        return new Random().Next(min, max);
    }
}

[Serializable]
public struct FloatRange
{
    public float min;
    public float max;

    public FloatRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public float Length
    {
        get
        {
            return max - min;
        }
    }

    public float GetRangomValue()
    {
        return new Random().Next((int)min, (int)max * 10) / 10;
    }
}

