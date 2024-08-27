using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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


    // ENEMIES
    public int MAX_ENEMY_SPEED_MULTIPLIER = 2;

    public GameObject talPrefab;
    public GameObject ramiPrefab;

    public EnemyState talsState = new EnemyState();
    public EnemyState ramisState = new EnemyState();

    private float nextTalSpawnTime = 0;
    private float nextRamiSpawnTime = 0;



    // SCORE
    private int score = 0;
    public int MAX_SCORE = 254;
    public PlayerLevel.Level currentPlayerLevel;


    // UI
    public AudioSource eatSound;
    public AudioSource backgroundMusic;
    public AudioSource buzzerSound;

    public TMP_Text scoreText;
    public TMP_Text playerLevelText;

    public Slider scodeSlider;


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

        Reset();
        FirstFoodSpawn();
    }

    void Update()
    {
        foodsState.speedMultiplier = Mathf.Max(((float) score / MAX_SCORE) * MAX_FOOD_SPEED_MULTIPLIER, 1);

        talsState.speedMultiplier  = Mathf.Max(((float) score / MAX_SCORE) * MAX_ENEMY_SPEED_MULTIPLIER, 1);
        talsState.maxNumberOfEntities = 2 + (score / 100);

        ramisState.speedMultiplier = Mathf.Max(((float)score / MAX_SCORE) * MAX_ENEMY_SPEED_MULTIPLIER, 1);
        ramisState.maxNumberOfEntities = 1 + (score / 50);

        ManageTals();
        ManageRamis();

        score = Mathf.Max(0, score);

        UpdatePlayerLevel();

        playerLevelText.text = PlayerLevel.getLevelLabel(currentPlayerLevel);
        scoreText.text = score.ToString();
        scodeSlider.value = score;

        CheckWin();
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

    void SpawnTal()
    {
        Instantiate(talPrefab, new Vector3(Random.Range(MIN_X_FOOD, MAX_X_FOOD), Random.Range(MIN_Y_FOOD, MAX_Y_FOOD)), Quaternion.identity);
        talsState.currentNumberOfEntities ++;
    }

    void SpawnRami()
    {
        Instantiate(ramiPrefab, new Vector3(Random.Range(MIN_X_FOOD, MAX_X_FOOD), Random.Range(MIN_Y_FOOD, MAX_Y_FOOD)), Quaternion.identity);
        ramisState.currentNumberOfEntities++;
    }

    public void PlayerCollideFood(GameObject food)
    {
        score += food.GetComponent<FoodController>().value;
        score = Mathf.Min(score, MAX_SCORE);

        eatSound.Play();

        ReplaceFood(food);
    }

    public IEnumerator PlayerCollideTal(GameObject tal)
    {
        PlayerPrefs.SetInt("score", score);
        Time.timeScale = 0;

        SpriteRenderer spriteRenderer = tal.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;

        backgroundMusic.Stop();
        buzzerSound.Play();

        yield return new WaitForSecondsRealtime(3f);

        buzzerSound.Stop();

        SceneManager.LoadScene("GameOver");
    }

    public IEnumerator PlayerCollideRami(GameObject tal)
    {
        score -= 50;

        Time.timeScale = 0;

        SpriteRenderer spriteRenderer = tal.GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;

        spriteRenderer.color = Color.red;

        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1;

        spriteRenderer.color = originalColor;
    }

    public void ReplaceFood(GameObject food)
    {
        Destroy(food);
        SpawnFood();
    }

    private void ManageTals()
    {
        if (talsState.currentNumberOfEntities < talsState.maxNumberOfEntities && Time.time > nextTalSpawnTime)
        { 
            nextTalSpawnTime = Time.time + Random.Range(1f, 5f);
            SpawnTal();
        }
    }

    private void ManageRamis()
    {
        if (ramisState.currentNumberOfEntities < ramisState.maxNumberOfEntities && Time.time > nextRamiSpawnTime)
        {
            nextRamiSpawnTime = Time.time + Random.Range(1f, 5f);
            SpawnRami();
        }
    }

    public void DestroyTal(GameObject tal)
    {
        talsState.currentNumberOfEntities--;

        Destroy(tal);

        if (talsState.currentNumberOfEntities < talsState.maxNumberOfEntities && Time.time > nextTalSpawnTime)
        {
            nextTalSpawnTime = Time.time + Random.Range(1f, 5f);
        }
    }

    public void DestroyRami(GameObject rami)
    {
        ramisState.currentNumberOfEntities--;

        Destroy(rami);

        if (ramisState.currentNumberOfEntities < ramisState.maxNumberOfEntities && Time.time > nextRamiSpawnTime)
        {
            nextRamiSpawnTime = Time.time + Random.Range(1f, 5f);
        }
    }

    private void CheckWin()
    {
        if (score >= MAX_SCORE)
        {
            PlayerPrefs.SetInt("score", score);
            SceneManager.LoadScene("Win");
        }
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

        PlayerPrefs.SetInt("score", 0);

        currentPlayerLevel = PlayerLevel.Level.BAS_SCOUTER;


        talsState = new EnemyState();
        ramisState = new EnemyState();

        nextTalSpawnTime = 0;
        nextRamiSpawnTime = 0;

    Time.timeScale = 1;
    }
}

public class FoodsState
{
    public float speedMultiplier = 1f;
    public bool speedUseRange = true;
}


public class EnemyState
{
    public float speedMultiplier = 1f;
    public int maxNumberOfEntities = 2;
    public int currentNumberOfEntities = 0;
}


public class PlayerLevel
{
    public enum Level
    {
        BAS_SCOUTER,
        SCOUTER,
        SUPER_SCOUTER,
        MANAGER,
        DATA_PROCESSOR,
        TABLEAU_GURU,
    }

    public static Level getLevel(int score)
    {
        if (score < 50)
        {
            return Level.BAS_SCOUTER;
        } 
        else if (score < 100)
        {
            return Level.SCOUTER;
        }
        else if (score < 150)
        {
            return Level.SUPER_SCOUTER;
        }
        else if (score < 200)
        {
            return Level.MANAGER;
        } 
        else if (score < 250)
        {
            return Level.DATA_PROCESSOR;
        } 
        else
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
            case Level.SUPER_SCOUTER:
                return "SUPER SCOUTER";
            case Level.MANAGER:
                return "MANAGER";
            case Level.DATA_PROCESSOR:
                return "DATA PROCESSOR";
            case Level.TABLEAU_GURU:
                return "TABLEAU GURU";
            default:
                return "";
        }
    }
}