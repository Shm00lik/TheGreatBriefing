using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{

    // FOOD SETTINGS
    public int value;

    [Min(0)]
    public float baseSpeed;

    [Min(0)]
    public float[] speedRange = new float[2];

    // BLINK
    private bool isBlinking = false;
    public Color blinkColor = Color.red;
    public float blinkDuration = 0.1f;

    private void Update()
    {
        float speed = GetSpeed(GameManager.instance.foodsState.speedMultiplier, GameManager.instance.foodsState.speedUseRange);
        transform.position -= new Vector3(0, speed * Time.deltaTime);

        if (transform.position.y < -100)
        {
            GameManager.instance.ReplaceFood(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Finish")
        {
            if (isBlinking)
            {
                isBlinking = false;
                GameManager.instance.ReplaceFood(this.gameObject);
            } else
            {
                isBlinking = true;
                StartCoroutine(Blink());
            }
        }
    }

    private IEnumerator Blink()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;

        while (isBlinking)
        {
            spriteRenderer.color = blinkColor;
            yield return new WaitForSeconds(blinkDuration);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    public float GetSpeed(float multiplier, bool useRange = false)
    {
        if (useRange)
        {
            return GetRandomSpeed() * multiplier;
        }

        return baseSpeed * multiplier;
    }

    private float GetRandomSpeed()
    {
        return Random.Range(speedRange[0], speedRange[1]);
    }   
}