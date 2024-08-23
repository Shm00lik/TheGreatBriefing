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
    private float MIN_X_FOOD = -7;
    private float MAX_X_FOOD = 7;
    private float MIN_Y_FOOD = 8;
    private float MAX_Y_FOOD = 10;

    // TAL
    public GameObject talPrefab;
    public GameObject tal;

    // SCORE
    private int score = 0;
    public int MAX_SCORE = 254;
    public PlayerLevel.Level currentPlayerLevel;

    // UI
    public AudioSource eatSound;
    public TMP_Text scoreText;
    public TMP_Text playerLevelText;


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
        tal = Instantiate(talPrefab, new Vector3(0f, 10f, 0f), Quaternion.identity);
    }

    void Update()
    {
        foodsState.speedMultiplier = Mathf.Max(((float) score / MAX_SCORE) * MAX_FOOD_SPEED_MULTIPLIER, 1);

        if (tal)
        {
            tal.transform.position = new Vector3(tal.transform.position.x, tal.transform.position.y - 0.5f * Time.deltaTime);
        }

        UpdatePlayerLevel();
        playerLevelText.text = PlayerLevel.getLevelLabel(currentPlayerLevel);
    }

    void UpdatePlayerLevel()
    {
        currentPlayerLevel = PlayerLevel.getLevel(score);
    }

    void SpawnFood()
    {
        int randomIndex = Random.Range(0, foodsPrefabs.Length);
        Instantiate(foodsPrefabs[randomIndex], new Vector3(Random.Range(MIN_X_FOOD, MAX_X_FOOD), Random.Range(MIN_Y_FOOD, MAX_Y_FOOD)), Quaternion.identity);
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
        //eatSound.Play();
        ReplaceFood(food);
    }

    private void PlayerCollideEnemy()
    {
        Reset();
        FirstFoodSpawn();
    }

    public void ReplaceFood(GameObject food)
    {
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

        score = 0;
        scoreText.text = "0";

        currentPlayerLevel = PlayerLevel.Level.BAS_SCOUTER;

        FoodController[] allFoods = FindObjectsByType<FoodController>(FindObjectsSortMode.None);

        foreach(FoodController food in allFoods)
        {
            Destroy(food.gameObject);
        }
    }
}

public class FoodsState
{
    public float speedMultiplier = 1f;
    public bool speedUseRange = true;
}


public class PlayerLevel
{
    public enum Level
    {
        BAS_SCOUTER,
        SCOUTER,
        SUPER_SCOUTING,
        MANAGER,
        CPU,
        TABLEAU_GURU
    }

    public static Level getLevel(int score)
    {
        if (score < 10)
        {
            return Level.BAS_SCOUTER;
        } else if (score < 20)
        {
            return Level.SCOUTER;
        } else if (score < 30)
        {
            return Level.MANAGER;
        } else if (score < 40)
        {
            return Level.CPU;
        } else
        {
            return Level.TABLEAU_GURU;
        }
    }

    public static string getLevelLabel(Level level)
    {
        switch (level)
        {
            case Level.BAS_SCOUTER:
                return "BAS SCOUTER";
            case Level.SCOUTER:
                return "SCOUTER";
            case Level.MANAGER:
                return "MANAGER";
            case Level.CPU:
                return "CPU";
            case Level.TABLEAU_GURU:
                return "TABLEAU GURU";
            default:
                return "";
        }
    }
}