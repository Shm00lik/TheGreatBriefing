using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // INSTANCE
    public static GameManager instance;

    // FOOD
    public int MAX_FOOD_NUM = 5;
    public int MAX_FOOD_SPEED_MULTIPLIER = 3;
    public GameObject[] foodsPrefabs;
    public FoodsState foodsState = new FoodsState();
    private List<GameObject> currentFoods = new List<GameObject>();
    private float MIN_X_FOOD = 0;
    private float MAX_X_FOOD = 0;
    private float MIN_Y_FOOD = 8;
    private float MAX_Y_FOOD = 10;

    // SCORE
    private int score = 80;
    public int MAX_SCORE = 254;

    // UI
    public AudioSource eatSound;
    public TMP_Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        Reset();

        FirstFoodSpawn();
    }

    void Update()
    {
        foodsState.speedMultiplier = Mathf.Max(((float) score / MAX_SCORE) * MAX_FOOD_SPEED_MULTIPLIER, 1);
    }

    void SpawnFood()
    {
        int randomIndex = Random.Range(0, foodsPrefabs.Length);
        GameObject f = Instantiate(foodsPrefabs[randomIndex], new Vector3(Random.Range(MIN_X_FOOD, MAX_X_FOOD), Random.Range(MIN_Y_FOOD, MAX_Y_FOOD), 0), Quaternion.identity);
        currentFoods.Add(f);
    }

    public void PlayerCollide(GameObject other)
    {
        switch (other.tag)
        {
            case "Food":
                PlayerCollideFood(other);
                break;
            case "Enemy":
                PlayerCollideEnemy();
                break;
            default:
                break;
        }
    }

    private void PlayerCollideFood(GameObject food)
    {
        score += food.GetComponent<FoodController>().value;
        scoreText.text = score.ToString();
        // eatSound.Play();
        ReplaceFood(food);
    }

    private void PlayerCollideEnemy()
    {
        Reset();
        FirstFoodSpawn();
    }

    public void ReplaceFood(GameObject food)
    {
        currentFoods.Remove(food);
        Destroy(food);
        SpawnFood();
    }

    private FoodController GetFoodData(GameObject gameObject)
    {
        return gameObject.GetComponent<FoodController>();
    }

    private void FirstFoodSpawn()
    {
        for (int i = 0; i < MAX_FOOD_NUM; i++)
        {
            SpawnFood();
        }
    }

    private void Reset()
    {
        foodsState = new FoodsState();
        currentFoods = new List<GameObject>();
        // score = 0;
    }
}

public class FoodsState
{
    public float speedMultiplier = 1f;
    public bool speedUseRange = true;
}
