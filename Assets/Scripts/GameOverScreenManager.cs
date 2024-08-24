using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreenManager : MonoBehaviour
{
    public TMP_Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " + PlayerPrefs.GetInt("score");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
