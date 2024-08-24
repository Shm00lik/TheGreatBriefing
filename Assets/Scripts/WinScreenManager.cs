using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class WinScreenManager : MonoBehaviour
{
    public TMP_Text textToBlink;
    public float blinkDuration = 0.5f;

    void Start()
    {
        StartCoroutine(Blink());
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            textToBlink.enabled = !textToBlink.enabled;
            yield return new WaitForSeconds(blinkDuration);
        }
    }
}
