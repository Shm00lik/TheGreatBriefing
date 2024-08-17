using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodData : MonoBehaviour
{
    public float value;
    public float baseSpeed;
    public float[] speedRange = new float[2];

    public float getSpeed(float multiplier, bool useRange = false)
    {
        if (useRange)
        {
            return baseSpeed * multiplier * Random.Range(speedRange[0], speedRange[1]);
        }

        return baseSpeed * multiplier;
    }

    private void OnValidate()
    {
        if (speedRange[0] > speedRange[1])
        {
            float temp = speedRange[0];
            speedRange[0] = speedRange[1];
            speedRange[1] = temp;
        }
    }
}
