
using UnityEngine;
[System.Serializable]
public struct FloatRange
{
    public float min;
    public float max;
    public float RandomValueInRange
    {
        get { return Random.Range(min, max); }
    }
}


[System.Serializable]
public struct IntRange
{

    public int min, max;

    public int RandomValueInRange
    {
        get
        {
            return Random.Range(min, max + 1);
        }

    }
}