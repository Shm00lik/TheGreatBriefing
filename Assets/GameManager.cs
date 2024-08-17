using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] foodsPrefabs;
    public AudioSource eatSound;
    private List<GameObject> currentFoods = new List<GameObject>();

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

        for (int i = 0; i < 5; i++)
        {
            SpawnFood();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject f in currentFoods)
        {
            f.transform.position = new Vector3(f.transform.position.x, f.transform.position.y - Time.deltaTime * f.GetComponent<FoodData>().baseSpeed, 0);
        }
    }

    void SpawnFood()
    {
        int randomIndex = Random.Range(0, foodsPrefabs.Length);
        GameObject f = Instantiate(foodsPrefabs[randomIndex], new Vector3(Random.Range(-7, 7), Random.Range(8, 10), 0), Quaternion.identity);
        currentFoods.Add(f);
    }

    public void Eat(GameObject food)
    {
        eatSound.Play();
        currentFoods.Remove(food);
        Destroy(food);
        SpawnFood();
    }
}
